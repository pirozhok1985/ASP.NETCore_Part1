using WebStore.Domain.Entities;

namespace WebStore.Domain.ViewModels;

public class PageViewModel
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public int TotalCount { get; set; }
}