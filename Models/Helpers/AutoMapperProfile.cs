using AutoMapper;
using TodoApi.Models.UserModels;
using TodoApi.Models.CrawlerModels;

namespace TodoApi.Models.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Phía trước là nguồn,phía sau là đích
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            CreateMap<TodoItem, TodoItemDTO>();
             CreateMap<TodoItemDTO, TodoItem>();
             CreateMap<News,NewsDTO>();
             CreateMap<NewsDTO,News>();
        }
    }
}