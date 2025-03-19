using Company.Web.BLL.Interfaces;
using Company.Web.BLL.Repositories;
using Company.Web.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Web.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyContext context;

        public IDepartmentRepository DepartmentRepository { get; }

        public IEmployeeRepository EmployeeRepository { get; }
        public UnitOfWork(CompanyContext _context)
        {
            context = _context;
            DepartmentRepository = new DepartmentRepository(context);
            EmployeeRepository = new EmployeeRepository(context);
        }
        public int SaveChanges()
        {
            return context.SaveChanges();
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
