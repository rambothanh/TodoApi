using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Models.Helpers;
using TodoApi.Models.UserModels;



namespace TodoApi.Services.TodoService
{
    public class TodoService : ITodoService
    {
        private TodoContext _context;
        private readonly IMapper _mapper;
        public TodoService(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<TodoItem> GetAllTodoItemsOfUser()
        {
            //  ClaimsPrincipal principal = _context.Current.User as ClaimsPrincipal;  
            // if (null != principal)  
            // {  
            // foreach (Claim claim in principal.Claims)  
            // {  
            //     Response.Write("CLAIM TYPE: " + claim.Type + "; CLAIM VALUE: " + claim.Value + "</br>");  
            // }  
            // }
            return _context.TodoItems;
            
        }
    }
}