using Demo_ChinMunWah.Domain;
using Demo_ChinMunWah.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo_ChinMunWah.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCustomer(UpsertCustomerModel model)
        {
            var isUpdate = model.CustomerGuid.HasValue;
            var isEmailExist = await _context.Customers
                .AnyAsync(e => e.Email == model.Email && (!isUpdate || e.CustomerGuid != model.CustomerGuid));

            if (isEmailExist)
            {
                return NotFound("Email is duplicated.");
            }

            if (!isUpdate)
            {
                Customer newCustomer = new Customer
                {
                    Name = model.Name,
                    Email = model.Email,
                    CustomerGuid = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow
                };

                await _context.Customers.AddAsync(newCustomer);
            }
            else
            {
                var entityCustomer = await _context.Customers
                    .FirstOrDefaultAsync(e => e.CustomerGuid == model.CustomerGuid);

                if (entityCustomer == null)
                {
                    return NotFound("Customer not found");
                }

                entityCustomer.UpdatedDate = DateTime.UtcNow;
                entityCustomer.Name = model.Name;
                entityCustomer.Email = model.Email;

                _context.Customers.Update(entityCustomer);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerDetails(string id)
        {
            if (!Guid.TryParse(id, out var customerGuid))
            {
                return NotFound("Customer guid incorrect");
            }

            var entityCustomer = await _context.Customers
                    .FirstOrDefaultAsync(e => e.CustomerGuid == customerGuid);

            if (entityCustomer == null)
            {
                return NotFound("Customer not found");
            }

            return Ok(entityCustomer);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            if (!Guid.TryParse(id, out var customerGuid))
            {
                return NotFound("Customer guid incorrect");
            }

            var entityCustomer = await _context.Customers
                    .FirstOrDefaultAsync(e => e.CustomerGuid == customerGuid);

            if (entityCustomer == null)
            {
                return NotFound("Customer not found");
            }

            entityCustomer.IsDeleted = true;
            entityCustomer.UpdatedDate = DateTime.UtcNow;
            _context.Customers.Update(entityCustomer);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
