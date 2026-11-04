using G_NET_65_MVC02.BLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlansAsync(CancellationToken ct = default);
        Task<PlanViewModel?> GetPlanDetailsAsync(int planId, CancellationToken ct = default);
        Task<PlanToUpdateViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default);
        Task<bool> UpdatePlanDetailsAsync(int planId, PlanToUpdateViewModel model, CancellationToken ct = default);
    }
}
