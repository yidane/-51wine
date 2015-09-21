﻿using WeiXinPF.Templates;
using System;
using System.Collections.Generic;
using WeiXinPF.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using Senparc.Weixin.MP;

namespace WeiXinPF.Web.shop
{
    public partial class confirmOrder : ShopBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.OnInit(e);
            if (errInitTemplates != "")
            {
                Response.Write(errInitTemplates);
                return;
            }

            //1获得模版基本信息
            BLL.wx_module_templates tBll = new BLL.wx_module_templates();
            templateFileName = tBll.GetTemplatesFileNameByWid("shop", wid);
            if (templateFileName == null || templateFileName.Trim() == "")
            {
                errInitTemplates = "不存在该帐号或者该帐号尚未设置模版！";
                Response.Write(errInitTemplates);
                Response.End();
                return;
            }

            //授权 04e11d01b9df2865
            //string openid = "";
            ////openid = MyCommFun.RequestOpenid();
            //BLL.wx_userweixin bll = new BLL.wx_userweixin();
            //Model.wx_userweixin wxModel = bll.GetModel(wid);

            //string code = MyCommFun.QueryString("code");
            //if (code == null || code.Trim() == "")
            //{
            //    int orderId = MyCommFun.RequestInt("orderid");
            //    string thisUrl = MyCommFun.getWebSite() + "/shop/confirmOrder.aspx?wid=" + wid;
            //    string newUrl = OAuthApi.GetAuthorizeUrl(wxModel.AppId, thisUrl, "confirmOrder", OAuthScope.snsapi_base);
            //    Response.Redirect(newUrl);
            //}
            //else
            //{
            //    var result = OAuthApi.GetAccessToken(wxModel.AppId, wxModel.AppSecret, code);
            //    openid = result.openid;
            //}

            BLL.wx_userweixin bll = new BLL.wx_userweixin();
            Model.wx_userweixin wxModel = bll.GetModel(wid);
            string thisUrl = MyCommFun.getWebSite() + "/shop/confirmOrder.aspx?wid=" + wid;
            OAuth2BaseProc(wxModel, "confirmOrder", thisUrl);

            //授权结束



            BLL.wx_shop_user_addr uAddrBll = new BLL.wx_shop_user_addr();
            IList<Model.wx_shop_user_addr> uaddrList = uAddrBll.GetOpenidAddr(openid, wid);
            if (uaddrList == null || uaddrList.Count <= 0 || uaddrList[0].id <= 0)
            {
                //该微信用户没有添加地址
                Response.Redirect("/shop/editaddr.aspx?wid=" + wid + "&openid=" + openid + "&frompage=confirmOrder.aspx");
                // MessageBox.ResponseScript(this, "window.location.href =/shop/editaddr.aspx?wid=" + wid + "&openid=" + openid + "&frompage=confirmOrder.aspx");
                return;
            }

            serverPath = MyCommFun.GetRootPath() + "/shop/templates/" + templateFileName + "/confirmOrder.html";
            ShopTemplateMgr template = new ShopTemplateMgr("/shop/templates/" + templateFileName, serverPath, wid);
            template.tType = TemplateType.confirmOrder;
            template.openid = openid;
            template.OutPutHtml(wid);
        }

        override protected void OnInit(EventArgs e)
        {

        }
    }
}