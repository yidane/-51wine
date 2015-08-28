﻿using Newtonsoft.Json;

namespace Travel.Infrastructure.WeiXin.Extra
{
    /// 粉丝分组
    /// </summary>
    public class WxGroup
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 组下用户数
        /// </summary>
        [JsonProperty(PropertyName = "cnt")]
        public int Cnt { get; set; }
    }
}
