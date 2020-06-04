using System;
using System.Collections.Generic;

namespace VoiceBotInterlocutors.Models
{
    public class InterlocutorsDto
    {
        public List<InterlocutorDto> Interlocutors { get; set; }
        public Guid? GroupId { get; set; }

        public class InterlocutorDto
        {
            public string PhoneNumber { get; set; }
            public Dictionary<string, string> Parameters { get; set; }
        }
    }
}