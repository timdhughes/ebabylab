using System;
using System.Collections.Generic;
using System.Text;

namespace eBabyLab
{
    public enum UserManagerStatus { Success, InvalidUserCredentials, SystemError }

    public class UserManager
    {
       public List<User> Users { get; private set; } = new List<User>();

        public bool Register(User user)
        {
            if (FindUserbyUserName(user.UserName) != null)
            {
                return false;
            }
            else
            {
                Users.Add(user);
                return true;
            }
        }

        public User FindUserbyUserName(string userName)
        {
            return Users.Find(x => x.UserName == userName);
        }

        public bool LogIn(string userName, string password, out UserManagerStatus status)
        {

           User user = FindUserbyUserName(userName);
            if (user == null || !user.CheckPassword(password))
            {
                status = UserManagerStatus.InvalidUserCredentials;
                return false;
            }

            user.LoggedIn = true;
            status = UserManagerStatus.Success;
            return true;
        }

        public bool LogOut (string userName)
        {
            User user = FindUserbyUserName(userName);
            if (user == null)
                return false;
            user.LoggedIn = false;
            return true;
        }

        public bool SetSeller(string userName)
        {
            User user = FindUserbyUserName(userName);
            if (user == null)
                return false;
            user.IsSeller = true;
            return true;
        }

    }
}
