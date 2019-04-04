using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace testWebApp.Models.Extend
{
    [MetadataType(typeof(UserMetaData))]
    public class Table
    {
        public string ConfirmPassword { get; set; }  //This will not save in DB. --> encapsulation
    }

    public class UserMetaData
    {
        [Required(AllowEmptyStrings = false, ErrorMessage ="Input name!")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Input lastname!")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Input e-mail!")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Input password!")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimum charactre is 8!")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimum charactre is 8!")]
        [Compare("Password", ErrorMessage = "password not match!")]
        public string ConfirmPassword { get; set; }

        

    }
}