using DeliveryClub.Domain.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeliveryClub.Web.API
{
    [Route("api/[controller]")]
    public class ProductGroupsController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public ProductGroupsController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        [Route("{productGroupName}/[action]")]
        public async Task<bool> PortionPriced(string productGroupName)
        {
            var hasPortionPrices = await _adminService.HasPortionPrices(productGroupName);
            return hasPortionPrices;
        }
    }
}
