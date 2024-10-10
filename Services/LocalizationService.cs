using LocalizationWithJsonResource.Helpers;
using LocalizationWithJsonResource.Models.DTOs;
using LocalizationWithJsonResource.Services.Interfaces;
using Newtonsoft.Json.Linq;

namespace LocalizationWithJsonResource.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LocalizationOptionDTO _localizationOption;
        private readonly MemoryCacheHelper _memoryCacheHelper;

        public LocalizationService(IHostEnvironment environment, IHttpContextAccessor httpContextAccessor, LocalizationOptionDTO localizationOption, MemoryCacheHelper memoryCacheHelper)
        {
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
            _localizationOption = localizationOption;
            _memoryCacheHelper = memoryCacheHelper;
        }

        public string GetLocalizedValue(string key)
        {
            var culture = "";
            if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers["Accept-Language"])) {
                culture = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString();
            }

            var supportedCulture = _localizationOption.SupportedCultures.Find(k => k.Equals(culture));
            if (supportedCulture == null) {
                var cultureShort = culture.Split('-').FirstOrDefault();
                supportedCulture = _localizationOption.SupportedCultures.Find(k => k.Equals(cultureShort));
                if (supportedCulture != null)
                {
                    culture = cultureShort;
                }
                else
                {
                    culture = _localizationOption.DefaultCulture;
                }
            }
            var cacheKey = $"Localization_{culture}";

            var localizationData = _memoryCacheHelper.GetOrCreate(cacheKey, () => LoadLocalizationData(culture), TimeSpan.FromHours(1));
            return localizationData[key];
        }

        private Dictionary<string, string> LoadLocalizationData(string culture)
        {
            var filePath = Path.Combine(_environment.ContentRootPath, "Resources", $"{culture}.json");

            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(_environment.ContentRootPath, "Resources", $"{_localizationOption.DefaultCulture}.json");
            }

            var jsonData = File.ReadAllText(filePath);
            var jsonObject = JObject.Parse(jsonData);

            return jsonObject.Descendants()
                             .OfType<JProperty>()
                             .ToDictionary(p => p.Name.ToString(), p => p.Value.ToString());
        }


    }
}
