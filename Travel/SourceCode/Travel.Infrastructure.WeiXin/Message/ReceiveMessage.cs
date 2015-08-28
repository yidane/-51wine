﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml.Linq;

namespace Travel.Infrastructure.WeiXin.Message
{
    /// <summary>
    /// 接收的消息
    /// </summary>
    public class ReceiveMessage : Infrastructure.WeiXin.Message.Message
    {
        /// <summary>
        /// 消息id
        /// </summary>
        [Output]
        public long MsgId { get; set; }


        #region 静态方法


        /// <summary>
        /// 从xml文件解析消息。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static ReceiveMessage Parse(string text)
        {
            var ret = ObtainByType(text);
            ret.ParseFrom(text);

            return ret;
        }

        /// <summary>
        /// 从HttpContext中获取信息并解析
        /// </summary>
        /// <returns></returns>
        public static ReceiveMessage ParseFromContext()
        {
            var request = HttpContext.Current.Request;
            var sr = new StreamReader(request.InputStream);
            var msg = Parse(sr.ReadToEnd());

            return msg;
        }

        /// <summary>
        /// 为了简单，直接使用switch
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static ReceiveMessage ObtainByType(string text)
        {
            var e = XElement.Parse(text);
            var xElement = e.Element("MsgType");
            if (xElement != null)
            {
                var t = xElement.Value;
                switch (t)
                {
                    case "text":
                        return new RecTextMessage();
                    case "image":
                        return new RecImageMessage();
                    case "location":
                        return new RecLocationMessage();
                    case "link":
                        return new RecLinkMessage();
                    case "event":
                        return new RecEventMessage();
                }
            }
            return null;
        }


        private static IMessageHandler _messageHandler;

        /// <summary>
        /// 注册消息处理程序。当收到消息是后执行相应的方法。
        /// </summary>
        /// <param name="handler"></param>
        public static void ResisterHandler(IMessageHandler handler)
        {
            _messageHandler = handler;
        }

        #endregion

        /// <summary>
        /// 从接收到的消息中获取信息以填充到响应消息中。
        /// </summary>
        /// <param name="msg"></param>
        private void FillRepMsg(ResponseMessage msg)
        {
            msg.FromUserName = ToUserName;
            msg.ToUserName = FromUserName;
        }

        /// <summary>
        /// 获取文本响应消息
        /// </summary>
        /// <returns></returns>
        public ResponseMessageSub GetTextResponse(string text = null)
        {
            var ret = new ResponseMessageSub();
            FillRepMsg(ret);
            ret.Data = (RepMsgDataSub)text;
            return ret;
        }

        /// <summary>
        /// 获取音乐响应消息
        /// </summary>
        public RepMusicMessage GetMusicResponse()
        {
            var ret = new RepMusicMessage();
            FillRepMsg(ret);
            return ret;
        }

        /// <summary>
        /// 获取图文响应消息
        /// </summary>
        /// <returns></returns>
        public RepNewsMessage GetNewsResponse(IEnumerable<NewsItem> data = null)
        {
            var ret = new RepNewsMessage();
            FillRepMsg(ret);
            if (data != null)
            {
                var msgData = new NewsMsgData();
                msgData.Items.AddRange(data);
                ret.Data = msgData;
            }
            return ret;
        }

        /// <summary>
        /// （调用已经定义的消息处理程序）处理消息
        /// <para>注意，请不要在接口IMessageHandler的实现方法内再次调用，这样可能会导致死循环。</para>
        /// </summary>
        /// <returns></returns>
        public ResponseMessage Process()
        {
            //如果没有定义事件处理程序则返回
            if (_messageHandler == null)
                return null;

            var dic = new Dictionary<MessageType, Func<ReceiveMessage, ResponseMessage>>
            {
                {MessageType.Text, rm => _messageHandler.OnTextMessage(rm as RecTextMessage)},
                {MessageType.Image, rm => _messageHandler.OnImageMessage(rm as RecImageMessage)},
                {MessageType.Link, rm => _messageHandler.OnLinkMessage((RecLinkMessage) rm)},
                {MessageType.Location, rm => _messageHandler.OnLocationMessage((RecLocationMessage) rm)},
                {MessageType.Event, rm => _messageHandler.OnEventMessage((RecEventMessage) rm)}
            };
            var ret = dic[MsgType](this);

            //处理消息后
            _messageHandler.OnAfterMessage(this, ret);

            return ret;
        }

    }
}
