using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.DB;
using QRT.Domain.Interface.Service;
using QRT.Domain.Interface.Repository;
using Op3ration.ExceptionHandler;
using QRT.Domain.ViewModel;

namespace QRT.Service.Implement
{
    public class mas_empService :Imas_empService
    {
        #region Utility
        private readonly Imas_empRepository _emp;
        private readonly Imas_companyService _compservice;
        //private ValidateHandler Validator;
        public mas_empService(Imas_empRepository imas_emprepository
            ,Imas_companyService imascompanyservice)
        {
            _emp = imas_emprepository;
            _compservice = imascompanyservice;
        }


        //private ValidateHandler ValidateModel(m_empViewModel model)
        //{
        //    Validator = new ValidateHandler();
        //    //if (String.IsNullOrEmpty(model.comp_name))
        //    //    Validator.AddMessage(MessageLevel.Error, "กรุณาระบุ Email");

        //    if (String.IsNullOrEmpty(model.fname))
        //        Validator.AddMessage(MessageLevel.Error, "Please enter FirstName");

        //    return Validator;
        //}
        #endregion

        public m_empViewModel GetAllEmp()
        {
            m_empViewModel model = new m_empViewModel();
            var data = _emp.Filter(c => c.emp_active != "D",inc=>inc.mas_company).ToList();
            if (data != null)
            {
                var emp = data.Select(x => new EmpData()
                {
                    id = x.emp_id,
                    title = x.emp_title,
                    name = x.emp_fname + " " + x.emp_surname,
                    code = x.emp_code,
                    comp_name = x.mas_company.comp_name,
                    status = x.emp_active == "A" ? "Active" : "Deactive",
                }).OrderByDescending(c => c.created_date).ToList();

                model.s_empData = emp;
                model.company = _compservice.GetCompany();
                return model;
            }
            else
            {
                model.s_empData = new List<EmpData>();
                return model;

            }
        }


        public m_empViewModel FilterEmp(m_empViewModel model)
        {
            var name = model.s_emp.name;
            var code = model.s_emp.code;
            var comp = Convert.ToInt32(model.s_emp.comp);
            
            var data = _emp.Filter(c => c.emp_active == "A", inc => inc.mas_company).ToList();

            if (!String.IsNullOrEmpty(name))
            {
                data = data.Where(c => c.emp_fname.ToUpper().Contains(name.ToUpper()) || c.emp_surname.ToUpper().Contains(name.ToUpper())).ToList();
            }

            if (!String.IsNullOrEmpty(code))
            {
                data = data.Where(c => c.emp_code.Contains(code.ToUpper())).ToList();
            }

            if (comp != 0)
            {
                data = data.Where(c => c.emp_comp == comp).ToList();
            }


            if (data != null)
            {
                var emp = data.Select(x => new EmpData()
                {
                    id = x.emp_id,
                    title = x.emp_title,
                    name = x.emp_fname + " " + x.emp_surname,
                    code = x.emp_code,
                    comp_name = x.mas_company.comp_name,
                    status = x.emp_active == "A" ? "Active" : "Deactive",
                }).OrderByDescending(c => c.created_date).ToList();

                model.s_empData = emp;
                model.company = _compservice.GetCompany();

                return model;
            }
            else
            {
                model.s_empData = new List<EmpData>();
                return model;

            }
        }


        public m_empViewModel GetById(long id)
        {
            var data = _emp.Filter(c => c.emp_id == id).SingleOrDefault();
            m_empViewModel emp = new m_empViewModel();
            emp.id = data.emp_id;
            emp.title = data.emp_title;
            emp.fname = data.emp_fname;
            emp.sname = data.emp_surname;
            emp.code = data.emp_code;
            emp.comp = data.emp_comp;
            emp.status = data.emp_active == "A" ? "On" : "Off";

            emp.company = _compservice.GetCompany();
            return emp;
        }

        public void Save(m_empViewModel model)
        {
            if (model.id == 0)
            {
                //Validator = ValidateModel(model);
                //if (Validator.HasError())
                //{
                //    throw Validator;
                //}
                //else
                //{
                    try
                    {
                        mas_emp emp = new mas_emp();
                        emp.emp_title = model.title;
                        emp.emp_fname = model.fname;
                        emp.emp_surname = model.sname;
                        emp.emp_comp = model.comp;
                        emp.emp_code = model.comp_name + model.code;
                        emp.emp_password = model.password;
                        emp.emp_active = model.status == "On" ? "A" : "D";
                        _emp.Create(emp);
                    }
                    catch (Exception ex)
                    {
                        if (ex.IsValidateHandler())
                            throw ex.ToValidateHandler();
                        throw new ValidateHandler(MessageLevel.Error, "Error:'" + ex.Message + "'");
                    }
                //}
            }
            else
            {
                //Validator = ValidateModel(model);
                //if (Validator.HasError())
                //{
                //    throw Validator;
                //}
                //else
                //{
                    var oldData = _emp.Filter(c => c.emp_id == model.id).SingleOrDefault();
                    if (oldData != null)
                    {
                        try
                        {
                            oldData.emp_title = model.title;
                            oldData.emp_fname = model.fname;
                            oldData.emp_surname = model.sname;
                            oldData.emp_comp = model.comp;
                            oldData.emp_code = model.comp_name + model.code;
                            oldData.emp_password = model.password ?? oldData.emp_password;
                            oldData.emp_active ="A";
                            _emp.Update(oldData);
                        }
                        catch (Exception ex)
                        {
                            if (ex.IsValidateHandler())
                                throw ex.ToValidateHandler();
                            throw new ValidateHandler(MessageLevel.Error, "Error:'" + ex.Message + "'");
                        }

                    }
                //}
            }
        }

        public void UpdateStatus(long id)
        {
            var oldData = _emp.Filter(c => c.emp_id == id).SingleOrDefault();
            try
            {
                oldData.emp_active = "D";
                _emp.Update(oldData);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public m_empViewModel GetEmp()
        {
            m_empViewModel emp = new m_empViewModel();
            emp.company = _compservice.GetCompany();
            return emp;
        }

        public string GetNewCode(long compid)
        {
            var company = _compservice.GetById(compid);

            string data;
            int newCode = 0;
            if (company.flag_internal == true)//Internal SiamPiwat Group
            {
                data = "";
            }
            else
            {
               var lastCode = _emp.Filter(c => c.emp_comp == compid).ToList();
                if (lastCode!=null && lastCode.Any())
                {
                    var lc = lastCode.OrderByDescending(o => o.emp_code);
                    var a = lc.First().emp_code;
                    var code = a.Substring(3);
                    newCode = Convert.ToInt32(code) + 1;
                }else
                {
                    newCode = 00001;
                }
              
               data = newCode.ToString("D5");
            }
            return data;
        }

        public EmpData CheckEmp(string emp_code, string pw)
        {
            var data = _emp.Filter(c => c.emp_code.Equals(emp_code) && c.emp_password.Equals(pw)).SingleOrDefault();
            EmpData emp = new EmpData();
            if (data != null)
            {
                emp.id = data.emp_id;
                emp.code = data.emp_code;
                emp.password = data.emp_password;
                emp.name = data.emp_fname + " " + data.emp_surname;
                emp.fname = data.emp_fname;
                emp.sname = data.emp_surname;
            }
            return emp;
        }

        //public EmpData ChkEmployee(string emp_code)
        //{
        //    var data = _emp.Filter(c => c.emp_code.Equals(emp_code)).SingleOrDefault();
        //    EmpData emp = new EmpData();
        //    if (data != null)
        //    {
        //        emp.id = data.emp_id;
        //        emp.code = data.emp_code;
        //        emp.name = data.emp_fname + " " + data.emp_surname;
        //        emp.fname = data.emp_fname;
        //        emp.sname = data.emp_surname;
        //    }
        //    return emp;
        //}
    }
}
