using Market.Abstrctions;
using Market.Models;
using Market.Models.DTO;
using Market.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("get_products")]
        public IActionResult GetProduct()
        {
            var productsList = _productRepository.GetProducts();
            return Ok(productsList);
        }

        [HttpPost("add_product")]
        public IActionResult AddProduct([FromBody] ProductDTO productDTO)
        {
            var result = _productRepository.AddProduct(productDTO);
            return Ok(result);
        }

        [HttpDelete("delete_product")]
        public IActionResult DeleteProduct(int id)
        {
            _productRepository.DeleteProduct(id);
            return Ok();
        }

        [HttpPatch("edit_price")]
        public IActionResult EditCost(int id, double newPrice)
        {
            _productRepository.EditPrice(id, newPrice);
            return Ok();
        }

        [HttpGet("get_cash_stats")]
        public IActionResult GetCacheStats()
        {
            return Ok(_productRepository.GetCacheStats());
        }

        [HttpGet("upload_to_csv")]
        public FileContentResult ToFile()
        {
            var content = _productRepository.UpLoadToCsv();
            return File(new UTF8Encoding().GetBytes(content), "text/csv", "products.csv");
        }

        [HttpGet("get_products_csv_url")]
        public ActionResult<string> GetProductsCsvUrl()
        {
            var fileName = _productRepository.GetProductsCsvUrl();
            return "https://" + Request.Host.ToString() + "/static/" + fileName;
        }

        [HttpGet("upload_cache_to_csv")]
        public ActionResult<string> UploadCacheToCsvUrl()
        {
            var fileName = _productRepository.UploadCacheToCsvUrl();
            return "https://" + Request.Host.ToString() + "/static/" + fileName;
        }
    }
}
