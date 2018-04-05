using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRT.Domain.Interface;
using QRT.DB;
using QRT.Domain.ViewModel;

namespace QRT.Domain.Interface.Repository
{
    public interface Itrn_answerRepository:IRepositoryBase<trn_answer>
    {
        answerDetailList GetByLocation(long? lid);
    }
}
