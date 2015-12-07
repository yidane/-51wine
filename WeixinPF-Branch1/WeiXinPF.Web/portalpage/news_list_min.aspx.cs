﻿using WeiXinPF.Common;
using WeiXinPF.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiXinPF.Web.portalpage
{
    public partial class news_list_min : PortalBasePage
    {
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (errInitTemplates != "")
            {
                Response.Write(errInitTemplates);
                return;
            }


            tPath = MyCommFun.GetRootPath() + "/templates_portal/newslist_min.html";
            PortalTemplate template = new PortalTemplate(tPath);
            template.tType = TemplateType.Class;

            template.OutPutHtml(templateIndexFileName);
        }
    }
}