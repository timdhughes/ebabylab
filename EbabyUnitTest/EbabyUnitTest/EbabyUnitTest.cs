using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eBabyLab;

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
        const decimal anyStartingPrice = 100.00M;


        User CreateDefaultUser()
        {
            return new User(anyFirstName, anyLastName, anyUserEmail, anyUserName, anyPassword);
        }

        User CreateDefaultBidder()
        {
            return new User("Bidder", "LastName", "bidder@gmail.com", "bidder", "bidder123!")
            { LoggedIn = true };
      
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

        Auction CreateSuccessfulAuction(Auction.ItemCategory itemCategory = Auction.ItemCategory.Misc)
        {
            Auction auction = CreateDefaultAuction();
            auction.Category = itemCategory;
            auction.OnStart();
            decimal bidAmount = anyStartingPrice + 1.00M;
            auction.Bid(CreateDefaultBidder(), bidAmount, out Auction.BidStatus status);
            auction.OnClose();

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
            decimal startingPrice = 100;
            DateTime startTime = DateTime.Now.AddDays(1);
            DateTime endTime = startTime.AddDays(5);

            // seller, desc, startingPrice, startTime, endTime
            // constraints - user must be a seller, seller logged in
            // start time > now, end time> start time
            Auction auction = Auction.Create(seller, itemDesc, startingPrice, startTime, endTime);
            Assert.IsNotNull(auction, "Auction can not be created");

            Assert.AreEqual(auction.Seller.UserName, anyUserName, "Username does not match");
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
            decimal bidAmount = auction.StartingPrice + 1.00M;
            bool success = auction.Bid(user, bidAmount, out Auction.BidStatus status);

            Assert.IsTrue(success, "Bid was not successful." + status.ToString());
            Assert.AreEqual(auction.CurrentPrice, bidAmount);
            Assert.AreEqual(auction.HighestBidder.UserName, user.UserName);
        }

        [TestMethod]

        public void TestUserCanNotBidIfLoggedOut()
        {
            User user = CreateDefaultUser();
            user.LoggedIn = false;

            Auction auction = CreateDefaultAuction();
            auction.OnStart();

            auction.Bid(user, anyStartingPrice + 1.0M, out Auction.BidStatus status);

            Assert.AreEqual(Auction.BidStatus.UserNotLoggedIn, status, "User should not be able to place a bid if not logged in.");

        }

        [TestMethod]

        public void TestNotificationHighestBidder()
        {
            
            Auction auction = CreateDefaultAuction();
            auction.OnStart();
            decimal bidAmount = anyStartingPrice + 1.00M;
            auction.Bid(CreateDefaultBidder(), bidAmount, out Auction.BidStatus status);

            auction.OnClose();

            string expectedMessage = "Congratulations! You won an auction for a " + auction.ItemDesc + " from " + anyUserName + " for " + bidAmount.ToString("C2") + ".";

            string message = auction.PostOffice.FindEmail(auction.HighestBidder.UserEmail, expectedMessage);

            Assert.IsFalse(string.IsNullOrEmpty(message), "Expected: " + expectedMessage);
        }

        [TestMethod]

        public void TestNotificationNoBidders()
        {

            Auction auction = CreateDefaultAuction();
            auction.OnStart();   
            auction.OnClose();

            string expectedMessage = "Sorry, your auction for " + auction.ItemDesc + " did not have any bidders.";
            string message = auction.PostOffice.FindEmail(auction.Seller.UserEmail, expectedMessage);
            Assert.IsFalse(string.IsNullOrEmpty(message), "Expected: " + expectedMessage);
        }

        [TestMethod]

        public void TestNotifySellerOnSuccessfullAuction()
        {
            Auction auction = CreateSuccessfulAuction();        
            string expectedMessage = "Your " + auction.ItemDesc + " auction sold to bidder " + auction.HighestBidder.UserEmail + " for " + auction.CurrentPrice.ToString("C2") + ".";
            string message = auction.PostOffice.FindEmail(auction.Seller.UserEmail, expectedMessage);
            Assert.IsFalse(string.IsNullOrEmpty(message), "Expected: " + expectedMessage);
        }

        [TestMethod]

        public void TestSellerAmount()
        {
            Auction auction = CreateSuccessfulAuction();
            Assert.AreEqual(.98M * auction.CurrentPrice, auction.SellerAmount);
        }

        [TestMethod]
        public void TestBuyerAmountMisc()
        {
            Auction auction = CreateSuccessfulAuction(Auction.ItemCategory.Misc);
            Assert.AreEqual(10.00M + auction.CurrentPrice, auction.BuyerAmount);

        }

        [TestMethod]
        public void TestBuyerAmountCar()
        {
            Auction auction = CreateSuccessfulAuction(Auction.ItemCategory.Car);
            Assert.AreEqual(auction.CurrentPrice + 1000.00M, auction.BuyerAmount);

        }

        [TestMethod]
        public void TestBuyerAmountCarOver50k()
        {
            Auction auction = CreateDefaultAuction();
            auction.Category = Auction.ItemCategory.Car;
            auction.OnStart();
            decimal bidAmount = 50000.01M;
            auction.Bid(CreateDefaultBidder(), bidAmount, out Auction.BidStatus status);
            auction.OnClose();
            Assert.AreEqual(auction.CurrentPrice * 1.04M + 1000.0M, auction.BuyerAmount);
            

        }

        [TestMethod]
        public void TestBuyerAmountDownloadableSoftware()
        {
            Auction auction = CreateSuccessfulAuction(Auction.ItemCategory.DownloadableSoftware);
            
            Assert.AreEqual(auction.CurrentPrice, auction.BuyerAmount);

        }


    }
}
