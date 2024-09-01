using Market.Models.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace Market.Abstrctions
{
    public interface IProductGroupRepository
    {
        public IEnumerable<ProductGroupDTO> GetProductGroups();
        public int AddProductGroup(ProductGroupDTO productGroupDTO);
        public void DeleteProductGroup(int id);
        public MemoryCacheStatistics GetCacheStats();
        public string UpLoadToCsv(IEnumerable<ProductGroupDTO> productGroupDTOs);
    }
}
