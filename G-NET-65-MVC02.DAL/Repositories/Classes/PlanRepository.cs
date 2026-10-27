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
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDBContext _context;

        public PlanRepository(GymDBContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Plan plan)
        {
            _context.Plans.Add(plan);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Plan plan)
        {
            _context.Plans.Remove(plan);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<Plan> query = tracking? _context.Plans : _context.Plans.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Plans.FindAsync(id, ct);
        }

        public async Task<int> UpdateAsync(Plan plan)
        {
            _context.Plans.Update(plan);
            return await _context.SaveChangesAsync();
        }
    }
}
