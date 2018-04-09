using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;

namespace QRT.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IDashboardService _dashboard;
        private readonly Imas_routeService _route;
        public HomeController(IDashboardService idashboardService,
            Imas_routeService irouteService
            )
        {
            _route = irouteService;
            _dashboard = idashboardService;
        }

        // GET: Home
        public ActionResult Index()
        {
            if (admin != null)
            {
                dashboardViewModel dashboardData = new dashboardViewModel();
                dashboardData.route = _route.GetRouteItem(admin);
                dashboardData.reportList = _dashboard.GetAnswer(admin.id);
                //var data = 
                return View(dashboardData);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
           
        }

        [HttpPost]
        public ActionResult Index(dashboardViewModel model)
        {
            if (admin != null)
            {
                dashboardViewModel dashboardData = new dashboardViewModel();
                dashboardData.route = _route.GetRouteItem(admin);
                dashboardData.reportList = _dashboard.GetAnswerFilter(model, admin.id);
                //var data = 
                return View(dashboardData);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }

        }
    }
}