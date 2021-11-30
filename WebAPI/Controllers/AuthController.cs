using Business.Abstract;
using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthService _authservice;

        public AuthController(IAuthService authservice)
        {
            _authservice = authservice;
        }

        [HttpPost("login")]
        public ActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authservice.Login(userForLoginDto);
            if (!userToLogin.Succsess)
            {
                return BadRequest(userToLogin.Message);
            }
            var result = _authservice.CreateAccessToken(userToLogin.Data);
            if (result.Succsess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }


        [HttpPost("Register")]
        public ActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExists = _authservice.UserExists(userForRegisterDto.Email);
            if (!userExists.Succsess)
            {
                return BadRequest(userExists.Message);
            }
            var registerResult = _authservice.Register(userForRegisterDto);
            var result = _authservice.CreateAccessToken(registerResult.Data);
            if (result.Succsess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
        }
    }
}
