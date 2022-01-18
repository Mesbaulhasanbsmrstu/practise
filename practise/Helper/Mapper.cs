using practise.DTO;
using practise.Model;
using AutoMapper;
namespace practise.Helper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Products, ProductDetails>();
            CreateMap<ProductDto, Products>();
        }

      
    }
}
