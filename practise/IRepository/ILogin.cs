using practise.Model;
using System.Threading.Tasks;

namespace practise.IRepository
{
    public interface ILogin
    {
        Task<Persons> getVarify(string username);
    }
}
