namespace WebStore.Domain.ViewModels;

public class SelectableSectionViewModel
{
    public IEnumerable<SectionViewModel> Sections { get; set; }
    public int? SectionId { get; set; }
    public int? ParentSectionId { get; set; }
}