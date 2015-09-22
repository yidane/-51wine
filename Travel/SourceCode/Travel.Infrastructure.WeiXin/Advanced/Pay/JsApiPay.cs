﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Xml.Linq;
using LitJson;
using OneGulp.WeChat.MP;
using OneGulp.WeChat.MP.AdvancedAPIs;
using OneGulp.WeChat.MP.TenPayLibV3;
using Travel.Infrastructure.WeiXin.Advanced.Pay.Model;
using Travel.Infrastructure.WeiXin.Log;

namespace Travel.Infrastructure.WeiXin.Advanced.Pay
{
    public class JsApiPay
    {

        /// <summary>
        /// 调用统一下单，获得下单结果
        /// </summary>
        /// <param name="request">统一下单结果</param>
        /// <returns>统一下单结果</returns>
        public UnifiedOrderResult GetUnifiedOrderResult(UnifiedOrderRequest request)
        {
            ////统一下单
            //var data = new WxPayData();
            //data.SetValue("body", request.body);
            //data.SetValue("attach", request.attach);
            //data.SetValue("out_trade_no", request.out_trade_no);
            //data.SetValue("total_fee", request.total_fee);
            ////data.SetValue("total_fee", 2);
            ////data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            ////data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            //data.SetValue("time_start", request.time_start);
            //data.SetValue("time_expire", request.time_expire);
            //data.SetValue("goods_tag", request.goods_tag);
            //data.SetValue("trade_type", "JSAPI");
            //data.SetValue("openid", request.openid);

            //var result = WxPayApi.UnifiedOrder(data);

            //if (string.IsNullOrEmpty(result.appid) || string.IsNullOrEmpty(result.prepay_id))
            //{
            //    throw new WxPayException("UnifiedOrder response error!");
            //}

            //return result;

            var packageReqHandler = new RequestHandler(null);
            //初始化
            packageReqHandler.Init();
            //packageReqHandler.SetKey(""/*TenPayV3Info.Key*/);

            var timeStamp = TenPayV3Util.GetTimestamp();
            var nonceStr = TenPayV3Util.GetNoncestr();

            //设置package订单参数
            packageReqHandler.SetParameter("appid", WxPayConfig.APPID);		  //公众账号ID
            packageReqHandler.SetParameter("mch_id", WxPayConfig.MCHID);		  //商户号
            packageReqHandler.SetParameter("nonce_str", nonceStr);                    //随机字符串
            packageReqHandler.SetParameter("body", request.body);  //商品描述
            packageReqHandler.SetParameter("attach", request.attach);
            packageReqHandler.SetParameter("out_trade_no", request.out_trade_no);		//商家订单号
            packageReqHandler.SetParameter("total_fee", request.total_fee.ToString());			        //商品金额,以分为单位(money * 100).ToString()
            packageReqHandler.SetParameter("spbill_create_ip", WxPayConfig.IP);   //用户的公网ip，不是商户服务器IP
            packageReqHandler.SetParameter("notify_url", WxPayConfig.NOTIFY_URL);		    //接收财付通通知的URL
            packageReqHandler.SetParameter("trade_type", TenPayV3Type.JSAPI.ToString());//交易类型
            packageReqHandler.SetParameter("openid", request.openid);	                    //用户的openId

            string sign = packageReqHandler.CreateMd5Sign("key", WxPayConfig.KEY);
            packageReqHandler.SetParameter("sign", sign);	                    //签名

            string data = packageReqHandler.ParseXML();

            var unifiedOrderResult = TenPayV3.Unifiedorder(data);
            var rtnUnifiedOrderResult = new UnifiedOrderResult(unifiedOrderResult);

            return rtnUnifiedOrderResult;
        }

        /**
        *  
        * 从统一下单成功返回的数据中获取微信浏览器调起jsapi支付所需的参数，
        * 微信浏览器调起JSAPI时的输入参数格式如下：
        * {
        *   "appId" : "wx2421b1c4370ec43b",     //公众号名称，由商户传入     
        *   "timeStamp":" 1395712654",         //时间戳，自1970年以来的秒数     
        *   "nonceStr" : "e61463f8efa94090b1f366cccfbbb444", //随机串     
        *   "package" : "prepay_id=u802345jgfjsdfgsdg888",     
        *   "signType" : "MD5",         //微信签名方式:    
        *   "paySign" : "70EA570631E4BB79628FBCA90534C63FF7FADD89" //微信签名 
        * }
        * @return string 微信浏览器调起JSAPI时的输入参数，json格式可以直接做参数用
        * 更详细的说明请参考网页端调起支付API：http://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=7_7
        * 
        */
        public string GetJsApiParameters()
        {
            LogManager.Debug(this.GetType().ToString(), "JsApiPay::GetJsApiParam is processing...");

            WxPayData jsApiParam = new WxPayData();
            //jsApiParam.SetValue("appId", unifiedOrderResult.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", WxPayHelper.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayHelper.GenerateNonceStr());
            //jsApiParam.SetValue("package", "prepay_id=" + unifiedOrderResult.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

            string parameters = jsApiParam.ToJson();

            LogManager.Debug(this.GetType().ToString(), "Get jsApiParam : " + parameters);
            return parameters;
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public RefundOrderResponse Refund(RefundOrderRequest request)
        {
            var payData = request.CreatePayData();
            var result = WxPayApi.Refund(payData);
            return new RefundOrderResponse(result);
        }

        public bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public RefundQueryResponse RefundQuery(RefundQueryRequest request)
        {
            return new RefundQueryResponse();
        }
    }
}
