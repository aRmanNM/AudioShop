namespace API.Models.Messages
{
    public class UserMessage
    {
        public int MessageId { get; set; }
        public Message Message { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool IsSeen { get; set; }
    }
}