namespace VS.Mvc._Extensions {
    using Microsoft.AspNetCore.StaticFiles;

    public class WebManifestTypeContentProvider : FileExtensionContentTypeProvider {
        public WebManifestTypeContentProvider() {
            Mappings.Add(".webmanifest", "application/manifest+json");
        }
    }
}