﻿using WeiXinPF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiXinPF.BLL;
using WeiXinPF.Model.KNSHotel;
using WeiXinPF.Web.weixin.qiangpiao;
using WeiXinPF.Web.weixin.WeChatPay;
using WeiXinPF.WeiXinComm;

namespace WeiXinPF.Web.weixin.KNSHotel
{
    using System.Transactions;

    using WeiXinPF.Application.DomainModules.IdentifyingCode.Service;
    using WeiXinPF.Infrastructure.DomainDataAccess.IdentifyingCode;

    /// <summary>
    /// hotel_info 的摘要说明
    /// </summary>
    public class hotel_info : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Dictionary<string, string> jsonDict = new Dictionary<string, string>();
            context.Response.ContentType = "text/json";
            string _action = MyCommFun.QueryString("myact");

            BLL.wx_hotel_dingdan dingdanbll = new BLL.wx_hotel_dingdan();
            Model.wx_hotel_dingdan dingdan = new Model.wx_hotel_dingdan();
         
            if (_action == "dingdan")
            {
               var isSuccess = this.CreateOrderProcess();

                if (isSuccess)
                {
               jsonDict.Add("ret", "ok");
               jsonDict.Add("content", "提交成功！");
                }
                else
                {
                    jsonDict.Add("ret", "err");
                    jsonDict.Add("content", "创建订单失败");
                }
               context.Response.Write(MyCommFun.getJsonStr(jsonDict));
               return;

             }

            if (_action =="dingdanedite")
            {

                dingdan.id = MyCommFun.RequestInt("dingdanidnum");
                dingdan.oderName=MyCommFun.QueryString("truename");
                dingdan.tel = MyCommFun.QueryString("tel");

                if (Convert.ToDateTime(MyCommFun.QueryString("arriveTime")) < DateTime.Now.AddDays(-1))
                {
                    jsonDict.Add("ret", "faile");
                    jsonDict.Add("content", "入住时间不能小于今天时间！");
                    context.Response.Write(MyCommFun.getJsonStr(jsonDict));
                    return;
                }

                if (MyCommFun.QueryString("arriveTime") !="")
                {
                    dingdan.arriveTime = Convert.ToDateTime(MyCommFun.QueryString("arriveTime"));
                }

                if (Convert.ToDateTime(MyCommFun.QueryString("leaveTime")) < dingdan.arriveTime)
                {
                    jsonDict.Add("ret", "faile");
                    jsonDict.Add("content", "离店时间不能小于入住时间！");
                    context.Response.Write(MyCommFun.getJsonStr(jsonDict));
                    return;
                }

                if (MyCommFun.QueryString("leaveTime") != "")
                {
                    dingdan.leaveTime = Convert.ToDateTime(MyCommFun.QueryString("leaveTime"));
                }



                dingdan.orderNum = MyCommFun.RequestInt("nums");
                dingdan.price = Convert.ToDecimal( MyCommFun.QueryString("xianjianum"));
                dingdan.yuanjia = Convert.ToDecimal( MyCommFun.QueryString("yuanjianum"));
                dingdan.remark = MyCommFun.QueryString("info");
                dingdan.IdentityNumber = MyCommFun.QueryString("identityNumber");
                dingdanbll.Updatehotel(dingdan);

                jsonDict.Add("ret", "ok");
                jsonDict.Add("content", "修改成功！");
                context.Response.Write(MyCommFun.getJsonStr(jsonDict));
                return;
            }

            if (_action == "dingdandelete")
            {
                //                int ddid = MyCommFun.RequestInt("dingdanidnum");
                //                dingdanbll.Update(ddid);
                //                jsonDict.Add("ret", "ok");
                //                jsonDict.Add("content", "删除成功！");
                //                context.Response.Write(MyCommFun.getJsonStr(jsonDict));
                UpdateOrder(dingdanbll, jsonDict, context, HotelStatusManager.OrderStatus.Cancelled.StatusId, "订单取消成功！");
                 
            }
            if (_action == "dingdancompleted")
            {
                //                int ddid = MyCommFun.RequestInt("dingdanidnum");
                //                dingdanbll.Update(ddid);
                //                jsonDict.Add("ret", "ok");
                //                jsonDict.Add("content", "删除成功！");
                //                context.Response.Write(MyCommFun.getJsonStr(jsonDict));
                UpdateOrder(dingdanbll, jsonDict, context, HotelStatusManager.OrderStatus.Completed.StatusId, "操作成功！");

            }
            //            if (_action == "paymentSuccess")
            //            {
            //                int ddid = MyCommFun.RequestInt("dingdanidnum");
            //                dingdanbll.Update(ddid, "3");
            //                jsonDict.Add("ret", "ok");
            //                jsonDict.Add("content", "订单支付成功！");
            //                context.Response.Write(MyCommFun.getJsonStr(jsonDict));
            //                return;
            //            }


            //            if (_action == "dingdanPayed")
            //            {
            //                UpdateOrder(dingdanbll, jsonDict, context, StatusManager.OrderStatus.Payed.StatusId, "订单支付成功！");
            //            }

            if (_action == "dingdanPaying")
            {
                GetPayUrl(dingdanbll, context);
            }
        }

        private bool CreateOrderProcess()
        {
            var hotel = new BLL.wx_hotels_info().GetModel(Convert.ToInt32(MyCommFun.QueryString("hotelid")));
            var isSuccess = false;

            if (hotel == null)
            {
                return false;
            }

            using (var scope = new TransactionScope())
            {
                var order = this.CreateOrder();

                if (order != null)
                {
                    for (var i = 0; i < order.orderNum; i++)
                    {
                        var iCode = new IdentifyingCodeInfo()
                        {
                            IdentifyingCodeId = Guid.NewGuid(),
                            CreateTime = DateTime.Now,
                            IdentifyingCode = string.Empty,
                            ModifyTime = DateTime.Now,
                            ModuleName = "hotel",
                            OrderCode = order.OrderNumber,
                            OrderId = order.id.ToString(),
                            ProductCode = order.roomType,
                            ProductId = order.roomid.ToString(),
                            ShopId = order.hotelid.Value.ToString(),
                            Wid = hotel.wid.Value,
                            Status = 0
                        };
                        IdentifyingCodeService.AddIdentifyingCode(iCode);
                    }

                    scope.Complete();
                    isSuccess = true;
                }                
            }

            return isSuccess;
        }

        private Model.wx_hotel_dingdan CreateOrder()
        {
            var dingdan = new Model.wx_hotel_dingdan
                              {
                                  hotelid = Convert.ToInt32(MyCommFun.QueryString("hotelid")),
                                  roomid = Convert.ToInt32(MyCommFun.QueryString("roomid")),
                                  openid = MyCommFun.QueryString("openid"),
                                  oderName = MyCommFun.QueryString("oderName"),
                                  tel = MyCommFun.QueryString("tel"),
                                  orderStatus = 0,
                                  IdentityNumber = MyCommFun.QueryString("identityNumber"),
                                  arriveTime =
                                      Convert.ToDateTime(MyCommFun.QueryString("arriveTime")),
                                  leaveTime =
                                      Convert.ToDateTime(MyCommFun.QueryString("leaveTime")),
                                  roomType = MyCommFun.QueryString("roomType"),
                                  orderTime = DateTime.Now,
                                  orderNum = MyCommFun.RequestInt("orderNum"),
                                  isDelete = 0,
                                  price = MyCommFun.Str2Decimal(MyCommFun.QueryString("price")),
                                  yuanjia = MyCommFun.Str2Decimal(MyCommFun.QueryString("yuanjia")),
                                  remark = MyCommFun.QueryString("remark"),
                                  OrderNumber =
                                      "H" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + Utils.Number(5)
                              };

            dingdan.id = new BLL.wx_hotel_dingdan().Add(dingdan);

            return dingdan;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dingdanbll"></param>
        /// <param name="jsonDict"></param>
        /// <param name="context"></param>
        private void GetPayUrl(wx_hotel_dingdan dingdanbll, HttpContext context)
        {
            int ddid = MyCommFun.RequestInt("dingdanidnum");
            if (ddid>0)
            {
                var dingdan = dingdanbll.GetModel(ddid);
                if (dingdan!=null)
                {
                    int wid = 0;
                    if (dingdan.hotelid != null)
                    {
                        var hotelInfo = new BLL.wx_hotels_info().GetModel(dingdan.hotelid.Value);
                        if (hotelInfo.wid != null) wid = hotelInfo.wid.Value;
                    }
                    //总花费
                    var dateSpan = dingdan.leaveTime.Value - dingdan.arriveTime.Value;
                    var totalPrice = dingdan.price.Value * dingdan.orderNum.Value * dateSpan.Days;
                    
                    var port = WebHelper.GetHostPort();
                    var url = string.Format("{0}/weixin/KNSHotel/hotel_order_paycallback.aspx", port);
                    var entity = new UnifiedOrderEntity
                    {
                        wid = wid,
                        total_fee = totalPrice == null ? 0 : (int)totalPrice,
                        out_trade_no = dingdan.OrderNumber,
                        openid = dingdan.openid,
                        OrderId = dingdan.id.ToString(),
                        body = string.Format("{0}间{1}",dingdan.orderNum,dingdan.roomType),
                        PayModuleID = (int)PayModuleEnum.Hotel,
                        PayComplete = url
                    };

                    entity.Extra.Add("dingdanidnum", ddid.ToString()); 
                    entity.Extra.Add("openid", dingdan.openid); 
                    entity.Extra.Add("hotelid", dingdan. hotelid.ToString()); 
                    entity.Extra.Add("roomid", dingdan. roomid.ToString());  

                    var ticket = EncryptionManager.CreateIV();
                    var payData = EncryptionManager.AESEncrypt(entity.ToJson(), ticket);

                    context.Response.Write(AjaxResult.Success(PayHelper.GetPayUrl(payData, ticket)));
                }
            }
            else
            {
                context.Response.Write(AjaxResult.Error("获取支付状态失败！"));
            }
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="dingdanbll"></param>
        /// <param name="jsonDict"></param>
        /// <param name="context"></param>
        private void UpdateOrder(wx_hotel_dingdan dingdanbll, Dictionary<string, string> jsonDict,
            HttpContext context, int statusId, string msg)
        {
            int ddid = MyCommFun.RequestInt("dingdanidnum");
            dingdanbll.Update(ddid, statusId.ToString());
            jsonDict.Add("ret", "ok");
            jsonDict.Add("content", msg);
            context.Response.Write(MyCommFun.getJsonStr(jsonDict));
            return;
        }



        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}