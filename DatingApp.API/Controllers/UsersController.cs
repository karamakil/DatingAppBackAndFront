using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Data;
using DatingApp.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    
    public class UsersController: BaseApiController
    {
        public DataContext _Context { get; }

        public UsersController(DataContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public ActionResult<List<AppUser>> GetData()
        {
            

            return new AppUser().GetAll();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<AppUser> GetById(int id)
        {
            using (var ctx = new DataContext())
            {
                return ctx.Users.FirstOrDefault(x=> x.Id == id);
            }
        }

        // GET api/values/""NAME""
        [HttpGet("{name}")]
        public ActionResult<AppUser> GetById(string name)
        {
            using (var ctx = new DataContext())
            {
                return ctx.Users.FirstOrDefault(x=> x.UserName == name);
            }
        }
    }
}