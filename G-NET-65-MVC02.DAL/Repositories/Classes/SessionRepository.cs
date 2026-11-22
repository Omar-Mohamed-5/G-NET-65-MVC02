using G_NET_65_MVC02.DAL.Data.DBContexts;
using G_NET_65_MVC02.DAL.Models;
using G_NET_65_MVC02.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDBContext _dBContext;

        public SessionRepository(GymDBContext DBContext) : base(DBContext)
        {
            _dBContext = DBContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(Expression<Func<Session, bool>>? predicate = null, CancellationToken ct = default)
        {
            IQueryable<Session> query = _dBContext.Sessions.AsNoTracking()
                .Include(s => s.Trainer).Include(s => s.Category);
            if(predicate is not null)
            {
                query = query.Where(predicate);
            }
            return await query.ToListAsync(ct);
        }

        public Task<int> GetCountOfBookedSlotsAsync(int SessionId, CancellationToken ct = default)
        => _dBContext.Bookings.AsNoTracking().CountAsync(b => b.SessionId == SessionId, ct);

        public Task<Session?> GetSessionWithTrainerAndCategoryAsync(int SessionId, CancellationToken ct = default)
        => _dBContext.Sessions.AsNoTracking().Include(s => s.Trainer).Include(s => s.Category)
            .FirstOrDefaultAsync(s => s.Id == SessionId, ct);
    }
}
