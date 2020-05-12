using System;
using System.Threading.Tasks;
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
    
        private readonly IItemsService AnimalCrossingStorageService;


        public ItemPricesController(IItemsService animalCrossingStorageService)
        {
            this.AnimalCrossingStorageService = animalCrossingStorageService;
        }

        [HttpGet("GetAllItems")]
        public async Task<List<ItemEntity>> GetAllItems()
        {
            return await AnimalCrossingStorageService.GetAllItems();
        }

        [HttpGet("Add")]
        public async Task<bool> Add(string itemName, string price)
        {
            if (Int32.TryParse(price, out int priceValue))
            {
                await AnimalCrossingStorageService.AddPriceForItem(itemName, priceValue);
                return true;
            }
            return false;
        }

        [HttpGet("Delete")]
        public async Task Delete(string itemName)
        {
            await AnimalCrossingStorageService.RemoveItemByName(itemName);
        }

    }
}
