using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpPost("postCategory")]
        public IActionResult AddCategory(string name, string description)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var category = new Category()
                    {
                        Name = name,
                        Description = description
                    };
                    context.Categories.Add(category);
                    context.SaveChanges();

                    return Ok();
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("deleteCategory")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (context.Categories.Any(x => x.Id.Equals(id)))
                    {
                        var category = new Category()
                        {
                            Id = id
                        };
                        context.Categories.Remove(category);
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

        [HttpGet("getCategories")]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var categories = context.Categories.Select(x => new Category
                    {
                        Id = x.Id,
                        Description = x.Description
                    }).ToList();
                    return categories;
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
