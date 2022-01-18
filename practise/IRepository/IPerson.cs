using practise.DTO;
using practise.Helper;
using practise.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace practise.IRepository
{
    public interface IPerson
    {
        Task<PaginatedPersons> getPersons(PaginationDTO pagination);
        Task<string> addPerson(Persondto persons);
        Task<string> editPerson(UpdatePersonDto personDto,Persons person);
    }
}
