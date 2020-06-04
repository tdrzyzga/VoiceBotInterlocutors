using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using NodaTime;
using RestSharp;
using RestSharp.Authenticators;
using VoiceBotInterlocutors.Models;

namespace VoiceBotInterlocutors.InterlocutorsGenerator
{
    public interface IInterlocutorsGenerator
    {
        Task Generate(Guid campaignId, Guid groupId, int interlocutorsCount);
    }
    
    public class InterlocutorsGenerator : IInterlocutorsGenerator
    {
        private readonly RestClient _restClient;
        private readonly Random _random = new Random();
        public InterlocutorsGenerator(GeneratorConfiguration configuration)
        {
            var uri = UriHelper.BuildAbsolute(Uri.UriSchemeHttp, new HostString(configuration.Host, configuration.Port));

            _restClient = new RestClient(uri);

            var request = new RestRequest($"/connect/token", Method.POST);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("username", configuration.User);
            request.AddParameter("password", configuration.Password);
            request.AddParameter("grant_type", "password");
            request.AddParameter("client_id", "swagger");

            var response = _restClient.Execute<Credentials>(request);
            _restClient.AddDefaultHeader("Authorization", $"Bearer {response.Data.Access_Token}");
        }

        public async Task Generate(Guid campaignId, Guid groupId, int interlocutorsCount)
        {
            var request = new RestRequest($"/api/campaigns/{campaignId}/interlocutors", Method.POST);

            var interlocutors = GetInterlocutors(groupId, interlocutorsCount);

            request.AddJsonBody(interlocutors);

            var response = await _restClient.ExecutePostAsync(request);

            await Console.Out.WriteLineAsync($"{response.StatusCode}, {response.ErrorMessage}");
        }

        private InterlocutorsDto GetInterlocutors(Guid groupId, int interlocutorsCount)
        {
            var interlocutors = new InterlocutorsDto
            {
                Interlocutors = new List<InterlocutorsDto.InterlocutorDto>(),
                GroupId = groupId
            };

            for (var i = 1; i <= interlocutorsCount; i++)
            {
                if (i < 10)
                {
                    interlocutors.Interlocutors.Add(new InterlocutorsDto.InterlocutorDto
                    {
                        PhoneNumber = $"00000000{i}",
                        Parameters = new Dictionary<string, string>
                        {
                            {"Termin Wizyty", RandomDate()},
                            {"Godzina Wizyty", RandomTime()},
                            {"Numer Rejestracyjny", $"STA 0000{i}"}
                        }
                    });
                }
                else if (i < 100)
                {
                    interlocutors.Interlocutors.Add(new InterlocutorsDto.InterlocutorDto
                    {
                        PhoneNumber = $"0000000{i}",
                        Parameters = new Dictionary<string, string>
                        {
                            {"Termin Wizyty", RandomDate()},
                            {"Godzina Wizyty", RandomTime()},
                            {"Numer Rejestracyjny", $"STA 000{i}"}
                        }
                    });
                }
                else if (i < 1000)
                {
                    interlocutors.Interlocutors.Add(new InterlocutorsDto.InterlocutorDto
                    {
                        PhoneNumber = $"000000{i}",
                        Parameters = new Dictionary<string, string>
                        {
                            {"Termin Wizyty", RandomDate()},
                            {"Godzina Wizyty", RandomTime()},
                            {"Numer Rejestracyjny", $"STA 00{i}"}
                        }
                    });
                }
            }

            return interlocutors;
        }

        private string RandomDate()
        {
            var newDate = LocalDate.FromYearMonthWeekAndDay(2020, _random.Next(1, 12), _random.Next(1, 5), (IsoDayOfWeek) _random.Next(1, 7)).ToString("dd.MM.yyyy", default);
            return newDate;
        }

        private string RandomTime()
        {
            var time = LocalTime.FromHourMinuteSecondTick(_random.Next(1, 23), _random.Next(1, 59), 0, 0).ToString("H:mm", default);
            return time;
        }
    }

}