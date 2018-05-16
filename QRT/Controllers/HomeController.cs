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
                dashboardData.s_dashboard = new SearchDataDashboard();
                ViewBag.AdminName = admin.username;
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
                dashboardData.s_dashboard = model.s_dashboard;
                ViewBag.AdminName = admin.username;
                return View(dashboardData);
            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }

        public ActionResult Authen()
        {
            ViewBag.AdminName = admin.username;
            return View("AuthenPage");
        }
    }
}