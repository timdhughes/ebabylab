using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EbabyLab;

namespace EbabyUnitTest
{
    [TestClass]
    public class EbabyUnitTest
    {
        const string defaultFirstName = "Nikita";
        const string defaultLastName = "Gupta";
        const string defaultUserEmail = "ngupta@miteksystems.com";
        const string defaultUserName = "ngupta";
        const string defaultPassword = "1Password!";


        User CreateDefaultUser()
        {
            return new User(defaultFirstName, defaultLastName, defaultUserEmail, defaultUserName, defaultPassword);
        }

        Auction CreateDefaultAuction()
        {
            User seller = CreateDefaultUser();
            UserManager userManager = new UserManager();
            bool registerSuccess = userManager.Register(seller);
            userManager.SetSeller(seller.UserName);
            userManager.LogIn(seller.UserName, seller.Password);

            // Auction information
            string itemDesc = "Laptop";
            double startingPrice = 100.00;
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = startTime.AddDays(5);

           return  Auction.Create(seller, itemDesc, startingPrice, startTime, endTime);
        }

        [TestMethod]
        public void testRetreivingParamsAfterConstruction()
        {
            User user = new User(defaultFirstName, defaultLastName, defaultUserEmail, defaultUserName, defaultPassword);

            Assert.AreEqual(defaultFirstName, user.FirstName);
            Assert.AreEqual(defaultLastName, user.LastName);
            Assert.AreEqual(defaultUserEmail, user.UserEmail);
            Assert.AreEqual(defaultUserName, user.UserName);
            Assert.AreEqual(defaultPassword, user.Password);
            Assert.IsFalse(user.LoggedIn, "User should be Logged out");

        }

        [TestMethod]
        public void testRegisterUser()
        {        
            User user = CreateDefaultUser();
            UserManager userManager = new UserManager();

            bool success = userManager.Register(user);
            Assert.IsTrue(success, "Registration unsucessfull");

            User foundUser = userManager.FindUserbyUserName(user.UserName);
            Assert.IsNotNull(foundUser, "User not found - object was null");
            Assert.AreEqual(foundUser.UserName, user.UserName, "User not found - name did not match");
            

        }
        [TestMethod]
        public void testUserNotFound()
        {
            UserManager userManager = new UserManager();
            User user = userManager.FindUserbyUserName("invalid");
            Assert.IsNull(user, "User should be null");
        }

        [TestMethod]
        public void testUserManagerLogIn()
        {
            User user = CreateDefaultUser();
            UserManager userManager = new UserManager();

            bool registerSuccess = userManager.Register(user);
            Assert.IsTrue(registerSuccess, "Registration was not successful");

            bool loginSuccess = userManager.LogIn(defaultUserName, defaultPassword);
            Assert.IsTrue(loginSuccess, "Log in was not successful");

            // final check for LoggedIn parameter
            Assert.IsTrue(user.LoggedIn, "User should be logged in.");


        }

        [TestMethod]
        public void testUserManagerLogOut()
        {
            User user = CreateDefaultUser();
            UserManager userManager = new UserManager();

            bool registerSuccess = userManager.Register(user);
            Assert.IsTrue(registerSuccess, "Registration was not successful");

            bool loginSuccess = userManager.LogIn(defaultUserName, defaultPassword);
            Assert.IsTrue(loginSuccess, "Log in was not successful");

            bool logOutSucess = userManager.LogOut(defaultUserName);
            Assert.IsTrue(logOutSucess, "Logout is not successfull");

            // final check for LoggedIn parameter
            Assert.IsFalse(user.LoggedIn, "User should be logged out.");


        }

        [TestMethod]
        public void testSellerCanCreateAuction()
        {
            // Set up user manager
            User seller = CreateDefaultUser();
            UserManager userManager = new UserManager();
            bool registerSuccess = userManager.Register(seller);            
            userManager.SetSeller(seller.UserName);
            userManager.LogIn(seller.UserName, seller.Password);

            // Auction information
            string itemDesc = "Laptop";
            double startingPrice = 100.00;
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = startTime.AddDays(5);

            // seller, desc, startingPrice, startTime, endTime
            // constraints - user must be a seller, seller logged in
            // start time > now, end time> start time
            Auction auction = Auction.Create(seller, itemDesc, startingPrice, startTime, endTime);
            Assert.IsNotNull(auction, "Auction can not be created");

            Assert.AreEqual(auction.Seller, defaultUserName, "Username does not match");
            Assert.AreEqual(auction.ItemDesc, itemDesc, "Item description does not match");
            Assert.AreEqual(auction.StartingPrice, startingPrice, "Username does not match");
            Assert.AreEqual(auction.StartTime, startTime, "Username does not match");
            Assert.AreEqual(auction.EndTime, endTime, "Username does not match");

        }
        [TestMethod]
        public void testAuctionStarted()
        {
           Auction  auction =CreateDefaultAuction();
           auction.OnStart();
            Assert.IsTrue(auction.Started, "Aution should not be started");
        }

    }
}
