namespace NLayer.Core.DTOs
{
    public class ProductWithCategoryDto:ProductDto
    {
        public CategoryDto category  { get; set; }
    }
}
