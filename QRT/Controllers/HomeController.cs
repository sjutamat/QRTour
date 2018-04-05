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
        public HomeController(IDashboardService idashboardService)
        {
            _dashboard = idashboardService;
        }

        // GET: Home
        public ActionResult Index()
        {
            dashboardViewModel dashboardData = new dashboardViewModel();
            dashboardData.reportList = _dashboard.GetAnswer(admin.id);
            //var data = 
            return View(dashboardData);
        }
    }
}