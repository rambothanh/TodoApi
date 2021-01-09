using AutoMapper;
using TodoApi.Models.UserModels;

namespace TodoApi.Models.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            CreateMap<TodoItem, TodoItemDTO>();
             CreateMap<TodoItemDTO, TodoItem>();
        }
    }
}