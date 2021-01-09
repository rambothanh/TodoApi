using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;
using TodoApi.Models.Helpers;
using TodoApi.Models.UserModels;

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
        //return null if username not exists or password is not correct
        //Note: user include: PasswordHash, PasswordSalt
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

        //Check password IsNullOrWhiteSpace
        //Check tra user.Username if exists
        //Create PasswordHash, PasswordSalt
        //Save user in database
        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.Username == user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        //Lấy user ở database (user) từ id của user ở Param (userParam)
        //Kiểm tra xem liệu userParam.Username IsNullOrWhiteSpace,
        //và khác với user.Username không. Nếu khác, kiểm tra tiếp
        //userParam.Username này đã có trên database chưa, nếu chưa
        //update Username này cho user.
        //Xong kiểm tra IsNullOrWhiteSpace cho các properties khác
        //và update chúng vào user.
        public void Update(User userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // throw error if the new username is already taken
                if (_context.Users.Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");

                user.Username = userParam.Username;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                user.FirstName = userParam.FirstName;

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
                user.LastName = userParam.LastName;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            // update Role if provided
            // if (!string.IsNullOrWhiteSpace(userParam.Role))
            //     user.Role = userParam.Role;

            _context.Users.Update(user);
            _context.SaveChanges();
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

        //Phương thức này dùng cho admin, vừa setRole vừa chỉnh sửa khác nếu cần.
        public void SetRoleAndUpdate(User userParam,string password = null)
        {
           var user = _context.Users.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // throw error if the new username is already taken
                if (_context.Users.Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");

                user.Username = userParam.Username;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                user.FirstName = userParam.FirstName;

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
                user.LastName = userParam.LastName;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }
            //update Role if provided
            if (!string.IsNullOrWhiteSpace(userParam.Role))
                user.Role = userParam.Role;

            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}