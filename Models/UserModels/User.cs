using System.ComponentModel.DataAnnotations.Schema;
using TodoApi.Models;


namespace TodoApi.Models.UserModels {
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Role { get; set; } = TodoApi.Models.UserModels.Role.User;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        //public ICollection<TodoApi.Models.TodoItem>  TodoItems { get; set; }
    }
}
