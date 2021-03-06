﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewLeaf.Services.Interface;
using NewLeaf.Services.Models.Entities;
using System.Collections.Generic;

namespace AnimalCrossingPrices.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ItemPricesController : Controller
    { 
        private readonly IItemsService ItemService;

        public ItemPricesController(IItemsService animalCrossingStorageService)
        {
            this.ItemService = animalCrossingStorageService;
        }

        [HttpGet("Get")]
        public async Task<List<ItemEntity>> GetAllItems()
        {
            return await ItemService.GetAllItems();
        }

        [HttpGet("Add")]
        public async Task<bool> Add(string itemName, string price)
        {
            if (int.TryParse(price, out int priceValue))
            {
                await ItemService.AddPriceForItem(itemName, priceValue);
                return true;
            }
            return false;
        }

        [HttpGet("Delete")]
        public async Task Delete(string itemName)
        {
            await ItemService.RemoveItemByName(itemName);
        }

    }
}
