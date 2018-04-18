using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.DB;
using QRT.Domain;
using QRT.Domain.Interface.Service;
using QRT.Domain.Interface.Repository;
using Op3ration.ExceptionHandler;
using QRT.Domain.ViewModel;

namespace QRT.Service.Implement
{
    public class mas_routeService:Imas_routeService
    {
        #region utility
        private readonly Imas_routeRepository _route;
        private readonly Imas_adminRepository _admin;
        private readonly Imas_companyService _compservice;
       
        public mas_routeService(Imas_routeRepository imas_routerepository
            ,Imas_companyService imascompservice
            ,Imas_adminRepository imas_adminrepository)
        {
            _route = imas_routerepository;
            _admin = imas_adminrepository;
            _compservice = imascompservice;
        }
        #endregion

        public m_routeViewModel GetRoute(UserViewModel user)
        {
            m_routeViewModel emp = new m_routeViewModel();
            emp.company = _compservice.GetCompany();
            return emp;
        }

        public m_routeViewModel GetAllRoute(UserViewModel user)
        {
            m_routeViewModel model = new m_routeViewModel();
            var data = _route.Filter(c=>c.adminid_create == user.id).ToList();
            if (data != null)
            {
                var route = data.Select(x => new RouteData()
                {
                    id = x.route_id,
                    title = x.route_title,
                    description = x.route_desc,
                    status = x.route_active == "A" ? "Active" : "Deactive",
                    created_date = x.route_cdate,
                }).OrderByDescending(c => c.created_date).ToList();
                model.s_route = new SearchData();
                model.s_routeData = route;
                model.company = _compservice.GetCompany();
                return model;
            }
            else
            {
                model.s_routeData = new List<RouteData>();
                return model;

            }
        }

        public m_routeViewModel FilterRoute(m_routeViewModel model, UserViewModel user)
        {
            var id = (model.s_route.id == null) ? 0 : Convert.ToInt32(model.s_route.id);
            var title = model.s_route.title;

            var data = _route.Filter(c=>c.adminid_create == user.id).ToList();

            if (id!=0 && model.s_route.title == null)
            {
                data = data.Where(c => c.route_id == id && c.route_active == "A").ToList();
            }
            else if (id == 0 && model.s_route.title !=null)
            {
                data = data.Where(c => c.route_title.Contains(model.s_route.title) && c.route_active == "A").ToList();
            }
            else if(id != 0 && model.s_route.title != null)
            {
                data = data.Where(c => c.route_title.Contains(model.s_route.title) && c.route_id == id && c.route_active == "A").ToList();
            }
            else
            {
                data = _route.Filter(c => c.route_active == "A").ToList();
            }
            

            if (data != null)
            {
                var route = data.Select(x => new RouteData()
                {
                    id = x.route_id,
                    title = x.route_title,
                    description = x.route_desc,
                    status = x.route_active == "A" ? "Active" : "Deactive",
                    created_date = x.route_cdate,
                }).OrderByDescending(c => c.created_date).ToList();

                model.s_routeData = route;
                //model.s_route.id = id;
                //model.s_route.title = title;
                model.company = _compservice.GetCompany();
                return model;
            }
            else
            {
                model.s_routeData = new List<RouteData>();
                return model;

            }

        }

        public m_routeViewModel GetById(long id, UserViewModel user)
        {
            var data = _route.Filter(c => c.route_id == id && c.adminid_create == user.id).SingleOrDefault();
            m_routeViewModel route = new m_routeViewModel();
            route.id = data.route_id;
            route.title = data.route_title;
            route.description = data.route_desc;
            route.status = data.route_active == "A" ? "On" : "Off";
            route.created_date = data.route_cdate;
            route.created_by = data.adminid_create;
            route.company = _compservice.GetCompany();
            return route;
        }

        public void Save(m_routeViewModel model, UserViewModel user)
        {
            if (model.id == 0)
            {
                try
                {
                    mas_route route = new mas_route();
                    route.route_title = model.title;
                    route.route_desc = model.description;
                    route.route_active = model.status == "On" ? "A" : "D";
                    route.route_cdate = DateTime.Now;
                    route.adminid_create = user.id;
                    route.route_udate = DateTime.Now;
                    route.adminid_update = user.id;
                    _route.Create(route);
                }
                catch (Exception ex)
                {
                    if (ex.IsValidateHandler())
                        throw ex.ToValidateHandler();
                    throw new ValidateHandler(MessageLevel.Error, "Error:'" + ex.Message + "'");
                }
                
            }
            else
            {
                var oldData = _route.Filter(c => c.route_id == model.id).SingleOrDefault();
                if (oldData != null)
                {
                    try
                    {
                        oldData.route_title = model.title;
                        oldData.route_desc = model.description;
                        oldData.route_active = model.status == "On" ? "A" : "D";
                        oldData.route_udate = DateTime.Now;
                        oldData.adminid_update = user.id;
                        _route.Update(oldData);
                    }
                    catch (Exception ex)
                    {
                        if (ex.IsValidateHandler())
                            throw ex.ToValidateHandler();
                        throw new ValidateHandler(MessageLevel.Error, "Error:'" + ex.Message + "'");
                    }
                       
                }
                
            }
        }

        public void UpdateStatus(long id, UserViewModel user)
        {
            var oldData = _route.Filter(c => c.route_id == id).SingleOrDefault();
            try
            {
                oldData.route_active = "D";
                oldData.route_udate = DateTime.Now;
                oldData.adminid_update = user.id;
                _route.Update(oldData);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<route_item> GetRouteItem(UserViewModel user)
        {
            //var admin = _admin.Filter(c => c.admin_id == user.id).SingleOrDefault();
            List<route_item> itemRoute = new List<route_item>();
            var routeData = _route.Filter(c => c.route_active == "A" && c.adminid_create == user.id).ToList();
            if (routeData != null)
            {
                foreach (var item in routeData)
                {
                    route_item r = new route_item();
                    r.id = item.route_id;
                    r.text = item.route_title;
                    itemRoute.Add(r);
                }
            }
            return itemRoute;
        }
        
    }
}
