﻿using WeiXinPF.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiXinPF.Web.weixin.restaurant
{
    public partial class diancai_oder : WeiXinPage
    {
        BLL.wx_diancai_dingdan_manage managebll = new BLL.wx_diancai_dingdan_manage();
        Model.wx_diancai_dingdan_manage manage = new Model.wx_diancai_dingdan_manage();
        public string str = "";
        public string openId = string.Empty;
        public string menuStr = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                openId = MyCommFun.QueryString("openid");
                var type = MyCommFun.QueryString("type");
                if (!string.IsNullOrEmpty(openId))
                {

                    if (!string.Equals(type, "Pay", StringComparison.CurrentCultureIgnoreCase))
                    {
                        menuStr = string.Format(
                    @"<a class='Pay' href='diancai_oder.aspx?openid={0}&type=pay'>已付款</a><a class='Refund menu-active' href='diancai_oder.aspx?openid={0}&type=refund'>退单</a>", openId);
                        GetRefund(openId);
                    }
                    else
                    {
                        menuStr = string.Format(
                  @"<a class='Pay menu-active' href='diancai_oder.aspx?openid={0}&type=pay'>已付款</a><a class='Refund' href='diancai_oder.aspx?openid={0}&type=refund'>退单</a>", openId);
                        GetPay(openId);
                    }
                }
            }
        }

        public void GetPay(string openID)
        {
            DataSet dr = managebll.GetListList(openID);
            if (dr.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dr.Tables[0].Rows.Count; i++)
                {
                    str += "<ul class=\"round\">";
                    str += "<li class=\"title\"><a href=\"diancai_orderDetail.aspx?shopid=" + dr.Tables[0].Rows[i]["shopinfoid"].ToString() + "&dingdan=" + dr.Tables[0].Rows[i]["id"].ToString() + "&openid=" + openid + "\"><span>" + dr.Tables[0].Rows[i]["oderTime"].ToString() + "<img src=\"images\\tel.png\" class=\"HomeImage\"></img>" + dr.Tables[0].Rows[i].Field<string>("hotelName") + "</span></a></li>";
                    str += " <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"cpbiaoge\">";
                    str += "<tr><th>订单编号</th>";
                    str += "<th width=\"70\" class=\"cc\">订单金额</th><th width=\"55\" class=\"cc\">订单状态</th></tr>";
                    str += "<tr><td>" + dr.Tables[0].Rows[i]["orderNumber"].ToString() + "</td><td class=\"cc\">" + dr.Tables[0].Rows[i]["payAmount"].ToString() + "元</td>";
                    str += "<td class=\"cc\"> ";
                    if (dr.Tables[0].Rows[i]["payStatus"].ToString() == "1")
                    {
                        str += "<em class=\"ok\">部分使用</em>";
                    }
                    else if (dr.Tables[0].Rows[i]["payStatus"].ToString() == "2")
                    {
                        str += "<em class=\"error\">全部使用</em>";
                    }
                    else
                    {
                        str += "<em class=\"no\">未处理</em>";
                    }
                    str += " </td></tr></table></ul>";
                }
            }
        }

        public void GetRefund(string openID)
        {
            DataSet dr = managebll.GetListList(openID);
            if (dr.Tables[0].Rows.Count > 0)
            {

                

                for (int i = 0; i < dr.Tables[0].Rows.Count; i++)
                {
                    //随机退单号
                    Random ran = new Random();
                    string randKey = string.Format("T-{0}", ran.Next(10000, 99999));
                    str += "<ul class=\"round\">";
                    str += "<li class=\"title\"><a href=\"diancai_RefundOrderDetail.aspx?shopid=" + dr.Tables[0].Rows[i]["shopinfoid"].ToString() + "&dingdan=" + dr.Tables[0].Rows[i]["id"].ToString() + "&openid=" + openid + "\"><span>" + dr.Tables[0].Rows[i]["oderTime"].ToString() + "<img src=\"images\\tel.png\" class=\"HomeImage\"></img>" + dr.Tables[0].Rows[i].Field<string>("hotelName") + "</span></a></li>";
                    str += " <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"cpbiaoge\">";
                    str += "<tr><th>退单编号</th><th>订单编号</th>";
                    str += "<th width=\"50\" class=\"cc\">退款总额</th><th width=\"40\" class=\"cc\">状态</th></tr>";
                    str += "<tr><td>"+ randKey + "</td><td><span>" + dr.Tables[0].Rows[i]["orderNumber"].ToString() + "</span></td><td class=\"cc\">" + dr.Tables[0].Rows[i]["payAmount"].ToString() + "元</td>";
                    str += "<td class=\"cc\"> ";
                    if (dr.Tables[0].Rows[i]["payStatus"].ToString() == "1")
                    {
                        str += "<em class=\"ok\">部分使用</em>";
                    }
                    else if (dr.Tables[0].Rows[i]["payStatus"].ToString() == "2")
                    {
                        str += "<em class=\"error\">全部使用</em>";
                    }
                    else
                    {
                        str += "<em class=\"no\">未处理</em>";
                    }
                    str += " </td></tr></table></ul>";
                }
            }
        }
    }
}


