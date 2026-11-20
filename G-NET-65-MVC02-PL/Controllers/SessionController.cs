using G_NET_65_MVC02.BLL.Services.Interfaces;
using G_NET_65_MVC02.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace G_NET_65_MVC02_PL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService SessionService)
        {
            _sessionService = SessionService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            return View(await _sessionService.GetAllSessionsAsync(ct));
        }


        [HttpGet]
        public async Task<IActionResult> Details(int Id, CancellationToken ct)
        {
            var Result = await _sessionService.GetSessionByIdAsync(Id, ct);
            if (Result is not null) return View(Result);
            else
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
        }


        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropdownAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                await PopulateDropdownAsync(ct);
                return View(model);
            }
            var Result = await _sessionService.CreateSessionAsync(model, ct);
            if(Result.Success)
            {
                TempData["SuccesMessage"] = "Session Created Succefully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = Result.Error;
                await PopulateDropdownAsync(ct);
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var Result = await _sessionService.GetSessionToUpdateAsync(id, ct);
            if(Result is not null)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(ct), "Id", "Name");
                return View(Result);
            }
            else
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(ct), "Id", "Name");
                return View(model);
            }
            var Result = await _sessionService.UpdateSessionAsync(id, model, ct);
            if (Result.Success)
            {
                TempData["SuccesMessage"] = "Session Updated Succefully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = Result.Error;
                ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(ct), "Id", "Name");
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var Result = await _sessionService.GetSessionByIdAsync(id, ct);
            if (Result is not null)
            {
                return View(Result);
            }
            else
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var Result = await _sessionService.DeleteSessionAsync(id, ct);
            if(Result is not null)
            { 
                TempData["SuccessMessage"] = "Session Deleted Successfully"; 
            }
            else
            { 
                TempData["ErrorMessage"] = "Session Delete Failed"; 
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownAsync(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(ct),"Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesForDropDownAsync(ct), "Id", "CategoryName");
        }
    }
}
    