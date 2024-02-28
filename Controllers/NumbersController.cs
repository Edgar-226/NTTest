using Microsoft.AspNetCore.Mvc;
using NTTest.Models;

namespace NTTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumbersController : Controller
    {
        private readonly Numbers numbersGroup;

        public NumbersController()
        {
            numbersGroup = new Numbers();
        }

        [HttpGet("{numero}")]
        public ActionResult<int> GetNumeroFaltante(int numero)
        {
            try
            {
                numbersGroup.Extract(numero);
                return numbersGroup.CalcularNumeroFaltante();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
