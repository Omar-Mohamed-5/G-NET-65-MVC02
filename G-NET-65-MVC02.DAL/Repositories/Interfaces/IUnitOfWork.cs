using G_NET_65_MVC02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : BaseEntity, new();
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        object GetRepository<T>();

        public ISessionRepository _SessionRepository { get; }
    }
}
