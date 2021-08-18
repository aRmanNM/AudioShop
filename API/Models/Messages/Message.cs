using System;
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
        public int ClockRangeBegin { get; set; }
        public int ClockRangeEnd { get; set; }
        public bool IsRepeatable { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool SendSMS { get; set; }
        public bool SendPush { get; set; }
        public MessageType MessageType { get; set; }
    }
}