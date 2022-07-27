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
    public class ProductCategoriesController : ControllerBase
    {
        private readonly ILogger<ProductCategoriesController> _logger;
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoriesController(ILogger<ProductCategoriesController> logger, IProductCategoryService productCategoryService)
        {
            this._logger = logger;
            this._productCategoryService = productCategoryService;
        }


        [HttpGet, AllowAnonymous]
        public async Task<Response<IEnumerable<ProductCategory>>> GetAsync(int page = -1, int size = -1)
        {
            try
            {
                IEnumerable<ProductCategory> dataFeedback = dataFeedback = await _productCategoryService.GetAllAsync(page, size); //Page Size Implementation
                return new Response<IEnumerable<ProductCategory>>(dataFeedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<IEnumerable<ProductCategory>>(null, false, ex.Message);
            }
        }

        [HttpGet("{id}"), AllowAnonymous]
        public async Task<Response<ProductCategory>> GetAsync(int id)
        {
            try
            {
                ProductCategory record = await _productCategoryService.GetAsync(id);
                return new Response<ProductCategory>(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<ProductCategory>(null, false, ex.Message);
            }
        }


        [HttpPost]
        public async Task<Response<int>> PostAsync([FromBody] ProductCategory category)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                await _productCategoryService.AddAsync(category);
                return new Response<int>(category.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<int>(0, false, ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<Response<bool>> PutAsync(int id, [FromBody] ProductCategory category)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                await _productCategoryService.UpdateAsync(category);
                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<bool>(false, false, ex.Message);
            }
        }



        [HttpDelete("{id}")]
        public async Task<Response<bool>> DeleteAsync(int id)
        {
            try
            {
                await _productCategoryService.RemoveAsync(id);
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
