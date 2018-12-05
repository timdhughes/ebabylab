using System;
using System.Collections.Generic;
using System.Text;

namespace EbabyLab
{
    public class UserManager
    {
       public List<User> Users { get; private set; } = new List<User>();

        public bool Register(User user)
        {
            if (this.FindUserbyUserName(user.UserName) != null)
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

        public bool LogIn(string userName, string password)
        {
           User user = FindUserbyUserName(userName);
            if (user == null || password != user.Password)
                return false;
            user.LoggedIn = true;
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
