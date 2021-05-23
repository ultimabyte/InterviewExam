using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using InterviewExamWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InterviewExamWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly InterviewExamContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public OrdersController(InterviewExamContext context, ILogger<OrdersController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("getOrder")]
        public async Task<IActionResult> Get(int orderId)
        {
            try
            {
                var order = await _context.TOrders
                    .Include(o => o.TOrderItems)
                    .FirstOrDefaultAsync(o => o.Id == orderId)
                    .ConfigureAwait(false);

                return Ok(_mapper.Map<TOrderDTO>(order));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("createOrder")]
        public async Task<IActionResult> Post([FromBody] TOrderDTO order)
        {
            try
            {
                TOrder newOrder = _mapper.Map<TOrder>(order);

                var itemList = await _context.TShoppingItems.Where(it => it.ShoppingCartId == order.ShoppingCartId)
                                .ToListAsync().ConfigureAwait(false);

                foreach (TShoppingItem item in itemList)
                {
                    TProductItem prodItem = await _context.TProductItems.FirstOrDefaultAsync(i => i.ProductId == item.ProductId).ConfigureAwait(false);
                    TOrderItem orderItem = new TOrderItem();
                    if (prodItem != null)
                    {
                        if (prodItem.QuantityRemain >= item.Quantity)
                        {
                            prodItem.QuantityRemain = prodItem.QuantityRemain - item.Quantity;
                        }
                        else
                        {
                            item.Quantity = prodItem.QuantityRemain;
                            prodItem.QuantityRemain = 0;
                        }

                        orderItem.Order = newOrder;
                        orderItem.ProductId = item.ProductId;
                        orderItem.Price = item.Price;
                        orderItem.Quantity = item.Quantity;
                        orderItem.Total = item.Price * item.Quantity;
                        orderItem.ShoppingItem = item;

                        _context.Add(orderItem);
                    }
                    else
                    {
                        _context.TShoppingItems.Remove(item);
                    }
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);


                return Ok(_mapper.Map<TOrderDTO>(newOrder));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("updateOrderDetail")]
        public async Task<IActionResult> Put(int orderId, [FromBody] TOrderDTO order)
        {
            try
            {

                TOrder uOrder = _mapper.Map<TOrder>(order);

                _context.Update(uOrder);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                var updateOrder = await _context.TOrders
                    .Include(o => o.TOrderItems)
                    .FirstOrDefaultAsync(o => o.Id == orderId)
                    .ConfigureAwait(false);

                return Ok(_mapper.Map<TOrderDTO>(updateOrder));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("deleteOrder")]
        public async Task<IActionResult> Delete(int orderId)
        {
            try
            {
                TOrder order = _context.TOrders.Where(o => o.Id == orderId).Include(o => o.TOrderItems).FirstOrDefault();
                _context.Remove(order);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
