namespace VS.Mvc.Components.Razor {
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Razor;
    using System;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class DebugRazorPage<T> : RazorPage<T> {


        private readonly string assemblyName;

        protected DebugRazorPage() {
            this.assemblyName = this.GetType().Assembly.GetName().Name;
        }


        public new HtmlString RenderSection(string name) {
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }
            return RenderSection(name, required: true);
        }

        public new  HtmlString RenderSection(string name, bool required) {
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }

            EnsureMethodCanBeInvoked(nameof(RenderSection));

            var task = RenderSectionAsync(name, required);
            return task.GetAwaiter().GetResult();
        }

        public new Task<HtmlString> RenderSectionAsync(string name) {
            if (name == null) {
                throw new ArgumentNullException(nameof(name));
            }

            return RenderSectionAsync(name, required: true);
        }

        public new Task<HtmlString> RenderSectionAsync(string name, bool required) {
            var viewBeingRendered = (RazorView)this.ViewContext.View;

            this.WriteLiteral($"\n<!-- {assemblyName}:{this.Path} in {name} -->\n");
            this.WriteLiteral($"\n<!-- {viewBeingRendered.RazorPage.GetType().Assembly.GetName().Name}:{viewBeingRendered.Path}-->\n");

            return base.RenderSectionAsync(name, required).ContinueWith((r) => {
                this.WriteLiteral($"\n<!-- {viewBeingRendered.RazorPage.GetType().Assembly.GetName().Name}:{viewBeingRendered.Path}-->\n");
                this.WriteLiteral($"\n<!-- {assemblyName}:{this.Path} out {name} -->\n");
                return r.Result;
            });
            
        }


        private void EnsureMethodCanBeInvoked(string methodName) {
            if (PreviousSectionWriters == null) {
                throw new InvalidOperationException($"{methodName} cannot be called because PreviousSectionWriters is null. Whatever the f*ck that means.");
            }
        }

        public string ViewPath => ViewContext.ExecutingFilePath;        

        protected override IHtmlContent RenderBody() {

            return new HtmlContentBuilder()
                .AppendHtml($"\n<!-- {assemblyName}:{this.Path} {this.ViewContext.View.Path} Body -->\n")
                .AppendHtml(base.RenderBody())
                .AppendHtml($"\n<!-- {assemblyName}:{this.Path} {this.ViewContext.View.Path} End Body -->\n");

        }
    }
}

