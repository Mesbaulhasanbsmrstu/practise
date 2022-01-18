using practise.DTO;
using practise.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace practise.IRepository
{
    public interface IProduct
    {
        Task<string> addProduct(Products product);
        Task<PersonDetails> getProducts(int personId);
        Task<string> editProduct(Products product);
    }
}
