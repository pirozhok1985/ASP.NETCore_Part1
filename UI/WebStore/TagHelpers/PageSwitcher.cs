using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebStore.Domain.ViewModels;

namespace WebStore.TagHelpers;


public class PageSwitcher : TagHelper
{
    private readonly IUrlHelperFactory _urlHelperFactory;
    
    public PageViewModel? PageViewModel { get; set; }
    public string? PageAction { get; set; }
    
    
    public PageSwitcher(IUrlHelperFactory urlHelperFactory) => _urlHelperFactory = urlHelperFactory;
    
    
    [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
    public Dictionary<string, object?> PageUrlValues { get; set; } = new (StringComparer.OrdinalIgnoreCase);
    
    [ViewContext, HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }
    
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var ul = new TagBuilder("ul");
        ul.AddCssClass("pagination");
        var urlHeler = _urlHelperFactory.GetUrlHelper(ViewContext);
        for (var i = 1; i <= PageViewModel?.TotalPages; i++)
        {
            ul.InnerHtml.AppendHtml(CreateElement(i,urlHeler));
        }

        output.Content.AppendHtml(ul);
    }

    private TagBuilder CreateElement(int pageNumber, IUrlHelper urlHeler)
    {
        var li = new TagBuilder("li");
        var a = new TagBuilder("a");
        a.InnerHtml.AppendHtml(pageNumber.ToString());
        if(PageViewModel?.Page == pageNumber)
            li.AddCssClass("active");
        else
        {
            // PageUrlValues["page"] = pageNumber;
            // a.Attributes["href"] = urlHeler.Action(PageAction, PageUrlValues);
            a.Attributes["href"] = "#";
        }

        PageUrlValues["Page"] = pageNumber;
        foreach (var (key,value) in PageUrlValues.Select(p => (p.Key,Value:p.Value?.ToString()))
                     .Where(p => p.Value! is {Length: > 0}))
        {
            a.MergeAttribute($"data-{key}", value);
        }

        li.InnerHtml.AppendHtml(a);
        return li;
    }
}