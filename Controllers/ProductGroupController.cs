using Market.Abstrctions;
using Market.Models.DTO;
using Market.Repo;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductGroupController:ControllerBase
    {
        private readonly IProductGroupRepository _productGroupRepository;
        public ProductGroupController(IProductGroupRepository productGroupRepository)
        {
            _productGroupRepository = productGroupRepository;
        }

        [HttpGet("get_product_groups")]
        public ActionResult GetProductGroups()
        {
            var productGroupsList = _productGroupRepository.GetProductGroups();
            return Ok(productGroupsList);
        }

        [HttpPost("add_product")]
        public IActionResult AddProductGroup([FromBody] ProductGroupDTO productGroupDTO)
        {
            var result = _productGroupRepository.AddProductGroup(productGroupDTO);
            return Ok(result);
        }

        [HttpDelete("delete_product_group")]
        public IActionResult DeleteProductGroups(int id)
        {
            _productGroupRepository.DeleteProductGroup(id);
            return Ok();
        }

        [HttpGet("get_cache_stats")]
        public ActionResult GetCacheStats()
        {
            return Ok(_productGroupRepository.GetCacheStats());
        }
        
    }
}
