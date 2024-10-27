using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{

    //https://localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StuedentsController : ControllerBase
    {
        //GET:/https://localhost:portnumber/api/students
        [HttpGet]
        public IActionResult GetAllStudents() 
        {
            string[] studentNames = new string[] { "John", "JAne", "MArk", "Emily" };
           return Ok(studentNames);
        }

    }
}
