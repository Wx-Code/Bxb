using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Models.Wechat
{
    public enum MessageType
    {
        Unknown = -1,
        Text = 0,
        Location = 1,
        Image = 2,
        Voice = 3,
        Video = 4,
        Link = 5,
        ShortVideo = 6,
        Event = 7,
        File = 8,
    }
}
