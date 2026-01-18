using PerpustakaanAppMVC.Model.Context;
using PerpustakaanAppMVC.Model.Entity;
using PerpustakaanAppMVC.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerpustakaanAppMVC.Controller
{
    public class UserController
    {

        private void UserValidation(User user)
        {
            if (string.IsNullOrEmpty(user.Name))
            {
                throw new ArgumentException("Name is required");
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("Email is required");
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Password is required");
            }
        }

        public int Create(User user)
        {
            UserValidation(user); // validasi 

            using (var context = new DbContext())
            {
                var _repo = new UserRepository(context);
                return _repo.Create(user);
            }

        }

        public List<User> ReadAll()
        {
            using (var context = new DbContext())
            {
                var _repo = new UserRepository(context);
                return _repo.ReadAll();
            }
        }

        public int Update(User user)
        {
            UserValidation(user); // validasi

            using (var context = new DbContext())
            {
                var _repo = new UserRepository(context);
                return _repo.Update(user);
            }
        }

        public int Delete(int id)
        {
            using (var context = new DbContext())
            {
                var _repo = new UserRepository(context);
                return _repo.Delete(id);
            }
        }

        public int ResetPassword(int id, string newPassword)
        {
            
            ValidatePassword(newPassword);

            using (var context = new DbContext())
            {
                var _repo = new UserRepository(context);
                var user = _repo.ReadById(id);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                user.Password = newPassword;
                return _repo.Update(user);
            }
        }

        private void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is required");
            }
            if (password.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters long");
            }

        }

        public User Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email is required");
            }
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is required");
            }

            using (var context = new DbContext())
            {
                var _repo = new UserRepository(context);
                var users = _repo.ReadAll();

                // Find user by email
                var user = users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

                if (user != null && user.Password.Equals(password)) // Simple password comparison (in production, use hashed passwords)
                {
                    return user;
                }

                return null; // Invalid credentials
            }
        }

        public User GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email is required");
            }

            using (var context = new DbContext())
            {
                var _repo = new UserRepository(context);
                return _repo.GetByEmail(email);
            }
        }
    }
}
