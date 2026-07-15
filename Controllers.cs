using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ProjetoMIragnum
{
   

        [ApiController]
        [Route("api/[Controller]")]
        public class MiragController : ControllerBase
    {
        private readonly MyDbContext _myContext;

        public MiragController(MyDbContext context)
        {
            _myContext = context;
        }
    }

    
}
