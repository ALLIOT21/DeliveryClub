using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Logic.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeliveryClub.Web.API
{
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<bool> HasPortionPrices(string productGroupName)
        {
            var hasPortionPrices = await _adminService.HasPortionPrices(HttpContext.User, productGroupName);
            return hasPortionPrices;
        }
    }
}
