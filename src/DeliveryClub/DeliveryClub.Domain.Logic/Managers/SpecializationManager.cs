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
        private readonly ApplicationDbContext _dbContext;
        
        public SpecializationManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public async Task<int> CreateSpecialization(Restaurant restaurant, Specialization specialization)
        {
            var specializationDTO = new SpecializationDAO()
            {
                RestaurantId = restaurant.Id,
                Specialization = specialization,
            };
            var addResult = await _dbContext.Specializations.AddAsync(specializationDTO);

            return addResult.Entity.Id;
        }

        public void DeleteSpecialization(Restaurant restaurant, Specialization specialization)
        {
            var specializationDTO = new SpecializationDAO()
            {
                RestaurantId = restaurant.Id,
                Specialization = specialization,
            };
            specializationDTO = _dbContext.Specializations.Where(r => r.RestaurantId == restaurant.Id).Where(sp => sp.Specialization == specialization).FirstOrDefault();
            _dbContext.Specializations.Remove(specializationDTO);
        }
    }
}
