using DatingApp.API.Data;
using DatingApp.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private DataContext databContext;
        public BuggyController(DataContext dbContext)
        {
            this.databContext = dbContext;

        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "sceret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var thing = this.databContext.Users.Find(-1);
            if (thing == null) return NotFound();
            return Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            // try
            // {
                var thing = databContext.Users.Find(-1);
                var retVal = thing.ToString();
                return retVal;
            // }
            // catch (System.Exception)
            // {
            //     return "Error";
            // }
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("not good request");
        }




    }
}