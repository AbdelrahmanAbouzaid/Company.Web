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
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (typeof(TEntity) == typeof(Employee))
            {
                return (IEnumerable<TEntity>)await _context.Employees.Include(e => e.Department).ToListAsync();
            }
            else if (typeof(TEntity) == typeof(Department))
            {
                return (IEnumerable<TEntity>)await _context.Departments.Include(e => e.Employees).ToListAsync();
            }
            return await _context.Set<TEntity>().ToListAsync();
        }
        public async Task<TEntity?> GetAsync(int id)
        {
            if (typeof(TEntity) == typeof(Employee))
            {
                return await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(e => e.Id == id) as TEntity;

            }
            else if (typeof(TEntity) == typeof(Department))
            {
                return await _context.Departments.Include(e => e.Employees).FirstOrDefaultAsync(e => e.Id == id) as TEntity;
            }
            return await _context.Set<TEntity>().FindAsync(id);
        }
        public async Task AddAsync(TEntity model)
        {
            await _context.Set<TEntity>().AddAsync(model);
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
