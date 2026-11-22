using G_NET_65_MVC02.DAL.Data.DBContexts;
using G_NET_65_MVC02.BLL.Services.Interfaces;
using G_NET_65_MVC02.BLL.ViewModels.PlanViewModels;
using G_NET_65_MVC02.DAL.Repositories.Classes;
using G_NET_65_MVC02.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace G_NET_65_MVC01.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;


        public PlanController(IPlanService PlanService)
        {
            _planService = PlanService;
        }


        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Plans = await _planService.GetAllPlansAsync(ct:ct);
            return View(Plans);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var Plan = await _planService.GetPlanDetailsAsync(id, ct:ct);
            if (Plan is null) { TempData["ErrorMessage"] = "Plan not found"; return RedirectToAction(nameof(Index)); }
            return View(Plan);
        }


        [HttpGet]
        public async Task<IActionResult> EditPlane(int id, CancellationToken ct)
        {
            var Plan = await _planService.GetPlanToUpdateAsync(id, ct:ct);
            if (Plan is null) { TempData["ErrorMessage"] = "Plan not found"; return RedirectToAction(nameof(Index)); }
            return View(Plan);
        }


        [HttpPost]
        public async Task<IActionResult> EditPlane(int id, PlanToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(EditPlane), model);
            var Result = await _planService.UpdatePlanDetailsAsync(id, model, ct);
            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Plan";
            }
            return View(model);
        }


        //private readonly GymDBContext context = new GymDBContext();
        //private readonly IPlanRepository _planRepository = new MockRepository();
        //private readonly IPlanRepository _planRepository;

        //public PlanController(IPlanRepository planRepository)
        //{
        //    _planRepository = planRepository;
        //}

        //public async Task<IActionResult> Index(CancellationToken ct)
        //{
        //    var Plans = await _planRepository.GetAllAsync(ct:ct);
        //    return View(Plans);
        //}

        //public async Task<IActionResult> Details(int id, CancellationToken ct)
        //{
        //    var Plan = await _planRepository.GetByIdAsync(id, ct);
        //    if (Plan is null)
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(Plan);
        //}
    }
}
