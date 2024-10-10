namespace LocalizationWithJsonResource.Models.DTOs
{
    public class LocalizationOptionDTO
    {
        public List<string> SupportedCultures { get; set; }
        public string DefaultCulture { get; set; }
    }
}
