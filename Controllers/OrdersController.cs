using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewExamWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InterviewExamWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly InterviewExamContext _context;
        private readonly ILogger _logger;

        public OrdersController(InterviewExamContext context, ILogger<OrdersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("getOrder")]
        public async Task<IActionResult> Get(int orderId)
        {
            try
            {
                var order = await _context.TOrders.Include(o => o.TOrderItems)
                    .Include(o => o.ShoppingCart)
                    .ThenInclude(oi => oi.TShoppingItems)
                    .FirstOrDefaultAsync(o => o.Id == orderId)
                    .ConfigureAwait(false);

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("createOrder")]
        public async Task<IActionResult> Post([FromBody] TOrder order)
        {
            try
            {
                TOrder newOrder = new TOrder()
                {
                    ShoppingCartId = order.ShoppingCartId,
                    Address1 = order.Address1,
                    Address2 = order.Address2,
                    CustomerName = order.CustomerName,
                    Email = order.Email,
                    Telephone = order.Telephone
                };

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

                return Ok(newOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("updateOrderDetail")]
        public async Task<IActionResult> Put([FromBody] TOrder order)
        {
            try
            {
                TOrder updateOrder = await _context.TOrders.FirstOrDefaultAsync(o => o.Id == order.Id).ConfigureAwait(true);

                if (updateOrder != null)
                {
                    updateOrder.CustomerName = order.CustomerName;
                    updateOrder.Address1 = order.Address1;
                    updateOrder.Address2 = order.Address2;
                    updateOrder.Email = order.Email;
                    updateOrder.Telephone = order.Telephone;

                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }

                return Ok(order.Id);
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
