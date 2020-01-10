using System.ComponentModel.DataAnnotations;

namespace SWA.WF.Services.Api.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
