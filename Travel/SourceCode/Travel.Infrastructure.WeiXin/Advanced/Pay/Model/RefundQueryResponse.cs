﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Travel.Infrastructure.WeiXin.Advanced.Pay.Model
{
    public class RefundQueryResponse
    {
        public string result_code { get; set; }
        public string err_code { get; set; }
        public string err_code_des { get; set; }
        public string appid { get; set; }
        public string mch_id { get; set; }
        public string device_info { get; set; }
        public string nonce_str { get; set; }
        public string sign { get; set; }
        public string transaction_id { get; set; }
        public string out_trade_no { get; set; }
        public int total_fee { get; set; }
        public string fee_type { get; set; }
        public int cash_fee { get; set; }
        public int refund_count { get; set; }
        public List<RefundInfo> RefundInfoList { get; set; }
        public bool IsSuccess
        {
            get { return string.Equals(result_code, "SUCCESS"); }
        }

        public RefundQueryResponse(string xmlString)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);
            Init(xmlDocument);
        }

        public RefundQueryResponse(XmlDocument xmlDocument)
        {
            Init(xmlDocument);
        }

        private void Init(XmlDocument xmlDocument)
        {
            var return_codeNode = xmlDocument.SelectSingleNode("xml/err_code");
            var return_msgNode = xmlDocument.SelectSingleNode("xml/err_code_des");
            var result_codeNode = xmlDocument.SelectSingleNode("xml/result_code");

            if (result_codeNode != null)
                this.result_code = result_codeNode.InnerText;

            if (string.Equals(result_code, "SUCCESS"))
            {
                appid = GetNodeInnerText(xmlDocument, "appid");
                mch_id = GetNodeInnerText(xmlDocument, "mch_id");
                device_info = GetNodeInnerText(xmlDocument, "device_info");
                nonce_str = GetNodeInnerText(xmlDocument, "nonce_str");
                sign = GetNodeInnerText(xmlDocument, "sign");
                result_code = GetNodeInnerText(xmlDocument, "result_code");
                err_code = GetNodeInnerText(xmlDocument, "xmlDocument");
                err_code_des = GetNodeInnerText(xmlDocument, "err_code_des");
                total_fee = int.Parse(GetNodeInnerText(xmlDocument, "total_fee"));
                fee_type = GetNodeInnerText(xmlDocument, "fee_type");
                cash_fee = int.Parse(GetNodeInnerText(xmlDocument, "cash_fee"));
                var couponfee = GetNodeInnerText(xmlDocument, "coupon_fee");
                var couponCount = GetNodeInnerText(xmlDocument, "coupon_count");
                transaction_id = GetNodeInnerText(xmlDocument, "transaction_id");
                out_trade_no = GetNodeInnerText(xmlDocument, "out_trade_no");
            }
        }

        private string GetNodeInnerText(XmlDocument xmlDocument, string nodeName)
        {
            var node = xmlDocument.SelectSingleNode(string.Format("xml/{0}", nodeName));
            return node != null ? node.InnerText : string.Empty;
        }
    }

    public class RefundInfo
    {
        public string out_refund_no { get; set; }
        public string refund_id { get; set; }
        public string refund_channel { get; set; }
        public int refund_fee { get; set; }
        public int coupon_refund_fee { get; set; }
        public int coupon_refund_count { get; set; }
        public string refund_status { get; set; }
        public List<CouponRefundInfo> CouponRefundInfoList { get; set; }
    }

    public class CouponRefundInfo
    {
        public string coupon_refund_batch_idP { get; set; }
        public string coupon_refund_id { get; set; }
        public int coupon_refund_fee { get; set; }
    }
}
