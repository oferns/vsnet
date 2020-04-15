namespace VS.Core.Identity {

    using MediatR;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;

    public sealed class LoginWithPassword : IRequest<ClaimsIdentity> {

        [Display(Name = "Email")]
        [Required(ErrorMessage = "A valid email address is required.")]
        [EmailAddress(ErrorMessage = "The email address is not valid.")]
        [DataType(DataType.EmailAddress)]
        [Description("Your registered email address")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "You need to provide a password to log in.")]
        [DataType(DataType.Password)]
        [Description("Your password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        [Description("Do not tick if using a shared computer!")]
        public bool Persistent { get; set; }

    }
}