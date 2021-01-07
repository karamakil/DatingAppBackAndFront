using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.API.Entities
{
    //this variable is used to make sure that the table in db is called photos
    [Table("Photos")]
    public class Photo
    {
        #region Properties

        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }  
        

        #endregion
    }
}