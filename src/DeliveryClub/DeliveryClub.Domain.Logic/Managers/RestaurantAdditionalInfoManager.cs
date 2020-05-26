using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class RestaurantAdditionalInfoManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;
        private readonly PaymentMethodManager _paymentMethodManager;

        public RestaurantAdditionalInfoManager(ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager,
                            PaymentMethodManager paymentMethodManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
            _paymentMethodManager = paymentMethodManager;
        }

        public RestaurantAdditionalInfo GetRestaurantAdditionalInfo(int id)
        {
            var restaurantAdditionalInfoDTO = _dbContext.RestaurantAdditionalInfos.Where(rai => rai.RestaurantId == id).FirstOrDefault();
            var restaurantAdditionalInfo = _mapper.Map<RestaurantAdditionalInfoDTO, RestaurantAdditionalInfo>(restaurantAdditionalInfoDTO);
            restaurantAdditionalInfo.PaymentMethods = _paymentMethodManager.GetPaymentMethods(restaurantAdditionalInfo);

            return restaurantAdditionalInfo;
        }

        public async Task<RestaurantAdditionalInfo> UpdateRestaurantAdditionalInfo(RestaurantAdditionalInfo restaurantAdditionalInfo, RestaurantInfoModel restaurantInfoModel)
        {
            var stringTimeSpanConverter = new StringTimeSpanConverter();

            restaurantAdditionalInfo.Description = restaurantInfoModel.Description;
            restaurantAdditionalInfo.DeliveryMaxTime = stringTimeSpanConverter.StringToTimeSpan(restaurantInfoModel.DeliveryMaxTime);
            restaurantAdditionalInfo.OrderTimeBegin = stringTimeSpanConverter.StringToTimeSpan(restaurantInfoModel.OrderTimeBegin);
            restaurantAdditionalInfo.OrderTimeEnd = stringTimeSpanConverter.StringToTimeSpan(restaurantInfoModel.OrderTimeEnd);
            var updateResult = _dbContext.RestaurantAdditionalInfos.Update(_mapper.Map<RestaurantAdditionalInfo, RestaurantAdditionalInfoDTO>(restaurantAdditionalInfo));
            await _paymentMethodManager.UpdatePaymentMethods(restaurantAdditionalInfo, _paymentMethodManager.CreatePaymentMethodHashSet(restaurantInfoModel.PaymentMethods));
            return _mapper.Map<RestaurantAdditionalInfoDTO, RestaurantAdditionalInfo>(updateResult.Entity);
        }
    }
}
