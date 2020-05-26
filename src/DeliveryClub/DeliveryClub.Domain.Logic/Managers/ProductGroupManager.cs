using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Logic.Extensions;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class ProductGroupManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;
        private readonly PortionPriceManager _portionPriceManager;
        private readonly PortionPriceProductGroupManager _portionPriceProductGroupManager;
        private readonly AdminManager _adminManager;
        private readonly RestaurantManager _restaurantManager;
        private readonly ProductManager _productManager;

        public ProductGroupManager(ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager,
                            PortionPriceManager portionPriceManager,
                            PortionPriceProductGroupManager portionPriceProductGroupManager,
                            AdminManager adminManager,
                            RestaurantManager restaurantManager,
                            ProductManager productManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
            _portionPriceManager = portionPriceManager;
            _portionPriceProductGroupManager = portionPriceProductGroupManager;
            _adminManager = adminManager;
            _restaurantManager = restaurantManager;
            _productManager = productManager;
        }
        public ProductGroup CreateProductGroup(ProductGroupModel model, Restaurant restaurant)
        {
            var newProductGroup = new ProductGroup
            {
                Name = model.Name,
                RestaurantId = restaurant.Id,
            };
            var newProductGroupDTO = _dbContext.ProductGroups.Add(_mapper.Map<ProductGroup, ProductGroupDTO>(newProductGroup));
            _dbContext.SaveChanges();
            
            var portionPrices = _portionPriceManager.CreatePortionPrices(model.PortionPrices);
            var portionPricesProductGroup = _portionPriceProductGroupManager.CreatePortionPricesProductGroup(portionPrices, _mapper.Map<ProductGroupDTO, ProductGroup>(newProductGroupDTO.Entity)).ToHashSet();

            newProductGroup.PortionPrices = portionPricesProductGroup;
            return newProductGroup;
        }

        public ICollection<ProductGroup> GetProductGroupsFull(Restaurant restaurant)
        {
            var productGroupsDTO = _dbContext.ProductGroups
                .Where(pg => pg.RestaurantId == restaurant.Id)
                .Include(pg => pg.Products)
                .ToList();

            foreach (var pgdto in productGroupsDTO)
            {
                pgdto.PortionPrices = _dbContext.PortionPriceProductGroups
                    .Where(pppg => pppg.ProductGroupId == pgdto.Id)
                    .Include(pp => pp.PortionPrice)
                    .ToHashSet();
            }            

            var productGroups = new List<ProductGroup>();
            foreach (var pgdto in productGroupsDTO)
            {
                var productGroup = _mapper.Map<ProductGroupDTO, ProductGroup>(pgdto);
                productGroup.Products = _productManager.GetProducts(productGroup.Id).ToHashSet();
                productGroups.Add(productGroup);
            }           

            return productGroups;
        }

        public ProductGroupDTO GetProductGroupDTOById(int id)
        {
            var result = _dbContext.ProductGroups.Where(pg => pg.Id == id)
                .FirstOrDefault();
            result.PortionPrices = _dbContext.PortionPriceProductGroups.
                Where(pp => pp.ProductGroupId == result.Id)
                .Include(pp => pp.PortionPrice)
                .ToHashSet();
            return result;
        }

        public async Task DeleteProductGroup(ClaimsPrincipal currentUser, int id)
        {
            var currentIdentityUser = await _userManager.GetCurrentIdentityUser(currentUser);
            var admin = _adminManager.GetAdmin(currentIdentityUser.Id);
            var restaurant = _restaurantManager.GetRestaurant(admin.RestaurantId);

            var productGroupDTO = GetProductGroupDTOById(id);
            foreach (var pp in productGroupDTO.PortionPrices)
            {
                var resultPPPG = _dbContext.PortionPriceProductGroups.Remove(pp);
                var resultPP = _dbContext.PortionPrices.Remove(pp.PortionPrice);
            }
            _dbContext.ProductGroups.Remove(productGroupDTO);

            _dbContext.SaveChanges();
        }

        public ProductGroup GetProductGroup(int restaurantId, string name)
        {
            var productGroupDTO = _dbContext.ProductGroups.Where(pg => pg.RestaurantId == restaurantId)
                                                          .Where(pg => pg.Name == name).FirstOrDefault();
            return _mapper.Map<ProductGroupDTO, ProductGroup>(productGroupDTO);
        }
    }
}
