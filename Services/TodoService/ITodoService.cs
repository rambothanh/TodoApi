using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Services.TodoService
{
    public interface ITodoService
    {
        IEnumerable<TodoItem> GetAllTodoItemsOfUser();
        // User Authenticate(string username, string password);

        // IEnumerable<User> GetAll();

        // User GetById(int id);

        // User Create(User user, string password);

        // void Update(User user, string password = null);
        // void SetRoleAndUpdate(User user,string password = null);

        // void Delete(int id);
    }
}