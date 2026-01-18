using Bunit;
using CMSGTechnical.Code;
using CMSGTechnical.Components.Pages;
using CMSGTechnical.Mediator.Dtos;
using CMSGTechnical.Mediator.Menu;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Moq;


public class MenuDisplayTests : BunitContext
{
	private static BasketService CreateBasketService(BunitContext ctx)
	{
		var dataProtectionProvider =
			DataProtectionProvider.Create("bunit-tests");

		var storage = new ProtectedLocalStorage(
			ctx.JSInterop.JSRuntime,
			dataProtectionProvider);

		return new BasketService(storage);
	}

	[Fact]
	public void Home_Should_Render_Menu_Items_Grouped_By_Category()
	{
		var menuItems = new[]
		{
		new MenuItemDto
		{
			Id = 1,
			Name = "Margherita Pizza",
			Description = "Classic pizza with fresh tomatoes, mozzarella cheese, and basil.",
			Price = 12.99m,
			Category = "Main"
		},
		new MenuItemDto
		{
			Id = 2,
			Name = "Caesar Salad",
			Description = "Crisp romaine lettuce with Caesar dressing, croutons, and parmesan cheese.",
			Price = 8.99m,
			Category = "Starter"
		},
		new MenuItemDto
		{
			Id = 3,
			Name = "Chocolate Cake",
			Description = "Chocolate cake description, to be filled out.",
			Price = 6.99m,
			Category = "Dessert"
		}
	};

		var mediator = new Mock<IMediator>();
		mediator
			.Setup(m => m.Send(It.IsAny<GetMenuItems>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(menuItems);

		Services.AddSingleton(mediator.Object);

		var basket = CreateBasketService(this);

		var cut = Render<Home>(p =>
		{
			p.AddCascadingValue(basket);
		});

		cut.Markup.Should().Contain("Main");
		cut.Markup.Should().Contain("Starter");
		cut.Markup.Should().Contain("Dessert");

		cut.Markup.Should().Contain("Margherita Pizza");
		cut.Markup.Should().Contain("Caesar Salad");
		cut.Markup.Should().Contain("Chocolate Cake");
	}

	[Fact]
	public void Home_Should_Render_All_Menu_Items_With_All_Fields()
	{
		var menuItems = new[]
		{
		new MenuItemDto
		{
			Id = 1,
			Name = "Margherita Pizza",
			Description = "Classic pizza with fresh tomatoes, mozzarella cheese, and basil.",
			Price = 12.99m,
			Category = "Main"
		},
		new MenuItemDto
		{
			Id = 2,
			Name = "Caesar Salad",
			Description = "Crisp romaine lettuce with Caesar dressing, croutons, and parmesan cheese.",
			Price = 8.99m,
			Category = "Starter"
		},
		new MenuItemDto
		{
			Id = 3,
			Name = "Chocolate Cake",
			Description = "Chocolate cake description, to be filled out.",
			Price = 6.99m,
			Category = "Dessert"
		}
	};

		var mediator = new Mock<IMediator>();
		mediator
			.Setup(m => m.Send(It.IsAny<GetMenuItems>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(menuItems);

		Services.AddSingleton(mediator.Object);

		var basket = CreateBasketService(this);

		var cut = Render<Home>(p =>
		{
			p.AddCascadingValue(basket);
		});

		cut.Markup.Should().Contain("Main");
		cut.Markup.Should().Contain("Starter");
		cut.Markup.Should().Contain("Dessert");

		foreach (var item in menuItems)
		{
			cut.Markup.Should().Contain(item.Name);
			cut.Markup.Should().Contain(item.Description);
			cut.Markup.Should().Contain($"£{item.Price}");
		}

		cut.FindAll(".menu-item").Count.Should().Be(menuItems.Length);
	}

	[Fact]
	public void Home_Should_Render_Add_Button_For_Each_Menu_Item()
	{
		var menuItems = new[]
		{
		new MenuItemDto {
			Id = 1,
			Name = "Margherita Pizza",
			Description = "Classic pizza with fresh tomatoes, mozzarella cheese, and basil.",
			Price = 12.99m,
			Category = "Main"
		}
	};

		var mediator = new Mock<IMediator>();
		mediator
			.Setup(m => m.Send(It.IsAny<GetMenuItems>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(menuItems);

		Services.AddSingleton(mediator.Object);

		var basket = CreateBasketService(this);

		var cut = Render<Home>(p =>
		{
			p.AddCascadingValue(basket);
		});

		cut.FindAll("button")
		   .Any(b => b.TextContent.Trim() == "+")
		   .Should().BeTrue();
	}

}