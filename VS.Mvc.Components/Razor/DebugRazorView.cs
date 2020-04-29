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

        public override Task RenderAsync(ViewContext context) {            
           
            context.Writer.WriteLine($"<!-- in {sourceAssembly}:{context.View.Path} -->");            
            return base.RenderAsync(context).ContinueWith((r) => {
                context.Writer.WriteLine($"<!-- out {sourceAssembly}:{context.View.Path} -->");
            });
        }       
    }
}