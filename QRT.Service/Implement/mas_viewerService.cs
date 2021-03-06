﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Op3ration.ExceptionHandler;
using QRT.DB;
using QRT.Domain.Interface.Repository;
using QRT.Domain.Interface.Service;
using QRT.Domain.ViewModel;

namespace QRT.Service.Implement
{
    public class mas_viewerService : Imas_viewerService
    {
        private readonly Imas_viewerRepository _viewer;
        private readonly Imas_adminRepository _admin;
        private ValidateHandler Validator;
        public mas_viewerService(
            Imas_viewerRepository imas_viewerrepository
            ,Imas_adminRepository imas_adminrepository
            )
        {
            _viewer = imas_viewerrepository;
            _admin = imas_adminrepository;
        }


        private ValidateHandler ValidateModel(m_empViewModel model)
        {
            Validator = new ValidateHandler();
            return Validator;
        }


        public m_viewerViewModel GetAllReportViewer(UserViewModel user)
        {
            m_viewerViewModel model = new m_viewerViewModel();
            var data = _viewer.Filter(c => c.admin_id == user.id).ToList();
            if (data != null && data.Count != 0)
            {
                var viewer = data.Select(s => new ViewerData()
                {
                    id = s.user_id.ToString(),
                    name = s.user_fname + " " + s.user_surname,
                    first_name = s.user_fname,
                    last_name = s.user_surname,
                    status = s.user_active.Trim() == "A" ? "Active" : "Deactive"
                }).ToList();

                model.s_viewerData = viewer;
                model.s_viewer = new SearchViewer();
                return model;
            }
            else
            {
                model.s_viewerData = new List<ViewerData>();
                model.s_viewer = new SearchViewer();
                return model;
            } 
        }



        public m_viewerViewModel FilterViewer(m_viewerViewModel model, UserViewModel user)
        {
            var name = model.s_viewer.name;
            var fname = string.Empty;
            var lname = string.Empty;
            if (name != null)
            {
                string[] words = name.Split(' ');
                if (words.Count() > 1 && words.Any())
                {
                    fname = words[0];
                    lname = words[1];
                }
                else
                {
                    fname = words[0];
                }
            }


            //var code = model.s_viewer.id;
            var id = Convert.ToInt32(model.s_viewer.id);
            var status = model.s_viewer.status;
            var data = _viewer.Filter(c => c.admin_id == user.id).ToList();

            if (!String.IsNullOrEmpty(fname) && !string.IsNullOrEmpty(lname))
            {
                data = data.Where(c => c.user_fname.ToUpper().Contains(fname.ToUpper()) || c.user_surname.ToUpper().Contains(lname.ToUpper())).ToList();
            }

            if (!String.IsNullOrEmpty(fname) && String.IsNullOrEmpty(lname))
            {
                data = data.Where(c => c.user_fname.ToUpper().Contains(name.ToUpper()) || c.user_surname.ToUpper().Contains(name.ToUpper()) || c.user_fname.ToUpper().Equals(name.ToUpper()) || c.user_surname.ToUpper().Equals(name.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(model.s_viewer.id))
            {
                data = data.Where(c => c.user_id == id).ToList();
            }

            if (!string.IsNullOrEmpty(model.s_viewer.status))
            {
                data = data.Where(c => c.user_active == status).ToList();
            }


            if (data != null)
            {
                var viewer = data.Select(s => new ViewerData()
                {
                    id = s.user_id.ToString(),
                    name = s.user_fname + " " + s.user_surname,
                    first_name = s.user_fname,
                    last_name = s.user_surname,
                    status = s.user_active.Trim() == "A" ? "Active" : "Deactive"
                }).ToList();

                model.s_viewerData = viewer;
                //model.s_viewer = new SearchViewer();
                return model;
            }
            else
            {
                model.s_viewerData = new List<ViewerData>();
                //model.s_viewer = new SearchViewer();
                return model;

            }
        }


        public m_viewerViewModel GetById(int id, UserViewModel user)
        {
            var data = _viewer.Filter(c => c.user_id == id && c.admin_id == user.id).SingleOrDefault();
            m_viewerViewModel viewer = new m_viewerViewModel();
            if (data != null)
            {
                viewer.id = data.user_id;
                viewer.username = data.user_login;
                viewer.first_name = data.user_fname;
                viewer.last_name = data.user_surname;
                viewer.status = data.user_active.Trim() == "A" ? "On" : "Off";
            }
            else
            {
                viewer.id = id;
            }

            return viewer;
        }


        public void Save(m_viewerViewModel model, UserViewModel user)
        {
            if (model.id == 0)
            {

                try
                {
                    mas_user viewer = new mas_user();
                    viewer.user_fname = model.first_name;
                    viewer.user_surname = model.last_name;
                    viewer.user_login = model.username;
                    viewer.user_password = model.password;
                    viewer.user_active = model.status == "On" ? "A": "D";
                    viewer.authen_edit = 0;
                    viewer.authen_view = 1;
                    viewer.user_cdate = DateTime.Now;
                    viewer.admin_id = user.id;
                    _viewer.Create(viewer);
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
                var oldData = _viewer.Filter(c => c.user_id == model.id).SingleOrDefault();
                if (oldData != null)
                {
                    try
                    {
                        oldData.user_fname = model.first_name;
                        oldData.user_surname = model.last_name;
                        oldData.user_active = model.status == "On" ? "A" : "D";
                        oldData.admin_id = user.id;
                        _viewer.Update(oldData);
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
            var oldData = _viewer.Filter(c => c.user_id == id).SingleOrDefault();
            try
            {
                oldData.user_active = "D";
                _viewer.Update(oldData);
                //_comp.Delete(oldData);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public string ChkDuplicate(m_viewerViewModel model)
        {
            //var adm = _admin.All().ToList();
            //var rv = _viewer.All().ToList();

            var data = (from a in _admin.All()
                        select a.admin_user == model.username && a.admin_password == model.password)
                        .Union(from v in _viewer.All()
                         select v.user_login == model.username && v.user_password == model.password).ToList();

            //var materialTypes = (from u in _viewer.All().SelectMany(mas_user)
            //                            select )
            //           .Union(from u in result.Users
            //                  select SqlFunctions.StringConvert((double)u.UserId)).ToList();
            //var data = _viewer.Filter(c => c.emp_fname.Equals(model.fname) && c.emp_surname.Equals(model.sname) && c.emp_comp.Equals(model.comp)).ToList();
            if (data[0] == true)
            {
                return "true"; //Case data is duplicate.
            }
            else
            {
                return "false"; //Case data is not duplicate.
            }
        }


        public UserViewModel Login(Login vm)
        {
            var query = _viewer.Filter(f => f.user_login.Equals(vm.username) && f.user_password.Equals(vm.password)).ToList()
                .Select(s => new UserViewModel()
                {
                    username = s.user_login,
                    id = s.user_id
                }).FirstOrDefault();
            return query;
        }

        public int GetAdminID(int id)
        {
            var data = _viewer.Filter(c => c.user_id == id).SingleOrDefault().admin_id;
            return Convert.ToInt32(data);
        }
    }
}
