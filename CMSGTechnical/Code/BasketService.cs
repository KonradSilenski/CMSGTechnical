using CMSGTechnical.Domain.Models;
using CMSGTechnical.Mediator.Basket;
using CMSGTechnical.Mediator.Dtos;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Cryptography;

namespace CMSGTechnical.Code
{

    public class BasketChangedEventArgs : EventArgs
    {
        public BasketDto Basket { get; set; }
    }

public class BasketService
    {
        private const string StorageKey = "basket";

        private readonly ProtectedLocalStorage _localStorage;

        public BasketDto Basket { get; private set; } = new();
        public event EventHandler? OnChange;

        public BasketService(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

		public async Task LoadAsync()
		{
			try
			{
				var result = await _localStorage.GetAsync<BasketDto>(StorageKey);

				if (result.Success && result.Value != null)
				{
					Basket = result.Value;
					NotifyStateChanged();
				}
			}
			catch (CryptographicException)
			{
				await _localStorage.DeleteAsync(StorageKey);
				Basket = new BasketDto();
			}
		}


		public async Task Add(MenuItemDto item)
        {
            Basket.MenuItems.Add(item);
            await PersistAsync();
        }

        public async Task Remove(MenuItemDto item)
        {
            var existing = Basket.MenuItems.FirstOrDefault(i => i.Id == item.Id);
            if (existing != null)
            {
                Basket.MenuItems.Remove(existing);
                await PersistAsync();
            }
        }

        private async Task PersistAsync()
        {
            await _localStorage.SetAsync(StorageKey, Basket);
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
            => OnChange?.Invoke(this, EventArgs.Empty);
    }


}
