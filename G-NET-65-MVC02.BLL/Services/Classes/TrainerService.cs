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

        public TrainerService(IGenericRepository<Trainer> TrainerRepository)
        {
            _trainerRepository = TrainerRepository;
        }
        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
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
                Address = $"{Trainer.Address.BuildingNumber}, {Trainer.Address.Street}, {Trainer.Address.City}"
            };
            return ViewModel;
        }

        public Task<TrainerToUpdateViewModel?> GetTrainerToUpdateViewModel(int trainerId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveTrainerAsync(int trainerId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTrainerDetailsAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
