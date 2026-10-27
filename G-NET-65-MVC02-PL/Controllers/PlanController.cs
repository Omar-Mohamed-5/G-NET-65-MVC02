using G_NET_65_MVC01.DBContexts;
using G_NET_65_MVC02.DAL.Repositories.Classes;
using G_NET_65_MVC02.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace G_NET_65_MVC01.Controllers
{
    public class PlanController : Controller
    {
        //private readonly GymDBContext context = new GymDBContext();
        //private readonly IPlanRepository _planRepository = new MockRepository();

        private readonly IPlanRepository _planRepository;
        public PlanController(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Plans = await _planRepository.GetAllAsync(ct:ct);
            return View(Plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var Plan = await _planRepository.GetByIdAsync(id, ct);
            if (Plan is null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }
    }
}
