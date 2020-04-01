using DeliveryClub.Data.DTO.EntitiesDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryClub.Data.Context
{
    public interface IApplicationDbContext
    {
        DbSet<OrderDTO> Orders { get; set; }

        DbSet<OrderedProductDTO> OrderedProducts { get; set; }

        DbSet<PortionPriceDTO> PortionPrices { get; set; }

        DbSet<ProductDTO> Products { get; set; }

        DbSet<ProductGroupDTO> ProductGroups { get; set; }

        DbSet<RestaurantAdditionalInfoDTO> RestaurantAdditionalInfos { get; set; }

        DbSet<RestaurantDTO> Restaurants { get; set; }

        DbSet<ReviewDTO> Reviews { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
