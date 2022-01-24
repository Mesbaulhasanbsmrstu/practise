using Microsoft.AspNetCore.Mvc;
using practise.DTO;
using practise.IRepository;
using practise.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace practise.Repository
{
    public class OrdersRepository:IOrder
    {
        private  practiseContext _db;
        public OrdersRepository(practiseContext db)
        {
            _db = db;
        }
        public async Task<string> addOrder(OrderDto obj)
        {

            obj.Orderdate= DateTime.Now;
           Orders order = new Orders
            {
             ProductId = obj.ProductId,
             OrderPrice=obj.OrderPrice,
             Orderdate=obj.Orderdate,
             PaymentMethod=obj.PaymentMethod,
             TransactionId=obj.TransactionId

            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return  "success";
        }

        public async Task<IQueryable<OrderDto>> getOrderDetails(DateTime date)
        {
            IQueryable<OrderDto> data = await Task.FromResult(from o in _db.Orders
                                             where o.Orderdate.Date == date.Date
                                             join p in _db.Products on o.ProductId equals p.ProductId
                                             select new OrderDto
                                             {
                                                 OrderId=o.OredrId,
                                                 Orderdate=o.Orderdate,
                                                 PaymentMethod=o.PaymentMethod,
                                                 ProductId=p.ProductId,
                                                 ProductName=p.ProductName,
                                                 OrderPrice=o.OrderPrice,
                                                 ProductType = p.ProductType,
                                                 TransactionId=o.TransactionId
                                                 
                                             }
                                             );
            return data;

        }
    }
}
