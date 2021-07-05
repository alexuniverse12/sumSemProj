using AutoMapper;
using summerSemesterProj.Dtos;
using summerSemesterProj.Models;

namespace summerSemesterProj.Profiles {
    public class RegProfile : Profile {
        public RegProfile(){
            CreateMap<User,RegDto>();
            CreateMap<RegDto,User>();
        }
    }
}