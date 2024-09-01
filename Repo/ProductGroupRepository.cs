using AutoMapper;
using Market.Abstrctions;
using Market.Models;
using Market.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace Market.Repo
{
    public class ProductGroupRepository : IProductGroupRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private MarketContext _context;

        public ProductGroupRepository(IMapper mapper, IMemoryCache memoryCache, MarketContext context)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _context = context;
        }
        public IEnumerable<ProductGroupDTO> GetProductGroups()
        {
            if (_memoryCache.TryGetValue("productGroupsList", out List<ProductGroupDTO> productGroupsList))
            {
                return productGroupsList;
            }

            using (_context)
            {
                productGroupsList = _context.ProductGroups.Select(x => _mapper.Map<ProductGroupDTO>(x)).ToList();
                _memoryCache.Set("productGroupsList", productGroupsList);
                return productGroupsList;
            }
        }
        public int AddProductGroup(ProductGroupDTO productGroupDTO)
        {
            using (_context)
            {
                var group = _context.ProductGroups.FirstOrDefault(x => x.Id == productGroupDTO.Id);
                if (group == null)
                {
                    group = _mapper.Map<ProductGroup>(productGroupDTO);
                    _context.Add(group);
                    _context.SaveChanges();
                    _memoryCache.Remove("productGroupsList");
                }
                return group.Id;
            }
        }
        public void DeleteProductGroup(int id)
        {
            using (_context)
            {
                if (_context.ProductGroups.Any(x => x.Id.Equals(id)))
                {
                    var group = new ProductGroup() { Id = id };
                    _context.ProductGroups.Remove(group);
                    _context.SaveChanges();
                }
            }
        }
        public MemoryCacheStatistics GetCacheStats()
        {
            return _memoryCache.GetCurrentStatistics();
        }
        public string UpLoadToCsv(IEnumerable<ProductGroupDTO> productGroupDTOs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var group in productGroupDTOs)
            {
                sb.AppendLine(group.Id + ";" + group.Name + ";" + group.Description);
            }
            return sb.ToString();
        }
    }
}
