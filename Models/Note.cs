using System.ComponentModel.DataAnnotations;


namespace summerSemesterProj.Models {
    public class Note {

        [Key]
        public string noteId {get; set;}
        [Required]
        public string noteContent {get; set;}
        [Required]
        public string creationDate {get;set;}

        
        // [ForeignKey("User")]
        // public string Id {get; set;}
        // public User User {get; set;}


    }
}