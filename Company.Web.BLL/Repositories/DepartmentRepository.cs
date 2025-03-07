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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly CompanyContext _context;
        public DepartmentRepository(CompanyContext context)
        {
            this._context = context;
        }
        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }
        public Department? Get(int id)
        {
            return _context.Departments.Find(id);
        }
        public int Add(Department model)
        {
            _context.Departments.Add(model);
            return _context.SaveChanges();
        }
        public int Update(Department model)
        {
            _context.Departments.Update(model);
            return _context.SaveChanges();
        }
        public int Delete(Department model)
        {
            _context.Departments.Remove(model);
            return _context.SaveChanges();
        }
    }
}
