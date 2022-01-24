namespace WebStore.Domain;

public class ProductFilter
{
    public int? SectionId { get; set; }
    public int? BrandId { get; set; }
    public int[]? IDs { get; set; }
}

