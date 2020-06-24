using System;

namespace VoiceBotInterlocutors.Requests
{
    public class GenerateRequest
    {
        public string Url { get; set; }
        public string AccessToken { get; set; }
        public Guid CampaignId { get; set; }
        public Guid GroupId { get; set; }
        public int InterlocutorsCount { get; set; }
        public bool HaveConstSentences { get; set; }
    }
}