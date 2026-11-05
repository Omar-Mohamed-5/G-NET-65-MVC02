using G_NET_65_MVC02.BLL.Services.Interfaces;
using G_NET_65_MVC02.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace G_NET_65_MVC02_PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService MemberService)
        {
            _memberService = MemberService;
        }


        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMembersAsync(ct);
            return View(members);
        }


        [HttpGet]
        public IActionResult Create() => View();


        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateTrainerViewModel model, CancellationToken ct)
        {
            if(!ModelState.IsValid) return View(nameof(Create), model);
            var Result = await _memberService.CreateMemberAsync(model, ct);
            if(Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Create";
            }
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsAsync(id, ct);
            if (member is null) { TempData["ErrorMessage"] = "Member not found"; return RedirectToAction(nameof(Index)); }
            return View(member);
        }


        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var Record = await _memberService.GetMemberHealthRecordAsync(id, ct);
            if (Record is null) { TempData["ErrorMessage"] = "Health record not found"; return RedirectToAction(nameof(Index)); }
            return View(Record);
        }


        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (member is null) { TempData["ErrorMessage"] = "Member not found"; return RedirectToAction(nameof(Index)); }
            return View(member);
        }


        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var Result = await _memberService.UpdateMemberDetailsAsync(id, model, ct);
            if(Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Member";
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsAsync(id, ct);
            if (member is null) { TempData["ErrorMessage"] = "Member not found"; return RedirectToAction(nameof(Index)); }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var Result = await _memberService.RemoveMemberAsync(id, ct);
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
