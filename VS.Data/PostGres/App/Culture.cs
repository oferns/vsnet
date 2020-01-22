namespace VS.Data.PostGres.App {
    using System.ComponentModel.DataAnnotations;

    public partial class Culture {

        [Required(ErrorMessage = "You must supply a valid unique culture code.")]
        [Range(1, 24, ErrorMessage = "Maximum length is 24 characters.")]
        public string Code { get; set; }

        [Required(ErrorMessage = "You must supply a valid unique culture code.")]
        [Range(1, 24, ErrorMessage = "Maximum length is 24 characters.")]
        public string SpecificCulture { get; set; }

        [Required(ErrorMessage = "You must supply an english name for the culture.")]
        [Range(2, 255, ErrorMessage = "Maximum length is 255 characters.")]
        public string EnglishName { get; set; }

        [Required(ErrorMessage = "You must supply a local name for the culture.")]
        [Range(2, 255, ErrorMessage = "Maximum length is 255 characters.")]
        public string LocalName { get; set; }

    }
}