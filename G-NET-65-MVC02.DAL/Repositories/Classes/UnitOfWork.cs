using G_NET_65_MVC01.DBContexts;
using G_NET_65_MVC02.DAL.Models;
using G_NET_65_MVC02.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];
        private readonly GymDBContext _dBContext;
        private readonly ISessionRepository sessionRepository;

        public UnitOfWork(GymDBContext DBContext, ISessionRepository sessionRepository)
        {
            _dBContext = DBContext;
            _SessionRepository = sessionRepository;
        }

        public ISessionRepository _SessionRepository { get; }

        public IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name;
            if (_repositories.TryGetValue(TypeName, out object? value))
            {
                return (IGenericRepository<TEntity>)value;
            }
            var Repo = new GenericRepository<TEntity>(_dBContext);
            _repositories[TypeName] = Repo;
            return Repo;
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
            => _dBContext.SaveChangesAsync(ct);
    }
}