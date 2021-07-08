using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Console;
using summerSemesterProj.Data;
using summerSemesterProj.Dtos;
using summerSemesterProj.Helpers;
using summerSemesterProj.Models;

namespace summerSemesterProj.Controllers {
    //  (/home)
    // [Route("home")]
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
        
        // home/createUser
        [HttpPost("createUser")]
        public IActionResult Register(RegDto user){
            var ourUser = _mapper.Map<User>(user);
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
            if(user.Email != currUser.Email){
                return BadRequest(new {message = "Invalid Credentials"});
            }

            var jwtToken = _jwtService.generate(currUser.Id);

            Response.Cookies.Append("jwtToken", jwtToken, new CookieOptions{HttpOnly = true, SameSite = SameSiteMode.None, Secure = true});

            return Ok(new {message = "logged in successfully"});
        }


        // home/user
        [HttpGet("user")]
        public IActionResult GetUserById(int id){
            // authorization by verifying token of the user
            try {
                var jwtToken = Request.Cookies["jwtToken"];
                var token = _jwtService.verifyToken(jwtToken);
                string userId = token.Issuer;
                var user = _repository.GetUserById(userId);

                return Ok(user);     
            } catch {
                return Unauthorized();
            }
        
        }

        // home/logout
        [HttpPost("logout")]
        public IActionResult Logout(int id){
            // deleting the cookie
            Response.Cookies.Delete("jwtToken");
            
            return Ok(new {message = "logged out successfully"});
        }
        
        

        // home/addNote
        [HttpPost("user/addNote")]
        public IActionResult AddNote(Note note){
            try {
                var jwtToken = Request.Cookies["jwtToken"];
                var token = _jwtService.verifyToken(jwtToken);
                string userId = token.Issuer;
                _repository.CreateNote(note, userId);     
                
                return Created("Note created", note);     
            } catch {
                return Unauthorized();
            }
        }

        // home/getAllNotes 
        [HttpGet("user/getAllNotes")]
        public IActionResult GetAllNotes(){
            try {
                var jwtToken = Request.Cookies["jwtToken"];
                var token = _jwtService.verifyToken(jwtToken);
                string userId = token.Issuer;
                var userNotes = _repository.GetNotes(userId);     
                
                return Ok(userNotes);     
            } catch (Exception e){
                WriteLine(e);
                return Unauthorized();
            }
        }


        // home/deleteNote
        [HttpDelete("user/deleteNote")]
        public IActionResult DeleteNote(Note note){
            try {
                var jwtToken = Request.Cookies["jwtToken"];
                var token = _jwtService.verifyToken(jwtToken);
                string userId = token.Issuer;
                var userWithNotes = _repository.GetNotes(userId); 
                _repository.DeleteNote(userWithNotes, note.noteId);     
                
                return NoContent();     
            } catch (Exception e){
                WriteLine(e);
                return Unauthorized();
            }
        }
    }
}