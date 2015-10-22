﻿/*----------------------------------------------------------------
    Copyright (C) 2015 Senparc
    
    文件名：RequestMessageEvent_Submit_Membercard_User_Info.cs
    文件功能描述：事件之接收会员信息事件通知（submit_membercard_user_info）
    
    
    创建标识：Senparc - 20150910
----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneGulp.WeChat.MP.Entities
{
    /// <summary>
    /// 事件之接收会员信息事件通知（submit_membercard_user_info）
    /// 卡券 会员卡
    /// </summary>
    public class RequestMessageEvent_Submit_Membercard_User_Info : RequestMessageEventBase, IRequestMessageEventBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public override Event Event
        {
            get { return Event.submit_membercard_user_info; }
        }

        /// <summary>
        /// 卡券ID
        /// </summary>
        public string CardId { get; set; }
        /// <summary>
        /// 卡券Code码
        /// </summary>
        public string UserCardCode { get; set; }
    }
}