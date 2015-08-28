﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jurassic.Library;
using Newtonsoft.Json;

namespace Travel.Infrastructure.WeiXin.Extra
{
    public class ResultPerPage
    {
        [JsonProperty(PropertyName = "pageIdx")]
        public int PageIndex { get; set; }

        [JsonProperty(PropertyName = "pageCount")]
        public int PageCount { get; set; }

        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }

        [JsonProperty(PropertyName = "groupsList")]
        public List<WxGroup> GroupsList { get; set; }

        [JsonProperty(PropertyName = "friendsList")]
        public List<WxUser> FriendsList { get; set; }

        [JsonProperty(PropertyName = "currentGroupId")]
        public int CurrentGroupId { get; set; }

        public static ResultPerPage FromObjectInstance(Jurassic.Library.ObjectInstance instance)
        {
            /*为了简单，省去1万地反射...*/
            var ret = new ResultPerPage();
            foreach (var pi in ret.GetType().GetProperties())
            {
                try
                {
                    var value = instance.GetPropertyValue(JsonPropertyName(pi));
                    if (pi.Name == "GroupsList")
                    {
                        var ls = value as ArrayInstance;
                        if (ls != null)
                        {
                            var wxGroups = (from ObjectInstance o in ls.ElementValues select FromObject<WxGroup>(o)).ToList();
                            pi.SetValue(ret, wxGroups, null);
                        }
                    }
                    else if (pi.Name == "FriendsList")
                    {
                        var ls = value as ArrayInstance;
                        if (ls != null)
                        {
                            var wxGroups = (from ObjectInstance o in ls.ElementValues select FromObject<WxUser>(o)).ToList();
                            pi.SetValue(ret, wxGroups, null);
                        }
                    }
                    else
                    {
                        pi.SetValue(ret, Convert.ChangeType(value, pi.PropertyType), null);
                    }
                }
                catch { }
            }
            return ret;
        }

        public static T FromObject<T>(Jurassic.Library.ObjectInstance instance) where T : new()
        {
            if (instance == null)
                return default(T);

            var ret = new T();
            foreach (var pi in ret.GetType().GetProperties())
            {
                try
                {
                    var value = instance.GetPropertyValue(JsonPropertyName(pi));
                    if (pi.PropertyType.IsPrimitive || pi.PropertyType == typeof(string))
                    {
                        pi.SetValue(ret, Convert.ChangeType(value, pi.PropertyType), null);
                    }
                }
                catch { }
            }
            return ret;
        }

        public static string JsonPropertyName(PropertyInfo pi)
        {
            var a = pi.GetCustomAttributes(typeof(JsonPropertyAttribute), true).FirstOrDefault() as JsonPropertyAttribute;
            return a == null ? pi.Name : a.PropertyName;
        }
    }
}
