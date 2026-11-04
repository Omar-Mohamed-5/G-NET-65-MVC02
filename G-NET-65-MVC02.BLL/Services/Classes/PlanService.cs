using G_NET_65_MVC02.BLL.Services.Interfaces;
using G_NET_65_MVC02.BLL.ViewModels.PlanViewModels;
using G_NET_65_MVC02.DAL.Models;
using G_NET_65_MVC02.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IGenericRepository<Plan> _planRepository;
        public PlanService(IGenericRepository<Plan> PlanRepository)
        {
            _planRepository = PlanRepository;
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default)
        {
            var Plans = await _planRepository.GetAllAsync(ct:ct);
            if (!Plans.Any()) return [];
            var PlansViewModel = Plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                DurationInDays = p.DurationDays,
                Description = p.Description,
                IsActive = p.IsActive
            });
            return PlansViewModel;
        }

        public async Task<PlanViewModel?> GetPlanDetailsAsync(int planId, CancellationToken ct = default)
        {
            var Plan = await _planRepository.GetByIdAsync(planId, ct: ct);
            if(Plan is null) return null;
            var PlanModel = new PlanViewModel
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Price = Plan.Price,
                DurationInDays = Plan.DurationDays,
                Description = Plan.Description,
                IsActive = Plan.IsActive
            };
            return PlanModel;
        }

        public async Task<PlanToUpdateViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var Plan = await _planRepository.GetByIdAsync(planId, ct:ct);
            if(Plan is null) return null;
            else
            {
                return new PlanToUpdateViewModel
                {
                    //Name = Plan.Name,
                    Price = Plan.Price,
                    DurationInDays = Plan.DurationDays,
                    Description = Plan.Description
                };
            }
        }

        public async Task<bool> UpdatePlanDetailsAsync(int planId, PlanToUpdateViewModel model, CancellationToken ct = default)
        {
            var Plan = _planRepository.GetByIdAsync(planId, ct:ct);
            if (Plan is null) return false;
            if(await _planRepository.AnyAsync(x => x.Name == model.Name && x.Id != planId)) return false;
            Plan.Result.Price = model.Price;
            Plan.Result.DurationDays = model.DurationInDays;
            Plan.Result.Description = model.Description;
            var Result = await _planRepository.UpdateAsync(Plan.Result);
            return Result > 0;
        }
    }
}
