using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpPost("postProducts")]
        public IActionResult PutProducts([FromQuery] string name, string description, int categoryId, double cost)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (!context.Products.Any(x => x.Name.ToLower().Equals(name)))
                    {
                        Product product = new Product()
                        {
                            Name = name,
                            Description = description,
                            CategoryId = categoryId,
                            Cost = cost
                        };
                        context.Add(product);
                        context.SaveChanges();

                        return Ok(product.Id);
                    }
                    else
                    {
                        return StatusCode(409);
                    }
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("deleteProduct")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (context.Products.Any(x => x.Id.Equals(id)))
                    {
                        var product = new Product()
                        {
                            Id = id
                        };
                        context.Products.Remove(product);
                        context.SaveChanges();
                    }
                    return Ok();
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("getProducts")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var products = context.Products.Select(x => new Product()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description
                    }).ToList();
                    return products;
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPatch("editCost")]
        public IActionResult EditCost(int id, double cost)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (context.Products.Any(x => x.Id.Equals(id)))
                    {
                        var product = new Product()
                        {
                            Id = id,
                            Cost = cost
                        };
                        context.Products.Update(product);
                        context.SaveChanges();
                    }
                    return Ok();
                }
            }
            catch
            {
                return StatusCode(404);
            }
        }
    }
}
