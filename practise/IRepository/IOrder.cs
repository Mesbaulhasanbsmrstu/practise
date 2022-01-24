using practise.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace practise.IRepository
{
    public interface IOrder
    {
        Task<string> addOrder(OrderDto obj);
        Task<IQueryable<OrderDto>> getOrderDetails(DateTime date);
    }
}
