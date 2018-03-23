using QRT.DB;
using QRT.Domain.Interface.Repository;
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
    }
}
