using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using practise.DTO;
using practise.Helper;
using practise.IRepository;
using practise.Model;
using practise.Store_Procedures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
namespace practise.Repository
{
    public class Person:IPerson
    {
        private readonly practiseContext _db;
        private readonly IHostingEnvironment _he;

        public Person(practiseContext db, IHostingEnvironment he)
        {
            _db = db;
            _he=he;
        }
        public async Task<PaginatedPersons>getPersons(PaginationDTO pagination)
        {


            var persons =  PaginatedPersons.ToPaginatedPost( _db.Persons.OrderByDescending(c=>c.PersonId),pagination.Page,pagination.recordsPerPage);
            return persons;
            //turn persons;
        }

        public async Task<string> addPerson(Persondto persondto)
        {
            var user = await _db.Persons.Where(b => b.FirstName == persondto.FirstName).FirstOrDefaultAsync();
            string msg;
            if (user == null) {
                Persons record;
                var name = "m";
                if (persondto.image!=null)
                {
                 
                     name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(persondto.image.FileName));
                    if(!File.Exists(name))
                    await persondto.image.CopyToAsync(new FileStream(name, FileMode.Create));
                   record = new Persons()
                    {
                      LastName= persondto.LastName,
                      FirstName= persondto.FirstName,
                      Address= persondto.Address,
                      City= persondto.City,

                      Image= "https://localhost:44312/Images/" + persondto.image.FileName
                };
                }
                else
                {
                    record = new Persons()
                    {
                        LastName = persondto.LastName,
                        FirstName = persondto.FirstName,
                        Address = persondto.Address,
                        City = persondto.City,
                        Image = ""
                    };
                }

                try
                {
                    _db.Persons.Add(record);
                    var result = await _db.SaveChangesAsync();
                    // msg = SP_Persons.addPerson(record);
                    msg = "success";
                    return msg;
                }catch(Exception ex)
                {
                    return ex.Message;
                }
            }
            return "Person having Username Already Exist";
 

           
        }

        public async Task<string> editPerson(UpdatePersonDto personDto,Persons user)
        {
            Persons record;
            try
            {
  

                    if (personDto.LastName == null)
                        personDto.LastName = user.LastName;
                    if (personDto.FirstName == null)
                        personDto.FirstName = user.FirstName;
                    if (personDto.Address == null)
                        personDto.Address = user.Address;
                    if (personDto.City == null)
                        personDto.City = user.City;
                    if (personDto.image != null)
                    {

                        var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(personDto.image.FileName));
                        if (!File.Exists(name))
                            await personDto.image.CopyToAsync(new FileStream(name, FileMode.Create));
                        record = new Persons()
                        {
                            LastName = personDto.LastName,
                            FirstName = personDto.FirstName,
                            Address = personDto.Address,
                            City = personDto.City,
                            PersonId = personDto.PersonId,

                            Image = name
                        };
                    }
                    else
                    {
                        record = new Persons()
                        {
                            LastName = personDto.LastName,
                            FirstName = personDto.FirstName,
                            Address = personDto.Address,
                            City = personDto.City,
                            PersonId = personDto.PersonId,
                            Image = user.Image
                        };
                    }
                   // _db.Persons.Remove(user);
                    //await _db.SaveChangesAsync();
                    _db.Persons.Update(record);
                    await _db.SaveChangesAsync();
                    return "Success";




            }catch (Exception ex)
            {
                return ex.Message;
            }

        }

    }
}
