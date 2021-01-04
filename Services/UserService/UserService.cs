
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using TodoApi.Models.UserModels;
using TodoApi.Services.UserService;

namespace TodoApi.Services.UserService
{
    public class UserService : IUserService
    {
        private TodoContext _context;

        public UserService(TodoContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public User Create(User user, string password)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(User user, string password = null)
        {
            throw new System.NotImplementedException();
        }
    }
}