using G_NET_65_MVC02.BLL.Services.Interfaces;
using G_NET_65_MVC02.BLL.ViewModels.TrainerViewModels;
using G_NET_65_MVC02.DAL.Models;
using G_NET_65_MVC02.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IGenericRepository<Trainer> _trainerRepository;
        private readonly IGenericRepository<Session> _sessionRepository;

        public TrainerService(IGenericRepository<Trainer> TrainerRepository, 
                              IGenericRepository<Session> SessionRepository)
        {
            _trainerRepository = TrainerRepository;
            _sessionRepository = SessionRepository;
        }
        public async Task<bool> CreateTrainerAsync(CreateTrainerView model, CancellationToken ct = default)
        {
            var EmailExists = await _trainerRepository.AnyAsync(x => x.Email == model.Email, ct);
            var PhoneExists = await _trainerRepository.AnyAsync(x=> x.PhoneNumber == model.PhoneNumber, ct);

            if(EmailExists && PhoneExists) return false;

            var Trainer = new Trainer
            {
                Name = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                Specialties = model.Specialties,
                Address = new Address
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City,
                }
            };
            var Result = await _trainerRepository.AddAsync(Trainer);
            return Result > 0;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync(CancellationToken ct = default)
        {
            var Trainers = await _trainerRepository.GetAllAsync(ct:ct);
            if (!Trainers.Any()) return [];
            var TrainersViewMode = Trainers.Select(x => new TrainerViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Specialties = x.Specialties.ToString()
            });
            return TrainersViewMode;
        }

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var Trainer = await _trainerRepository.GetByIdAsync(trainerId,ct);
            if (Trainer is null) return null;
            var ViewModel = new TrainerViewModel
            {
                Name = Trainer.Name,
                Email = Trainer.Email,
                PhoneNumber = Trainer.PhoneNumber,
                DateOfBirth = Trainer.DateOfBirth.ToShortDateString(),
                Specialties = Trainer.Specialties.ToString(),
                Address = $"{Trainer.Address.BuildingNumber} - {Trainer.Address.Street} - {Trainer.Address.City}"
            };
            return ViewModel;
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var Trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);
            if (Trainer is null) return null;
            else
            {
                return new TrainerToUpdateViewModel()
                {
                    Name = Trainer.Name,
                    Email = Trainer.Email,
                    PhoneNumber = Trainer.PhoneNumber,
                    BuildingNumber = Trainer.Address.BuildingNumber,
                    Street = Trainer.Address.Street,
                    City = Trainer.Address.City,
                    Specialties = Trainer.Specialties
                };
            }
        }

        public async Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            var Trainer = await _trainerRepository.GetByIdAsync(trainerId,ct);
            if (Trainer is null) return false;
            var HasFutureSessions = await _sessionRepository.AnyAsync(x => x.TrainerId == trainerId && x.CreatedAt > DateTime.Now, ct);
            if (HasFutureSessions) return false;
            var Result = await _trainerRepository.DeleteAsync(Trainer);
            return Result > 0;
        }

        public async Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var Trainer = await _trainerRepository.GetByIdAsync(trainerId, ct);
            if (Trainer is null) return false;
            if(await _trainerRepository.AnyAsync(x => x.Email == model.Email && x.Id != trainerId, ct)) return false;
            if(await _trainerRepository.AnyAsync(x => x.PhoneNumber == model.PhoneNumber && x.Id != trainerId, ct)) return false;

            Trainer.Email = model.Email;
            Trainer.PhoneNumber = model.PhoneNumber;
            Trainer.Address.BuildingNumber = model.BuildingNumber;
            Trainer.Address.Street = model.Street;
            Trainer.Address.City = model.City;
            Trainer.Specialties = model.Specialties;

            var Result = await _trainerRepository.UpdateAsync(Trainer);
            return Result > 0 ? true : false;
        }
    }
}
