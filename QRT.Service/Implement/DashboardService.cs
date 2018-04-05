using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.Domain.Interface.Repository;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;

namespace QRT.Service.Implement
{
     public class DashboardService : IDashboardService
    {
        private readonly Itrn_answerService _answerService;
        public readonly Imas_routeRepository _route;
        public readonly Imas_adminRepository _admin;
        public readonly Itrn_answerRepository _answer;
        private readonly Imas_locationRepository _location;
        public DashboardService(
            Itrn_answerService itrnanswerservice
            ,Imas_routeRepository irouterepository
            ,Imas_adminRepository iadminrepository
            ,Itrn_answerRepository ianswerrepository
            ,Imas_locationRepository ilocationrepository
            )
        {
            _answerService = itrnanswerservice;
            _route = irouterepository;
            _admin = iadminrepository;
            _answer = ianswerrepository;
            _location = ilocationrepository;
        }


        public List<answerHeader> GetAnswer(int admin_id)
        {
            //dashboardViewModel model = new dashboardViewModel();
            List<answerHeader> headers = new List<answerHeader>();
            
            var admin = _admin.Filter(c => c.admin_id.Equals(admin_id)).SingleOrDefault();
            //get route
            //var route = _route.GetRouteByCompanmy(admin.company_id);
            var route = _route.Filter(c => c.company_id == admin.company_id && c.route_active == "A").ToList();
            if (route != null && route.Any())
            {
                for (int i = 0; i < route.Count(); i++)
                {
                    List<answerDetailList> detail = new List<answerDetailList>();
                    answerHeader h = new answerHeader();
                    var r_id = route[i].route_id;
                    h.route_id = route[i].route_id;
                    h.name = route[i].route_title;
                    
                    var jj = _location.Filter(c => c.route_id == r_id);
                    //get location
                    var locationlist = _location.Filter(c => c.route_id == r_id).ToList();
                    if (locationlist != null && locationlist.Any())
                    {
                        foreach (var item in locationlist)
                        {
                            //all answer
                            var ans = _answer.Filter(c => c.location_id == item.location_id, inc => inc.mas_location, incl => incl.mas_emp).ToList();
                            if (ans != null && ans.Any())
                            {
                                var summary_flag = ans.Where(c => c.answer_flag == "No        ").ToList();
                                if (summary_flag.Count() == 0)
                                {
                                    answerDetailList d = new answerDetailList();
                                    d.location_id = item.location_id;
                                    d.location_name = item.location_title;
                                    d.answer_cdate = ans[0].answer_cdate;
                                    d.answer_flag = "Yes";
                                    d.answer_comment = ans[0].answer_txt;
                                    d.answer_emp_name = ans[0].mas_emp.emp_fname + " " + ans[0].mas_emp.emp_surname;
                                    detail.Add(d);
                                }
                                else
                                {
                                    answerDetailList d = new answerDetailList();
                                    d.location_id = item.location_id;
                                    d.location_name = item.location_title;
                                    d.answer_cdate = ans[0].answer_cdate;
                                    d.answer_flag = "No";
                                    d.answer_comment = ans[0].answer_txt;
                                    d.answer_emp_name = ans[0].mas_emp.emp_fname + " " + ans[0].mas_emp.emp_surname;
                                    detail.Add(d);
                                }
                            }
                            
                        }
                    }

                    h.start_time =(detail != null && detail.Any())? detail.First().answer_cdate : DateTime.Now;
                    h.end_time = (detail != null && detail.Any()) ? detail.Last().answer_cdate : DateTime.Now;
                    h.emp_name = (detail != null && detail.Any()) ? detail.First().answer_emp_name : "";
                    h.answerList = detail;
                    headers.Add(h);
                }
                //model.reportList.Add(headers);
            }
           

            return headers;
        }


    }
}
