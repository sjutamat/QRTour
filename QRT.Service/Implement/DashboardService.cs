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
        private readonly Imas_routeRepository _route;
        private readonly Imas_adminRepository _admin;
        private readonly Itrn_transactionRepository _trn;
        private readonly Itrn_answerRepository _answer;
        private readonly Imas_locationRepository _location;
        public DashboardService(
            Itrn_answerService itrnanswerservice
            ,Imas_routeRepository irouterepository
            ,Imas_adminRepository iadminrepository
            ,Itrn_transactionRepository itrnrepository
            ,Itrn_answerRepository ianswerrepository
            ,Imas_locationRepository ilocationrepository
            )
        {
            _answerService = itrnanswerservice;
            _route = irouterepository;
            _admin = iadminrepository;
            _trn = itrnrepository;
            _answer = ianswerrepository;
            _location = ilocationrepository;
        }


        public List<answerHeader> GetAnswer(int admin_id)
        {
            //dashboardViewModel model = new dashboardViewModel();
            List<answerHeader> headers = new List<answerHeader>();
            var currentSdate = DateTime.Now.Date;
            var currentEdate = DateTime.Now.AddDays(1);
          //  var admin = _admin.Filter(c => c.admin_id.Equals(admin_id)).SingleOrDefault();
            //get route
            //var route = _route.GetRouteByCompanmy(admin.company_id);
            var route = _route.Filter(c => c.adminid_create == admin_id && c.route_active == "A").ToList();
            if (route != null && route.Any())
            {
                for (int i = 0; i < route.Count(); i++)
                {
                    var r_id = route[i].route_id;
                    var trnData = _trn.Filter(c => c.route_id == r_id, inc => inc.mas_route).ToList();
                    
                    List<answerDetailList> detail = new List<answerDetailList>();


                    var location = _trn.Filter(c => c.route_id == r_id)
                                .GroupBy(g => new
                                {
                                    g.location_id
                                }).ToList();
                    
                    foreach (var item in location)
                    {
                        //all answer in each location
                        var location_key = item.Key;
                        var detaildata = _trn.Filter(c => c.location_id == location_key.location_id && (c.transaction_cdate >= currentSdate && c.transaction_cdate<= currentEdate), inc => inc.mas_location, incl => incl.mas_route).OrderBy(o=>o.transaction_cdate).FirstOrDefault();
                        var dataans = _answer.Filter(c => c.session_id == detaildata.session_id,inc=>inc.mas_emp).FirstOrDefault();
                        if (detaildata != null)
                        {
                           
                            answerDetailList d = new answerDetailList();
                            d.location_id = detaildata.location_id;
                            d.location_name = detaildata.mas_location.location_title;
                            d.answer_cdate = detaildata.transaction_cdate;
                            d.answer_cdate_string = detaildata.transaction_cdate.Value.ToShortTimeString();
                            d.answer_flag = (detaildata.transaction_answer == true) ? "Yes" : "No";
                            d.answer_comment = detaildata.transaction_comment;
                            d.answer_emp_name = dataans.mas_emp.emp_fname + " " + dataans.mas_emp.emp_surname;
                            detail.Add(d);
                        }

                    }
                    answerHeader h = new answerHeader();
                    h.route_id = route[i].route_id;
                    h.name = route[i].route_title;

                    //get last transaction on this route.


                    //get location
                    //var locationlist = _location.Filter(c => c.route_id == r_id).ToList().OrderBy(s=>s.seq_number).ToList();
                    //if (locationlist != null && locationlist.Any())
                    //{
                    //    foreach (var item in locationlist)
                    //    {
                    //        //all answer in each location
                    //        var ans = _answer.Filter(c => c.location_id == item.location_id, inc => inc.mas_location, incl => incl.mas_emp).ToList();
                    //        var jdata = _answer.Filter(c => c.location_id == item.location_id, inc => inc.mas_location, incl => incl.mas_emp)
                    //            .GroupBy(g => new {
                    //                g.answer_cdate,
                    //                g.emp_id
                    //            }).Select(grp => grp.ToList()).ToList();


                    //        if (ans != null && ans.Any())
                    //        {
                    //            var summary_flag = ans.Where(c => c.answer_txt == "No        ").ToList();
                    //            if (summary_flag.Count() == 0)
                    //            {
                    //                answerDetailList d = new answerDetailList();
                    //                d.location_id = item.location_id;
                    //                d.location_name = item.location_title;
                    //                d.answer_cdate = ans[0].answer_cdate;
                    //                d.answer_cdate_string = ans[0].answer_cdate.Value.ToShortTimeString();
                    //                d.answer_flag = "Yes";
                    //                d.answer_comment = ans[0].answer_txt;
                    //                d.answer_emp_name = ans[0].mas_emp.emp_fname + " " + ans[0].mas_emp.emp_surname;
                    //                detail.Add(d);
                    //            }
                    //            else
                    //            {
                    //                answerDetailList d = new answerDetailList();
                    //                d.location_id = item.location_id;
                    //                d.location_name = item.location_title;
                    //                d.answer_cdate = ans[0].answer_cdate;
                    //                d.answer_cdate_string = ans[0].answer_cdate.Value.ToShortTimeString();
                    //                d.answer_flag = "No";
                    //                d.answer_comment = ans[0].answer_txt;
                    //                d.answer_emp_name = ans[0].mas_emp.emp_fname + " " + ans[0].mas_emp.emp_surname;
                    //                detail.Add(d);
                    //            }
                    //        }

                    //    }
                    //}

                    h.start_time = (detail != null && detail.Any()) ? detail.Last().answer_cdate : DateTime.Now;
                    //h.start_time_string = h.start_time.Value.ToShortDateString();
                    h.start_time_string = h.start_time.Value.ToString();
                    h.end_time = (detail != null && detail.Any()) ? detail.First().answer_cdate : DateTime.Now;
                    //h.end_time_string = h.end_time.Value.ToShortDateString();
                    h.end_time_string = h.end_time.Value.ToString();
                    h.emp_name = (detail != null && detail.Any()) ? detail.First().answer_emp_name : "";
                    h.answerList = detail;
                    headers.Add(h);
                }
                //model.reportList.Add(headers);
            }
           

            return headers;
        }

        public List<answerHeader> GetAnswerFilter(dashboardViewModel model,int admin_id)
        {
            List<answerHeader> headers = new List<answerHeader>();
            
            var rt = (model.s_dashboard.route != null) ? Convert.ToInt32(model.s_dashboard.route) : 0;
            


            //DateTime startDate = DateTime.ParseExact(model.s_dashboard.date_start, "yyyy-MM-dd HH:mm:ss,fff",
            //                           System.Globalization.CultureInfo.InvariantCulture);

            //DateTime endDate = DateTime.ParseExact(model.s_dashboard.date_end, "yyyy-MM-dd HH:mm:ss,fff",
            //                           System.Globalization.CultureInfo.InvariantCulture);



            //var admin = _admin.Filter(c => c.admin_id.Equals(admin_id)).SingleOrDefault();
            //get route
            List<DB.mas_route> route = new List<DB.mas_route>();
            if (rt != 0)
            {
                route = _route.Filter(c => c.route_id == rt && c.adminid_create == admin_id && c.route_active == "A").ToList(); // only 1 row
            }
            else
            {
                route = _route.Filter(c => c.adminid_create == admin_id && c.route_active == "A").ToList(); // only 1 row
            }
            
            
            
            if (route != null && route.Any())
            {
                for (int i = 0; i < route.Count(); i++)
                {
                   
                    List<answerDetailList> detail = new List<answerDetailList>();
                    answerHeader h = new answerHeader();
                    var r_id = route[i].route_id;
                    h.route_id = route[i].route_id;
                    h.name = route[i].route_title;

                    //get location
                    var locationlist = _location.Filter(c => c.route_id == r_id).ToList().OrderBy(s => s.seq_number).ToList();
                    if (locationlist != null && locationlist.Any())
                    {
                        foreach (var item in locationlist)
                        {
                            //all answer in each location
                            //var ans = _answer.Filter(c => c.location_id == item.location_id && (c.answer_cdate<= sDate && c.answer_cdate >= eDate), inc => inc.mas_location, incl => incl.mas_emp).ToList();

                            var ans = _answer.Filter(c => c.location_id == item.location_id , inc => inc.mas_location, incl => incl.mas_emp).ToList();

                            var jdata = _answer.Filter(c => c.location_id == item.location_id, inc => inc.mas_location, incl => incl.mas_emp)
                                .GroupBy(g => new {
                                    g.answer_cdate,
                                    g.emp_id
                                }).Select(grp => grp.ToList()).ToList();


                            if (ans != null && ans.Any())
                            {
                                var summary_flag = ans.Where(c => c.answer_txt == "No        ").ToList();
                                if (summary_flag.Count() == 0)
                                {
                                    answerDetailList d = new answerDetailList();
                                    d.location_id = item.location_id;
                                    d.location_name = item.location_title;
                                    d.answer_cdate = ans[0].answer_cdate;
                                    d.answer_cdate_string = ans[0].answer_cdate.Value.ToShortTimeString();
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
                                    d.answer_cdate_string = ans[0].answer_cdate.Value.ToShortTimeString();
                                    d.answer_flag = "No";
                                    d.answer_comment = ans[0].answer_txt;
                                    d.answer_emp_name = ans[0].mas_emp.emp_fname + " " + ans[0].mas_emp.emp_surname;
                                    detail.Add(d);
                                }
                            }

                        }
                    }

                    var sDate = new DateTime();
                    var eDate = new DateTime();
                    //if (model.s_dashboard.date_start != null)
                    //    sDate = Convert.ToDateTime(model.s_dashboard.date_start);
                    //if (model.s_dashboard.date_end != null)
                    //    eDate = Convert.ToDateTime(model.s_dashboard.date_end);


                    if (model.s_dashboard.date_start != null && model.s_dashboard.date_end !=null)
                    {
                        sDate = Convert.ToDateTime(model.s_dashboard.date_start);
                        eDate = Convert.ToDateTime(model.s_dashboard.date_end);
                        detail = detail.Where(c => c.answer_cdate >= sDate && c.answer_cdate <= eDate).ToList();
                        h.answerList = detail;
                    }
                    else if (model.s_dashboard.date_start != null && model.s_dashboard.date_end == null)
                    {
                        sDate = Convert.ToDateTime(model.s_dashboard.date_start);
                        eDate = Convert.ToDateTime(model.s_dashboard.date_start).AddDays(1);
                        detail = detail.Where(c => c.answer_cdate >= sDate || c.answer_cdate <= eDate).ToList();
                        h.answerList = detail;
                    }
                    else if (model.s_dashboard.date_start == null && model.s_dashboard.date_end != null)
                    {
                        eDate = Convert.ToDateTime(model.s_dashboard.date_start).AddDays(1);
                        detail = detail.Where(c => c.answer_cdate >= sDate && c.answer_cdate <= eDate).ToList();
                        h.answerList = detail;
                    }
                    else
                    {
                        h.answerList = detail;
                    }

                    

                    h.start_time = (detail != null && detail.Any()) ? detail.First().answer_cdate : DateTime.Now;
                    h.start_time_string = h.start_time.Value.ToShortDateString();
                    h.end_time = (detail != null && detail.Any()) ? detail.Last().answer_cdate : DateTime.Now;
                    h.end_time_string = h.end_time.Value.ToShortDateString();
                    h.emp_name = (detail != null && detail.Any()) ? detail.First().answer_emp_name : "";
                    
                    headers.Add(h);
                }
                //model.reportList.Add(headers);
            }


            return headers;
        }
    }
}
