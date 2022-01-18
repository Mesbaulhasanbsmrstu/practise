using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practise.DTO;
using practise.IRepository;
using practise.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace practise.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProduct _product;
        private readonly practiseContext _db;
        private readonly IMapper _mapper;
        public ProductsController(IProduct product,practiseContext db,IMapper mapper )
        {
            _product = product;
            _db = db;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> addProduct([FromBody] Products product)
        {
            var message = await _product.addProduct(product);
            return Ok(message);

        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> getProducts(int id)
        {
            var user = _db.Persons.Where(b => b.PersonId == id).FirstOrDefault();
            if (user != null)
            {
                var products = await _product.getProducts(id);
                return Ok(products);
            }
            else
            {
                return Ok("User Not Allow!!!");
            }
              

        }

        [HttpPut]
        public async Task<IActionResult> editProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var user = _db.Persons.Where(b => b.PersonId == productDto.PersonId).FirstOrDefault();
                if (user != null)
                {
                    var record = _db.Products.FirstOrDefault(p => p.ProductId == productDto.ProductId);
                    if (record != null)
                    {
                        if (productDto.ProductName == null)
                            productDto.ProductName = record.ProductName;
                        if (productDto.ProductType == null)
                            productDto.ProductType = record.ProductType;
                        if (productDto.ProductPrice == null)
                            productDto.ProductPrice = (decimal)record.ProductPrice;

                        /*Products product = new Products
                        {
                            ProductId = productDto.ProductId,
                            ProductName = productDto.ProductName,
                            ProductType = productDto.ProductType,
                            ProductPrice = productDto.ProductPrice,
                            PersonId = productDto.PersonId
                        };*/
                        Products product = _mapper.Map<Products>(productDto);

                        var message= await _product.editProduct(product);
                        return Ok(message);

                    }
                    else
                    {
                        return Ok("Product Not Exist!!!");
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }catch(Exception ex)
            {
                return Ok(ex.ToString());
            }
           
            
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> deleteProduct([FromRoute] int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var product = _db.Products.FirstOrDefault(c => c.ProductId == id);
            if (product == null)
            {
                return Unauthorized();
            }
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return Ok("Successfully Deleted");
        }
    }}
