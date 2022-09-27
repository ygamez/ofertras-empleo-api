using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationAPI.Entities;

namespace WebApplicationAPI.Helpers
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<User, UserModel>();
            CreateMap<RegisterModel, Usuario>();
            //CreateMap<UpdateModel, User>();
        }
    }
}
