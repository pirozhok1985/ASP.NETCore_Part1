using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Components;

public class SectionsViewComponent : ViewComponent
{
    private readonly IEnumerable<Section> _Sections;

    public SectionsViewComponent(IProductData sections)
    {
        _Sections = sections.GetSections();
    }
    public IViewComponentResult Invoke() => View();
}

