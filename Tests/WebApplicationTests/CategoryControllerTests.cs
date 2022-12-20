using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication.Controllers;
using WebApplication.Interfaces;
using WebApplication.Models;

namespace WebApplicationTests
{
    [TestClass]
    public class CategoryControllerTests
    {
        private CategoryController _categoryController;

        private Mock<ILogger<CategoryController>> _logerMock;
        private Mock<IMapper> _maperMock;
        private Mock<ICategoryService> _serviceMock;
        private Mock<IMemoryCache> _memoryMock;

        [TestInitialize]
        public void Initialize()
        {
            _logerMock = new Mock<ILogger<CategoryController>>();
            _maperMock = new Mock<IMapper>();
            _serviceMock = new Mock<ICategoryService>();
            _memoryMock = new Mock<IMemoryCache>();

            _categoryController = new CategoryController(_logerMock.Object, _maperMock.Object, _serviceMock.Object, _memoryMock.Object);
            _categoryController.ControllerContext = new ControllerContext();
            _categoryController.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [TestMethod]
        public async Task GetCategoriesTest()
        {
            _serviceMock.Setup(x => x.GetCategoriesAsync());
            await _categoryController.GetCategories();
            _serviceMock.VerifyAll();
        }

        [TestMethod]
        public async Task GetPictureGetTest()
        {
            _serviceMock.Setup(x => x.GetСategoryAsync(It.IsAny<int>()));
            await _categoryController.GetPicture(1);
            _serviceMock.VerifyAll();
        }

        [TestMethod]
        public async Task GetImageTest()
        {
            _serviceMock.Setup(x => x.GetСategoryImageAsync(It.IsAny<int>()));
            await _categoryController.GetImage(1);
            _serviceMock.VerifyAll();
        }

        [TestMethod]
        public async Task GetPictureTest()
        {
            _serviceMock.Setup(x => x.GetСategoryAsync(It.IsAny<int>()));
            _serviceMock.Setup(x => x.EditСategoryAsync(It.IsAny<Сategory>()));
            await _categoryController.GetPicture(1, default);
            _serviceMock.VerifyAll();
        }
    }
}
