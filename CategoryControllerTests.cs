using Book.DataAceess.Repositories.Interfaces;
using Book.Models;
using BookWeb.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;

namespace BookStore.Test
{
    public class CategoryControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly CategoryController _categoryController;

        public CategoryControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _categoryController = new CategoryController(_mockUnitOfWork.Object);
        }

        [Fact]
        public void Index_ReturnsViewWithCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };
            _mockUnitOfWork.Setup(u => u.Category.GetAll(null, null)).Returns(categories);

            // Act
            var result = _categoryController.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categories, result.Model);
        }

        [Fact]
        public void Index_ReturnsEmptyView_WhenNoCategories()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Category.GetAll(null, null)).Returns(new List<Category>());

            // Act
            var result = _categoryController.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Model as IEnumerable<Category>);
        }


        [Fact]
        public void Create_ReturnsView()
        {
            // Act
            var result = _categoryController.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Create_WithValidCategory_RedirectsToIndex()
        {
            // Arrange
            var category = new Category { Name = "Category 1", DisplayOrder = 1 };

            // Act
            var result = _categoryController.Create(category) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Null(result.ControllerName); // Add this line to fix the bug
        }

        [Fact]
        public void Create_WithInvalidCategory_ReturnsViewWithModelError()
        {
            // Arrange
            var category = new Category { Name = "Category 1", DisplayOrder = 1 };
            _categoryController.ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");

            // Act
            var result = _categoryController.Create(category) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.ViewData.ModelState.ContainsKey("name"));
        }

        [Fact]
        public void Edit_WithValidId_ReturnsViewWithCategory()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Category 1" };
            _mockUnitOfWork.Setup(u => u.Category.Get(It.IsAny<Expression<Func<Category, bool>>>(), null, false)).Returns(category);

            // Act
            var result = _categoryController.Edit(categoryId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category, result.Model);
        }


        [Fact]
        public void Edit_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = _categoryController.Edit((int?)null) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("This Category Is Not Found", result.Value);
        }

        [Fact]
        public void Edit_WithInvalidCategory_ReturnsView()
        {
            // Arrange
            var category = new Category { Name = "Category 1", DisplayOrder = 1 };
            _categoryController.ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");

            // Act
            var result = _categoryController.Edit(category) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete_WithValidId_ReturnsViewWithCategory()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Category 1" };
            _mockUnitOfWork.Setup(u => u.Category.Get(It.IsAny<Expression<Func<Category, bool>>>(), null, false)).Returns(category);

            // Act
            var result = _categoryController.Delete(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category, result.Model);
        }

        [Fact]
        public void Delete_WithInvalidId_ReturnsNotFound()
        {
            // Act
            var result = _categoryController.Delete((int?)null) as NotFoundObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("This Category Is Not Found", result.Value);
        }

        [Fact]
        public void Delete_WithValidId_RedirectsToIndex()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Category 1" };
            _mockUnitOfWork.Setup(u => u.Category.Get(It.IsAny<Expression<Func<Category, bool>>>(), null, false)).Returns(category);

            // Act
            var result = _categoryController.Delete(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void DeletePost_WithValidId_RedirectsToIndex()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Category 1" };
            _mockUnitOfWork.Setup(u => u.Category.Get(It.IsAny<Expression<Func<Category, bool>>>(), null, false)).Returns(category);

            // Act
            var result = _categoryController.Delete(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
