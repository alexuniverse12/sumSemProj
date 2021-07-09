using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using summerSemesterProj.Data;
using summerSemesterProj.Dtos;
using summerSemesterProj.Helpers;
using summerSemesterProj.Models;

namespace summerSemesterProj.Controllers {

    [ApiController]
    public class MainController : ControllerBase {
        private readonly IUsersRepo _repository;
        private readonly IMapper _mapper;
        private readonly JWTService _jwtService;

        public MainController(IUsersRepo repository, IMapper mapper, JWTService jwtService){
            _repository = repository; 
            _mapper = mapper; 
            _jwtService = jwtService;
        }
        
        // /createUser
        [HttpPost("createUser")]
        public IActionResult Register(RegDto user){
            var ourUser = _mapper.Map<User>(user);
            var  checkUser = _repository.GetUserByEmail(ourUser.Email);
            if(checkUser != null){
                // return BadRequest(new {message = "Already Exists"});
                throw new Exception();
            }
            _repository.CreateUser(ourUser);

            return Created("success", ourUser.Email);
        }

        // home/login 
        [HttpPost("login")]
        public IActionResult Login(RegDto user){
            var currUser = _repository.GetUserByEmail(user.Email);
            // var mappedUser = _mapper.Map<User>(currUser);
            
            //email not found
            if(currUser == null){
                return BadRequest(new {message = "Invalid Credentials"});
            }

            // not correct password
            if(user.Password != currUser.Password){
                return BadRequest(new {message = "Invalid Credentials"});
            }

            var jwtToken = _jwtService.generate(currUser.Id);

            Response.Cookies.Append("jwtToken", jwtToken, new CookieOptions{HttpOnly = true, SameSite = SameSiteMode.None, Secure = true});

            return Ok(new {message = "logged in successfully"});
        }


        // /user
        [HttpGet("user")]
        public IActionResult GetUserById(int id){
            string userId = _jwtService.validateToken(Request);
            var user = _repository.GetUserById(userId);

            if(user == null){
                return BadRequest(new {message = "Not Authorized"});
            }

            return Ok(user); 
        }

        // /logout
        [HttpPost("logout")]
        public IActionResult Logout(int id){
            // deleting the cookie
            Response.Cookies.Delete("jwtToken", new CookieOptions{HttpOnly = true, SameSite = SameSiteMode.None, Secure = true});
            
            return Ok(new {message = "logged out successfully"});
        }
        
        

        // /addNote
        [HttpPost("user/addNote")]
        public IActionResult AddNote(Note note){
            try {
                string userId = _jwtService.validateToken(Request);
                _repository.CreateNote(note, userId);     
                
                return Created("Note created", note);     
            } catch (Exception error) {
                return BadRequest(new {error});
            }
        }

        // /getAllNotes 
        [HttpGet("user/getAllNotes")]
        public IActionResult GetAllNotes(){
            string userId = _jwtService.validateToken(Request);
            var userNotes = _repository.GetNotes(userId);     

            if(userNotes == null){
                return BadRequest(new {message = "Not Authorized"});
            }

            return Ok(userNotes); 
        }


        // /deleteNote
        [HttpDelete("user/deleteNote")]
        public IActionResult DeleteNote(Note note){
            try {
                string userId = _jwtService.validateToken(Request);
                var userWithNotes = _repository.GetNotes(userId); 
                _repository.DeleteNote(userWithNotes, note.noteId);     
                
                return NoContent();     
            } catch (Exception error){
                return BadRequest(new {error});;
            }
        }
    }
}