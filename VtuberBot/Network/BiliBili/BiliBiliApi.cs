﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using OfflineServer.Lib.Network;

namespace VtuberBot.Network.BiliBili
{
    public class BiliBiliApi
    {
        public static BiliBiliUser GetBiliBiliUser(long userId)
        {
            var client = new MyHttpClient();
            var json = JObject.Parse(client.Get($"https://api.bilibili.com/x/space/app/index?mid={userId}"));
            return json["data"]["info"].ToObject<BiliBiliUser>();
        }

        public static List<BiliBiliUser> SearchBiliBiliUsers(string keyword)
        {
            var client = new MyHttpClient();
            var json = JObject.Parse(client.Get($"https://app.bilibili.com/x/v2/search/type?&build=12080&highlight=1&keyword={Uri.EscapeDataString(keyword)}&order=totalrank&order_sort=1&type=2"));
            if (json["data"].HasValues)
            {
                var items = json["data"]["items"];
                var users = items.Select(v => new BiliBiliUser()
                {
                    Username = v["title"]?.ToString(),
                    Id = int.Parse(v["param"]?.ToString()),
                    Description = v["sign"]?.ToString(),
                    Follower = v["fans"]?.ToObject<int>() ?? 0,
                    IsUploader = v["is_up"]?.ToObject<bool>() ?? false
                });
                return users.ToList();
            }
            return new List<BiliBiliUser>();
        }

        public static List<BiliBiliDynamic> GetDynamicsByUser(long userId)
        {
            using (var client = new HttpClient())
            {
                var json = JObject.Parse(
                    client.GetStringAsync(
                            "https://api.vc.bilibili.com/dynamic_svr/v1/dynamic_svr/space_history?host_uid=" + userId)
                        .GetAwaiter().GetResult());
                if (json.Value<int>("code") != 0)
                    return new List<BiliBiliDynamic>();
                return json["data"]["cards"].ToArray().Select(v => v.ToObject<BiliBiliDynamic>()).ToList();
            }

        }


    }
}
