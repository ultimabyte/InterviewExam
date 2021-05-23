using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    public class ProductsController : ControllerBase
    {
        private readonly InterviewExamContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ProductsController(InterviewExamContext context, ILogger<ProductsController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("getProductList")]
        public async Task<IActionResult> Get(int id, int categoryId, string name, string detail, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                var product = await _context.TProducts
                                    .Where(p => (p.IsActive == true) &&(id == default(int) ? p.Id != default(int) : p.Id == id) && (categoryId == default(int) ? p.ProductCategoryId != default(int) : p.ProductCategoryId == categoryId))
                                    .Include(p => p.TProductInfos)
                                    .Where(p => p.TProductInfos.Any(pi => (name == null ? pi.Title.Contains("") : pi.Title.Contains(name) && (detail == null ? pi.Detail.Contains("") : EF.Functions.Like(pi.Detail, "%" + detail + "%")))))
                                    .Include(p => p.TProductItems)
                                    .Include(p => p.ProductCategory)
                                    .ToListAsync().ConfigureAwait(false);

                return Ok(_mapper.Map<List<TProductDTO>>(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> Post([FromBody] TProductDTO product)
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

                TProductInfo newProductInfo = new TProductInfo()
                {
                    Detail = product.Detail,
                    EffectDate = product.EffectDate,
                    Image1400x400 = product.Image1400x400,
                    Image2400x400 = product.Image2400x400,
                    Image3400x400 = product.Image3400x400,
                    Image4400x400 = product.Image4400x400,
                    Title = product.Title,
                    Product = newProduct
                };

                TProductItem newProductItem = new TProductItem()
                {
                    Barcode = product.Barcode,
                    DateIn = product.DateIn,
                    Quantity = product.Quantity,
                    QuantityMaximum = product.QuantityMaximum,
                    QuantityMinimum = product.QuantityMinimum,
                    QuantityRemain = product.QuantityRemain,
                    Price = product.Price,
                    CreatedBy = "admin",
                    CreatedDate = DateTime.Now,
                    UpdatedBy = "admin",
                    UpdatedDate = DateTime.Now,
                    Product = newProduct
                };

                _context.AddRange(newProductItem, newProductInfo);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return Ok(_mapper.Map<TProductDTO>(newProduct));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPut("updateProduct")]
        public async Task<IActionResult> Put(int productId, [FromBody] TProductDTO product)
        {
            try
            {
                TProduct pr = _context.TProducts.Where(p => p.Id == productId).Include(p => p.TProductInfos).Include(p => p.TProductItems).FirstOrDefault();
                TProduct updateProduct = new TProduct()
                {
                    Id = productId,
                    ProductCategoryId = product.ProductCategoryId,
                    Name = product.Name,
                    Sku = product.Sku,
                    CreatedBy = pr.CreatedBy,
                    CreatedDate = pr.CreatedDate,
                    IsActive = product.IsActive,
                    UpdatedBy = "admin",
                    UpdatedDate = DateTime.Now
                };

                TProductInfo updateProductInfo = new TProductInfo()
                {
                    Id = pr.TProductInfos.First().Id,
                    Detail = product.Detail,
                    EffectDate = product.EffectDate,
                    Image1400x400 = product.Image1400x400,
                    Image2400x400 = product.Image2400x400,
                    Image3400x400 = product.Image3400x400,
                    Image4400x400 = product.Image4400x400,
                    Title = product.Title,
                    Product = updateProduct
                };

                TProductItem updateProductItem = new TProductItem()
                {
                    Id = pr.TProductItems.First().Id,
                    Barcode = product.Barcode,
                    DateIn = product.DateIn,
                    Quantity = product.Quantity,
                    QuantityMaximum = product.QuantityMaximum,
                    QuantityMinimum = product.QuantityMinimum,
                    QuantityRemain = product.QuantityRemain,
                    Price = product.Price,
                    CreatedBy = pr.CreatedBy,
                    CreatedDate = pr.CreatedDate,
                    UpdatedBy = "admin",
                    UpdatedDate = DateTime.Now,
                    Product = updateProduct
                };

                _context.Entry(pr).State = EntityState.Detached;
                _context.UpdateRange(updateProductItem, updateProductInfo);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return Ok(_mapper.Map<TProductDTO>(updateProduct));
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
