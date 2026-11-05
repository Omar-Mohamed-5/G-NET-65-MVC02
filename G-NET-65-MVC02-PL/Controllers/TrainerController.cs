using G_NET_65_MVC02.BLL.Services.Interfaces;
using G_NET_65_MVC02.BLL.ViewModels.MemberViewModels;
using G_NET_65_MVC02.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
            var trainers = await _trainerService.GetAllTrainersAsync(ct);
            return View(trainers);
        }


        [HttpGet]
        public async Task<IActionResult> TrainerDetails(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(id, ct);
            if (trainer is null) { TempData["ErrorMessage"] = "Trainer not found"; return RedirectToAction(nameof(Index)); }
            return View(trainer);
        }


        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateTrainer(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);
            var Result = await _trainerService.CreateTrainerAsync(model, ct);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Create";
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> EditTrainer(int id, CancellationToken ct)
        {
            var trainer = await _trainerService.GetTrainerToUpdateAsync(id, ct);
            if (trainer is null) { TempData["ErrorMessage"] = "Trainer not found"; return RedirectToAction(nameof(Index)); }
            return View(trainer);
        }


        [HttpPost]
        public async Task<IActionResult> EditTrainer(int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var Result = await _trainerService.UpdateTrainerDetailsAsync(id, model, ct);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Update";
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var Result = await _trainerService.GetTrainerDetailsAsync(id, ct);
            if(Result is null) { TempData["ErrorMessage"] = "Trainer not found"; return RedirectToAction(nameof(Index)); }
            return View(Result);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfiemed(int id, CancellationToken ct)
        {
            var Result = await _trainerService.RemoveTrainerAsync(id, ct);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Delete";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
