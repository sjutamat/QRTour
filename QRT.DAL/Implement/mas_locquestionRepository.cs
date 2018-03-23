using QRT.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.Domain.Interface.Repository;

namespace QRT.DAL.Implement
{
    public class mas_locquestionRepository :RepositoryBase<mas_locquestion>,Imas_locquestionRepository
    {
        private readonly QRCodeTourEntities _dbContext;
        public mas_locquestionRepository(QRCodeTourEntities _Context) : base(_Context)
        {
            _dbContext = _Context;
        }
    }
}
