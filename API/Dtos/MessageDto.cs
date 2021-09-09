using System;
using API.Helpers;

namespace API.Dtos
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Link { get; set; }
        public int CourseId { get; set; }
        public string UserId { get; set; }
        public int RepeatAfterHour { get; set; }
        public bool IsRepeatable { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool SendSMS { get; set; }
        public bool SendPush { get; set; }
        public bool SendInApp { get; set; }
        public MessageType MessageType { get; set; }
        public bool InAppSeen { get; set; }
        public bool SMSSent { get; set; }
        public bool PushSent { get; set; }
    }
}