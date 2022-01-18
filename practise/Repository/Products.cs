using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practise.DTO;
using practise.IRepository;
using practise.Model;

namespace practise.Repository
{
    public class Products : IProduct
    {
        private readonly practiseContext _db;
        private readonly IMapper _mapper;
        public Products(practiseContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<string> addProduct(Model.Products product)
        {
            string msg;
            try
            {
                var user = _db.Persons.Where(b => b.PersonId == product.PersonId).FirstOrDefault();
                if (user != null) {
                    _db.Products.Add(product);
                    var result = await _db.SaveChangesAsync();
                    // msg = SP_Persons.addPerson(record);
                    msg = "success";
                }
                else
                {
                    msg = "User Not Allow!!!";
                }

                return msg;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

           
        }
        public async Task<PersonDetails> getProducts(int personId)
        {
            
            var productsList=await _db.Products.Where(b => b.PersonId == personId).ToListAsync();
            var person=await _db.Persons.FirstOrDefaultAsync(c=>c.PersonId==personId);

            PersonDetails personDetails = new PersonDetails
            {
                LastName=person.LastName,
                FirstName=person.FirstName,
                Address=person.Address,
                City=person.City,
                Image=person.Image,
                products= _mapper.Map<List<ProductDetails>>(productsList)

            };
          return personDetails;
        }

        public async Task<string> editProduct(Model.Products product)
        {
            //return "success";
            _db.Products.Update(product);
             await _db.SaveChangesAsync();
            return "success";
        }
    }
}
