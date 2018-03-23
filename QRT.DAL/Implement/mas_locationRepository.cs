using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.Domain.Interface.Repository;
using QRT.DB;

namespace QRT.DAL.Implement
{
    public class mas_locationRepository : RepositoryBase<mas_location>,Imas_locationRepository
    {
        private readonly QRCodeTourEntities _dbContext;
        public mas_locationRepository(QRCodeTourEntities _Context):base(_Context){
            _dbContext = _Context;
        }
    }
}
