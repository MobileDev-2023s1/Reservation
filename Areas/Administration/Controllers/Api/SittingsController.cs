using Group_BeanBooking.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group_BeanBooking.Areas.Administration.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SittingsController : ControllerBase
    {
        
        
        private readonly ApplicationDbContext _context; 

        public SittingsController(ApplicationDbContext context)
        {
           _context = context;

        }
        
        [Route("events")]
        public async Task<IActionResult> GetEvents(DateTime start, DateTime end) { 
          var result= await _context.Sittings
                .Where(s=>(s.Start>start&&s.Start<end) || (s.End > start && s.End < end))
                .Select(s => new { title=s.Name,  s.Start, s.End }).ToListAsync();
                
            return Ok(result);
        }




    }
}
