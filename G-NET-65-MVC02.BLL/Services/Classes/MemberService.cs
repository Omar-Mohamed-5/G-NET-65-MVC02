using G_NET_65_MVC02.BLL.Services.Interfaces;
using G_NET_65_MVC02.BLL.ViewModels.MemberViewModels;
using G_NET_65_MVC02.DAL.Models;
using G_NET_65_MVC02.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<MemberShip> _memberShipRepository;
        private readonly IGenericRepository<Plan> _planRepository;
        private readonly IGenericRepository<HealthRecord> _healthRecordRepository;

        public MemberService(IGenericRepository<Member> MemberRepository, 
            IGenericRepository<MemberShip> MemberShipRepository,
            IGenericRepository<Plan> PlanRepository, IGenericRepository<HealthRecord> HealthRecordRepository)
        {
            _memberRepository = MemberRepository;
            _memberShipRepository = MemberShipRepository;
            _planRepository = PlanRepository;
            _healthRecordRepository = HealthRecordRepository;
        }
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default)
        {
            var EmailExists = await _memberRepository.AnyAsync(x => x.Email == model.Email, ct);

            var PhoneExists = await _memberRepository.AnyAsync(x => x.PhoneNumber == model.PhoneNumber, ct);

            if (EmailExists || PhoneExists) return false;

            var member = new Member
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street
                },
                HealthRecord = new HealthRecord
                {
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight
                }
            };

            var Result = await _memberRepository.AddAsync(member);
            return Result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var Members = await _memberRepository.GetAllAsync(ct:ct);
            if (!Members.Any()) return [];
            var MembersViewModel = Members.Select(x => new MemberViewModel
            {
               Id = x.Id,
               Name = x.Name,
               Email = x.Email,
               PhoneNumber = x.PhoneNumber,
               Photo = x.Photo,
               Gender = x.Gender.ToString()
            });
            return MembersViewModel;
        }

        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
            var Member = _memberRepository.GetByIdAsync(memberId, ct);
            if (Member is null) return null;
            var ViewModel = new MemberViewModel
            {
                Name = Member.Result.Name,
                Email = Member.Result.Email,
                PhoneNumber = Member.Result.PhoneNumber,
                DateOfBirth = Member.Result.DateOfBirth.ToShortDateString(),
                Gender = Member.Result.Gender.ToString(),
                Address = $"{Member.Result.Address.BuildingNumber}, {Member.Result.Address.Street}, {Member.Result.Address.City}"
            };
            var ActiveMemberShip = await _memberShipRepository.FirstOrDefaultAsync(m => m.MemperId == memberId && m.IsActive, ct:ct);
            if(ActiveMemberShip is not null)
            {
                var ActivePlan = await _planRepository.GetByIdAsync(ActiveMemberShip.PlanId, ct);
                ViewModel.PlanName = ActivePlan?.Name;
                ViewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
            }
            return ViewModel;
        }

        public Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveMemberAsync(int memberId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMemberDetailsAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
