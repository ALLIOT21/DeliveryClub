using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class ProductManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;
        private readonly PortionPriceManager _portionPriceManager;
        private readonly PortionPriceProductManager _portionPriceProductManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductManager(ApplicationDbContext dbContext,
                            PortionPriceManager portionPriceManager,
                            PortionPriceProductManager portionPriceProductManager,
                            IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
            _portionPriceManager = portionPriceManager;
            _portionPriceProductManager = portionPriceProductManager;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<Product> CreateProduct(ProductModel model, int productGroupId)
        {
            string fileName = null;
            if (model.Image != null)
            {
                var imgServ = new ImageService(_hostingEnvironment.WebRootPath);
                fileName = await imgServ.CreateImage(model.Image);
            }

            var newProduct = new Product
            {
                Name = model.Name,
                Description = model.Description,
                ImageName = fileName,
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

        public Product GetProduct(int id)
        {
            var productDTO = _dbContext.Products.Find(id);

            productDTO.ProductGroup = _dbContext.ProductGroups.Find(productDTO.ProductGroupId);

            var product = _mapper.Map<ProductDTO, Product>(productDTO);

            var portionPricesProduct = _portionPriceProductManager.GetPortionPriceProducts(product.Id).ToHashSet();

            product.PortionPrices = portionPricesProduct;

            return product;
        }

        public async Task<Product> UpdateProduct(ProductModel newModel, int productGroupId)
        {
            var product = GetProduct(newModel.Id);
            
            if (newModel.Image != null)
            {
                string fileName;
                var imgServ = new ImageService(_hostingEnvironment.WebRootPath);
                if (product.ImageName != null)
                {
                    imgServ.DeleteImage(product.ImageName);
                    fileName = await imgServ.CreateImage(newModel.Image);
                }
                else
                {
                    fileName = await imgServ.CreateImage(newModel.Image);
                }
                product.ImageName = fileName;
            }

            product.Id = newModel.Id;
            product.Name = newModel.Name;
            product.Description = newModel.Description;
            product.ProductGroupId = productGroupId;

            var portionPrices = _portionPriceManager.CreatePortionPrices(newModel.PortionPrices);
            _portionPriceProductManager.UpdatePortionPriceProduct(portionPrices, product);
            _dbContext.Products.Update(_mapper.Map<Product, ProductDTO>(product));
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProduct(int id)
        {
            var product = GetProduct(id);
            foreach (var ppp in product.PortionPrices)
            {
                _portionPriceProductManager.DeletePortionPriceProduct(ppp);
            }

            _dbContext.Products.Remove(_mapper.Map<Product, ProductDTO>(product));
            await _dbContext.SaveChangesAsync();
        }

        public void DeleteProductWithoutSaving(int id)
        {
            var product = GetProduct(id);
            foreach (var ppp in product.PortionPrices)
            {
                _portionPriceProductManager.DeletePortionPriceProduct(ppp);
            }

            _dbContext.Products.Remove(_mapper.Map<Product, ProductDTO>(product));
        }
    }
}
