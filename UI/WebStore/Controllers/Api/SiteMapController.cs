using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers.Api;

public class SiteMapController : ControllerBase
{
    public IActionResult Index([FromServices]IProductData productData)
    {
        var nodes = new List<SitemapNode>
        {
            new SitemapNode(Url.Action("Index","Home")),
            new SitemapNode(Url.Action("Index","Shop")),
            new SitemapNode(Url.Action("Index","WebApi")),
            new SitemapNode(Url.Action("Index","Blogs")),
            new SitemapNode(Url.Action("Blog","Blogs")),
        };
        nodes.AddRange(productData.GetSections()!
            .Select(s => new SitemapNode(Url.Action("Index","Shop",new {SectionId = s.Id}))));
        nodes.AddRange(productData.GetBrands()!
            .Select(b => new SitemapNode(Url.Action("Index","Shop", new {BrandId = b.Id}))));
        nodes.AddRange(productData.GetProducts()!
            .Select(p => new SitemapNode(Url.Action("Details","Shop", new {Id = p!.Id}))));
        return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
    }
}