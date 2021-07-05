using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using summerSemesterProj.Models;
using System.Collections.Generic;


namespace summerSemesterProj.Models {
    public class User {
        [Key]
        public string Id {get; set;}

        [Required]
        public string Email {get; set;}
        [Required]
        [JsonIgnore]
        public string Password {get; set;}
        public ICollection<Note> Notes { get; set;} = new List<Note>();
    }
}
// namespace summerSemesterProj.Models {
//     public class User {

//         public User(){
//             Notes = new List<Note>();
//         }

//         [Key]
//         public string Id {get; set;}

//         [Required]
//         public string Email {get; set;}
//         [Required]
//         [JsonIgnore]
//         public string Password {get; set;}
//         public virtual List<Note> Notes { get; set;}
//     }
// }