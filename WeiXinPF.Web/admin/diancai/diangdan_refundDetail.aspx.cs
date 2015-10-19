﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinPF.Common;

namespace WeiXinPF.Web.admin.diancai
{
    public partial class diangdan_refundDetail :Web.UI.ManagePage
    {
        BLL.wx_diancai_dingdan_manage managebll = new BLL.wx_diancai_dingdan_manage();
        Model.wx_diancai_dingdan_manage manage = new Model.wx_diancai_dingdan_manage();

        public string Dingdanlist = "";
        public string dingdanren = "";
        BLL.wx_diancai_shopinfo shopinfo = new BLL.wx_diancai_shopinfo();
        Model.wx_diancai_shopinfo sjopmodel = new Model.wx_diancai_shopinfo();
        public string id = "";
        public int ids = 0;
        public int shopid = 0;
        public string openid = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            ids = MyCommFun.RequestInt("id");
            id = MyCommFun.QueryString("id");
            shopid = MyCommFun.RequestInt("shopid");
            if (!IsPostBack)
            {

                if (ids != 0)
                {
                    List(ids);
                }
            }

        }

        protected void save_groupbase_Click(object sender, EventArgs e)
        {
            id = MyCommFun.QueryString("id");
            shopid = MyCommFun.RequestInt("shopid");
            string status = ddlStatusType.SelectedItem.Value;

            managebll.Updatestatus(id, status);

            manage = managebll.GetModel(MyCommFun.Str2Int(id));




            BLL.wx_diancai_member menbll = new BLL.wx_diancai_member();
            if (status == "1")
            {

                menbll.Update(manage.openid);
            }
            if (status == "2")
            {

                menbll.Updatefail(manage.openid);
            }


            AddAdminLog(MXEnums.ActionEnum.Edit.ToString(), "修改支付状态，主键为" + id); //记录日志
            JscriptMsg("修改成功！", "dingdan_manage.aspx?shopid=" + shopid + "", "Success");


        }


        public void List(int ids)
        {

            //退单

            Dingdanlist = "";
            dingdanren = "";

            DataSet dr = managebll.Getcaopin(id);
            if (dr.Tables[0].Rows.Count > 0)
            {
                decimal amount = 0;



                Dingdanlist += "<tr><th>商品信息名称</th><th class=\"cc\">单价</th><th class=\"cc\">购买份数</th><th class=\"rr\">总价</th> </tr>";
                for (int i = 0; i < dr.Tables[0].Rows.Count; i++)
                {
                    Dingdanlist += " <tr><td>" + dr.Tables[0].Rows[i]["cpName"] + "</td>";
                    Dingdanlist += "<td class=\"cc\">" + dr.Tables[0].Rows[i]["price"] + "</td>";
                    Dingdanlist += "<td class=\"cc\">" + dr.Tables[0].Rows[i]["num"] + "</td>";
                    Dingdanlist += "<td class=\"rr\">￥" + dr.Tables[0].Rows[i]["totpric"] + "</td></tr>";
                    amount += Convert.ToDecimal(dr.Tables[0].Rows[i]["totpric"]);
                }

                sjopmodel = shopinfo.GetModel(shopid);//配送费
                decimal zongji = amount + Convert.ToDecimal(sjopmodel.sendCost);
                if (sjopmodel != null)
                {
                    Dingdanlist += "<tr><td>商品总费</td><td class=\"cc\">￥" + amount + "</td>  <td class=\"cc\" >配送费</td><td class=\"rr\" >￥" + sjopmodel.sendCost + "</td></tr>";
                }
                else
                {
                    Dingdanlist += "<tr><td>商品总费</td><td class=\"cc\">￥" + amount + "</td>  <td class=\"cc\" >配送费</td><td class=\"rr\" >￥" + 0 + "</td></tr>";
                }
                Dingdanlist += "<tr><td></td><td ></td><td ></td><td class=\"rr\">总计：<span class='text-danger'>￥" + zongji + "</span></td></tr>";

            }


            manage = managebll.GetModeldingdan(id);
            //退单信息
            if (manage != null)
            {
                dingdanren += "<tr><td width=\"70\">退单编号： " + manage.orderNumber + "</td></tr>";
                dingdanren += "<tr> <td>退单日期：" + manage.oderTime + "</td></tr>";
                //todo:加订单号
                var url = '#';
                dingdanren += "<tr> <td>订单号：" +8888888 + "<a href='"+ url + "' class='btn btn-primary btn-lg active' role='button'>订单查看</a></td></tr>";
                dingdanren += "<tr><td>退单人：" + manage.customerName + "</td></tr>";
                dingdanren += "<tr><td>电话：" + manage.customerTel + "</td></tr>";
                //                dingdanren += "<tr><td>地址：" + manage.address + "</td></tr>";
                //                dingdanren += "<tr><td>备注 ：" + manage.oderRemark + "</td></tr>";
                if (manage.payStatus == 1)
                {
                    dingdanren += "<tr><td>退单状态：<em  style='width:70px;' class='ok'>已处理</em></td></tr>";
                }
                else
                {
                    dingdanren += "<tr><td>退单状态：<em  style='width:70px;' class='no'>未处理</em></td></tr>";
                }
            }
            else
            {
                dingdanren += "<tr><td width=\"70\">退单编号：</td></tr>";
                dingdanren += "<tr> <td>下单时间：</td></tr>";
                dingdanren += "<tr><td>联系人：</td></tr>";
                dingdanren += "<tr><td>联系电话：</td></tr>";
                dingdanren += "<tr><td>地址：</td></tr>";
                dingdanren += "<tr><td>备注 ：</td></tr>";


                dingdanren += "<tr><td>退单状态：<em  style='width:70px;' class='no'>未处理</em></td></tr>";

            }


            //            dingdanren += "<tr><td>商家留言：</td></tr> <tr> <td></td></tr>";
        }
    }
}