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
        //private readonly Itrn_answerService _answerService;
        private readonly Imas_routeRepository _route;
        private readonly Imas_adminRepository _admin;
        private readonly Itrn_transactionRepository _trn;
        private readonly Itrn_answerRepository _answer;
        private readonly Imas_locationRepository _location;
        private readonly Imas_empRepository _emp;
        public DashboardService(
            //Itrn_answerService itrnanswerservice
            Imas_routeRepository irouterepository
            ,Imas_adminRepository iadminrepository
            ,Itrn_transactionRepository itrnrepository
            ,Itrn_answerRepository ianswerrepository
            ,Imas_locationRepository ilocationrepository
            ,Imas_empRepository iemprepository
            )
        {
           // _answerService = itrnanswerservice;
            _route = irouterepository;
            _admin = iadminrepository;
            _trn = itrnrepository;
            _answer = ianswerrepository;
            _location = ilocationrepository;
            _emp = iemprepository;
        }


        public List<answerHeader> GetAnswer(int admin_id)
        {
            var route1 = _route.Filter(c => c.adminid_create == admin_id && c.route_active == "A").ToList();
            var startDate = DateTime.Now.Date;
            var endDate = DateTime.Now.AddDays(1).Date;
            List<answerHeader> routeList = new List<answerHeader>();
            if (route1.Count() > 0 && route1.Any())
            {
                foreach (var ritem in route1)
                {
                    answerHeader r = new answerHeader();
                    r.name = ritem.route_title;
                    r.start_time = null;
                    r.end_time = null;
                    

                    List<employeeData> empList = new List<employeeData>();
                    var trn = _trn.Filter(c => c.route_id == ritem.route_id)
                        .Where(c=>c.transaction_cdate >= startDate && c.transaction_cdate < endDate )
                        .GroupBy(g => g.session_id)
                        .Select(grp => grp.ToList())
                        .ToList();
                    if (trn.Count() > 0 && trn.Any())
                    {
                        
                        foreach (var t in trn)
                        {
                            
                            employeeData e = new employeeData();
                            if (t.Any())
                            {
                                List<answerDetailList> ansList = new List<answerDetailList>();
                                foreach (var a in t)
                                {
                                    answerDetailList d = new answerDetailList();
                                    d.location_id = a.location_id;
                                    d.location_name = _location.Filter(c => c.location_id == a.location_id).Select(s => s.location_title).SingleOrDefault();
                                    d.answer_cdate = a.transaction_cdate;
                                    d.answer_cdate_string = a.transaction_cdate.Value.ToString("HH:mm");
                                    d.answer_flag = a.transaction_answer == true ? "Yes" : "No";
                                    d.answer_comment = a.transaction_comment;
                                    d.answer_emp_name = a.session_id;
                                    ansList.Add(d);
                                }
                                var emp_id = Convert.ToInt32(t.Select(s=>s.session_id).FirstOrDefault());
                                var employee = _emp.Filter(c => c.emp_id == emp_id).SingleOrDefault();
                                e.id = employee.emp_id;
                                e.name = employee.emp_fname + " " + employee.emp_surname;
                                e.answerSummary = ansList;
                                e.start_time = ansList.Select(s => s.answer_cdate).First();
                                e.end_time = ansList.Select(s => s.answer_cdate).Last();
                                e.start_time_string = ansList.Select(s => s.answer_cdate).First().Value.ToString("dd/MM/yyyy HH:mm");
                                e.end_time_string = ansList.Select(s => s.answer_cdate).Last().Value.ToString("dd/MM/yyyy HH:mm");
                                empList.Add(e);
                            }
                        }
                    }
                    r.start_time_string = string.Empty;
                    r.end_time_string = string.Empty;
                    r.listEmpData = empList;
                    routeList.Add(r);
                }
            }
            return routeList;
        }

        public List<answerHeader> GetAnswerFilter(dashboardViewModel model,int admin_id)
        {
            var route = model.s_dashboard.route !="" ? Convert.ToInt32(model.s_dashboard.route) : 0;
            var startDate = !String.IsNullOrEmpty(model.s_dashboard.date_start) ? Convert.ToDateTime(model.s_dashboard.date_start) : DateTime.Now.Date;
            var endDate = !String.IsNullOrEmpty(model.s_dashboard.date_end) ? Convert.ToDateTime(model.s_dashboard.date_end) : DateTime.Now.AddDays(1).Date;

            var route1 = _route.Filter(c => c.adminid_create == admin_id && c.route_active == "A").ToList();
            if (route != 0 )
            {
                route1 = route1.Where(c => c.adminid_create == admin_id && c.route_id == route).ToList();
            }
            
            List<answerHeader> routeList = new List<answerHeader>();
            if (route1.Count() > 0 && route1.Any())
            {
                foreach (var ritem in route1)
                {
                    answerHeader r = new answerHeader();
                    r.name = ritem.route_title;
                    r.start_time = null;
                    r.end_time = null;


                    List<employeeData> empList = new List<employeeData>();
                    var trn = _trn.Filter(c => c.route_id == ritem.route_id)
                        .Where(c => c.transaction_cdate >= startDate && c.transaction_cdate < endDate)
                        .GroupBy(g => g.session_id)
                        .Select(grp => grp.ToList())
                        .ToList();
                    if (trn.Count() > 0 && trn.Any())
                    {

                        foreach (var t in trn)
                        {

                            employeeData e = new employeeData();
                            if (t.Any())
                            {
                                List<answerDetailList> ansList = new List<answerDetailList>();
                                foreach (var a in t)
                                {
                                    answerDetailList d = new answerDetailList();
                                    d.location_id = a.location_id;
                                    d.location_name = _location.Filter(c => c.location_id == a.location_id).Select(s => s.location_title).SingleOrDefault();
                                    d.answer_cdate = a.transaction_cdate;
                                    d.answer_cdate_string = a.transaction_cdate.Value.ToString("HH:mm");
                                    d.answer_flag = a.transaction_answer == true ? "Yes" : "No";
                                    d.answer_comment = a.transaction_comment;
                                    d.answer_emp_name = a.session_id;
                                    ansList.Add(d);
                                }
                                var emp_id = Convert.ToInt32(t.Select(s => s.session_id).FirstOrDefault());
                                var employee = _emp.Filter(c => c.emp_id == emp_id).SingleOrDefault();
                                e.id = employee.emp_id;
                                e.name = employee.emp_fname + " " + employee.emp_surname;
                                e.answerSummary = ansList;
                                e.start_time = ansList.Select(s => s.answer_cdate).First();
                                e.end_time = ansList.Select(s => s.answer_cdate).Last();
                                e.start_time_string = ansList.Select(s => s.answer_cdate).First().Value.ToString("dd/MM/yyyy HH:mm");
                                e.end_time_string = ansList.Select(s => s.answer_cdate).Last().Value.ToString("dd/MM/yyyy HH:mm");
                                empList.Add(e);
                            }
                        }
                    }
                    r.start_time_string = string.Empty;
                    r.end_time_string = string.Empty;
                    r.listEmpData = empList;
                    routeList.Add(r);
                }
            }
            return routeList;
        }
    }
}
