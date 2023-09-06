using Xunit;
using ProductManagement.Controllers;
using ProductManagement.Models.DomainModel;
using ProductManagement.Models.ViewModel;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TestProduct
{
    public class ProductControllerTest
    {
        [Fact]
        public void TestAllProducts()
        {
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productServiceMock.Object);

            var products = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = Guid.NewGuid(),
                    Name = "iPhone",
                    Price = 10000
                },
                new ProductModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Vivo",
                    Price = 20000
                }
            };

            productServiceMock.Setup(service => service.GetAllProducts())
                .Returns(products);
            var result = productController.AllProducts() as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(products, result.Model);
            Assert.Null(result.ViewName);
        }

        [Fact]
        public void TestAddProduct()
        {
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productServiceMock.Object);

            var productToAdd = new ProductModel
            {
                Id = Guid.NewGuid(),
                Name = "iPhone",
                Price = 10
            };

            productServiceMock.Setup(service => service.AddProduct(It.IsAny<ProductModel>()))
                .Verifiable();
            var result = productController.AddProduct(productToAdd) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("AllProducts", result.ActionName);

            productServiceMock.Verify(service => service.AddProduct(productToAdd), Times.Once);
        }

        [Fact]
        public void TestEditProduct()
        {
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productServiceMock.Object);

            var productId = Guid.NewGuid();
            var productToEdit = new ProductModel
            {
                Id = productId,
                Name = "iPhone",
                Price = 15000
            };

            productServiceMock.Setup(service => service.GetProductById(productId))
                .Returns(productToEdit);
            var result = productController.EditProduct(productId) as ViewResult;
            Assert.NotNull(result);
            var model = Assert.IsType<ProductModel>(result.Model);
            Assert.Equal(productToEdit, model);
        }

        [Fact]
        public void TestDeleteProduct()
        {
            var productServiceMock = new Mock<IProductService>();
            var productController = new ProductController(productServiceMock.Object);

            var productId = Guid.NewGuid();

            productServiceMock.Setup(service => service.DeleteProduct(productId))
                .Verifiable();
            var result = productController.DeleteProduct(productId) as RedirectToActionResult;
            Assert.NotNull(result);
            Assert.Equal("AllProducts", result.ActionName);

            productServiceMock.Verify(service => service.DeleteProduct(productId), Times.Once);
        }

        [Fact]
        public void TestViewProducts()
        {
            var productServiceMock = new Mock<IProductService>();
            var userController = new UserController(productServiceMock.Object);

            var products = new List<ProductModel>
            {
                new ProductModel
                {
                    Id = Guid.NewGuid(),
                    Name = "iPhone",
                    Price = 10000
                },
                new ProductModel
                {
                    Id = Guid.NewGuid(),
                    Name = "Vivo",
                    Price = 20000
                }
            };

            productServiceMock.Setup(service => service.AllProducts())
                .Returns(products);
            var result = userController.ViewProducts() as ViewResult;
            Assert.NotNull(result);
            Assert.Equal(products, result.Model);
            Assert.Null(result.ViewName);

        }
    }
}
