using AutoMapper;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Repositories;
using NLayer.Core.UnitOfWorks;

namespace NLayer.Service.Service
{
    public class ProductService : Service<Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;



        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IProductRepository productRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductWithCategoryDto>> GetProductsWithCategory()
        {
            var products = await _productRepository.GetProductsWithCategoryAsync();
            var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
            return productsDto;

        }
    }
}
