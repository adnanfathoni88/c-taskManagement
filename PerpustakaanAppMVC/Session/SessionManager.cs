using PerpustakaanAppMVC.Model.Entity;
using System;

namespace PerpustakaanAppMVC.Session
{
    public static class SessionManager
    {
        private static User _currentUser;
        
        public static bool IsLoggedIn => _currentUser != null;
        
        public static User CurrentUser
        {
            get => _currentUser;
            private set => _currentUser = value;
        }
        
        public static void Login(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
                
            CurrentUser = user;
        }
        
        public static void Logout()
        {
            CurrentUser = null;
        }
        
        public static int GetCurrentUserId()
        {
            return IsLoggedIn ? CurrentUser.Id : 0;
        }
        
        public static string GetCurrentUserName()
        {
            return IsLoggedIn ? CurrentUser.Name : string.Empty;
        }
        
        public static string GetCurrentUserRole()
        {
            return IsLoggedIn ? CurrentUser.RoleName : string.Empty;
        }

        public static bool IsAdmin()
        {
            return IsLoggedIn && CurrentUser.RoleName == "Admin" ? true : false;
        }
    }
}