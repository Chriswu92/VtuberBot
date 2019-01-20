﻿using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using QQ.Framework;
using QQ.Framework.Domains;
using QQ.Framework.Utils;
using VtuberBot;
using VtuberBot.Database.Entities;
using VtuberBot.Robots;
using VtuberBot.Tools;

namespace BilibiliPlugin.Commands
{
    public class BilibiliLiveCommand : RobotCommandBase
    {
        private IMongoCollection<BiliBiliLiveInfo> _liveCollection;
        public override string[] Names { get; } = { "!blive", "！blive", "!b站直播", "！b站直播" };
        public BilibiliLiveCommand(ISendMessageService service) : base(service)
        {
            _liveCollection = Program.Database.GetCollection<BiliBiliLiveInfo>("bili-live-details");
        }

        [RobotCommand(offset: 1, subCommandName: "历史")]
        public void HistoryCommand(MessageInfo message, string[] args)
        {
            var vtuber = Config.DefaultConfig.GetVtuber(args[2]);
            if (vtuber == null)
            {
                _service.SendToGroup(message.GroupNumber, "数据库中不存在" + args[2]);
                return;
            }


        }
        public override void ShowHelpMessage(MessageInfo message, string[] args)
        {
            _service.SendToGroup(message.GroupNumber,"B站直播帮助:" +
                                                     "\r\n!B站直播 历史 <Vtuber>  -查询B站直播历史" +
                                                     "\r\n!B站直播 历史 <Vtuber> <序号>  -查询直播详情");
        }
    }
}