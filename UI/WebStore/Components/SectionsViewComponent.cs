using System.Collections;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components;

public class SectionsViewComponent : ViewComponent
{
    private readonly IEnumerable<Section> _Sections;

    public SectionsViewComponent(IProductData data)
    {
        _Sections = data.GetSections();
    }

    public IViewComponentResult Invoke()
    {
        var parent_sections = _Sections.Where(s => s.ParentId == null);
        var parent_sections_views = parent_sections.Select(s => new SectionViewModel
        {
            Id = s.Id,
            Name = s.Name,
            Order = s.Order
        }).ToList();

        foreach (var parent_section_view in parent_sections_views)
        {
            var children = _Sections.Where(s => s.ParentId == parent_section_view.Id);
            foreach (var child in children)
            {
                parent_section_view.ChildSections.Add(new SectionViewModel
                {
                    Id = child.Id,
                    Name = child.Name,
                    Order = child.Order,
                    ParentSection = parent_section_view
                });
            }
            parent_section_view.ChildSections.Sort((a,b) => Comparer.Default.Compare(a.Order,b.Order));
        }
        parent_sections_views.Sort((a,b) => Comparer.Default.Compare(a.Order,b.Order));
        return View(parent_sections_views);
    }
}

