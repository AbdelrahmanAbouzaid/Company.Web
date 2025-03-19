using AutoMapper;
using Company.Web.DAL.Models;
using Company.Web.PL.Dtos;

namespace Company.Web.PL.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            //CreateMap<Employee, CreateEmployeeDto>().ForMember(e => e.Name, o => o.MapFrom(s => s.Name));
            CreateMap<Employee, CreateEmployeeDto>().ReverseMap();
        }
    }
}
