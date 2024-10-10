using LocalizationWithJsonResource.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalizationWithJsonResource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizationController : ControllerBase
    {
        private readonly ILocalizationService _localizationService;

        public LocalizationController(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        [HttpPost("GetMessage")]
        public IActionResult GetMessage()
        {
            var welcomeMessage = _localizationService.GetLocalizedValue("Messages.Welcome");
            return Ok(welcomeMessage);
        }

    }
}
