using Compent.Shared.Repository.Contract.Persistence;

namespace Uintra20.Persistence
{
    public abstract class SqlEntity<TKey>
    {
        public abstract TKey Id { get; set; }
    }
}