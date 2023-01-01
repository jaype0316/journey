using AutoMapper;
using Journey.Api.Models;
using Journey.Api.ViewModels;

namespace Journey.Api.Mappers
{
    public class ApiMappers : Profile
    {
        public ApiMappers()
        {
            CreateMap<AppUser, UserProfile>();
        }
    }
}
