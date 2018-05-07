using Op3ration.ExceptionHandler;
using QRT.DB;
using QRT.Domain.Interface.Repository;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.Service.Implement
{
    public class mas_compService : Imas_companyService
    {
        #region Utility
        private readonly Imas_companyRepository _comp;
        private ValidateHandler Validator;
        public mas_compService(Imas_companyRepository imas_comprepository)
        {
            _comp = imas_comprepository;
        }


        private ValidateHandler ValidateModel(m_empViewModel model)
        {
            Validator = new ValidateHandler();
            return Validator;
        }
        #endregion
        public List<comp_item> GetCompany()
        {
            List<comp_item> itemComp = new List<comp_item>();
            var compData = _comp.All().ToList();
            if (compData != null)
            {
                foreach (var item in compData)
                {
                    comp_item c = new comp_item();
                    c.id = item.comp_id;
                    c.text = item.comp_name;
                    c.flag_internal = item.flag_internal;
                    itemComp.Add(c);
                }
            }
            return itemComp;
        }



        public List<comp_item> GetCompanyByAdmin(long adminId)
        {
            List<comp_item> itemComp = new List<comp_item>();
            var compData = _comp.Filter(c => c.admin_id == adminId).ToList();
            if (compData != null)
            {
                foreach (var item in compData)
                {
                    comp_item c = new comp_item();
                    c.id = item.comp_id;
                    c.text = item.comp_name;
                    c.flag_internal = item.flag_internal;
                    itemComp.Add(c);
                }
            }
            return itemComp;
        }


        public comp_item GetById(long id)
        {
            var compData = _comp.Filter(c => c.comp_id == id).SingleOrDefault();
            comp_item cmodel = new comp_item();
            if (compData != null)
            {
                cmodel.id = compData.comp_id;
                cmodel.text = compData.comp_name;
                cmodel.flag_internal = compData.flag_internal;
            }
            return cmodel;
        }


        public m_companyViewModel GetAllCompany(UserViewModel user)
        {
            m_companyViewModel model = new m_companyViewModel();
            var data = _comp.Filter(c => c.admin_id == user.id).ToList();
            if (data != null)
            {
                var company = data.Select(x => new Comp_data()
                {
                    id = x.comp_id,
                    title = x.comp_name,
                    description = x.comp_desc,
                    flag_internal = x.flag_internal == true ? "Yes" : "No",
                    admin_id = x.admin_id
                }).OrderByDescending(o => o.id).ToList();

                model.s_comp = new SearchComp();
                model.s_compData = company;
                return model;
            }
            else
            {
                model.s_compData = new List<Comp_data>();
                return model;
            }
        }


        public m_companyViewModel GetFilterCompany(m_companyViewModel model, UserViewModel user)
        {
            List<mas_company> data = new List<mas_company>();
            if (model.s_comp != null)
            {
                var id = (model.s_comp.id == null) ? 0 : Convert.ToInt32(model.s_comp.id);
                var title = model.s_comp.title;

                data = _comp.Filter(c => c.admin_id == user.id).ToList();

                if (id != 0 && model.s_comp.title == null)
                {
                    data = data.Where(c => c.comp_id == id).ToList();
                }


                if (id == 0 && model.s_comp.title != null)
                {
                    data = data.Where(c => c.comp_name.ToUpper().Contains(model.s_comp.title.ToUpper())).ToList();
                }


                if(model.s_comp.title != null && id != 0) /* (id != 0 && model.s_comp.title != null)*/
                {
                    data = data.Where(c => c.comp_name.ToUpper().Contains(model.s_comp.title.ToUpper()) && c.comp_id == id).ToList();
                }
            }
            else
            {
                data = _comp.Filter(c => c.admin_id == user.id).ToList();
            }
                


                //var data = _comp.Filter(c => c.admin_id == user.id).ToList();
                if (data != null)
                {
                    var company = data.Select(x => new Comp_data()
                    {
                        id = x.comp_id,
                        title = x.comp_name,
                        description = x.comp_desc,
                        flag_internal = x.flag_internal == true ? "Yes" : "No",
                        admin_id = x.admin_id
                    }).OrderByDescending(o => o.id).ToList();

                    //model.s_comp = new SearchComp();
                    model.s_compData = company;
                    return model;
                }
                else
                {
                    model.s_compData = new List<Comp_data>();
                    return model;
                }
            
            
        }



        public void Save(m_companyViewModel model, UserViewModel user)
        {
            if (model.id == 0)
            {
                try
                {
                    mas_company comp = new mas_company();
                    comp.comp_name = model.title;
                    comp.comp_desc = model.description;
                    comp.flag_internal = model.flag_internal == "On" ? true : false;
                    comp.admin_id = user.id;
                    _comp.Create(comp);
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
                var oldData = _comp.Filter(c => c.comp_id == model.id).SingleOrDefault();
                if (oldData != null)
                {
                    try
                    {
                        oldData.comp_name = model.title;
                        oldData.comp_desc = model.description;
                        oldData.flag_internal = model.flag_internal == "On" ? true : false;
                        oldData.admin_id = user.id;
                        _comp.Update(oldData);
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



        public m_companyViewModel GetCompanyById(long id, UserViewModel user)
        {
            var data = _comp.Filter(c => c.comp_id == id && c.admin_id == user.id).SingleOrDefault();
            m_companyViewModel comp = new m_companyViewModel();
            if (data != null)
            {
                comp.id = data.comp_id;
                comp.title = data.comp_name;
                comp.description = data.comp_desc;
                comp.flag_internal = data.flag_internal == true ? "On" : "Off";
                comp.admin_id = data.admin_id;
            }

            return comp;
        }


        public void UpdateStatus(long id, UserViewModel user)
        {
            var oldData = _comp.Filter(c => c.comp_id == id).SingleOrDefault();
            try
            {
                //oldData.route_active = "D";
                //oldData.route_udate = DateTime.Now;
                //oldData.adminid_update = user.id;
                //_route.Update(oldData);
                _comp.Delete(oldData);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}

