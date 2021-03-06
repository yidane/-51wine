﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace WeiXinPF.BLL
{
    public class wx_diancai_tuidan_manage
    {
        private readonly WeiXinPF.DAL.wx_diancai_tuidan_manage dal = new WeiXinPF.DAL.wx_diancai_tuidan_manage();
        public void AddRefundModel(List<Model.wx_diancai_tuidan_manage> modelList)
        {
            dal.AddRefundModel(modelList);
        }

        public void Refund(string refundcode)
        {
            dal.Refund(refundcode);
        }

        public void Refund(int refundId)
        {
            dal.Refund(refundId);
        }

        public void RefundComplete(string refundCode, string refundReason)
        {
            dal.RefundComplete(refundCode, refundReason);
        }

        public void RefundFail(string refundCode, string refundReason)
        {
            dal.RefundFail(refundCode, refundReason);
        }

        public DataSet GetRefundList(string openId)
        {
            return dal.GetRefundList(openId);
        }

        public DataSet GetRefundList(int shopId, int pageSize, int pageIndex,
                                                            DateTime beginDate, DateTime endDate, int payAmountMin,
                                                            int payAmountMax, string refundNumber, string orderNumber,
                                                            string customerName, string customerTel, int refundStatus, out int totalCount)
        {
            return dal.GetRefundList(shopId, pageSize, pageIndex,
                                                        beginDate, endDate, payAmountMin,
                                                        payAmountMax, refundNumber, orderNumber,
                                                        customerName, customerTel, refundStatus, out totalCount);
        }

        /// <summary>
        /// 获取订单菜品详情信息
        /// </summary>
        /// <param name="refundCode"></param>
        /// <returns></returns>
        public string GetRefundDetail(string refundCode)
        {
            return dal.GetRefundDetail(refundCode);
        }

        public DataSet GetRefundDetailWithOrderDetail(int shopId, int orderId, string refundCode)
        {
            return dal.GetRefundDetail(shopId, orderId, refundCode);
        }

        /// <summary>
        /// 获取微信所必须的退单参数
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="dingdanId"></param>
        /// <param name="refundCode"></param>
        /// <returns></returns>
        public DataSet GetWeChatRefundParams(int shopId, int dingdanId, string refundCode)
        {
            return dal.GetWeChatRefundParams(shopId, dingdanId, refundCode);
        }
    }
}
