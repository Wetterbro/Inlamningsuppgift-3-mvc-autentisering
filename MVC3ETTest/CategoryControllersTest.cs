using System.Reflection;
using EntityFrameworkCore.Testing.Moq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC3ET.Controllers;
using MVC3ET.Data;
using MVC3ET.Models;

namespace MVC3ETTest;

[TestFixture]
public class CategorysControllersTest
{
    private ApplicationDbContext mockContext;
    private CategoryController controller;
    
    [SetUp]
    public void Setup()
    {
        // Skapa en mockad DbContext
        mockContext = Create.MockedDbContextFor<ApplicationDbContext>();
        // Konfigurera mockad DbSet
        var categories = new List<Category>
        {
            new Category
            {
                Id = 1,
                Name = "Category1",
            },
            new Category
            {
                Id = 2,
                Name = "Category2",
            }
        };
        // Använd SetupDbSet för att konfigurera DbSet
        mockContext.Set<Category>().AddRange(categories);
        mockContext.SaveChanges();
        
        mockContext.ChangeTracker.Clear();
        
        //Skapa controller med mockad DbContext
        controller = new CategoryController(mockContext);
        
    }

    [Test]
    public async Task Index_ReturnsViewResult_WithListOfCategorys()
    {
        // Act
        var result = await controller.Index();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());

        var viewResult = result as ViewResult;
        var model = viewResult.Model;
        Assert.That(model, Is.InstanceOf<List<Category>>());
        
        var Categorys = model as List<Category>;
        Assert.That(Categorys.Count, Is.EqualTo(2));
    }
    [Test]
    public void Create_ReturnsViewResult()
    {
        // Act
        var result = controller.Create();

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
    }
    
    [Test]
    public async Task Create_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
    {
        // Arrange
        var newCategory = new Category 
        {
            Id = 10,
            Name = "Category1",
        };

        // Act
        var result = await controller.Create(newCategory);

        // Assert
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        var redirectToActionResult = result as RedirectToActionResult;
        Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
    }

    [Test]
    public async Task Create_Post_ReturnsViewResult_WhenModelStateIsInvalid()
    {
        // Arrange
        controller.ModelState.AddModelError("Error", "Model error");
        var newCategory = new Category 
        {
            Id = 1,
            Name = "Category1",
        };

        // Act
        var result = await controller.Create(newCategory);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
    }
    
    [Test]
    public async Task Edit_Get_ReturnsNotFoundResult_WhenIdIsNull()
    {
        // Act
        var result = await controller.Edit(null);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
    
    [Test]
    public async Task Edit_Get_ReturnsNotFoundResult_WhenCategoryDoesNotExist()
    {
        // Act
        var result = await controller.Edit(999);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
    
    [Test]
    public async Task Edit_Get_ReturnsViewResult_WithCategory()
    {
        // Act
        var result = await controller.Edit(1);
        
        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsInstanceOf<Category>(viewResult.Model);
        var model = viewResult.Model as Category;
        Assert.That(model.Id, Is.EqualTo(1));
    }
    
    [Test]
    public async Task Edit_Post_ReturnsNotFoundResult_WhenIdDoesNotMatchCategoryId()
    {
        // Arrange
        var newCategory = new Category 
        {
            Id = 1,
            Name = "Category1",
        };
        
        // Act
        var result = await controller.Edit(100, newCategory);
        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
    [Test]
    public async Task Edit_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
    {
        // Arrange
        var newCategory = new Category 
        {
            Id = 1,
            Name = "Category1",
        };


        // Act
        var result = await controller.Edit(1, newCategory);

        // Assert
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        var redirectToActionResult = result as RedirectToActionResult;
        Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
    }
    [Test]
    public async Task Edit_Post_ReturnsViewResult_WhenModelStateIsInvalid()
    {
        // Arrange
        controller.ModelState.AddModelError("Error", "Model error");
        var newCategory = new Category 
        {
            Id = 1,
            Name = "Category1",
        };
        
        // Act
        var result = await controller.Edit(1, newCategory);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
    }
    [Test]
    public async Task Edit_Post_ReturnsNotFoundResult_WhenCategoryDoesNotExist()
    {
        // Arrange
        var newCategory = new Category 
        {
            Id = 99,
            Name = "Category1",
        };
        
        // Act
        var result = await controller.Edit(99, newCategory);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
    
    [Test]
    public async Task Delete_Get_ReturnsNotFoundResult_WhenIdIsNull()
    {
        // Act
        var result = await controller.Delete(null);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
    
    [Test]
    public async Task Delete_Get_ReturnsNotFoundResult_WhenCategoryDoesNotExist()
    {
        // Act
        var result = await controller.Delete(99);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
    [Test]
    public async Task Delete_Get_ReturnsViewResult_WithCategory()
    {
        // Act
        var result = await controller.Delete(2);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsInstanceOf<Category>(viewResult.Model);
        var model = viewResult.Model as Category;
        Assert.That(model.Id, Is.EqualTo(2));
    }
    
    [Test]
    public async Task DeleteConfirmed_ReturnsRedirectToActionResult()
    {
        // Act
        var result = await controller.DeleteConfirmed(1);

        // Assert
        Assert.IsInstanceOf<RedirectToActionResult>(result);
        var redirectToActionResult = result as RedirectToActionResult;
        Assert.That(redirectToActionResult.ActionName, Is.EqualTo("Index"));
    }
    
    [Test]
    public async Task Index_ThrowsException()
    {
        // Arrange
        mockContext.Dispose();

        // Act & Assert
        Assert.ThrowsAsync<ObjectDisposedException>(() => controller.Index());
    }
    
    [Test]
    public async Task Create_ThrowsException()
    {
        // Arrange
        mockContext.Dispose();
        var newCategory = new Category 
        {
            Id = 1,
            Name = "Category1",
        };
        // Act & Assert
        Assert.ThrowsAsync<ObjectDisposedException>(() => controller.Create(newCategory));
    }
    
    [Test]
    public void Controller_ThrowsException()
    {
        // Arrange
        mockContext.Dispose();
        var newCategory = new Category 
        {
            Id = 65,
            Name = "Category1",
        };

        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ObjectDisposedException>(() => controller.Index());
            Assert.ThrowsAsync<ObjectDisposedException>(() => controller.Create(newCategory));
            Assert.ThrowsAsync<ObjectDisposedException>(() => controller.Edit(65));
            Assert.ThrowsAsync<ObjectDisposedException>(() => controller.Edit(65, newCategory));
            Assert.ThrowsAsync<TargetInvocationException>(() => controller.Delete(65));
            Assert.ThrowsAsync<ObjectDisposedException>(() => controller.DeleteConfirmed(65));
            Assert.ThrowsAsync<TargetInvocationException>(() => controller.Details(65));
        });
    }
    
    [Test]
    public void Get_HasAllowAnonymousAttribute()
    {
        // Arrange
        var methodInfoDetails = typeof(CategoryController).GetMethod("Details",[typeof(int?)]);
        var attributesDetails = methodInfoDetails.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);
        
        var methodInfoIndex = typeof(CategoryController).GetMethod("Index");
        var attributesIndex = methodInfoIndex.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(attributesDetails, Is.Not.Empty);
            Assert.That(attributesIndex, Is.Not.Empty);
        });
    }
    [Test]
    public void Get_Has_authorize_attribute()
    {
        // Arrange
        var methodInfoCreate = typeof(CategoryController).GetMethod("Create",new Type[]{});
        var methodInfoDelete = typeof(CategoryController).GetMethod("Delete",[typeof(int?)]);
        var methodInfoEdit= typeof(CategoryController).GetMethod("Edit",[typeof(int?)]);
        
        var attributesCreate = methodInfoCreate.GetCustomAttributes(typeof(AuthorizeAttribute), false);
        var attributesDelete = methodInfoDelete.GetCustomAttributes(typeof(AuthorizeAttribute), false);
        var attributesEdit = methodInfoEdit.GetCustomAttributes(typeof(AuthorizeAttribute), false);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(attributesCreate, Is.Not.Empty);
            Assert.That(attributesDelete, Is.Not.Empty);
            Assert.That(attributesEdit, Is.Not.Empty);
        });
    }
    
    
    [Test]
    public void Controller_has_authorize_attribute()
    {
        // Arrange
        var typeInfo = typeof(CategoryController);
        // Act
        var attributes = typeInfo.GetCustomAttributes(typeof(AuthorizeAttribute), false);
        // Assert
        Assert.That(attributes, Is.Not.Empty);
    }
    
    
    [TearDown]
    public void TearDown()
    {
        mockContext.Dispose();
        CategoryController controller = null;
    }
}