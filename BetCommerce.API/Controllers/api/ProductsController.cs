using BetCommerce.Entity.Core;
using BetCommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetCommerce.API.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            this._logger = logger;
            this._productService = productService;
        }


        [HttpGet, AllowAnonymous]
        public async Task<Response<IEnumerable<Product>>> Get(int page = -1, int size = -1)
        {
            try
            {
                IEnumerable<Product> dataFeedback = dataFeedback = await _productService.GetAllAsync(page); //Page Size Implementation
                return new Response<IEnumerable<Product>>(dataFeedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<IEnumerable<Product>>(null, false, ex.Message);
            }
        }

        [HttpGet("{id}"), AllowAnonymous]
        public async Task<Response<Product>> Get(int id)
        {
            try
            {
                Product record = await _productService.GetAsync(id);
                return new Response<Product>(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<Product>(null, false, ex.Message);
            }
        }


        [HttpPost]
        public async Task<Response<int>> Post([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                await _productService.AddAsync(product);
                return new Response<int>(product.ProductId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<int>(0, false, ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<Response<bool>> Put(int id, [FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                await _productService.UpdateAsync(product);
                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<bool>(false, false, ex.Message);
            }
        }



        [HttpDelete("{id}")]
        public async Task<Response<bool>> Delete(int id)
        {
            try
            {
                await _productService.RemoveAsync(id);
                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<bool>(false, false, ex.Message);
            }
        }
    }
}
