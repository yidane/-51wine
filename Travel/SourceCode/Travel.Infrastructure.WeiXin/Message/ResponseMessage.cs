﻿using System;
using System.Web;

namespace Travel.Infrastructure.WeiXin.Message
{
    /// <summary>
    /// 响应消息
    /// </summary>
    public class ResponseMessage : Infrastructure.WeiXin.Message.Message
    {
        public ResponseMessage()
        {
            CreateTime = (int)(DateTime.Now - DateTime.Parse("1970-1-1")).TotalSeconds;
        }

        public RepMsgData Data { get; set; }

        public override string InnerToXmlText()
        {
            var ret = base.InnerToXmlText();
            if (Data != null)
            {
                ret += "\n" + Data.ToXmlText();
            }
            return ret;
        }

        /// <summary>
        /// 将响应写入响应流。
        /// </summary>
        /// <param name="end">如果为true，调用response.End()方法</param>
        public void Response(bool end = true)
        {
            var response = HttpContext.Current.Response;
            if (response.IsClientConnected)
            {
                response.Write(ToXmlText());
                if (end)
                    response.End();
            }
        }
    }

    /// <summary>
    /// 响应消息数据
    /// </summary>
    public abstract class RepMsgData
    {
        public abstract string ToXmlText();

        public override string ToString()
        {
            return ToXmlText();
        }
    }
}
