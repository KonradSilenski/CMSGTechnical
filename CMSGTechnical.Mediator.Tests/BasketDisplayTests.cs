using AngleSharp.Diffing.Extensions;
using Bunit;
using CMSGTechnical.Code;
using CMSGTechnical.Components.Shared;
using CMSGTechnical.Mediator.Dtos;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

public class BasketDisplayTests : BunitContext
{
	private ProtectedLocalStorage CreateProtectedLocalStorage()
	{
		var dataProtectionProvider =
			DataProtectionProvider.Create("bunit-tests");

		return new ProtectedLocalStorage(
			JSInterop.JSRuntime,
			dataProtectionProvider
		);
	}

	[Fact]
	public void BasketDisplay_Should_Render_Items()
	{
		var storage = CreateProtectedLocalStorage();
		var basketService = new BasketService(storage);

		basketService.Basket.MenuItems.Add(
			new MenuItemDto
			{
				Id = 1,
				Name = "Margherita Pizza",
				Description = "Classic pizza with fresh tomatoes, mozzarella cheese, and basil.",
				Price = 12.99m,
				Category = "Main"
			}
		);

		Services.AddSingleton(basketService);

		var cut = Render<BasketDisplay>(p =>
		{
			p.AddCascadingValue(basketService);
		});

		cut.Markup.Should().Contain("Margherita Pizza");
		cut.Markup.Should().Contain("£12.99");
	}

	[Fact]
	public void BasketDisplay_Should_Show_Empty_Message_When_No_Items()
	{
		var storage = CreateProtectedLocalStorage();
		var basketService = new BasketService(storage);

		Services.AddSingleton(basketService);

		var cut = Render<BasketDisplay>(p =>
		{
			p.AddCascadingValue(basketService);
		});

		cut.Markup.Should().Contain("The basket is empty");
	}

	[Fact]
	public void BasketDisplay_Should_Stack_Items_And_Calculate_Totals_Correctly()
	{
		var storage = CreateProtectedLocalStorage();
		var basketService = new BasketService(storage);

		basketService.Basket.MenuItems.AddRange(new[]
		{
		new MenuItemDto {
			Id = 1,
			Name = "Margherita Pizza",
			Description = "Classic pizza with fresh tomatoes, mozzarella cheese, and basil.",
			Price = 12.99m,
			Category = "Main"
		},
		new MenuItemDto {
			Id = 1,
			Name = "Margherita Pizza",
			Description = "Classic pizza with fresh tomatoes, mozzarella cheese, and basil.",
			Price = 12.99m,
			Category = "Main"
		},
		new MenuItemDto {
			Id = 3,
			Name = "Chocolate Cake",
			Description = "Chocolate cake description, to be filled out.",
			Price = 6.99m,
			Category = "Dessert"
		}
	});

		Services.AddSingleton(basketService);

		var cut = Render<BasketDisplay>(p =>
		{
			p.AddCascadingValue(basketService);
		});

		cut.Markup.Should().Contain("Margherita Pizza");
		cut.Markup.Should().Contain("Qty: 2 · £25.98");

		cut.Markup.Should().Contain("Chocolate Cake");
		cut.Markup.Should().Contain("Qty: 1 · £6.99");

		cut.Markup.Should().Contain("Items");
		cut.Markup.Should().Contain("£32.97");
		cut.Markup.Should().Contain("£34.97");
	}



}
