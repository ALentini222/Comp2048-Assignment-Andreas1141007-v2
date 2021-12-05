using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comp2048_Assignment_Andreas1141007.Controllers;
using Comp2048_Assignment_Andreas1141007.Data;
using Comp2048_Assignment_Andreas1141007.Models;
namespace Comp2048_Assignment_Andreas1141007_Test


{
    [TestClass]
    public class GamesControllerTest
    {
        private ApplicationDbContext _context;
        private GamesController controller;
        List<Game> games = new List<Game>();

        [TestInitialize]
        public void TestInitialize()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            _context = new ApplicationDbContext(options);

            controller = new GamesController(_context);

            var Category = new Category
            {
                CategoryId = 1,
                GameName = "Pac-Man"
            };
            games.Add(new Game
            {
                GameId = 2,
                GameName = "Mario",
                AveragePlaytime = 6,
                AverageRating = 10,
            });
            games.Add(new Game
            {
                GameId = 4,
                GameName = "Luigi",
                AveragePlaytime = 3,
                AverageRating = 5,
            });
            foreach (var game in games)
            {
                _context.Games.Add(game);
            }
            _context.SaveChanges();


        }
        #region Index

        [TestMethod]
        public void IndexLoadsCorrectView()
        {
            // act
            var result = (ViewResult)controller.Index().Result;

            // assert
            Assert.AreEqual("Index", result.ViewName);

        }

        [TestMethod]
        public void IndexLoadsGames()
        {
            var result = (ViewResult)controller.Index().Result;
            List<Game> model = (List<Game>)result.Model;

            CollectionAssert.AreEqual(_context.Games.ToList(), model);
        }
        #endregion
        #region Details
        [TestMethod]
        public void DetailsNoIdLoads404()
        {
            var result = (ViewResult)controller.Details(null).Result;

            Assert.AreEqual("404", result.ViewName);
        }
        [TestMethod]
        public void DetailsInvalidIdLoads404()
        {
            var result = (ViewResult)controller.Details(-10).Result;

            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void DetailsReturnsValidView()
        {
            var result = (ViewResult)controller.Details(2).Result;
            Game game = (Game)result.Model;

            Assert.AreEqual(games[0], game);
        }

        [TestMethod]
        public void DetailsValidLoadsView()
        {
            var result = (ViewResult)controller.Details(2).Result;

            Assert.AreEqual("Details", result.ViewName);
        }
        #endregion        
        #region Create
        [TestMethod]
        public void CreateReturnsValidList()
        {
            var result = controller.Create();
            var data = controller.ViewData["GameId"];
            Assert.IsNotNull(data);
        }
        [TestMethod]
        public void CreateViewLoads()
        {
            var result = (ViewResult)controller.Create();
            Assert.AreEqual("Create", result.ViewName);
        }
        [TestMethod]
        public void CreatePostReturnsCreate()
        {
            //create test produst            
            var games = new Game { };
            controller.ModelState.AddModelError("Name", "Key");
            var result = controller.Create(games);
            var viewResult = (ViewResult)result.Result;

            // assert         
            Assert.AreEqual("Create", viewResult.ViewName);
        }

        [TestMethod]
        public void CreateGameToDb()
        {
            var game = new Game { GameId = 4, GameName = "Gaming", AveragePlaytime = 44, AverageRating = 6 };
            _context.Games.Add(game);
            _context.SaveChanges();
            Assert.AreEqual(game, _context.Games.ToArray()[3]);
        }
        
        #endregion
        #region Edit
        [TestMethod]
        public void EditIdReturnsNull()
        {
            var result = (ViewResult)controller.Edit(null).Result;

            Assert.AreEqual("404", result.ViewName);
        }
        [TestMethod]
        public void EditIdReturnsInvalid()
        {
            var result = (ViewResult)controller.Edit(-12).Result;

            Assert.AreEqual("404", result.ViewName);
        }
        [TestMethod]
        public void EditLoadsSuccessfully()
        {
            var result = (ViewResult)controller.Edit(2).Result;
            Game game = (Game)result.Model;
            Assert.AreEqual(_context.Games.Find(2), game);

        }
        [TestMethod]
        public void EditSave()
        {
            var game = games[0];
            game.GameName = "New Name";
            var result = controller.Edit(game.GameId, game);
            var redirectResult = (RedirectToActionResult)result.Result;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }
        #endregion
        #region Delete
        [TestMethod]
        public void DeleteNullId()
        {
            var result = (ViewResult)controller.Delete(null).Result;
            Assert.AreEqual("404", result.ViewName);
        }

        [TestMethod]
        public void DeleteIdNotExists()
        {
            var result = (ViewResult)controller.Delete(99).Result;
            Assert.AreEqual("404", result.ViewName);
        }
        [TestMethod]
        public void DeleteIdExists()
        {
            var result = (ViewResult)controller.Delete(2).Result;
            Assert.AreEqual("Delete", result.ViewName);
        }
        [TestMethod]
        public void DeleteGame()
        {
            var result = (ViewResult)controller.Delete(2).Result;
            Game game = (Game)result.Model;

            Assert.AreEqual(games[0], game);
        }
        [TestMethod]
        public void DeleteConfirmedSuccess()
        {
            var result = controller.DeleteConfirmed(2);
            var product = _context.Games.Find(2);
            Assert.AreEqual(product, null);
        }
        [TestMethod]
        public void DeleteConfirmedRedirectIndex()
        {
            var result = controller.DeleteConfirmed(2); 
            var actionResult = (RedirectToActionResult)result.Result;
            Assert.AreEqual("Index", actionResult.ActionName);
        }
        #endregion   
    }

 
}