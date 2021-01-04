using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoApi.Models;
using TodoApi.Models.Helpers;
using TodoApi.Models.UserModels;
using TodoApi.Services.UserService;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;

        
        public UsersController(TodoContext context, IMapper mapper, 
        IUserService userService,
        IOptions<AppSettings> appSettings)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        // GET: api/Users
        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        // PUT: api/Users/update/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{id}")]
        public IActionResult PutUser(int id, [FromBody]UpdateModel model)
        {
           // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;
            try
            {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        
        //POST: api/Users/register
        //[AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Users/delete/5
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
             _userService.Delete(id);
            return Ok();
        }

    }
}
