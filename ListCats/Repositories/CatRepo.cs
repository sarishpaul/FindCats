using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ListCats.Repositories
{
    public class CatRepo : ICatRepo
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string _requesturl;    
        private readonly IConfiguration _configuration;

        public CatRepo(IConfiguration configuration)
        {
            _requesturl = configuration.GetSection("RequestURL").Value;
        }
        public async Task<string> ProcessRepositories()
        {
            try
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/jon"));

                var getCats = client.GetStringAsync(_requesturl);

                return await getCats;
            }
            catch(Exception ex)
            {
                throw new Exception("Fetching data failed.", ex);
            }
        }
    }
}

