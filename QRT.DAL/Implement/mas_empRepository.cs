using QRT.DB;
using QRT.Domain.Interface.Repository;
using QRT.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRT.DAL.Implement
{
    public class mas_empRepository :RepositoryBase<mas_emp>, Imas_empRepository
    {
        private readonly QRCodeTourEntities _dbContext;
        public mas_empRepository(QRCodeTourEntities _Context):base(_Context){
            _dbContext = _Context;
        }


        public List<EmpData> GetEmployeeByAdminId(long admin_id)
        {
            var empQuery = from c in _dbContext.mas_company
                           join e in _dbContext.mas_emp on c.comp_id equals e.emp_comp
                           where c.admin_id == admin_id
                           select new EmpData
                           {
                               id = e.emp_id,
                               title = e.emp_title,
                               name = e.emp_fname + " " + e.emp_surname,
                               code = e.emp_code,
                               comp_name = e.mas_company.comp_name,
                               status = e.emp_active == "A" ? "Active" : "Deactive",
                           };
            var data = empQuery.ToList();
            return data;
        }
    }
}
