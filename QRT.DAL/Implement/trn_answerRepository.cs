using QRT.DB;
using QRT.Domain.Interface.Repository;
using QRT.Domain.ViewModel;


namespace QRT.DAL.Implement
{
    public class trn_answerRepository : RepositoryBase<trn_answer>,Itrn_answerRepository
    {
        private readonly QRCodeTourEntities _dbContext;
        public trn_answerRepository(QRCodeTourEntities _Context) : base(_Context)
        {
            _dbContext = _Context;
        }


        public answerDetailList GetByLocation(long? lid)
        {
            answerDetailList data = new answerDetailList();
            return data;
        }

    }
}
