using System;

namespace ChikoRokoBot.AntiBotNotify.Models
{
    public class TargetSiteState
    {
        public DateTimeOffset? Timestamp { get; set; }
        public bool AntiBotEnabled { get; set; }
        public string Url { get; set; }
    }
}

