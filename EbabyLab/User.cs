using System;

namespace EbabyLab
{
    public class User
    {
        public User(string firstName, string lastName, string userEmail, string userName, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            UserEmail = userEmail;
            UserName = userName;
            Password = password;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }


        public bool LoggedIn { get; set; } = false;

        public bool IsSeller { get; set; } = false;


    }
}
