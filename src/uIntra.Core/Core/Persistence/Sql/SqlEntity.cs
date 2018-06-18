namespace Uintra.Core.Persistence
{
    public abstract class SqlEntity<TKey>
    {
       public abstract TKey Id { get; set; }
    }
}