using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Teams.DTO;

namespace Teams.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<Group, GroupDTO>();
            CreateMap<GroupDTO, Group>();
            CreateMap<Target, TargetDTO>();
            CreateMap<TargetDTO, Target>();
            CreateMap<ExecutorDTO, Executor>();
            CreateMap<MemberDTO, Member>();
            CreateMap<Task<Group>, Group>();
        }
    }
}