using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.AuxiliaryModels.Dispatcher;
using DeliveryClub.Domain.AuxiliaryModels.Guest;
using DeliveryClub.Domain.AuxiliaryModels.SuperUser;
using DeliveryClub.Domain.Logic.Managers;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryClub.Domain.Logic.Mapping
{
    public class AuxiliaryMapper
    {
        public RestaurantInfoModel CreateRestaurantInfoModel(Restaurant restaurant)
        {
            var restaurantInfo = new RestaurantInfoModel();
            var stringTimeSpanConverter = new StringTimeSpanConverter();

            restaurantInfo.Name = restaurant.Name;
            restaurantInfo.Specializations = CreateSpecializationModelList(restaurant.Specializations);
            restaurantInfo.DeliveryCost = restaurant.DeliveryCost;
            restaurantInfo.MinimalOrderPrice = restaurant.MinimalOrderPrice;
            restaurantInfo.Description = restaurant.RestaurantAdditionalInfo.Description;
            restaurantInfo.CoverImageName = restaurant.CoverImageName;
            restaurantInfo.PaymentMethods = CreatePaymentMethodModelList(restaurant.RestaurantAdditionalInfo.PaymentMethods);
            restaurantInfo.DeliveryMaxTime = stringTimeSpanConverter.TimeSpanToString(restaurant.RestaurantAdditionalInfo.DeliveryMaxTime);
            restaurantInfo.OrderTimeBegin = stringTimeSpanConverter.TimeSpanToString(restaurant.RestaurantAdditionalInfo.OrderTimeBegin);
            restaurantInfo.OrderTimeEnd = stringTimeSpanConverter.TimeSpanToString(restaurant.RestaurantAdditionalInfo.OrderTimeEnd);
            return restaurantInfo;
        }

        public ICollection<ProductGroupModel> CreateProductGroupModels(ICollection<ProductGroup> productGroups)
        {
            var result = new List<ProductGroupModel>();
            foreach (var pg in productGroups)
            {
                var productGroupModel = new ProductGroupModel
                {
                    Id = pg.Id,
                    Name = pg.Name,
                    PortionPrices = CreatePortionPriceModels(pg.PortionPrices).ToList(),
                    Products = CreateProductModels(pg.Products).ToList()
                };
                result.Add(productGroupModel);
            }
            return result;
        }

        public ICollection<PortionPriceModel> CreatePortionPriceModels(ICollection<PortionPriceProductGroup> portionPrices)
        {
            var result = new List<PortionPriceModel>();
            foreach (var pp in portionPrices)
            {
                var ppm = new PortionPriceModel
                {
                    Id = pp.PortionPrice.Id,
                    Portion = pp.PortionPrice.Portion,
                    Price = pp.PortionPrice.Price,
                };
                result.Add(ppm);
            }
            return result;
        }

        public ICollection<PortionPriceModel> CreatePortionPriceModels(ICollection<PortionPriceProduct> portionPrices)
        {
            var result = new List<PortionPriceModel>();
            foreach (var pp in portionPrices)
            {
                var ppm = new PortionPriceModel
                {
                    Id = pp.PortionPrice.Id,
                    Portion = pp.PortionPrice.Portion,
                    Price = pp.PortionPrice.Price,
                };
                result.Add(ppm);
            }
            return result;
        }

        public ICollection<ProductModel> CreateProductModels(ICollection<Product> products)
        {
            var result = new List<ProductModel>();
            foreach (var p in products)
            {
                var productModel = new ProductModel
                {
                    Name = p.Name,
                    Description = p.Description,
                    Id = p.Id,
                    ImageName = p.ImageName,
                    PortionPrices = CreatePortionPriceModels(p.PortionPrices).ToList()
                };
                result.Add(productModel);
            }
            return result;
        }

        public ICollection<GetDispatcherModel> CreateGetDispatcherModels(ICollection<Dispatcher> dispatchers)
        {
            var getDispatcherModels = new List<GetDispatcherModel>();
            foreach (var d in dispatchers)
            {
                var gdm = new GetDispatcherModel()
                {
                    Email = d.User.Email,
                    IsActive = d.IsActive,
                    Id = d.Id,
                };
                getDispatcherModels.Add(gdm);
            }
            return getDispatcherModels;
        }

        public UpdateDispatcherModel CreateUpdateDispatcherModel(Dispatcher dispatcher)
        {
            var udm = new UpdateDispatcherModel
            {
                Id = dispatcher.Id,
                OldEmail = dispatcher.User.Email,
            };
            return udm;
        }

        public ProductModel CreateProductModel(Product product)
        {
            var productModel = new ProductModel
            {
                Name = product.Name,
                Description = product.Description,
                Id = product.Id,
                ImageName = product.ImageName,
                ProductGroupName = product.ProductGroup.Name,
                PortionPrices = CreatePortionPriceModels(product.PortionPrices).ToList()
            };
            return productModel;
        }

        public List<SpecializationModel> CreateSpecializationModelList(HashSet<Specialization> specializations)
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

        public List<PaymentMethodModel> CreatePaymentMethodModelList(HashSet<PaymentMethod> paymentMethods)
        {
            var result = new List<PaymentMethodModel>();
            foreach (var pm in Enum.GetValues(typeof(PaymentMethod)))
            {
                if (paymentMethods.Contains((PaymentMethod)pm))
                {
                    result.Add(new PaymentMethodModel { PaymentMethod = (PaymentMethod)pm, IsSelected = true });
                }
                else
                {
                    result.Add(new PaymentMethodModel { PaymentMethod = (PaymentMethod)pm, IsSelected = false });
                }
            }
            return result;
        }
        public HashSet<PaymentMethod> CreatePaymentMethodHashSet(List<PaymentMethodModel> paymentMethods)
        {
            var result = new HashSet<PaymentMethod>();
            foreach (var sp in paymentMethods)
            {
                if (sp.IsSelected)
                {
                    result.Add(sp.PaymentMethod);
                }
            }
            return result;
        }

        public ICollection<RestaurantPartialModel> CreateRestaurantPartialModels(ICollection<Restaurant> restaurants)
        {
            var rpms = new List<RestaurantPartialModel>();

            foreach (var r in restaurants)
            {
                var rpm = new RestaurantPartialModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    DeliveryCost = r.DeliveryCost,
                    MinimalOrderPrice = r.MinimalOrderPrice,
                    CoverImageName = r.CoverImageName,
                    Specializations = CreateSpecializationModelList(r.Specializations)
                };
                rpms.Add(rpm);
            }
            return rpms;
        }

        public RestaurantFullModel CreateRestaurantFullModel(Restaurant restaurant)
        {
            var stringTimeSpanConverter = new StringTimeSpanConverter();

            var restaurantFullModel = new RestaurantFullModel
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                DeliveryCost = restaurant.DeliveryCost,
                MinimalOrderPrice = restaurant.MinimalOrderPrice,
                DeliveryMaxTime = stringTimeSpanConverter.TimeSpanToString(restaurant.RestaurantAdditionalInfo.DeliveryMaxTime),
                OrderTimeBegin = stringTimeSpanConverter.TimeSpanToString(restaurant.RestaurantAdditionalInfo.OrderTimeBegin),
                OrderTimeEnd = stringTimeSpanConverter.TimeSpanToString(restaurant.RestaurantAdditionalInfo.OrderTimeEnd),
                Description = restaurant.RestaurantAdditionalInfo.Description,
                Menu = CreateProductGroupModels(restaurant.Menu),
                Specializations = CreateSpecializationModelList(restaurant.Specializations),
                PaymentMethods = CreatePaymentMethodModelList(restaurant.RestaurantAdditionalInfo.PaymentMethods)
            };

            return restaurantFullModel;
        }

        public DispatcherOrderModel CreateDispatcherOrderModel(Order order)
        {
            var dom = new DispatcherOrderModel
            {
                Id = order.Id,
                Name = order.Name,
                DeliveryAddress = order.DeliveryAddress,
                PhoneNumber = order.PhoneNumber,
            };
            return dom;
        }
    }
}
