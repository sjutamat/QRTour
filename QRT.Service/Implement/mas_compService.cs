using Op3ration.ExceptionHandler;
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
            var compData = _comp.Filter(c=>c.admin_id == adminId).ToList();
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
    }
}
