namespace VS.Mvc._Extensions {
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Razor;

    public abstract class DebugPage<T> : RazorPage<T> {
        protected DebugPage() {
        }


        public override void BeginContext(int position, int length, bool isLiteral) {
            base.BeginContext(position, length, isLiteral);
        }
       

        protected override IHtmlContent RenderBody() {

            return new HtmlContentBuilder()
                .AppendHtml("<!-- This is a start comment -->")
                .AppendHtml(base.RenderBody())
                .AppendHtml("<!-- This is a end comment -->");

        }
    }
}

