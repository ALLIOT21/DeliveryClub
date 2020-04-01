﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Web.Controllers
{
    [Authorize(Roles = "SuperUser")]
    public class SuperUserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}