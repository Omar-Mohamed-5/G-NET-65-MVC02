using G_NET_65_MVC02.BLL.Services.Interfaces;
using G_NET_65_MVC02.BLL.ViewModels.MemberViewModels;
using G_NET_65_MVC02.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace G_NET_65_MVC02_PL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService TrainerService)
        {
            _trainerService = TrainerService;
        }


        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var Trainers = await _trainerService.GetAllTrainersAsync(ct);
            return View(Trainers);
        }


        [HttpGet]
        public async Task<IActionResult> TrainerDetails(int id, CancellationToken ct)
        {
            var Trainer = await _trainerService.GetTrainerDetailsAsync(id, ct);
            if (Trainer is null) { TempData["ErrorMessage"] = "Trainer not found"; return RedirectToAction(nameof(Index)); }
            return View(Trainer);
        }


        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateTrainer(CreateTrainerView model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);
            var Result = await _trainerService.CreateTrainerAsync(model, ct);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Create Trainer";
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> EditTrainer(int id, CancellationToken ct)
        {
            var Trainer = await _trainerService.GetTrainerToUpdateAsync(id, ct);
            if (Trainer is null) { TempData["ErrorMessage"] = "Trainer not found"; return RedirectToAction(nameof(Index)); }
            return View(Trainer);
        }

        [HttpPost]
        public async Task<IActionResult> EditTrainer(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if(!ModelState.IsValid) return View(model);
            var Result = await _trainerService.UpdateTrainerDetailsAsync(id, model, ct);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Trainer";
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var Trainer = await _trainerService.GetTrainerDetailsAsync(id, ct);
            if (Trainer is null) { TempData["ErrorMessage"] = "Trainer not found"; return RedirectToAction(nameof(Index)); }
            return View(Trainer);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var Result = await _trainerService.RemoveTrainerAsync(id, ct);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Member, Ensure there are no future bookings";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
