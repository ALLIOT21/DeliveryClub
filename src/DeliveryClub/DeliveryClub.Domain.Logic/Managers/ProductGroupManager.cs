using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class ProductGroupManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;
        private readonly PortionPriceManager _portionPriceManager;
        private readonly PortionPriceProductGroupManager _portionPriceProductGroupManager;
        private readonly ProductManager _productManager;

        public ProductGroupManager(ApplicationDbContext dbContext,
                            PortionPriceManager portionPriceManager,
                            PortionPriceProductGroupManager portionPriceProductGroupManager,
                            ProductManager productManager)
        {
            _dbContext = dbContext;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
            _portionPriceManager = portionPriceManager;
            _portionPriceProductGroupManager = portionPriceProductGroupManager;
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

        public async Task UpdateProductGroup(ProductGroupModel model)
        {
            var productGroupDTO = GetProductGroupDTOById(model.Id);
            var productGroup = _mapper.Map<ProductGroupDTO, ProductGroup>(productGroupDTO);

            productGroup.Name = model.Name;
            var portionPrices = _portionPriceManager.CreatePortionPrices(model.PortionPrices);
            _portionPriceProductGroupManager.UpdatePortionPricesProductGroup(portionPrices, productGroup);
            _dbContext.ProductGroups.Update(_mapper.Map<ProductGroup, ProductGroupDTO>(productGroup));
            await _dbContext.SaveChangesAsync();
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

        public async Task DeleteProductGroup(int id)
        {           
            var products = _dbContext.Products.Where(p => p.ProductGroupId == id);

            foreach (var product in products)
            {
                _productManager.DeleteProductWithoutSaving(product.Id);
            }

            var productGroupDTO = GetProductGroupDTOById(id);                       
            foreach (var pp in productGroupDTO.PortionPrices)
            {
                var resultPPPG = _dbContext.PortionPriceProductGroups.Remove(pp);
                var resultPP = _dbContext.PortionPrices.Remove(pp.PortionPrice);
            }
            _dbContext.SaveChanges();

            productGroupDTO = GetProductGroupDTOById(id);
            _dbContext.ProductGroups.Remove(productGroupDTO);
            await _dbContext.SaveChangesAsync();
        }

        public ProductGroup GetProductGroup(int restaurantId, string name)
        {
            var productGroupDTO = _dbContext.ProductGroups.Where(pg => pg.RestaurantId == restaurantId)
                                                          .Where(pg => pg.Name == name).FirstOrDefault();
            return _mapper.Map<ProductGroupDTO, ProductGroup>(productGroupDTO);
        }
    }
}
