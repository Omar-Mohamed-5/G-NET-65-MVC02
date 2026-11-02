using G_NET_65_MVC02.BLL.Services.Interfaces;
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
    }
}
