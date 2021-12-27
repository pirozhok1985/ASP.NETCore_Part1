using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities.Base;

public class Entity : IEntity
{
    public int Id { get; set; }
}