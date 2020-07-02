using DeliveryClub.Domain.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DeliveryClub.Web.API
{
    [Route("api/[controller]")]
    public class DispatcherController : ControllerBase
    {
        private readonly ISuperUserService _superUserService;

        public DispatcherController(ISuperUserService superUserService)
        {
            _superUserService = superUserService;
        }

        [HttpPost]
        [Route("{id}/[action]")]
        public async Task Active(int id)
        {
            await _superUserService.ToggleDispatcherActivity(id);
        }
    }
}
