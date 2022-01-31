using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication.Controllers;
using WebApplication.Interfaces;
using WebApplication.Models;
using WebApplication.ModelsUI;
using WebApplication.ViewModels;

namespace WebApplicationTests
{
    [TestClass]
    public class ProductControllerTests
    {
        private ProductController _productController;

        private Mock<ILogger<ProductController>> _logerMock;
        private Mock<IMapper> _maperMock;
        private Mock<IProductService> _serviceMock;

        [TestInitialize]
        public void Initialize()
        {
            _logerMock = new Mock<ILogger<ProductController>>();
            _maperMock = new Mock<IMapper>();
            _serviceMock = new Mock<IProductService>();

            _productController = new ProductController(_logerMock.Object, _maperMock.Object, _serviceMock.Object);
        }

        [TestMethod]
        public async Task GetProductsTest()
        {
            _serviceMock.Setup(x => x.GetProductsUiAsync());
            await _productController.GetProducts();
            _serviceMock.VerifyAll();
        }

        [TestMethod]
        public async Task CreateProductGetTest()
        {
            _serviceMock.Setup(x => x.GetСategoriesAsync());
            _serviceMock.Setup(x => x.GetSuppliersAsync());
            await _productController.CreateProduct();
            _serviceMock.VerifyAll();
        }

        [TestMethod]
        public async Task CreateProductTest()
        {
            _serviceMock.Setup(x => x.CreateProductAsync(It.IsAny<Product>()));
            await _productController.CreateProduct(new ProductViewModel(){Product = new ProductUI()});
            _serviceMock.VerifyAll();
        }

        [TestMethod]
        public async Task EditProductGetTest()
        {
            _serviceMock.Setup(x => x.GetProductAsync(It.IsAny<int>()));
            _serviceMock.Setup(x => x.GetСategoriesAsync());
            _serviceMock.Setup(x => x.GetSuppliersAsync());
            await _productController.EditProduct(1);
            _serviceMock.VerifyAll();
        }

        [TestMethod]
        public async Task EditProductTest()
        {
            _serviceMock.Setup(x => x.EditProductAsync(It.IsAny<Product>()));
            await _productController.EditProduct(new ProductViewModel() { Product = new ProductUI() });
            _serviceMock.VerifyAll();
        }
    }
}
