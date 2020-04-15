namespace VS.Mvc._TagHelpers {

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System.ComponentModel;

    [HtmlTargetElement("select", Attributes = ForAttributeName)]
    [HtmlTargetElement("input", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    [HtmlTargetElement("textarea", Attributes = ForAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class ToolTipTagHelper : TagHelper {


        private const string ForAttributeName = "asp-for";

        /// <summary>
        /// An expression to be evaluated against the current model.
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (context is null) {
                throw new System.ArgumentNullException(nameof(context));
            }

            if (output is null) {
                throw new System.ArgumentNullException(nameof(output));
            }

            if (For != null) {
                var attributes = For.Metadata
                                       .ContainerType
                                       .GetProperty(For.Metadata.PropertyName)
                                       .GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0) {
                    var descatt = attributes[0] as DescriptionAttribute;
                    var icontag = new TagBuilder("i") { TagRenderMode = TagRenderMode.Normal };

                    icontag.Attributes.Add("tabindex", "-1");
                    icontag.Attributes.Add("aria-hidden", "true");
                    icontag.Attributes.Add("role", "tooltip");
                    icontag.AddCssClass("tooltip");

                    var tooltipspan = new TagBuilder("span") {
                        TagRenderMode = TagRenderMode.Normal
                    };
                    output.TagMode = output.TagMode.Equals(TagMode.StartTagOnly) ? TagMode.StartTagAndEndTag : output.TagMode;
                    tooltipspan.InnerHtml.Append(descatt.Description);
                    tooltipspan.AddCssClass("tooltiptext");
                    icontag.InnerHtml.AppendHtml(tooltipspan);
                    output.PostElement.AppendHtml(icontag);

                }
            }
        }
    }
}
