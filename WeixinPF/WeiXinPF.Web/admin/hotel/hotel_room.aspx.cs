﻿using WeiXinPF.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WeiXinPF.Web.admin.hotel
{
    public partial class hotel_room : Web.UI.ManagePage
    {
        protected int totalCount;
        protected int page;
        protected int pageSize;
        BLL.wx_hotel_room gbll = new BLL.wx_hotel_room();
        protected string keywords = string.Empty;
        public int hotelid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.keywords = MXRequest.GetQueryString("keywords");
            hotelid = MyCommFun.RequestInt("hotelid");
            this.pageSize = GetPageSize(10); //每页数量
            if (!Page.IsPostBack)
            {
                Model.wx_userweixin weixin = GetWeiXinCode();
                RptBind(CombSqlTxt(keywords), "createDate desc,id desc");
            }
        }

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby)
        {
            Model.wx_userweixin weixin = GetWeiXinCode();

            //判断是否已经设置了微留言基本信息
            BLL.wx_hotel_room sbll = new BLL.wx_hotel_room();
            hotelid = MyCommFun.RequestInt("hotelid");
            _strWhere = "hotelid=" + hotelid + " " + _strWhere;
            this.page = MXRequest.GetQueryInt("page", 1);
            txtKeywords.Text = this.keywords;
            DataSet ds = gbll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
            // DataSet ds = gbll.GetList( _strWhere);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr;

                int count = ds.Tables[0].Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    dr = ds.Tables[0].Rows[i];

                }
                ds.AcceptChanges();
            }
            this.rptList.DataSource = ds;
            this.rptList.DataBind();

            //绑定页码
            txtPageNum.Text = this.pageSize.ToString();
            string pageUrl = Utils.CombUrlTxt("hotel_list.aspx", "keywords={0}&page={1}", this.keywords, "__id__");
            PageContent.InnerHtml = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords)
        {
            StringBuilder strTemp = new StringBuilder();
            _keywords = _keywords.Replace("'", "");
            if (!string.IsNullOrEmpty(_keywords))
            {
                strTemp.Append(" and  roomType like  '%" + _keywords + "%' ");
            }

            return strTemp.ToString();
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size)
        {
            int _pagesize;
            if (int.TryParse(Utils.GetCookie("hotel_list_page_size"), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    return _pagesize;
                }
            }
            return _default_size;
        }
        #endregion

        //关健字查询
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(Utils.CombUrlTxt("hotel_list.aspx", "keywords={0}", txtKeywords.Text));
        }

        //设置分页数量
        protected void txtPageNum_TextChanged(object sender, EventArgs e)
        {
            int _pagesize;
            if (int.TryParse(txtPageNum.Text.Trim(), out _pagesize))
            {
                if (_pagesize > 0)
                {
                    Utils.WriteCookie("hotel_list_page_size", _pagesize.ToString(), 14400);
                }
            }
            Response.Redirect(Utils.CombUrlTxt("hotel_list.aspx", "keywords={0}", this.keywords));
        }

        //批量删除
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int sucCount = 0;
            int errorCount = 0;

            for (int i = 0; i < rptList.Items.Count; i++)
            {
                int id = Convert.ToInt32(((HiddenField)rptList.Items[i].FindControl("hidId")).Value);
                CheckBox cb = (CheckBox)rptList.Items[i].FindControl("chkId");
                if (cb.Checked)
                {
                    if (gbll.Delete(id))
                    {
                        sucCount += 1;
                    }
                    else
                    {
                        errorCount += 1;
                    }
                }
            }
            AddAdminLog(MXEnums.ActionEnum.Delete.ToString(), "信息" + sucCount + "条，失败" + errorCount + "条"); //记录日志

            JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("hotel_list.aspx", "keywords={0}", this.keywords), "Success");
        }

        /// <summary>
        /// 审核通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAudit_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 审核不通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {

        }
    }
}