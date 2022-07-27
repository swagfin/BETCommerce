using AutoMapper;
using BetCommerce.Entity.Core;
using BetCommerce.Entity.Core.Requests;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService, IMapper mapper)
        {
            this._logger = logger;
            this._orderService = orderService;
            this._mapper = mapper;
        }


        [HttpGet]
        public async Task<Response<IEnumerable<Order>>> GetAsync(int page = -1, int size = -1)
        {
            try
            {
                IEnumerable<Order> dataFeedback = dataFeedback = await _orderService.GetAllAsync(page, size); //Page Size Implementation
                return new Response<IEnumerable<Order>>(dataFeedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<IEnumerable<Order>>(null, false, ex.Message);
            }
        }

        [Authorize]
        [HttpGet("GetOrdersByUserId/{userId}")]
        public async Task<Response<IEnumerable<Order>>> GetOrdersByUserIdAsync(string userId, int page = -1, int size = -1)
        {
            try
            {
                IEnumerable<Order> dataFeedback = dataFeedback = await _orderService.GetAllByUserIdAsync(userId, page, size); //Page Size Implementation
                return new Response<IEnumerable<Order>>(dataFeedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<IEnumerable<Order>>(null, false, ex.Message);
            }
        }

        [HttpPost]
        public async Task<Response<int>> PostAsync([FromBody] OrderRequest orderRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                Order model = _mapper.Map<Order>(orderRequest);
                await _orderService.AddAsync(model);
                return new Response<int>(model.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<int>(0, false, ex.Message);
            }
        }


        [HttpGet("{id}")]
        public async Task<Response<Order>> GetAsync(int id)
        {
            try
            {
                Order record = await _orderService.GetAsync(id);
                return new Response<Order>(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<Order>(null, false, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<Response<bool>> PutAsync(int id, [FromBody] Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                await _orderService.UpdateAsync(order);
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
                await _orderService.RemoveAsync(id);
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
