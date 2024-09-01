using Market.Models;
using Market.Models.DTO;
using Microsoft.Extensions.Caching.Memory;

namespace Market.Abstrctions
{
    public interface IProductRepository
    {
        public IEnumerable<ProductDTO> GetProducts();
        public int AddProduct(ProductDTO product);
        public void DeleteProduct(int id);
        public void EditPrice(int id, double newPrice);
        public MemoryCacheStatistics GetCacheStats();
        public string UpLoadToCsv();
        public string GetProductsCsvUrl();
        public string UploadCacheToCsvUrl();
    }
}
