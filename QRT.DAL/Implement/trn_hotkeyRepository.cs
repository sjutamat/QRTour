using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.Domain.Interface.Repository;
using QRT.DB;

namespace QRT.DAL.Implement
{
    public class trn_hotkeyRepository : RepositoryBase<trn_hotkey>,Itrn_hotkeyRepository
    {
        private readonly QRCodeTourEntities _dbContext;
        public trn_hotkeyRepository(QRCodeTourEntities _Context) : base(_Context)
        {
            _dbContext = _Context;
        }
    }
}
