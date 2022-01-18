using Microsoft.EntityFrameworkCore;
using practise.IRepository;
using practise.Model;
using System.Linq;
using System.Threading.Tasks;

namespace practise.Repository
{
    public class Login:ILogin
    {
        private readonly practiseContext _db;
        public Login(practiseContext db)
        {
            _db=db;
        }
        public async Task<Persons> getVarify(string username)
        {
            //var record = await _context.books.Where(c => c.Id == id).FirstOrDefaultAsync();
            var user=await _db.Persons.Where(b =>b.FirstName==username).FirstOrDefaultAsync();
            if(user==null)
            {
                return null;
            }
            return user;
        }
    }
}
