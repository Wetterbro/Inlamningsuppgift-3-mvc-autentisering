using System.Reflection;
using EntityFrameworkCore.Testing.Moq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC3ET.Controllers;
using MVC3ET.Data;
using MVC3ET.Models;
using MVC3ET.ViewModels;

namespace MVC3ETTest;

[TestFixture]
public class ProductsControllersTest
{
    private ApplicationDbContext mockContext;
    private ProductController controller;
    
    [SetUp]
    public void Setup()
    {

        mockContext = Create.MockedDbContextFor<ApplicationDbContext>();
        var Products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Product1",
                Price = 10,
                CategoryId = 2
            },
            new Product
            {
                Id = 2,
                Name = "Product2",
                Price = 20,
                CategoryId = 1
            }
        };
        // Använd SetupDbSet för att konfigurera DbSet
        mockContext.Set<Product>().AddRange(Products);
        mockContext.SaveChanges();
        
        mockContext.ChangeTracker.Clear();
        
        //Skapa controller med mockad DbContext
        controller = new ProductController(mockContext);
        
    }

    [Test]
    public async Task Index_ReturnsViewResult_WithListOfProducts()
    {
        // Act
        var result = await controller.Index();

        // Assert
        Assert.That(result, Is.InstanceOf<ViewResult>());

        var viewResult = result as ViewResult;
        var model = viewResult.Model;
        Assert.That(model, Is.InstanceOf<List<Product>>());
        
        var Products = model as List<Product>;
        Assert.That(Products.Count, Is.EqualTo(2));
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
        var newProduct = new Product 
        {
            Name = "Product1",
            Price = 10,
            CategoryId = 2 
        };
        var newProductVM = new ProductCreateViewModel { Products = newProduct };
        
        // Act
        var result = await controller.Create(newProductVM);

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
        var newProduct = new Product 
        {
            Name = "Product1",
            Price = 10,
            CategoryId = 2 
        };
        var newProductVM = new ProductCreateViewModel { Products = newProduct };

        // Act
        var result = await controller.Create(newProductVM);

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
    public async Task Edit_Get_ReturnsNotFoundResult_WhenProductDoesNotExist()
    {
        // Act
        var result = await controller.Edit(999);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
    [Test]
    public async Task Edit_Get_ReturnsViewResult_WithProduct()
    {
        // Act
        var result = await controller.Edit(1);
        
        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsInstanceOf<ProductCreateViewModel>(viewResult.Model);
        var model = viewResult.Model as ProductCreateViewModel;
        Assert.That(model.Products.Id, Is.EqualTo(1));
    }
    
    [Test]
    public async Task Edit_Post_ReturnsNotFoundResult_WhenIdDoesNotMatchProductId()
    {
        // Arrange
        var newProduct = new Product 
        {
            Name = "Product1",
            Price = 10,
            CategoryId = 2 
        };
        var newProductVM = new ProductCreateViewModel { Products = newProduct };
        // Act
        var result = await controller.Edit(100, newProductVM);
        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
    [Test]
    public async Task Edit_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
    {
        // Arrange
        var newProduct = new Product 
        {
            Id = 1,
            Name = "Product1",
            Price = 10,
            CategoryId = 20
        };
        var newProductVM = new ProductCreateViewModel { Products = newProduct };

        // Act
        var result = await controller.Edit(1, newProductVM);

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
        var newProduct = new Product 
        {
            Id = 1,
            Name = "Product1",
            Price = 10,
            CategoryId = 20
        };
        var newProductVM = new ProductCreateViewModel { Products = newProduct };
        
        // Act
        var result = await controller.Edit(1, newProductVM);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
    }
    [Test]
    public async Task Edit_Post_ReturnsNotFoundResult_WhenProductDoesNotExist()
    {
        // Arrange
        var newProduct = new Product 
        {
            Id = 99,
            Name = "Product1",
            Price = 10,
            CategoryId = 20
        };
        var newProductVM = new ProductCreateViewModel { Products = newProduct };
        
        // Act
        var result = await controller.Edit(99, newProductVM);

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
    public async Task Delete_Get_ReturnsNotFoundResult_WhenProductDoesNotExist()
    {
        // Act
        var result = await controller.Delete(99);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
    [Test]
    public async Task Delete_Get_ReturnsViewResult_WithProduct()
    {
        // Act
        var result = await controller.Delete(2);

        // Assert
        Assert.IsInstanceOf<ViewResult>(result);
        var viewResult = result as ViewResult;
        Assert.IsInstanceOf<ProductCreateViewModel>(viewResult.Model);
        var model = viewResult.Model as ProductCreateViewModel;
        Assert.That(model.Products.Id, Is.EqualTo(2));
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
        var newProduct = new Product 
        {
            Name = "Product1",
            Price = 10,
            CategoryId = 2 
        };
        var newProductVM = new ProductCreateViewModel { Products = newProduct };
        // Act & Assert
        Assert.ThrowsAsync<ObjectDisposedException>(() => controller.Create(newProductVM));
    }
    
    [Test]
    public void Controller_ThrowsException()
    {
        // Arrange
        mockContext.Dispose();
        var newProduct = new Product 
        {
            Id = 65,
            Name = "Product1",
            Price = 10,
            CategoryId = 2 
        };
        var newProductVM = new ProductCreateViewModel { Products = newProduct };

        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.ThrowsAsync<ObjectDisposedException>(() => controller.Index());
            Assert.ThrowsAsync<ObjectDisposedException>(() => controller.Create(newProductVM));
            Assert.ThrowsAsync<ObjectDisposedException>(() => controller.Edit(65));
            Assert.ThrowsAsync<ObjectDisposedException>(() => controller.Edit(65, newProductVM));
            Assert.ThrowsAsync<TargetInvocationException>(() => controller.Delete(65));
            Assert.ThrowsAsync<ObjectDisposedException>(() => controller.DeleteConfirmed(65));
            Assert.ThrowsAsync<TargetInvocationException>(() => controller.Details(65));
        });
    }
    
    [Test]
    public void Get_HasAllowAnonymousAttribute()
    {
        // Arrange
        var methodInfoDetails = typeof(ProductController).GetMethod("Details",[typeof(int?)]);
        var attributesDetails = methodInfoDetails.GetCustomAttributes(typeof(AllowAnonymousAttribute), false);
        
        var methodInfoIndex = typeof(ProductController).GetMethod("Index");
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
        var methodInfoCreate = typeof(ProductController).GetMethod("Create",new Type[]{});
        var methodInfoDelete = typeof(ProductController).GetMethod("Delete",[typeof(int?)]);
        var methodInfoEdit= typeof(ProductController).GetMethod("Edit",[typeof(int?)]);
        
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
        var typeInfo = typeof(ProductController);
        // Act
        var attributes = typeInfo.GetCustomAttributes(typeof(AuthorizeAttribute), false);
        // Assert
        Assert.That(attributes, Is.Not.Empty);
    }
    
    
    
    [TearDown]
    public void TearDown()
    {
        mockContext.Dispose();
        ProductController controller = null;
    }
}