namespace Uintra20.Persistence.Sql
{
    public abstract class SqlEntity<TKey>
    {
        public abstract TKey Id { get; set; }
    }
}