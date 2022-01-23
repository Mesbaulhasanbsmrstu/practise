using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using practise.DTO;
using practise.Helper;
using practise.IRepository;
using practise.Model;
using practise.Store_Procedures;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace practise.Controllers
{
    //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
   
    public class PersonController : ControllerBase
    {
        private readonly IPerson _person;
        private readonly practiseContext _db;
        public PersonController(IPerson person,practiseContext db)
        {
            _person= person;
            _db = db;
        }
        [HttpGet]
        [Route("person/{page}/{limit}")]
        public async Task<IActionResult> getPersons([FromRoute]int page, [FromRoute] int limit)
        {
            if (page <= 0)
                page++;

            PaginationDTO pagination=new PaginationDTO(page,limit);
           // return Ok(await _person.getPersons(pagination));
            int count=_db.Persons.Count();
            
            var dt = SP_Persons.getAllPersons((page - 1) * limit, pagination.recordsPerPage);
            // var items= JsonConvert.SerializeObject(SP_Persons.getAllPersons((page-1)*limit,limit));
            List<Persons> items = new List<Persons>();
            items = (from DataRow dr in dt.Rows
                           select new Persons()
                           {
                               LastName=dr["LastName"].ToString(),
                               FirstName=dr["FirstName"].ToString(),
                               Address = dr["Address"].ToString(),
                               City=dr["City"].ToString(),
                               Image=dr["Image"].ToString(),
                               PersonId = Convert.ToInt32(dr["PersonId"])
                           
                           }).ToList();
            return Ok( new PaginatedPersons(items, count, page, pagination.recordsPerPage));

            /* var dt = SP_Persons.getAllPersons();
             var result = JsonConvert.SerializeObject(dt);
             return Ok(result);*/

        }

        [HttpPost]
        public async Task<IActionResult> addPerson([FromForm]DTO.Persondto person)
        {
            //  return Ok(await _person.addPerson(person));\
            try {
                return Ok(await _person.addPerson(person));
            }catch(Exception EX)
            {
                return Ok(EX);
            }
           
        }

        [HttpPut]
        public async Task<IActionResult> editPerson([FromForm] UpdatePersonDto personDto)
        {

            Persons record;
            try
            {
                var user = _db.Persons.FirstOrDefault(b => b.PersonId == personDto.PersonId);
                if (user != null)
                {
                    if (personDto.LastName == null)
                        personDto.LastName = user.LastName;
                    if (personDto.FirstName == null)
                        personDto.FirstName = user.FirstName;
                    if (personDto.Address == null)
                        personDto.Address = user.Address;
                    if (personDto.City == null)
                        personDto.City = user.City;

                    return Ok(await _person.editPerson(personDto,user));



                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                return Ok( ex.Message);
            }

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> deletePerson([FromRoute]int? id)
        {
            if(id==null)
            {
                return BadRequest();
            }
            var user=_db.Persons.FirstOrDefault(c=>c.PersonId==id);
            if(user==null)
            {
                return Unauthorized();
            }
            _db.Persons.Remove(user);
            await _db.SaveChangesAsync();
            return Ok("Successfully Deleted");
        }
    }
}
