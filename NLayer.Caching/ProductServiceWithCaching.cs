using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Service.Service;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.Repositories;
using NLayer.Core.UnitOfWorks;

namespace NLayer.Caching
{
    public class ProductServiceWithCaching : IProductService
    {
        private const string CacheProductKey = "productsCache";
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;


        public ProductServiceWithCaching(IMapper mapper, IMemoryCache memoryCache, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _memoryCache = memoryCache;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;

            if (!_memoryCache.TryGetValue(CacheProductKey, out _))
            {
                _memoryCache.Set(CacheProductKey, _productRepository.GetAll().ToList());
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> AddAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(Product entity)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveRange(IEnumerable<Product> entities)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            throw new NotImplementedException();
        }
    }
}
