using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EbabyLab;

namespace EbabyUnitTest
{
    [TestClass]
    public class EbabyUnitTest
    {
        const string anyFirstName = "Nikita";
        const string anyLastName = "Gupta";
        const string anyUserEmail = "ngupta@miteksystems.com";
        const string anyUserName = "ngupta";
        const string anyPassword = "1Password!";
        const double anyStartingPrice = 100.00;


        User CreateDefaultUser()
        {
            return new User(anyFirstName, anyLastName, anyUserEmail, anyUserName, anyPassword);
        }

        Auction CreateDefaultAuction()
        {
            User seller = CreateDefaultUser();
            seller.LoggedIn = true;
            seller.IsSeller = true;

            // Auction information
            string itemDesc = "Laptop";
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = startTime.AddDays(5);
            Auction auction = Auction.Create(seller, itemDesc, anyStartingPrice, startTime, endTime);
            Assert.IsNotNull(auction, "Auction was not created");

            return auction;
        }

        [TestMethod]
        public void TestRetreivingParamsAfterConstruction()
        {
            User user = new User(anyFirstName, anyLastName, anyUserEmail, anyUserName, anyPassword);

            Assert.AreEqual(anyFirstName, user.FirstName);
            Assert.AreEqual(anyLastName, user.LastName);
            Assert.AreEqual(anyUserEmail, user.UserEmail);
            Assert.AreEqual(anyUserName, user.UserName);
            Assert.IsTrue(user.CheckPassword(anyPassword));
            Assert.IsFalse(user.LoggedIn, "User should be Logged out");

        }

        [TestMethod]
        public void TestRegisterUser()
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
        public void TestUserNotFound()
        {
            UserManager userManager = new UserManager();
            User user = userManager.FindUserbyUserName("invalid");
            Assert.IsNull(user, "User should be null");
        }

        [TestMethod]
        public void TestUserManagerLogIn()
        {
            User user = CreateDefaultUser();
            UserManager userManager = new UserManager();

            bool registerSuccess = userManager.Register(user);
            Assert.IsTrue(registerSuccess, "Registration was not successful");

            bool loginSuccess = userManager.LogIn(anyUserName, anyPassword, out UserManagerStatus status);
            Assert.IsTrue(loginSuccess, "Log in was not successful");

            // final check for LoggedIn parameter
            Assert.IsTrue(user.LoggedIn, "User should be logged in.");


        }

        [TestMethod]
        public void TestUserManagerLogOut()
        {
            User user = CreateDefaultUser();
            UserManager userManager = new UserManager();

            bool registerSuccess = userManager.Register(user);
            Assert.IsTrue(registerSuccess, "Registration was not successful");

            bool loginSuccess = userManager.LogIn(anyUserName, anyPassword, out UserManagerStatus status);
            Assert.IsTrue(loginSuccess, "Log in was not successful");

            bool logOutSucess = userManager.LogOut(anyUserName);
            Assert.IsTrue(logOutSucess, "Logout is not successfull");

            // final check for LoggedIn parameter
            Assert.IsFalse(user.LoggedIn, "User should be logged out.");


        }

        [TestMethod]
        public void TestSellerCanCreateAuction()
        {
            // Set up user manager
            User seller = CreateDefaultUser();
            UserManager userManager = new UserManager();
            bool registerSuccess = userManager.Register(seller);
            userManager.SetSeller(seller.UserName);
            userManager.LogIn(seller.UserName, anyPassword, out UserManagerStatus status);

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

            Assert.AreEqual(auction.Seller, anyUserName, "Username does not match");
            Assert.AreEqual(auction.ItemDesc, itemDesc, "Item description does not match");
            Assert.AreEqual(auction.StartingPrice, startingPrice, "Username does not match");
            Assert.AreEqual(auction.StartingPrice, auction.CurrentPrice, "At the beginning of the auction, starting price should equal current price.");
            Assert.AreEqual(auction.StartTime, startTime, "Username does not match");
            Assert.AreEqual(auction.EndTime, endTime, "Username does not match");

        }

        [TestMethod]
        public void TestAuctionStarted()
        {
            Auction auction = CreateDefaultAuction();

            auction.OnStart();
            Assert.IsTrue(auction.Started, "Aution should not be started");
        }

        [TestMethod]
        public void TestUserCanBidOnAuction()
        {
            User user = CreateDefaultUser();
            user.LoggedIn = true;

            Auction auction = CreateDefaultAuction();
            auction.OnStart();

            // user will bid 1 dollar over starting price
            double bidAmount = auction.StartingPrice + 1.00;
            bool success = auction.Bid(user, bidAmount, out Auction.BidStatus status);

            Assert.IsTrue(success, "Bid was not successful." + status.ToString());
            Assert.AreEqual(auction.CurrentPrice, bidAmount);
            Assert.AreEqual(auction.HighestBidder, user.UserName);
        }

        [TestMethod]

        public void TestUserCanNotBidIfLoggedOut()
        {
            User user = CreateDefaultUser();
            user.LoggedIn = false;

            Auction auction = CreateDefaultAuction();
            auction.OnStart();

            auction.Bid(user, anyStartingPrice + 1.0, out Auction.BidStatus status);

            Assert.AreEqual(Auction.BidStatus.UserNotLoggedIn, status, "User should not be able to place a bid if not logged in.");

        }
    }
}
