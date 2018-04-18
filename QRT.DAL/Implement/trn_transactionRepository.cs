using QRT.DB;
using QRT.Domain.Interface.Repository;
using QRT.Domain.ViewModel;

namespace QRT.DAL.Implement
{
    public class trn_transactionRepository : RepositoryBase<trn_transaction> ,Itrn_transactionRepository
    {
        private readonly QRCodeTourEntities _dbContext;
        public trn_transactionRepository(QRCodeTourEntities _Context) : base(_Context)
        {
            _dbContext = _Context;
        }
    }
}
