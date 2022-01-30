using WebStore.Domain.Entities;

namespace WebStore.Domain.DTO;

public class SectionDto
{
    public int Id { get; set; }
    public int Order { get; set; }
    public string Name { get; set; }
    public int? ParentId { get; set; }
}
public static class sectionpper
{
    public static SectionDto? ToDto(this Section? section) => section is null 
        ? null
        : new SectionDto
        {
            Id = section.Id,
            Order = section.Order,
            Name = section.Name,
            ParentId = section.ParentId,
        };
    public static Section? FromDto(this SectionDto? section) => section is null 
        ? null
        : new Section
        {
            Id = section.Id,
            Order = section.Order,
            Name = section.Name,
            ParentId = section.ParentId,
        };

    public static IEnumerable<SectionDto?> ToDto(this IEnumerable<Section?> sections) => sections.Select(ToDto);
    public static IEnumerable<Section?> FromDto(this IEnumerable<SectionDto?> section) => section.Select(FromDto);
}