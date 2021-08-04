using Domain.Entities.Users;
using Infrastructure.DataAccess.EntityFramework.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Applications.Website.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        // Fields
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly UserManager<ApplicationUser> UserManager;

        // Properties
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        // Constructors
        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
        )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        // Methods

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            IEnumerable<AuthenticationScheme> authenticationSchemes = await SignInManager.GetExternalAuthenticationSchemesAsync();
            ExternalLogins = authenticationSchemes.ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            IEnumerable<AuthenticationScheme> authenticationSchemes = await SignInManager.GetExternalAuthenticationSchemesAsync();
            ExternalLogins = authenticationSchemes.ToList();
            if (!ModelState.IsValid)
            {
                // redisplay form
                return Page();
            }
            User domainUser = new(
                Input.UniqueMasterCitizenNumber, 
                Input.FirstName, 
                Input.LastName
            );
            ApplicationUser applicationUser = new(domainUser);
            IdentityResult identityResult = await UserManager.CreateAsync(applicationUser, Input.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, identityError.Description);
                }
                return Page();
            }
            await SignInManager.SignInAsync(applicationUser, isPersistent: false);
            returnUrl ??= Url.Content("~/");
            return LocalRedirect(returnUrl);
        }

        // Classes
        public class InputModel
        {
            [Required]
            // TODO add string and regex restrictions
            [Display(Name = "Unique master citizen number")]
            public string UniqueMasterCitizenNumber { get; set; }

            [Required]
            // TODO add length restrictions
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            // TODO add length restrictions
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}
