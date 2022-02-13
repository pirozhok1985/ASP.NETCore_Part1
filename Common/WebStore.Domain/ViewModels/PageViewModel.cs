using WebStore.Domain.Entities;

namespace WebStore.Domain.ViewModels;

public class PageViewModel
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => TotalCount == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
}