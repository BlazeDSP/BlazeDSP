namespace Frontend.Models
{
    using System.ComponentModel.DataAnnotations;

    public class AuthenticatorViewModel
    {
        [Required]
        [Display(Name = "Two Fator Authentication Password")]
        public string Factor { get; set; }
    }
}