using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentSIMS.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressId { get; set; }
        public int StreetNumber { get; set; }
        public string Street { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public int PostCode { get; set; }
        [Required]
        public string Country { get; set; }
        //Foreign Key
        public int StudentId { get; set; }
        //Navigation property
        //Ref:https://docs.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-2
        public virtual Student Student { get; set; }
    }
}
