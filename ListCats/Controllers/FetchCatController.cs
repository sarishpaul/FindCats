using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ListCats.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ListCats.Repositories;

namespace ListCats.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FetchCatController : ControllerBase
    {
        private readonly ILogger<FetchCatController> _logger;
        private readonly ICatRepo _catRepo;
       
        public FetchCatController(ILogger<FetchCatController> logger, ICatRepo catRepo)
        {
            _logger = logger;         
            _catRepo = catRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _catRepo.ProcessRepositories();
                var obj = JsonConvert.DeserializeObject<List<Owner>>(result);
                Dictionary<string, List<object>> listOfCats = new Dictionary<string, List<object>>();
                var formattedResult = obj.Where(o => o.Pets != null).SelectMany(petOwner => petOwner?.Pets, (petOwner, pet) => new { petOwner, pet }).Where(ownerAndPet => ownerAndPet.pet.Type == "Cat").Select((ownerAndPet, ind) => new { id = ind, OwnerGender = ownerAndPet.petOwner.Gender, Pet = ownerAndPet.pet.Name });
                foreach (var itm in formattedResult)
                {
                    if (listOfCats.ContainsKey(itm.OwnerGender))
                    {
                        List<object> l = listOfCats[itm.OwnerGender].ToList();
                        l.Add(itm.Pet);
                        listOfCats[itm.OwnerGender] = l;
                    }
                    else
                    {
                        List<object> l = new List<object>();
                        l.Add(itm.Pet);
                        listOfCats.Add(itm.OwnerGender, l);
                    }
                }
                List<string> keys = new List<string>(listOfCats.Keys);
                foreach (var k in keys)
                {
                    List<object> l = listOfCats[k].ToList();
                    listOfCats[k] =l.Distinct().ToList();
                }
                return Ok(listOfCats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Data manipulation went wrong");
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
