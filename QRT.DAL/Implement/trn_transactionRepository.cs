using QRT.DB;
using QRT.Domain.Interface.Repository;
using QRT.Domain.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace QRT.DAL.Implement
{
    public class trn_transactionRepository : RepositoryBase<trn_transaction> ,Itrn_transactionRepository
    {
        private readonly QRCodeTourEntities _dbContext;
        public trn_transactionRepository(QRCodeTourEntities _Context) : base(_Context)
        {
            _dbContext = _Context;
        }

        public IEnumerable<answerHeader> Load()
        {
            var query = _dbContext.trn_transaction
                .Join(_dbContext.mas_emp, a => a.session_id, b => b.emp_id.ToString(), (a, b) => new answerHeader()
                {
                    emp_id = b.emp_id,
                    emp_name = b.emp_fname + " " + b.emp_surname,
                    route_id = a.route_id.Value,
                    start_time = a.transaction_cdate,
                    end_time = a.transaction_cdate,
                });
            return query;
        }
    }
}
