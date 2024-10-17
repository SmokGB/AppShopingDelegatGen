
using AppShoping.Entities;

namespace AppShoping.Repositories
{
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
        where T : class, IEntity
    {

    }
}
