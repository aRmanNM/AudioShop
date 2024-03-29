using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using API.Helpers;

namespace API.Models.Messages
{
    public class Message
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public int CourseId { get; set; }
        public string UserId { get; set; }
        public int RepeatAfterHour { get; set; }
        public bool IsRepeatable { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool SendSMS { get; set; }
        public bool SendPush { get; set; }
        public bool SendInApp { get; set; }
        public MessageType MessageType { get; set; }
        public ICollection<UserMessage> UserMessages { get; set; }

        public Message()
        {
            UserMessages = new Collection<UserMessage>();
        }
    }
}