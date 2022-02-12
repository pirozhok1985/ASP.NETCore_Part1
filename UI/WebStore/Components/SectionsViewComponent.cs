using System.Collections;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Components;

public class SectionsViewComponent : ViewComponent
{
    private readonly IEnumerable<Section>? _Sections;

    public SectionsViewComponent(IProductData data)
    {
        _Sections = data.GetSections();
    }

    public IViewComponentResult Invoke(string sectionId)
    {
        var sectId = int.TryParse(sectionId, out var id) ? id : (int?)null;
        var sections = GetSections(sectId, out var parentSectionId);
        return View(new SelectableSectionViewModel
        {
            Sections = sections,
            SectionId = sectId,
            ParentSectionId = parentSectionId
        });
    }

    private IEnumerable<SectionViewModel> GetSections(int? sectionId, out int? parentSectionId)
    {
        parentSectionId = null;
        var parentSections = _Sections.Where(s => s.ParentId == null);
        var parentSectionsViews = parentSections.Select(s => new SectionViewModel
        {
            Id = s.Id,
            Name = s.Name,
            Order = s.Order
        }).ToList();

        foreach (var parentSectionView in parentSectionsViews)
        {
            var children = _Sections.Where(s => s.ParentId == parentSectionView.Id);
            foreach (var child in children)
            {
                if (child.Id == sectionId)
                    parentSectionId = (int) child.ParentId!;
                parentSectionView.ChildSections.Add(new SectionViewModel
                {
                    Id = child.Id,
                    Name = child.Name,
                    Order = child.Order,
                    ParentSection = parentSectionView
                });
            }
            parentSectionView.ChildSections.Sort((a,b) => Comparer.Default.Compare(a.Order,b.Order));
        }
        parentSectionsViews.Sort((a,b) => Comparer.Default.Compare(a.Order,b.Order));
        return parentSectionsViews;
    }
}

