using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewExamWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InterviewExamWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly InterviewExamContext _context;
        private readonly ILogger _logger;

        public CartController(InterviewExamContext context, ILogger<CartController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET api/<CartController>/5
        [HttpGet("getCart")]
        public async Task<IActionResult> Get(int shoppingCartId)
        {
            try
            {
                var shoppingCart = await _context.TShoppingCarts.Include(s => s.TShoppingItems)
                    .FirstOrDefaultAsync(s => s.Id == shoppingCartId).ConfigureAwait(false);

                return Ok(shoppingCart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/<CartController>
        [HttpPost("addItemToCart")]
        public async Task<IActionResult> Post([FromBody] ICollection<TShoppingItem> items)
        {
            try
            {
                TShoppingCart shoppingCart = new TShoppingCart();
                shoppingCart.CreatedDate = DateTime.Now;

                foreach (TShoppingItem item in items)
                {
                    TProductItem prodItem = await _context.TProductItems.FirstOrDefaultAsync(i => i.ProductId == item.ProductId).ConfigureAwait(false);
                    if (prodItem != null)
                    {
                        if (prodItem.QuantityRemain <= item.Quantity)
                        {
                            item.Quantity = prodItem.QuantityRemain;
                        }

                        item.Total = item.Price * item.Quantity;
                        item.ShoppingCart = shoppingCart;

                        _context.Add(item);
                    }
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return Ok(shoppingCart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("updateItemCart")]
        public async Task<IActionResult> Put(int cartId,[FromBody] ICollection<TShoppingItem> items)
        {
            try
            {
                TShoppingItem updateItem = null;
                TProductItem prodItem = null;
                foreach (TShoppingItem item in items)
                {
                    updateItem = await _context.TShoppingItems.FirstOrDefaultAsync(i => i.ProductId == item.ProductId && i.ShoppingCartId == cartId).ConfigureAwait(false);
                    prodItem = await _context.TProductItems.FirstOrDefaultAsync(i => i.ProductId == item.ProductId).ConfigureAwait(false);

                    if (updateItem == null)
                    {
                        if (prodItem != null)
                        {
                            TShoppingItem newItem = new TShoppingItem();
                            if (prodItem.QuantityRemain <= item.Quantity)
                            {
                                newItem.Quantity = prodItem.QuantityRemain;
                            }
                            else
                            {
                                newItem.Quantity = item.Quantity;
                            }

                            newItem.Price = item.Price;
                            newItem.Total = item.Price * item.Quantity;
                            newItem.ShoppingCartId = cartId;
                            newItem.ProductId = item.ProductId;


                            _context.Entry(prodItem).State = EntityState.Detached;
                            _context.Add(newItem);
                        }
                    }
                    else
                    {
                        if (prodItem != null)
                        {
                            item.Id = updateItem.Id;
                            item.ShoppingCartId = updateItem.ShoppingCartId;
                            if (prodItem.QuantityRemain <= item.Quantity)
                            {
                                item.Quantity = prodItem.QuantityRemain;
                            }

                            item.Total = item.Price * item.Quantity;

                            _context.Entry(updateItem).State = EntityState.Detached;
                            _context.Entry(prodItem).State = EntityState.Detached;
                            _context.Update(item);
                        }
                        else
                        {
                            _context.Entry(updateItem).State = EntityState.Detached;
                            _context.Entry(prodItem).State = EntityState.Detached;
                            _context.Remove(item);
                        }
                    }
                }
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpDelete("deleteItem")]
        public async Task<IActionResult> Delete(int cartId, int productId)
        {
            try
            {
                TShoppingItem removeItem = new TShoppingItem();
                removeItem = await _context.TShoppingItems.
                    FirstOrDefaultAsync(i => i.ProductId == productId && i.ShoppingCartId == cartId).ConfigureAwait(false);
                _context.Remove(removeItem);
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
