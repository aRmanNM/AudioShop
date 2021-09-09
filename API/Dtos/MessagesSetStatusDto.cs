using System.Collections.Generic;

namespace API.Dtos
{
    public class MessagesSetStatusDto
    {
        public string UserId { get; set; }
        public List<int> MessageIdsForPushStatus { get; set; } = new List<int>();
        public List<int> MessageIdsForInAppStatus { get; set; } = new List<int>();
    }
}