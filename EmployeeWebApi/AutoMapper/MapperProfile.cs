using AutoMapper;
using Employee.Domain.Model;
using Employee.WebApi.Model;
using EmployeeWebAPI.Model;

namespace EmployeeWebAPI.AutoMapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<EmployeeViewModel, EmployeeModel>().ReverseMap();
        CreateMap<SkillViewModel, SkillModel>().ReverseMap();
        CreateMap<HobbyViewModel, HobbyModel>().ReverseMap();
    }
}
