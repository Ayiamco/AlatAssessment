using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AlatAssessment.DataAccess.Interfaces
{
    public interface IGenericRepository<T1, T2> where T1 : class
    {
        Task Create(T1 entity);

        void Delete(T1 entity);

        IEnumerable<T1> Find(Expression<Func<T1, bool>> func_predicate);

        IEnumerable<T1> GetAll();

        Task<T1> Read(T2 id);

        Task<List<T1>> ReadAll();

        void Update(T1 entity);
    }
}
