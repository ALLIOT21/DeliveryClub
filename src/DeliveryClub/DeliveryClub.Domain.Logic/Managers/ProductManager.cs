using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class ProductManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;
        private readonly PortionPriceManager _portionPriceManager;
        private readonly PortionPriceProductManager _portionPriceProductManager;

        public ProductManager(ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager,
                            PortionPriceManager portionPriceManager,                           
                            PortionPriceProductManager portionPriceProductManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
            _portionPriceManager = portionPriceManager;
            _portionPriceProductManager = portionPriceProductManager;
        }        

        public Product CreateProduct(ProductModel model, int productGroupId)
        {
            var newProduct = new Product
            {
                Name = model.Name,
                Description = model.Description,
                ProductGroupId = productGroupId
            };
            var newProductDTO = _dbContext.Products.Add(_mapper.Map<Product, ProductDTO>(newProduct));            
            _dbContext.SaveChanges();

            var portionPrices = _portionPriceManager.CreatePortionPrices(model.PortionPrices);
            var portionPricesProduct = _portionPriceProductManager.CreatePortionPricesProduct(portionPrices, _mapper.Map<ProductDTO, Product>(newProductDTO.Entity));
            
            newProduct.PortionPrices = portionPricesProduct.ToHashSet();
            return newProduct;
        }    
        
        public ICollection<Product> GetProducts(int productGroupId)
        {
            var productsDTO = _dbContext.Products.Where(p => p.ProductGroupId == productGroupId);                          
                                        
            var products = new HashSet<Product>();
            foreach (var pdto in productsDTO)
            {
                products.Add(_mapper.Map<ProductDTO, Product>(pdto));
            }

            foreach (var p in products) 
            {
                var portionPricesProduct = _portionPriceProductManager.GetPortionPriceProducts(p.Id).ToHashSet();
                p.PortionPrices = portionPricesProduct;
            }

            return products;
        }
    }
}
