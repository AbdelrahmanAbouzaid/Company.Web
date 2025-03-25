using Company.Web.BLL.Interfaces;
using Company.Web.DAL.Data.Contexts;
using Company.Web.DAL.Models;
using Microsoft.EntityFrameworkCore;
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
            if (typeof(TEntity) == typeof(Employee))
            {
                return (IEnumerable<TEntity>)_context.Employees.Include(e => e.Department).ToList();
            }else if (typeof(TEntity) == typeof(Department))
            {
                return (IEnumerable<TEntity>)_context.Departments.Include(e => e.Employees).ToList();
            }
            return _context.Set<TEntity>().ToList();
        }
        public TEntity? Get(int id)
        {
            if (typeof(TEntity) == typeof(Employee))
            {
                return _context.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id) as TEntity;
                
            }
            else if (typeof(TEntity) == typeof(Department))
            {
                return _context.Departments.Include(e => e.Employees).FirstOrDefault(e => e.Id == id) as TEntity;
            }
            return _context.Set<TEntity>().Find(id);
        }
        public void Add(TEntity model)
        {
            _context.Set<TEntity>().Add(model);
        }
        public void Update(TEntity model)
        {
            _context.Set<TEntity>().Update(model);
        }
        public void Delete(TEntity model)
        {
            _context.Set<TEntity>().Remove(model);
        }

    }
}
