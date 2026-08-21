using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ITI_GRADUATION.TagHelpers
{
    // <avatar name="..." image="..." size="lg"></avatar>
    // Renders the employee's uploaded photo when available, otherwise falls back
    // to a circular badge with their initials in a deterministic color derived
    // from their name, so every employee without a photo still looks distinct.
    [HtmlTargetElement("avatar")]
    public class AvatarTagHelper : TagHelper
    {
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
        public string Size { get; set; } = "md"; // md | lg

        private static readonly string[] Palette =
        {
            "#10B981", "#4F46E5", "#F59E0B", "#EF4444", "#0EA5E9",
            "#8B5CF6", "#EC4899", "#14B8A6", "#F97316", "#6366F1"
        };

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var sizeClass = Size == "lg" ? "nx-avatar nx-avatar-lg" : "nx-avatar";

            if (!string.IsNullOrWhiteSpace(Image))
            {
                output.TagName = "img";
                output.Attributes.SetAttribute("src", Image);
                output.Attributes.SetAttribute("alt", Name);
                output.Attributes.SetAttribute("class", sizeClass);
                output.TagMode = TagMode.SelfClosing;
                return;
            }

            var initials = GetInitials(Name);
            var color = GetColorFor(Name);

            output.TagName = "div";
            output.Attributes.SetAttribute("class", sizeClass);
            output.Attributes.SetAttribute("style", $"background-color:{color};");
            output.Content.SetContent(initials);
            output.TagMode = TagMode.StartTagAndEndTag;
        }

        private static string GetInitials(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "?";
            var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return parts[0][0].ToString().ToUpperInvariant();
            return $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant();
        }

        private static string GetColorFor(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Palette[0];
            var hash = 0;
            foreach (var c in name) hash = (hash * 31 + c) & 0x7FFFFFFF;
            return Palette[hash % Palette.Length];
        }
    }
}
