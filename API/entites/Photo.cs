using System.ComponentModel.DataAnnotations.Schema;
namespace API.entites
{   
    [Table("Photos")]
    public class Photo
    {
        public int id{get; set;}
        public string Url {get; set;}
        public bool IsMain {get; set;}
        public string PublicId {get; set;}

        //conventions - fully defined relationships
        //create dependency on user to uplaod photo 
        //if user wants to add photo then user id will be compulsory to mention
        // if user is deleted then photo will also be deleted based on cascading 
        public int AppUserId{get; set;}
        public AppUser AppUser{get; set;}
    }
}