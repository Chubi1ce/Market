using AutoMapper;
using Azure.Core;
using Market.Abstrctions;
using Market.Models;
using Market.Models.DTO;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System.Text;
using System.Text.Json;

namespace Market.Repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private MarketContext _context;
        public ProductRepository(IMapper mapper, IMemoryCache memoryCache, MarketContext context)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _context = context;
        }
        public IEnumerable<ProductDTO> GetProducts()
        {
            using (_context)
            {
                if (_memoryCache.TryGetValue("productsList",out List<ProductDTO> productsList))
                {
                    return productsList;
                }
                productsList = _context.Products.Select(x => _mapper.Map<ProductDTO>(x)).ToList();
                _memoryCache.Set("productsList", productsList, TimeSpan.FromMinutes(30));
                return productsList;
            }
        }
        public int AddProduct(ProductDTO productDTO)
        {
            using (_context)
            {
                var product = _context.Products.FirstOrDefault(x => x.Id == productDTO.Id);
                if (product == null)
                {
                    product = _mapper.Map<Product>(productDTO);
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    _memoryCache.Remove("productsList");
                }
                return product.Id;
            }
        }
        public void DeleteProduct(int id)
        {
            using (_context)
            {
                if (_context.Products.Any(x => x.Id.Equals(id)))
                {
                    var product = new Product() { Id = id };
                    _context.Products.Remove(product);
                    _context.SaveChanges();
                }
            }
        }
        public void EditPrice(int id, double newPrice)
        {
            using (_context)
            {
                if (_context.Products.Any(x => x.Id.Equals(id)))
                {
                    var product = _context.Products.Where(x => x.Id == id).FirstOrDefault();
                    product.Price = newPrice;
                    _context.SaveChanges();
                }
            }
        }
        public MemoryCacheStatistics GetCacheStats()
        {
            return _memoryCache.GetCurrentStatistics();
        }
        public string FormatToCsv(IEnumerable<ProductDTO> ProductDTOs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var product in ProductDTOs)
            {
                sb.AppendLine(product.Id + ";" + product.Name + ";" + product.Description);
            }
            return sb.ToString();
        }
        public string UpLoadToCsv()
        {
            using (_context)
            {
                var products = _context.Products.Select(x => new ProductDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();
                var content = FormatToCsv(products);
                return content;
            }
        }
        public string GetProductsCsvUrl()
        {
            var contetnt = "";
            contetnt = UpLoadToCsv();
            string fileName = null;
            fileName = "products_" + DateTime.Now.ToBinary().ToString() + ".csv";
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName), contetnt);
            return fileName;
        }

        public string UploadCacheToCsvUrl()
        {
            var stat = _memoryCache.GetCurrentStatistics();
            string fileName = String.Empty;
            string contetnt = String.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append($"CurrentEntryCount: {stat.CurrentEntryCount.ToString()};\n");
            var CurrentEstimatedSize = stat.CurrentEstimatedSize == null ? "null": stat.CurrentEstimatedSize.ToString();
            sb.Append($"CurrentEstimatedSize: {CurrentEstimatedSize};\n");
            sb.Append($"TotalMisses: {stat.TotalMisses.ToString()};\n");
            sb.Append($"TotalHits: {stat.TotalHits.ToString()};\n");
            contetnt = sb.ToString();

            fileName = "cacheStat" + DateTime.Now.ToBinary().ToString() + ".csv";
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName), contetnt);
            return fileName;
        }
    }
}
