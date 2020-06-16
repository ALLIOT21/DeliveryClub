using DeliveryClub.Domain.AuxiliaryModels.Guest;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Web.ViewModels.Guest;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeliveryClub.Web.Controllers
{
    public class GuestController : Controller
    {
        private readonly IGuestService _guestService;
        private readonly Mapper _mapper;
        private const int PAGE_SIZE = 3;

        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public IActionResult Index(int page = 1)
        {
            var currentUserRole = _guestService.GetUserRole();
            if (currentUserRole != null)
            {
                return RedirectToAction("Index", currentUserRole);
            }
            else
            {              
                ICollection<RestaurantPartialViewModel> rpvms = new List<RestaurantPartialViewModel>();
                foreach (var rpm in _guestService.GetRestaurantsPartially())
                {
                    rpvms.Add(_mapper.Map<RestaurantPartialModel, RestaurantPartialViewModel>(rpm));
                }

                var count = rpvms.Count;
                var items = rpvms.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();

                PageViewModel pageViewModel = new PageViewModel(count, page, PAGE_SIZE);
                IndexViewModel viewModel = new IndexViewModel
                {
                    PageViewModel = pageViewModel,
                    RestaurantPartialViewModels = items
                };
                
                return View(viewModel);
            }
        }

    }
}
