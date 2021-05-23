using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewExamWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AutoMapper;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InterviewExamWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly InterviewExamContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CartController(InterviewExamContext context, ILogger<CartController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // GET api/<CartController>/5
        [HttpGet("getCart")]
        public async Task<IActionResult> Get(int shoppingCartId)
        {
            try
            {
                var shoppingCart = await _context.TShoppingCarts.Include(s => s.TShoppingItems)
                    .FirstOrDefaultAsync(s => s.Id == shoppingCartId).ConfigureAwait(false);

                return Ok(_mapper.Map<TShoppingCartDTO>(shoppingCart));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/<CartController>
        [HttpPost("newCartWithItem")]
        public async Task<IActionResult> Post([FromBody] ICollection<TShoppingItemDTO> items)
        {
            try
            {
                TShoppingCart shoppingCart = new TShoppingCart();
                shoppingCart.CreatedDate = DateTime.Now;

                foreach (TShoppingItemDTO item in items)
                {
                    TProductItem prodItem = await _context.TProductItems.FirstOrDefaultAsync(i => i.ProductId == item.ProductId).ConfigureAwait(false);
                    
                    if (prodItem != null)
                    {
                        TShoppingItem newItem = _mapper.Map<TShoppingItem>(item);
                        if (prodItem.QuantityRemain <= newItem.Quantity)
                        {
                            newItem.Quantity = prodItem.QuantityRemain;
                        }

                        newItem.Total = newItem.Price * newItem.Quantity;
                        newItem.ShoppingCart = shoppingCart;

                        _context.Add(newItem);
                    }
                }

                await _context.SaveChangesAsync().ConfigureAwait(false);

                return Ok(_mapper.Map<TShoppingCartDTO>(shoppingCart));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("addUpdateItemExistingCart")]
        public async Task<IActionResult> Put(int cartId,[FromBody] ICollection<TShoppingItemDTO> items)
        {
            try
            {
                TShoppingItem findItem = null;
                TProductItem prodItem = null;
                foreach (TShoppingItemDTO item in items)
                {
                    findItem = await _context.TShoppingItems.FirstOrDefaultAsync(i => i.ProductId == item.ProductId && i.ShoppingCartId == cartId).ConfigureAwait(false);
                    prodItem = await _context.TProductItems.FirstOrDefaultAsync(i => i.ProductId == item.ProductId).ConfigureAwait(false);

                    if (findItem == null)
                    {
                        if (prodItem != null)
                        {
                            TShoppingItem newItem = _mapper.Map<TShoppingItem>(item);
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
                            TShoppingItem updateItem = _mapper.Map<TShoppingItem>(item);
                            updateItem.ShoppingCartId = cartId;
                            updateItem.Id = findItem.Id;
                            if (prodItem.QuantityRemain <= updateItem.Quantity)
                            {
                                updateItem.Quantity = prodItem.QuantityRemain;
                            }

                            updateItem.Total = updateItem.Price * updateItem.Quantity;

                            _context.Entry(findItem).State = EntityState.Detached;
                            _context.Entry(prodItem).State = EntityState.Detached;
                            _context.Update(updateItem);
                        }
                        else
                        {
                            _context.Entry(findItem).State = EntityState.Detached;
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
