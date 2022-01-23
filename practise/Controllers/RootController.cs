using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practise.DTO;
using practise.Model;
using System.Collections.Generic;

namespace practise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name ="getRoot")]
        public ActionResult<IEnumerable<Link>> get()
        {
            List<Link> links = new List<Link>();
            links.Add(new Link(href: Url.Link("getRoot", new {}),rel:"self",method:"GET"));
            links.Add(new Link(href: Url.Link("getConfiguration", new { }), rel: "show-Confiuration data", method: "GET"));
            links.Add(new Link(href: Url.Link("login", new { }), rel: "User LogIn", method: "POST"));
            links.Add(new Link(href: Url.Link("TokenRefresh", new { }), rel: "get new Token", method: "POST"));
            return Ok(links);
        }
        
    }
}
