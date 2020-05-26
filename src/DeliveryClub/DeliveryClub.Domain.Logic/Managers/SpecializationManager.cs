using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Domain.Models.Enumerations;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class SpecializationManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;
        
        public SpecializationManager(ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public HashSet<Specialization> GetSpecializations(Restaurant restaurant)
        {
            var specializations = _dbContext.Specializations.Where(s => s.RestaurantId == restaurant.Id).ToHashSet();
            var result = new HashSet<Specialization>();
            foreach (var s in specializations)
            {
                result.Add(s.Specialization);
            }
            return result;
        }

        public async Task UpdateSpecializations(Restaurant restaurant, HashSet<Specialization> newSpecializations)
        {
            var oldSpecializations = GetSpecializations(restaurant);

            foreach (var os in oldSpecializations)
            {
                DeleteSpecialization(restaurant, os);
            }

            foreach (var ns in newSpecializations)
            {
                await CreateSpecialization(restaurant, ns);
            }
        }

        public List<SpecializationModel> CreateSpecializationList(HashSet<Specialization> specializations)
        {
            var result = new List<SpecializationModel>();
            if (specializations != null)
            {
                foreach (var sp in Enum.GetValues(typeof(Specialization)))
                {
                    if (specializations.Contains((Specialization)sp))
                    {
                        result.Add(new SpecializationModel { Specialization = (Specialization)sp, IsSelected = true });
                    }
                    else
                    {
                        result.Add(new SpecializationModel { Specialization = (Specialization)sp, IsSelected = false });
                    }
                }
            }
            return result;
        }

        public HashSet<Specialization> CreateSpecializationHashSet(List<SpecializationModel> specializations)
        {
            var result = new HashSet<Specialization>();
            foreach (var sp in specializations)
            {
                if (sp.IsSelected)
                {
                    result.Add(sp.Specialization);
                }
            }
            return result;
        }

        public async Task<int> CreateSpecialization(Restaurant restaurant, Specialization specialization)
        {
            var specializationDTO = new SpecializationDTO()
            {
                RestaurantId = restaurant.Id,
                Specialization = specialization,
            };
            var addResult = await _dbContext.Specializations.AddAsync(specializationDTO);

            return addResult.Entity.Id;
        }

        public void DeleteSpecialization(Restaurant restaurant, Specialization specialization)
        {
            var specializationDTO = new SpecializationDTO()
            {
                RestaurantId = restaurant.Id,
                Specialization = specialization,
            };
            specializationDTO = _dbContext.Specializations.Where(r => r.RestaurantId == restaurant.Id).Where(sp => sp.Specialization == specialization).FirstOrDefault();
            _dbContext.Specializations.Remove(specializationDTO);
        }
    }
}
