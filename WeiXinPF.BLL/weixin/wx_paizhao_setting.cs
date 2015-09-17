﻿using System;
using System.Data;
using System.Collections.Generic;
using WeiXinPF.Common;
using WeiXinPF.Model;
namespace WeiXinPF.BLL
{
	/// <summary>
	/// 微拍系统设置
	/// </summary>
	public partial class wx_paizhao_setting
	{
		private readonly WeiXinPF.DAL.wx_paizhao_setting dal=new WeiXinPF.DAL.wx_paizhao_setting();
		public wx_paizhao_setting()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(WeiXinPF.Model.wx_paizhao_setting model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(WeiXinPF.Model.wx_paizhao_setting model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			
			return dal.Delete(id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string idlist )
		{
			return dal.DeleteList(idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public WeiXinPF.Model.wx_paizhao_setting GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

	 

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<WeiXinPF.Model.wx_paizhao_setting> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<WeiXinPF.Model.wx_paizhao_setting> DataTableToList(DataTable dt)
		{
			List<WeiXinPF.Model.wx_paizhao_setting> modelList = new List<WeiXinPF.Model.wx_paizhao_setting>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				WeiXinPF.Model.wx_paizhao_setting model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public WeiXinPF.Model.wx_paizhao_setting GetModelByWid(int wid)
        {

            return dal.GetModelByWid(wid);
        }

        /// <summary>
        /// 取微拍的提示语言
        /// </summary>
        /// <param name="wid">微帐号id</param>
        /// <param name="enterKeyWords">关键词</param>
        /// <returns></returns>
        public string GetPromptByWid(int wid, string enterKeyWords)
        {
            return dal.GetPromptByWid(  wid,   enterKeyWords);
        }
        /// <summary>
        ///  是否开启了，若开启返回true,否则返回false
        /// </summary>
        /// <param name="wid">微帐号id</param>
        /// <returns></returns>
        public bool isOpened(int wid)
        {
            return dal.isOpened(wid);
        }
		#endregion  ExtensionMethod
	}
}
