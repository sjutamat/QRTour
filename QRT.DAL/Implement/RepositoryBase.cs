using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Op3ration.RepositoryHandler;

namespace QRT.DAL.Implement
{
    public class RepositoryBase<T> : Repositories<T>where T :class
    {
        public RepositoryBase(DbContext _Context) : base(_Context)
        {
             _Context.Configuration.ProxyCreationEnabled = false;
            _Context.Configuration.LazyLoadingEnabled = false;
        }
    }
}
