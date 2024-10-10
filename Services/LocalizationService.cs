using LocalizationWithJsonResource.Helpers;
using LocalizationWithJsonResource.Models.DTOs;
using LocalizationWithJsonResource.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

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
            var acceptLanguage = _httpContextAccessor.HttpContext.Request.Headers["Accept-Language"];
            var culture = acceptLanguage.Count > 0 ? acceptLanguage.ToString().Split('-').FirstOrDefault() : "";
            var localizationData = LoadLocalizationData(culture);
            return localizationData[key];
        }


        private Dictionary<string, string> LoadLocalizationData(string culture)
        {
            var d = TryGetLocalizationFromCache(culture) ?? TryLoadLocalizationFromFile(culture) ?? TryGetFallbackLocalization();
            return d;
        }

        private Dictionary<string, string> TryGetLocalizationFromCache(string culture)
        {
            var cacheKey = $"Localization_{culture}";
            return _memoryCacheHelper.GetFromCache<Dictionary<string, string>>(cacheKey);
        }

        private Dictionary<string, string> TryLoadLocalizationFromFile(string culture)
        {
            var filePath = GetFilePathForCulture(culture);
            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                var jsonObject = JObject.Parse(jsonData);
                var localizationData = jsonObject.Descendants()
                                                 .OfType<JProperty>()
                                                 .ToDictionary(p => p.Name, p => p.Value.ToString());
                _memoryCacheHelper.SetCache($"Localization_{culture}", localizationData, TimeSpan.FromMinutes(1));
                return localizationData;
            }
            return null;
        }

        private string GetFilePathForCulture(string culture)
        {
            return Path.Combine(_environment.ContentRootPath, "Resources", $"{culture}.json");
        }

        private Dictionary<string, string> TryGetFallbackLocalization()
        {
            var defaultCulture = _localizationOption.DefaultCulture;
            return TryGetLocalizationFromCache(defaultCulture) ?? TryLoadLocalizationFromFile(defaultCulture);
        }
    }
}
