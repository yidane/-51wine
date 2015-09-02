﻿using MxWeiXinPF.BLL;
using MxWeiXinPF.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MxWeiXinPF.Web.admin.diancai
{
    public partial class shop_setup : Web.UI.ManagePage
    {
    
        public int shopid = 0;
     
      
       

        BLL.wx_diancai_shop_advertisement guanggBll = new BLL.wx_diancai_shop_advertisement();
        Model.wx_diancai_shop_advertisement guangg = new Model.wx_diancai_shop_advertisement();
        protected string editetype = "";
        wx_diancai_shop_advertisement iBll = new wx_diancai_shop_advertisement();

        protected void Page_Load(object sender, EventArgs e)
        {
            editetype = MyCommFun.QueryString("type");
            shopid = MyCommFun.RequestInt("shopid");

            if (!IsPostBack)
            {
                 BLL.wx_diancai_shop_setup setupBll = new BLL.wx_diancai_shop_setup();

                  DataSet ds = setupBll.Getsetup(shopid);
                  TextBox txtAdvertisementName;
                  TextBox txtSortid;
                  TextBox txtPicUrl;
                  TextBox txtWebsetUrl;
                  if (ds.Tables[0].Rows.Count > 0)
                  {
                      int setupid = MyCommFun.Obj2Int(ds.Tables[0].Rows[0]["id"]);
                      this.unionManage.InnerText = ds.Tables[0].Rows[0]["unionManage"].ToString();
                      this.unionTel.Text = ds.Tables[0].Rows[0]["unionTel"].ToString();
                      DataSet dritem = guanggBll.GetListByid(setupid);

                      IList<Model.wx_diancai_shop_advertisement> itemlist = iBll.GetModelList("setupid=" + setupid + " order by sortid asc,createDate");
                      if (itemlist != null && itemlist.Count > 0)
                      {
                          int count = itemlist.Count;

                          Model.wx_diancai_shop_advertisement itemEntity = new Model.wx_diancai_shop_advertisement();
                          for (int i = 1; i <= count; i++)
                          {
                              itemEntity = itemlist[(i - 1)];
                              txtAdvertisementName = this.FindControl("advertisementName" + i) as TextBox;
                              txtSortid = this.FindControl("sortid" + i) as TextBox;
                              txtPicUrl = this.FindControl("picUrl" + i) as TextBox;
                              txtWebsetUrl = this.FindControl("websetUrl" + i) as TextBox;

                              txtAdvertisementName.Text = itemEntity.advertisementName.ToString();
                              txtSortid.Text = itemEntity.sortid.ToString();
                              txtPicUrl.Text = itemEntity.picUrl.ToString();
                              txtWebsetUrl.Text = itemEntity.websetUrl.ToString();
                             
                          }

                      }
                  }

                

            }
        }


        protected void save_setup_Click(object sender, EventArgs e)
        {

            TextBox advertisementName;
            TextBox sortid;
            TextBox picUrl;
            TextBox websetUrl;

            editetype = MyCommFun.QueryString("type");
            Model.wx_userweixin weixin = GetWeiXinCode();
            int wid = weixin.id;
            shopid = MyCommFun.RequestInt("shopid");
            BLL.wx_diancai_shop_setup setupBll = new BLL.wx_diancai_shop_setup();
            Model.wx_diancai_shop_setup setup = new Model.wx_diancai_shop_setup();

            //修改
            #region 
            DataSet dr = setupBll.Getsetup(shopid);

            if (dr.Tables[0].Rows.Count>0)
            {

                int setupid= MyCommFun.Obj2Int(dr.Tables[0].Rows[0]["id"]);
                //赋值
                setup.id = setupid;
                setup.wid = wid;
                setup.unionManage = this.unionManage.InnerText;
                setup.unionTel = this.unionTel.Text;
                setup.shopid = shopid;
                setupBll.Update(setup);


               //广告图片

                //先删除
                guanggBll.DeleteByShopId(setupid);

          
               for (int i = 1; i <= 6; i++)
               {
                   advertisementName = this.FindControl("advertisementName" + i) as TextBox;
                   sortid = this.FindControl("sortid" + i) as TextBox;
                   picUrl = this.FindControl("picUrl" + i) as TextBox;
                   websetUrl = this.FindControl("websetUrl" + i) as TextBox;
                   // isdisplay=this.FindControl("isdisplay" + i) as  CheckBox;
                   if (advertisementName.Text.Trim() != "" && sortid.Text.Trim() != "")
                   {
                       guangg.setupid = setupid;
                       guangg.advertisementName = advertisementName.Text.ToString();
                       guangg.sortid = MyCommFun.Str2Int(sortid.Text.ToString());
                       guangg.picUrl = picUrl.Text.ToString();
                       guangg.websetUrl = websetUrl.Text.ToString();
                       guangg.createDate = DateTime.Now.ToString();
                       // guangg.isdisplay= Convert.ToBoolean(isdisplay.Text.ToString());
                       guanggBll.Add(guangg); ;

                   }
               }


               AddAdminLog(MXEnums.ActionEnum.Edit.ToString(), "修改商家设置，主键为" + setupid); //记录日志
               JscriptMsg("修改成功！", "shop_list.aspx", "Success");
                return;
            }
            #endregion

            //新增
            #region
            editetype = MyCommFun.QueryString("type");
            if (editetype == "add")
            {
                setup.wid = wid;
                setup.unionManage = this.unionManage.InnerText;
                setup.unionTel = this.unionTel.Text;
                setup.shopid = shopid;


                int id = setupBll.Add(setup);

                for (int i = 1; i <= 6; i++)
                {
                    advertisementName = this.FindControl("advertisementName" + i) as TextBox;
                    sortid = this.FindControl("sortid" + i) as TextBox;
                    picUrl = this.FindControl("picUrl" + i) as TextBox;
                    websetUrl = this.FindControl("websetUrl" + i) as TextBox;
                   // isdisplay=this.FindControl("isdisplay" + i) as  CheckBox;
                    if (advertisementName.Text.Trim() != "" && sortid.Text.Trim() != "")
                    {
                        guangg.setupid = id;
                        guangg.advertisementName= advertisementName.Text.ToString();
                        guangg.sortid = MyCommFun.Str2Int( sortid.Text.ToString());
                        guangg.picUrl = picUrl.Text.ToString();
                        guangg.websetUrl = websetUrl.Text.ToString();
                        guangg.createDate = DateTime.Now.ToString();
                       // guangg.isdisplay= Convert.ToBoolean(isdisplay.Text.ToString());
                        guanggBll.Add(guangg);;

                    }
                }
                          
                AddAdminLog(MXEnums.ActionEnum.Add.ToString(), "添加商城设置，主键为" + id); //记录日志
                JscriptMsg("添加成功！", Utils.CombUrlTxt("shop_list.aspx", "keywords={0}", ""), "Success");

            }
            #endregion

        }
    }
}