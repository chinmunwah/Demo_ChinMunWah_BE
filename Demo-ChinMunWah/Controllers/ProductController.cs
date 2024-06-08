using AutoMapper;
using Demo_ChinMunWah.Domain;
using Demo_ChinMunWah.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace Demo_ChinMunWah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> UpsertProduct(UpsertProductModel model)
        {
            var isUpdate = model.ProductGuid.HasValue;

            if (!isUpdate)
            {
                var newProduct = _mapper.Map<Product>(model);
                newProduct.ProductGuid = Guid.NewGuid();
                newProduct.CreatedDate = DateTime.UtcNow;

                await _context.Products.AddAsync(newProduct);
                model.ProductGuid = newProduct.ProductGuid;
            }
            else
            {
                var entityProduct = await _context.Products
                    .FirstOrDefaultAsync(e => e.ProductGuid == model.ProductGuid);

                if (entityProduct == null)
                {
                    return NotFound("Product not found");
                }

                entityProduct.UpdatedDate = DateTime.UtcNow;
                entityProduct.Name = model.Name;
                entityProduct.Description = model.Description;
                entityProduct.Stock = model.Stock;
                entityProduct.Price = model.Price;

                _context.Products.Update(entityProduct);
            }
            await _context.SaveChangesAsync();
            return Ok(model.ProductGuid.ToString());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductDetails(string id)
        {
            if (!Guid.TryParse(id, out var productGuid))
            {
                return NotFound("Product guid incorrect");
            }

            var entityProduct = await _context.Products
                    .FirstOrDefaultAsync(e => e.ProductGuid == productGuid);

            if (entityProduct == null)
            {
                return NotFound("Product not found");
            }

            return Ok(entityProduct);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (!Guid.TryParse(id, out var productGuid))
            {
                return NotFound("Product guid incorrect");
            }

            var entityProduct = await _context.Products
                    .FirstOrDefaultAsync(e => e.ProductGuid == productGuid);

            if (entityProduct == null)
            {
                return NotFound("Product not found");
            }

            entityProduct.IsDeleted = true;
            entityProduct.UpdatedDate = DateTime.UtcNow;
            _context.Products.Update(entityProduct);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
