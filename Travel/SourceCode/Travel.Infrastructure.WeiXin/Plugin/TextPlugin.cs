﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Travel.Infrastructure.WeiXin.Message;

namespace Travel.Common.WeiXin.Plugin
{
    /// <summary>
    /// 处理文本消息的插件基类。
    /// <para>继承自类不是只能处理文本消息的，此类只用于为文本消息提供一些通用的处理逻辑。</para>
    /// </summary>
    public class TextPlugin : Plugin
    {
        protected string Pattern;

        protected TextPlugin(string pattern)
        {
            Pattern = pattern;
        }

        public override bool CanProcess(PluginContext ctx)
        {
            var t = ctx.ReceiveMessage as RecTextMessage;
            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace;
            var notExist = string.IsNullOrEmpty(Pattern);
            return t != null && (notExist || Regex.IsMatch(t.Content, Pattern, options));
        }
    }
}
