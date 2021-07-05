using System.Collections.Generic;
using summerSemesterProj.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace summerSemesterProj.Data {

    public interface IUsersRepo {
        void CreateUser(User user);
        void CreateNote(Note note, string id);
        void DeleteNote(User user, string id);
        User GetUserById(string id);
        User GetUserByEmail(string email);
        User GetNotes(string id);

    }
    
}