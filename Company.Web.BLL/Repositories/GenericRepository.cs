using Company.Web.BLL.Interfaces;
using Company.Web.DAL.Data.Contexts;
using Company.Web.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Web.BLL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly CompanyContext _context;

        public GenericRepository(CompanyContext context)
        {
            _context = context;
        }
        public IEnumerable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().ToList();
        }
        public TEntity? Get(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }
        public int Add(TEntity model)
        {
            _context.Set<TEntity>().Add(model);
            return _context.SaveChanges();
        }
        public int Update(TEntity model)
        {
            _context.Set<TEntity>().Update(model);
            return _context.SaveChanges();
        }
        public int Delete(TEntity model)
        {
            _context.Set<TEntity>().Remove(model);
            return _context.SaveChanges();
        }

    }
}
