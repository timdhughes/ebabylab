using System;

namespace eBabyLab
{
    public class User
    {
        public User(string firstName, string lastName, string userEmail, string userName, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            UserEmail = userEmail;
            UserName = userName;
            userPassword = password;
        }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public bool LoggedIn { get; set; } = false;

        public bool IsSeller { get; set; } = false;

        private string userPassword;

        public bool CheckPassword(string password)
        {
            return userPassword == password;
        }

    }
}
