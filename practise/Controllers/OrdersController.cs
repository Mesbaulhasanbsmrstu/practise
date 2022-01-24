using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practise.DTO;
using practise.IRepository;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace practise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrder _order;
        public OrdersController(IOrder order)
        {
            _order = order;
        }
        [HttpPost]
        public async Task<IActionResult> addOrder([FromBody] OrderDto obj)
        {
            try
            {
                await _order.addOrder(obj);
                return Ok("Success");

            }catch(Exception ex)
            {
                //return Ok(ex.Message);
                throw new Exception(ex.Message);
            }

        }

        [HttpGet]
        [Route("{date}")]
        public async Task<IActionResult> get([FromRoute]string date)
        {

          

            try
            {
                Regex r = new Regex(@"\d{4}-\d{2}-\d{2}");
                if (r.IsMatch(date))
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    DateTime tempDate = Convert.ToDateTime(date, culture);
                    return Ok(await _order.getOrderDetails(tempDate));
                    //return Ok(tempDate);
                }
                else
                {
                    throw new Exception("InValid date format");
                }

                //return Ok(await _order.getOrderDetails(date));

            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
