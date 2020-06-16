using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Domain.Models.Enumerations;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class PaymentMethodManager
    {
        private readonly ApplicationDbContext _dbContext;

        public PaymentMethodManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }        

        public async Task<int> CreatePaymentMethod(RestaurantAdditionalInfo restaurantAdditionalInfo, PaymentMethod paymentMethod)
        {
            var paymentMethodDTO = new PaymentMethodDTO
            {
                RestaurantAdditionalInfoId = restaurantAdditionalInfo.Id,
                PaymentMethod = paymentMethod,
            };
            var addResult = await _dbContext.PaymentMethods.AddAsync(paymentMethodDTO);
            return addResult.Entity.Id;
        }

        public void DeletePaymentMethod(RestaurantAdditionalInfo restaurantAdditionalInfo, PaymentMethod paymentMethod)
        {
            var paymentMethodDTO = new PaymentMethodDTO
            {
                RestaurantAdditionalInfoId = restaurantAdditionalInfo.Id,
                PaymentMethod = paymentMethod,
            };
            paymentMethodDTO = _dbContext.PaymentMethods.Where(r => r.RestaurantAdditionalInfoId == restaurantAdditionalInfo.Id).Where(sp => sp.PaymentMethod == paymentMethod).FirstOrDefault();
            _dbContext.PaymentMethods.Remove(paymentMethodDTO);
        }

        public HashSet<PaymentMethod> GetPaymentMethods(RestaurantAdditionalInfo restaurantAdditionalInfo)
        {
            var paymentMethods = _dbContext.PaymentMethods.Where(pm => pm.RestaurantAdditionalInfoId == restaurantAdditionalInfo.Id).ToHashSet();
            var result = new HashSet<PaymentMethod>();
            foreach (var pm in paymentMethods)
            {
                result.Add(pm.PaymentMethod);
            }
            return result;
        }

        public async Task UpdatePaymentMethods(RestaurantAdditionalInfo restaurantAdditionalInfo, HashSet<PaymentMethod> newPaymentMethods)
        {
            var oldPaymentMethod = GetPaymentMethods(restaurantAdditionalInfo);

            foreach (var osp in oldPaymentMethod)
            {
                DeletePaymentMethod(restaurantAdditionalInfo, osp);
            }

            foreach (var nsp in newPaymentMethods)
            {
                await CreatePaymentMethod(restaurantAdditionalInfo, nsp);
            }
        }
    }
}
