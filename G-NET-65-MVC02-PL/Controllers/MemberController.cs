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
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
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
    }
}
