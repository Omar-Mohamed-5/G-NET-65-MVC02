using AutoMapper;
using G_NET_65_MVC02.BLL.Common;
using G_NET_65_MVC02.BLL.Services.Interfaces;
using G_NET_65_MVC02.BLL.ViewModels.SessionViewModels;
using G_NET_65_MVC02.DAL.Models;
using G_NET_65_MVC02.DAL.Models.Enums;
using G_NET_65_MVC02.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork UnitOfWork, IMapper Mapper)
        {
            _unitOfWork = UnitOfWork;
            _mapper = Mapper;
        }

        public async Task<IEnumerable<SessionViewModel>?> GetAllSessionsAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork._SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct:ct);
            if (sessions?.Any() != true) return null;
            sessions = sessions.OrderByDescending(s => s.StartDate);
            var MappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in MappedSessions)
            {
                session.AvailableSlots = session.Capacity - (await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct));
            }
            return MappedSessions;
        }

        public async Task<SessionViewModel?> GetSessionByIdAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork._SessionRepository.GetSessionWithTrainerAndCategoryAsync(SessionId, ct);
            if (session == null) return null;
            var MapedSession = _mapper.Map<Session, SessionViewModel>(session);
            MapedSession.AvailableSlots = MapedSession.Capacity - (await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(MapedSession.Id, ct));
            return MapedSession;
        }


        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if(model.EndDate <= model.StartDate)
                return Result.Validation("End date must be after start date");
            if(model.StartDate <= DateTime.Now)
                return Result.Validation("Start date must be in the future");

            var TrainerRepo = _unitOfWork.GenericRepository<Trainer>();
            var Trainer = await TrainerRepo.GetByIdAsync(model.TrainerId, ct);
            if(Trainer is null)
                return Result.NotFound("Invalid Trainer Id");

            var CategoryRepo = _unitOfWork.GenericRepository<Category>();
            var Category = await CategoryRepo.GetByIdAsync(model.CategoryId, ct);
            if (Category is null)
                return Result.NotFound("Category Not Found");

            var IsValidSpecialty = Enum.TryParse<Specialties>(Category.Name, true, out var CategorySpecialty);
            if (!IsValidSpecialty || Trainer.Specialties != CategorySpecialty)
                return Result.Validation("Trainer Specialty Does Not Match Session Category");
            var session = _mapper.Map<Session>(model);
            await _unitOfWork.GenericRepository<Session>().AddAsync(session);
            var AffectedRows = await _unitOfWork.SaveChangesAsync(ct);
            return AffectedRows > 0 ? Result.Ok() : Result.Fail("Failed To Create Session");
        }
        

        public async Task<Result> UpdateSessionAsync(int SessionId, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var SessionRepo = _unitOfWork.GenericRepository<Session>();
            var session = await SessionRepo.GetByIdAsync(SessionId, ct);
            if (session is null)
                return Result.NotFound("Session Not Found");

            if (session.StartDate <= DateTime.Now)
                return Result.Fail("Cannot Update Session That Has Already Started");

            var BookedCount = await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(SessionId, ct);
            if (BookedCount > 0)
                return Result.Fail("Cannot Update A Session That Has Booking");

            if (model.EndDate <= model.StartDate)
                return Result.Validation("End date must be after start date");
            if (model.StartDate <= DateTime.Now)
                return Result.Validation("Start date must be in the future");

            var TrainerRepo = _unitOfWork.GenericRepository<Trainer>();
            var Trainer = await TrainerRepo.GetByIdAsync(model.TrainerId, ct);
            if (Trainer is null)
                return Result.NotFound("Invalid Trainer Id");

            var CategoryRepo = _unitOfWork.GenericRepository<Category>();
            var Category = await CategoryRepo.GetByIdAsync(session.CategoryId, ct);
            if (Category is null)
                return Result.NotFound("Category Not Found");

            var IsValidSpecialty = Enum.TryParse<Specialties>(Category.Name, true, out var CategorySpecialty);
            if (!IsValidSpecialty || Trainer.Specialties != CategorySpecialty)
                return Result.Validation("Trainer Specialty Does Not Match Session Category");
            _mapper.Map(model, session);
            session.UpdatedAt = DateTime.Now;
            await SessionRepo.UpdateAsync(session);
            var AffectedRows = await _unitOfWork.SaveChangesAsync(ct);
            return AffectedRows > 0 ? Result.Ok() : Result.Fail("Failed To Update Session");
        }


        public async Task<Result> DeleteSessionAsync(int SessionId, CancellationToken ct = default)
        {
            var SessionRepo = _unitOfWork.GenericRepository<Session>();
            var session = await SessionRepo.GetByIdAsync(SessionId, ct);
            if (session is null)
                return Result.NotFound("Session Not Found");

            if (session.EndDate >= DateTime.Now)
                return Result.Fail("Cannot Delete A Session That Has Session");

            var BookedCount = await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(SessionId, ct);
            if (BookedCount > 0)
                return Result.Fail("Cannot Update A Session That Has Booking");
            await SessionRepo.DeleteAsync(session);
            var AffectedRows = await _unitOfWork.SaveChangesAsync(ct);
            return AffectedRows > 0 ? Result.Ok() : Result.Fail("Failed To Delete Session");
        }


        public async Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int SessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GenericRepository<Session>().GetByIdAsync(SessionId, ct);
            if (session == null) return null;
            if (!await IsSessionAvaliableForUpdatingAsync(session, ct)) return null;
            return _mapper.Map<UpdateSessionViewModel>(session); ;
        }

        #region Helper methods
        private async Task<bool> IsSessionAvaliableForUpdatingAsync(Session session, CancellationToken ct = default)
        {
            if (session.StartDate <= DateTime.Now) return false;
            var Booked = await _unitOfWork._SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);
            return Booked == 0;
        }
        #endregion


        public async Task<IEnumerable<CategorySelectViewModel>> GetCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var categories = await _unitOfWork.GenericRepository<Category>().GetAllAsync(ct:ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }


        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GenericRepository<Trainer>().GetAllAsync(ct:ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }
    }
}
