using System;

namespace VoiceBotInterlocutors.Requests
{
    public class GenerateRequest
    {
        public Guid CampaignId { get; set; }
        public Guid GroupId { get; set; }
        public int InterlocutorsCount { get; set; }
    }
}