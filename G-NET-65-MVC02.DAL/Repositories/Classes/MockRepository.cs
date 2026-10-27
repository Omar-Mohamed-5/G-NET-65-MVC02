using G_NET_65_MVC02.DAL.Models;
using G_NET_65_MVC02.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.DAL.Repositories.Classes
{
    public class MockRepository : IPlanRepository
    {
        public Task<int> AddAsync(Plan plan)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Plan plan)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            List<Plan> plans = new List<Plan>()
            {
                new Plan() {Name = "Test"}
            };
            return await Task.FromResult(plans.AsEnumerable());
        }

        public Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Plan plan)
        {
            throw new NotImplementedException();
        }
    }
}
