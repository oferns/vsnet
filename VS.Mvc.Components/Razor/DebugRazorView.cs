
namespace VS.Mvc.Components.Razor {


    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    public class DebugRazorView : RazorView {
        private readonly string sourceAssembly;

        public DebugRazorView(string sourceAssembly, IRazorViewEngine viewEngine, IRazorPageActivator pageActivator, IReadOnlyList<IRazorPage> viewStartPages, IRazorPage razorPage, HtmlEncoder htmlEncoder, DiagnosticListener diagnosticListener)
            : base(viewEngine, pageActivator, viewStartPages, razorPage, htmlEncoder, diagnosticListener) {
            this.sourceAssembly = sourceAssembly;
        }

        public override async Task RenderAsync(ViewContext context) {
            context.Writer.WriteLine($"{context.Writer.NewLine}<!-- in {sourceAssembly}:{context.View.Path} -->{context.Writer.NewLine}");
            await base.RenderAsync(context);
            context.Writer.WriteLine($"{context.Writer.NewLine}<!-- out {sourceAssembly}:{context.View.Path} -->{context.Writer.NewLine}");
            
        }

    }
}
