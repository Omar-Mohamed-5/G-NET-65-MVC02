using G_NET_65_MVC02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? predicate = null, CancellationToken ct = default);
        Task<Session?> GetSessionWithTrainerAndCategoryAsync(int SessionId, CancellationToken ct = default);
        Task<int> GetCountOfBookedSlotsAsync(int SessionId, CancellationToken ct = default);
    }
}
