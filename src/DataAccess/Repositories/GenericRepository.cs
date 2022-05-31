using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Contexts;
using AlatAssessment.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AlatAssessment.DataAccess.Repositories
{
    public abstract class GenericRepository<T1, T2> : IGenericRepository<T1, T2> where T1 : class
    {

        protected readonly AppDbContext context;

        protected GenericRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task Create(T1 entity)
        {
            await context.Set<T1>().AddAsync(entity);
            return;
        }
        public async Task<T1> Read(T2 id)
        {
            return await context.Set<T1>().FindAsync(id);
        }

        public async Task<List<T1>> ReadAll()
        {
            return await context.Set<T1>().ToListAsync();
        }

        public void Delete(T1 entity)
        {
            context.Set<T1>().Remove(entity);
        }
        public void Update(T1 entity)
        {
            context.Set<T1>().Update(entity);
        }

        public IEnumerable<T1> GetAll()
        {
            return context.Set<T1>().ToList();
        }

        public IEnumerable<T1> Find(Expression<Func<T1, bool>> func_predicate)
        {
            return context.Set<T1>().Where(func_predicate);
        }
    }
}
