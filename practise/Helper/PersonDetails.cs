using AutoMapper;
using practise.DTO;
using practise.Model;
using System.Collections.Generic;

namespace practise.Helper
{
    public class PersonDetails:Profile
    {
        public PersonDetails()
        {
           CreateMap<Products, ProductDetails>();
        }
   
    }
}
