using BetCommerce.Entity.Core;
using BetCommerce.WebClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetCommerce.WebClient.Pages.Shop
{
    public class IndexModel : PageModel
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<IndexModel> _logger;
        public string ServerAPIEndpoint;

        public List<Product> DataResponse { get; set; }
        public List<ProductCategory> DataCategoriesResponse { get; set; }
        public string ErrorResponse { get; set; } = null;

        public IndexModel(IHttpService httpService, ILogger<IndexModel> logger, IOptions<WebClientOptions> options)
        {
            this._httpService = httpService;
            this._logger = logger;
            this.ServerAPIEndpoint = options.Value.ApiUrl;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ErrorResponse = null;
                DataResponse = null;
                await RetrieveCategoriesAsync();
                var response = await _httpService.GetAsync<Response<List<Product>>>("api/products/");
                if (!response.IsSucess)
                    throw new Exception(response.ResponseBody);
                this.DataResponse = response.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ErrorResponse = ex.Message;
            }
            return Page();
        }

        public async Task RetrieveCategoriesAsync()
        {
            try
            {
                var response = await _httpService.GetAsync<Response<List<ProductCategory>>>("api/productCategories/");
                if (!response.IsSucess)
                    throw new Exception(response.ResponseBody);
                this.DataCategoriesResponse = response.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
