using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Service.Service;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NLayer.Core.Repositories;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Exceptions;

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
                _memoryCache.Set(CacheProductKey, _productRepository.GetProductsWithCategoryAsync().Result);
            }
        }

        public Task<Product> GetByIdAsync(int id)
        {
            var product = _memoryCache.Get<List<Product>>(CacheProductKey)
                .FirstOrDefault(x => x.Id == id);
            if (product is null)
            {
                throw new NotFoundException($"{typeof(Product).Name}({id}) not found");
            }

            return Task.FromResult(product);
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult(_memoryCache.Get<IEnumerable<Product>>(CacheProductKey));
        }

        public Task<CustomResponseDto<List<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            var products = _memoryCache.Get<IEnumerable<Product>>(CacheProductKey);
            var productsWithCategoryDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return Task.FromResult(CustomResponseDto<List<ProductWithCategoryDto>>.Success(200, productsWithCategoryDto));
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> expression)
        {
            return _memoryCache.Get<List<Product>>(CacheProductKey).Where(expression.Compile()).AsQueryable();
        }

        public async Task<bool> AnyAsync(Expression<Func<Product, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> AddAsync(Product entity)
        {
            await _productRepository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
            return entity;
        }

        public async Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> entities)
        {
            await _productRepository.AddRangeAsync(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();
            return entities;
        }

        public async Task UpdateAsync(Product entity)
        {
            _productRepository.Update(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();

        }

        public async Task RemoveAsync(Product entity)
        {
            _productRepository.Remove(entity);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();

        }

        public async Task RemoveRange(IEnumerable<Product> entities)
        {
            _productRepository.RemoveRange(entities);
            await _unitOfWork.CommitAsync();
            await CacheAllProducts();

        }



        public async Task CacheAllProducts()
        {
            _memoryCache.Set(CacheProductKey, await _productRepository.GetAll().ToListAsync());
        }
    }
}
