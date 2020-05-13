using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewLeaf.Services.Interface;
using NewLeaf.Services.Models.Entities;
using System.Collections.Generic;
using System;

namespace AnimalCrossingPrices.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TownController : Controller
    { 
        private readonly ITownService TownService;

        public TownController(ITownService townService)
        {
            this.TownService = townService;
        }

        [HttpGet("Get")]
        public async Task<List<TownEntity>> Get(string userName)
        {
            return await TownService.GetMyTowns(userName);
        }

        [HttpGet("Add")]
        public async Task<TownEntity> Add(string userName, string townName, string mayorName, DateTime createdDate, int nativeFruit)
        {
            return await TownService.CreateNewTown(userName, townName, mayorName, createdDate, nativeFruit);
        }

        //[HttpGet("Delete")]
        //public async Task Delete()
        //{
        //    return;
        //}

    }
}
