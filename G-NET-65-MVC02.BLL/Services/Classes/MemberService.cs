using G_NET_65_MVC02.BLL.Services.Interfaces;
using G_NET_65_MVC02.BLL.ViewModels.MemberViewModels;
using G_NET_65_MVC02.DAL.Models;
using G_NET_65_MVC02.DAL.Repositories.Classes;
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
        private readonly IGenericRepository<Booking> _bookingRepository;

        public MemberService(IGenericRepository<Member> MemberRepository, 
                             IGenericRepository<MemberShip> MemberShipRepository,
                             IGenericRepository<Plan> PlanRepository,
                             IGenericRepository<HealthRecord> HealthRecordRepository,
                             IGenericRepository<Booking> BookingRepository)
        {
            _memberRepository = MemberRepository;
            _memberShipRepository = MemberShipRepository;
            _planRepository = PlanRepository;
            _healthRecordRepository = HealthRecordRepository;
            _bookingRepository = BookingRepository;
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
            var Member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (Member is null) return null;
            var ViewModel = new MemberViewModel
            {
                Name = Member.Name,
                Email = Member.Email,
                PhoneNumber = Member.PhoneNumber,
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Gender = Member.Gender.ToString(),
                Address = $"{Member.Address.BuildingNumber}, {Member.Address.Street}, {Member.Address.City}"
            };
            var ActiveMemberShip = await _memberShipRepository.GetAllAsync(ct: ct);
            var activeMemberShip = ActiveMemberShip.FirstOrDefault(m => m.MemperId == memberId && m.IsActive);

            if (activeMemberShip is not null)
            {
               var ActivePlan = await _planRepository.GetByIdAsync(activeMemberShip.PlanId, ct);
               ViewModel.PlanName = ActivePlan?.Name;
               ViewModel.MemberShipStartDate = activeMemberShip.CreatedAt.ToShortDateString();
               ViewModel.MemberShipEndDate = activeMemberShip.EndDate.ToShortDateString();
            }
            return ViewModel;
        }



        public async Task<HealthRecordViewModel?> GetMemberHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
            var Records = await _healthRecordRepository.GetAllAsync(ct:ct);
            var Record = Records.FirstOrDefault(x => x.MemberId == memberId);
            if (Record is null) return null;
            else
            {
                return new HealthRecordViewModel()
                {
                    Height = Record.Height,
                    Weight = Record.Weight,
                    BloodType = Record.BloodType,
                    Note = Record.Note
                };
            }
        }



        public async Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int memberId, CancellationToken ct = default)
        {
            var Member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (Member is null) return null;
            else
            {
                return new MemberToUpdateViewModel()
                {
                    Name = Member.Name,
                    Email = Member.Email,
                    PhoneNumber = Member.PhoneNumber,
                    Street = Member.Address.Street,
                    City = Member.Address.City,
                    BuildingNumber = Member.Address.BuildingNumber,
                    Photo = Member.Photo
                };
            }
        }



        public async Task<bool> RemoveMemberAsync(int memberId, CancellationToken ct = default)
        {
            var Member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (Member is null) return false;
            var HasFutureSessions = await _bookingRepository.AnyAsync(x => x.MemberId == memberId && x.Session.StartDate > DateTime.Now, ct);
            if (HasFutureSessions) return false;
            var Result = await _memberRepository.DeleteAsync(Member);
            return Result > 0;
        }



        public async Task<bool> UpdateMemberDetailsAsync(int memberId, MemberToUpdateViewModel model, CancellationToken ct = default)
        {
            var Member = await _memberRepository.GetByIdAsync(memberId, ct);
            if (Member is null) return false;
            if(await _memberRepository.AnyAsync(x => x.Email == model.Email && x.Id != memberId, ct))
               return false;
            if(await _memberRepository.AnyAsync(x => x.PhoneNumber == model.PhoneNumber && x.Id != memberId, ct))
               return false;
            Member.Email = model.Email;
            Member.PhoneNumber = model.PhoneNumber;
            Member.Address.City = model.City;
            Member.Address.Street = model.Street;
            Member.Address.BuildingNumber = model.BuildingNumber;
            Member.UpdatedAt = DateTime.Now;
            var Result = await _memberRepository.UpdateAsync(Member);
            return Result > 0 ? true : false;
        }
    }
}
