
using System;
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

        //Tạo hai biến: passwordHash, và passwordSalt dạng byte[] rỗng cùng với chuỗi password
        //được người dùng gửi cho vào phương thức, phương thức sẽ kiểm tra
        //password đưa vào, nếu null hoặc có khoản trắng sẽ báo Exception
        //Phương thước sẽ thay đổi 2 biến passwordHash, và passwordSalt 
        //dùng để trữ trên database cùng với các thông tin khác của User
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            //Khởi tạo instance mới của lớp HMACSHA512 với Key được tạo ngẫu nhiên.
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                //Lấy Key (được tạo ngẫu nhiên trong phương thức khởi tạo)
                //cho vào passwordSalt để lưu Database
                passwordSalt = hmac.Key;
                //GetBytes tạo ra 1 mảng byte từ giá trị string đưa vào 
                //ComputeHash sẽ Tính toán giá trị băm khi cho vào 1 mảng byte
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}