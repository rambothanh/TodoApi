using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using TodoApi.Models.Helpers;
using TodoApi.Models.UserModels;

namespace TodoApi.Services.TodoService
{
    public class TodoService : ITodoService
    {
        private TodoContext _context;

        public TodoService(TodoContext context)
        {
            _context = context;
        }

    }
}