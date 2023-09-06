using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductManagement.Controllers;
using ProductManagement.Data.Repositories;
using ProductManagement.Models.DomainModel;
using ProductManagement.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace ProductTest
{
    public class ProductControllerTest
    {
        [Fact]
        public void AllProducts_ReturnsViewWithProductList()
        {
            // Arrange
            var productServiceMock = new Mock<ProductService>();
            var productRepositoryMock = new Mock<ProductRepository>();
            var controller = new ProductController(productServiceMock.Object, productRepositoryMock.Object);

            // Mock ProductService's GetAllProducts method to return test data
            productServiceMock.Setup(service => service.GetAllProducts())
                .Returns(new List<ProductModel>
                {
                    new ProductModel { Id = Guid.NewGuid(), Name = "Product 1", Price = (int)10.99 },
                    new ProductModel { Id = Guid.NewGuid(), Name = "Product 2", Price = (int)19.99 }
                });

            // Act
            var result = controller.AllProducts() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ProductModel>>(result.Model);
        }

        [Fact]
        public void AddProduct_ReturnsView()
        {
            // Arrange
            var productServiceMock = new Mock<ProductService>();
            var productRepositoryMock = new Mock<ProductRepository>();
            var controller = new ProductController(productServiceMock.Object, productRepositoryMock.Object);

            // Act
            var result = controller.AddProduct() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        // Add more test cases for other actions in your ProductController
    }
}
