using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using NodaTime;
using RestSharp;
using VoiceBotInterlocutors.Models;

namespace VoiceBotInterlocutors.InterlocutorsGenerator
{
    public interface IInterlocutorsGenerator
    {
        Task Generate(string url, string accessToken, Guid campaignId, Guid groupId, int interlocutorsCount, bool haveConstSentences);
    }

    public class InterlocutorsGenerator : IInterlocutorsGenerator
    {
        private readonly Random _random = new Random();
        private readonly RestClient _restClient;

        public InterlocutorsGenerator()
        {
            _restClient = new RestClient();
        }

        public async Task Generate(string url, string accessToken, Guid campaignId, Guid groupId, int interlocutorsCount, bool haveConstSentences)
        {
            _restClient.BaseUrl = new Uri(url);

            var request = new RestRequest($"/api/campaigns/{campaignId}/interlocutors", Method.POST);
            var interlocutors = GetInterlocutors(groupId, interlocutorsCount, haveConstSentences);
            
            request.AddJsonBody(interlocutors);
            request.AddHeader("Authorization", $"Bearer {accessToken}");

            var response = await _restClient.ExecutePostAsync(request);

            await Console.Out.WriteLineAsync($"{response.StatusCode}, {response.ErrorMessage}");
        }

        private InterlocutorsDto GetInterlocutors(Guid groupId, int interlocutorsCount, bool haveConstSentences)
        {
            var interlocutors = new InterlocutorsDto
            {
                Interlocutors = new List<InterlocutorsDto.InterlocutorDto>(),
                GroupId = groupId
            };

            for (var i = 1; i <= interlocutorsCount; i++)
            {
                interlocutors.Interlocutors.Add(new InterlocutorsDto.InterlocutorDto
                {
                    PhoneNumber = RandomPhoneNumber(i),
                    Parameters = new Dictionary<string, string>
                    {
                        {"Termin Wizyty", haveConstSentences ? "02.04.2020" : RandomDate()},
                        {"Godzina Wizyty", haveConstSentences ? "13:30" : RandomTime()},
                        {"Numer Rejestracyjny", haveConstSentences ? "STA 00001": RandomRegistrationNumber(i)}
                    }
                });
            }

            return interlocutors;
        }

        private string RandomPhoneNumber(int count)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:000000000}", count);
        }

        private string RandomDate()
        {
            var newDate = LocalDate.FromYearMonthWeekAndDay(2020, _random.Next(1, 12), _random.Next(1, 5), (IsoDayOfWeek) _random.Next(1, 7))
                                   .ToString("dd.MM.yyyy", default);
            return newDate;
        }

        private string RandomTime()
        {
            var time = LocalTime.FromHourMinuteSecondTick(_random.Next(1, 23), _random.Next(1, 59), 0, 0)
                                .ToString("H:mm", default);
            return time;
        }
        
        private string RandomRegistrationNumber(int count)
        {
            return string.Format(CultureInfo.InvariantCulture,"STA" + "{0}", _random.Next(1000, 9000));
        }
    }
}