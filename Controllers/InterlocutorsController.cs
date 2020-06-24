using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoiceBotInterlocutors.InterlocutorsGenerator;
using VoiceBotInterlocutors.Requests;

namespace VoiceBotInterlocutors.Controllers
{
    [Route("api/interlocutors")]
    [ApiController]
    [AllowAnonymous]
    public class InterlocutorsController : ControllerBase
    {
        private readonly IInterlocutorsGenerator _interlocutorsGenerator;

        public InterlocutorsController(IInterlocutorsGenerator interlocutorsGenerator)
        {
            _interlocutorsGenerator = interlocutorsGenerator;
        }

        [HttpPost]
        public Task Generate([FromBody] GenerateRequest request)
        {
            return _interlocutorsGenerator.Generate(request.Url, 
                                                    request.AccessToken, 
                                                    request.CampaignId, 
                                                    request.GroupId, 
                                                    request.InterlocutorsCount, 
                                                    request.HaveConstSentences);
        }
    }
}