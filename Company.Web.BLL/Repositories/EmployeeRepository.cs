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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyContext context;
        #region MyRegion
        //private readonly CompanyContext _context;

        //public EmployeeRepository(CompanyContext context)
        //{
        //    _context = context;
        //}
        //public IEnumerable<Employee> GetAll()
        //{
        //    return _context.Employees.ToList();
        //}
        //public Employee? Get(int id)
        //{
        //    return _context.Employees.Find(id);
        //}
        //public int Add(Employee model)
        //{
        //    _context.Employees.Add(model);
        //    return _context.SaveChanges();
        //}
        //public int Update(Employee model)
        //{
        //    _context.Employees.Update(model);
        //    return _context.SaveChanges();
        //}
        //public int Delete(Employee model)
        //{
        //    _context.Employees.Remove(model);
        //    return _context.SaveChanges();
        //} 
        #endregion
        public EmployeeRepository(CompanyContext context) : base(context)
        {
            this.context = context;
        }

        IEnumerable<Employee> IEmployeeRepository.GetByName(string name)
        {
            return context.Employees.Include(e=>e.Department).Where(e => e.Name.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}
