
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

        //return user if username exists
        //returt null if username not exists or password is not correct
        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
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

        //Cho vào phương thức 1 password dạng string, và một mảng byte storedHash 
        //và storedSalt, 2 mảng này được lấy từ database. Phương thức sẽ tiến hành
        //Hash password đưa vào này với storedSalt, vào so sánh kết quả có khớp
        //với storedHash không.
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");
            //Khởi tạo instance mới của lớp HMACSHA512 với Key = storedSalt đưa vào.
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}