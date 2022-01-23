using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using practise.Repository;
using System;
using System.Threading.Tasks;

namespace practise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly IDataProtector dataProtector;
        private readonly HashService _hashService;
        public SecurityController(IDataProtectionProvider protectionProvider,HashService hashService)
        {
            dataProtector = protectionProvider.CreateProtector("value secret and unique");
            _hashService = hashService;
        }

        [HttpGet]
        [Route("{text}")]
        public IActionResult GET([FromRoute]string text)
        {
            string encriptText = dataProtector.Protect(text);
            string decrepText=dataProtector.Unprotect(encriptText);
            return Ok(new { orginalText= text,EncriptedText=encriptText,DecriptedText=decrepText });

        }

        [HttpGet]
        [Route("TimeBound/{text}")]
        public async Task<IActionResult> GETTimeBound([FromRoute] string text)
        {
            var protectorTimeBound = dataProtector.ToTimeLimitedDataProtector();
            string encriptText = protectorTimeBound.Protect(text,lifetime:TimeSpan.FromSeconds(5));
            await Task.Delay(6000);
            string decrepText = protectorTimeBound.Unprotect(encriptText);
           
            return Ok(new { orginalText = text, EncriptedText = encriptText, DecriptedText = decrepText });

        }

        [HttpGet]
        [Route("hash/{data}")]
        public IActionResult getHash(string data)
        {
            return Ok(new { data= data, hashResult1= _hashService.Hash(data), hashResult2 = _hashService.Hash(data) });
        }
    }
}
