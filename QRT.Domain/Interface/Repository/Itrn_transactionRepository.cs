using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.DB;
using QRT.Domain.ViewModel;

namespace QRT.Domain.Interface.Repository
{
    public interface Itrn_transactionRepository : IRepositoryBase<trn_transaction>
    {
        IEnumerable<answerHeader> Load();
    }
}
