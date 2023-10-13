using System;

namespace ChikoRokoBot.AntiBotNotify.Models
{
	public class User
	{
        public DateTimeOffset? Timestamp { get; set; }
        public long? ChatId { get; set; }
        public int? TopicId { get; set; }
    }
}

