﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Travel.Application.DomainModules.Order.DTOs
{
    public class OrderWithDetailListDTO
    {
        public Guid OrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderCode { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPersonName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string MobilePhoneNumber { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdentityCardNumber { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public string SinglePrice { get; set; }

        /// <summary>
        /// 票数量
        /// </summary>
        public int Count
        {
            get { return TicketCodeList.Count; }
        }

        /// <summary>
        /// 支付时间，下单时间
        /// </summary>
        public string PayTime { get; set; }

        /// <summary>
        /// 订单串码列表
        /// </summary>
        public List<string> TicketCodeList = new List<string>();
    }
}