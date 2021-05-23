using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterviewExamWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InterviewExamWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly InterviewExamContext _context;
        private readonly ILogger _logger;

        public ProductsController(InterviewExamContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("getProductList")]
        public async Task<IActionResult> Get(int id, int categoryId, string name, string detail, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                var product = await _context.TProducts
                                    .Where(p => (id == default(int) ? p.Id != default(int) : p.Id == id) && (categoryId == default(int) ? p.ProductCategoryId != default(int) : p.ProductCategoryId == categoryId))
                                    .Include(p => p.TProductInfos).Where(p => p.TProductInfos.Any(pi => (name == null ? pi.Title.Contains("") : pi.Title.Contains(name) && (detail == null ? pi.Detail.Contains("") : EF.Functions.Like(pi.Detail, "%" + detail + "%")))))
                                    .Include(p => p.TProductItems).ToListAsync().ConfigureAwait(false);

                
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> Post([FromBody] TProduct product)
        {
            try
            {
                TProduct newProduct = new TProduct()
                {
                    ProductCategoryId = product.ProductCategoryId,
                    Name = product.Name,
                    Sku = product.Sku,
                    IsActive = product.IsActive,
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = "admin",
                    UpdatedDate = DateTime.Now,
                };

                TProductInfo tempPInfo = product.TProductInfos.First();
                TProductInfo newProductInfo = new TProductInfo()
                {
                    Detail = tempPInfo.Detail,
                    EffectDate = tempPInfo.EffectDate,
                    Image1400x400 = tempPInfo.Image1400x400,
                    Image2400x400 = tempPInfo.Image2400x400,
                    Image3400x400 = tempPInfo.Image3400x400,
                    Image4400x400 = tempPInfo.Image4400x400,
                    Title = tempPInfo.Title,
                    Product = newProduct
                };

                TProductItem tempItem = product.TProductItems.First();
                TProductItem newProductItem = new TProductItem()
                {
                    Barcode = tempItem.Barcode,
                    DateIn = tempItem.DateIn,
                    Quantity = tempItem.Quantity,
                    QuantityMaximum = tempItem.QuantityMaximum,
                    QuantityMinimum = tempItem.QuantityMinimum,
                    QuantityRemain = tempItem.QuantityRemain,
                    Price = tempItem.Price,
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = "admin",
                    UpdatedDate = DateTime.Now,
                    Product = newProduct
                };

                _context.AddRange(newProductItem, newProductInfo);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return Ok(newProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPut("updateProduct")]
        public async Task<IActionResult> Put(int productId, [FromBody] TProduct product)
        {
            try
            {
                TProduct updateProduct = new TProduct()
                {
                    Id = productId,
                    ProductCategoryId = product.ProductCategoryId,
                    Name = product.Name,
                    Sku = product.Sku,
                    CreatedBy = product.CreatedBy,
                    CreatedDate = product.CreatedDate,
                    IsActive = product.IsActive,
                    UpdatedBy = "admin",
                    UpdatedDate = DateTime.Now
                };

                TProductInfo tempPInfo = product.TProductInfos.First();
                TProductInfo updateProductInfo = new TProductInfo()
                {
                    Id = tempPInfo.Id,
                    Detail = tempPInfo.Detail,
                    EffectDate = tempPInfo.EffectDate,
                    Image1400x400 = tempPInfo.Image1400x400,
                    Image2400x400 = tempPInfo.Image2400x400,
                    Image3400x400 = tempPInfo.Image3400x400,
                    Image4400x400 = tempPInfo.Image4400x400,
                    Title = tempPInfo.Title,
                    Product = updateProduct
                };

                TProductItem tempItem = product.TProductItems.First();
                TProductItem updateProductItem = new TProductItem()
                {
                    Id = tempItem.Id,
                    Barcode = tempItem.Barcode,
                    DateIn = tempItem.DateIn,
                    Quantity = tempItem.Quantity,
                    QuantityMaximum = tempItem.QuantityMaximum,
                    QuantityMinimum = tempItem.QuantityMinimum,
                    QuantityRemain = tempItem.QuantityRemain,
                    Price = tempItem.Price,
                    CreatedBy = tempItem.CreatedBy,
                    CreatedDate = tempItem.CreatedDate,
                    UpdatedBy = "admin",
                    UpdatedDate = DateTime.Now,
                    Product = updateProduct
                };

                _context.UpdateRange(updateProductItem, updateProductInfo);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return Ok(updateProductInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("deleteProduct")]
        public async Task<IActionResult> Delete(int productId)
        {
            try
            {
                TProduct product = _context.TProducts.Where(p => p.Id == productId).Include(p => p.TProductInfos).Include(p => p.TProductItems).FirstOrDefault();
                _context.Remove(product);
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
