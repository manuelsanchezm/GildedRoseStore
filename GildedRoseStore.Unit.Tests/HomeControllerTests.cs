using GildedRoseStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;
using GildedRoseStore.Models;
using System.Collections.Generic;
using TestsGildedRoseStore.Unit.Tests;

namespace GildedRoseStore.Unit.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Home_Login_ReturnsAView_Success()
        {
            // Arrange
            var _logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(_logger.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }
        [Fact]
        public void Home_Login_ReturnsCompleteModel_Success()
        {
            // Arrange
            var _logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(_logger.Object);
            List<Item> testData = GetItems();

            // Act
            var model = (controller.Index() as ViewResult)?.ViewData.Model
                as IEnumerable<Item>;

            // Assert
            Assert.Equal(testData, model,
                Comparer.Get<Item>((p1, p2) => p1.id == p2.id
                && p1.name == p2.name && p1.color == p2.color
                && p1.image == p2.image && p1.price == p2.price
                && p1.size == p2.size && p1.inStock == p2.inStock));
        }

        [Fact]
        public void Home_PurchaseItem_ReturnsAView_Success()
        {
            // Arrange
            var _logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(_logger.Object);
            int id = 0;

            // Act
            ViewResult result = controller.PurchaseItem(id) as ViewResult;

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
        }

        [Fact]
        public void Home_PurchaseItem_ReturnsAValidModel_Success()
        {
            // Arrange
            var _logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(_logger.Object);
            List<Item> testData = GetItems();
            int id = 1;
            testData.Find(i => i.id == id).inStock--;


            // Act
            var model = (controller.PurchaseItem(id) as ViewResult)?.ViewData.Model
                as IEnumerable<Item>;

            // Assert
            Assert.Equal(testData, model,
                Comparer.Get<Item>((p1, p2) => p1.id == p2.id
                && p1.name == p2.name && p1.color == p2.color
                && p1.image == p2.image && p1.price == p2.price
                && p1.size == p2.size && p1.inStock == p2.inStock));
        }

        internal List<Item> GetItems()
        {
            return new List<Item>()
            {
                new Item { id = 1
                , name = "Trouser"
                , color = "blue"
                , size = "medium"
                , price = 2.5M
                , inStock = 5
                , image = "trouser.jpg" },
                new Item { id = 2
                , name = "T-Shirt"
                , color = "red"
                , size = "large"
                , price = 2.5M
                , inStock = 2
                , image = "tshirt.jpg" },
                new Item { id = 3
                , name = "Hoodie"
                , color = "white"
                , size = "small"
                , price = 2.5M
                , inStock = 2
                , image = "hoodie.jpg" }
            };
        }
    }
}
