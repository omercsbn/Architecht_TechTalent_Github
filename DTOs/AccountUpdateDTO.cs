using System.ComponentModel.DataAnnotations;
namespace FinansalYonetimWebApi.DTOs
{
    public class AccountUpdateDto
    {
        [Required(ErrorMessage = "The accountName field is required.")]
        public string AccountName { get; set; }
    }
}
