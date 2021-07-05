using System.Collections.Generic;
using summerSemesterProj.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using static System.Console;
using Microsoft.EntityFrameworkCore;

namespace summerSemesterProj.Data {

    public class UsersSqlRepo : IUsersRepo {
        private readonly Context _context;
        public UsersSqlRepo(Context context){
            _context = context;
        }
        public void CreateUser(User user){
            if(user == null){
                throw new ArgumentNullException(nameof(user));
            }

            _context.Users.Add(user);
            _context.SaveChanges();
 
        }
        public void CreateNote(Note note, string id){
            if(note == null){
                throw new ArgumentNullException(nameof(note));
            }

            _context.Users.FirstOrDefault(user => user.Id == id).Notes.Add(note);
            _context.SaveChanges();
 
        }
        public User GetNotes(string id){
            var currUser = _context.Users
                       .Where(us => us.Id == id)
                       .Include(us => us.Notes)
                       .FirstOrDefault();
            WriteLine(currUser);
            return currUser;
        }

        public void DeleteNote(User user, string noteID){
            var currUser = _context.Users
                       .Where(us => us.Id == user.Id)
                       .Include(us => us.Notes)
                       .FirstOrDefault();
            _context.Notes.Remove(currUser.Notes.FirstOrDefault(note => note.noteId == noteID));                   
            _context.SaveChanges();
        }

        public User GetUserById(string id)
        {
            return _context.Users.FirstOrDefault(user => user.Id == id);
        }
        public User GetUserByEmail(string email){
            return _context.Users.FirstOrDefault(user => user.Email == email);
        }
    }

}