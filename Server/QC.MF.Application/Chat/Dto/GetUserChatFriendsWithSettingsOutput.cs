using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Castle.Components.DictionaryAdapter;
using QC.MF.Friendships.Dto;

namespace QC.MF.Chat.Dto
{
    public class GetUserChatFriendsWithSettingsOutput
    {
        public DateTime ServerTime { get; set; }

        public List<FriendDto> Friends { get; set; }

        public List<FriendDto> Blockes { get; set; }
    }
}
