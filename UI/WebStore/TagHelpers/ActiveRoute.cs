using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebStore.TagHelpers;

[HtmlTargetElement(Attributes = AttributeName)]
public class ActiveRoute : TagHelper
{
    private const string AttributeName = "ws-is-active-route";
    private const string IgnoreAction = "ws-ignore-action";

    [HtmlAttributeName("asp-controller")] public string Controller { get; set; }

    [HtmlAttributeName("asp-action")] public string Action { get; set; }

    [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
    public Dictionary<string, string> RouteValues { get; set; } =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    [ViewContext, HtmlAttributeNotBound] public ViewContext ViewContext { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var result = output.Attributes.RemoveAll(AttributeName);
        var isActionIgnored = output.Attributes.RemoveAll(IgnoreAction);
        if (IsActive(isActionIgnored))
            MakeActive(output);
    }

    private bool IsActive(bool isActionIgnored)
    {
        var routeValues = ViewContext.RouteData.Values;
        var routeController = routeValues["Controller"]?.ToString();
        var routeAction = routeValues["Action"]?.ToString();

        if (!isActionIgnored && Action is {Length: > 0} action && !string.Equals(routeAction, action))
            return false;
        if (Controller is {Length: > 0} controller && !string.Equals(routeController, controller))
            return false;

        foreach (var (key, value) in RouteValues)
        {
            if (!routeValues.ContainsKey(key) || routeValues[key]?.ToString() != value)
                return false;
        }

        return true;
    }

    private static void MakeActive(TagHelperOutput output)
    {
        var classAttribute = output.Attributes.FirstOrDefault(attr => attr.Name == "class");
        if(classAttribute is null)
            output.Attributes.Add("class", "active");
        else
        {
            if (classAttribute.Value?.ToString()?.Equals("active") ?? false)
                return;
            output.Attributes.SetAttribute("class","active");
        }
    }
}