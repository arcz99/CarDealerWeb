using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace CarDealerWeb.Pages
{
    public class ContactModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        [EmailAddress]
        public string Email { get; set; }
        [BindProperty]
        public string Message { get; set; }

        public bool MessageSent { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            // Tu mo¿esz podpi¹æ wysy³kê maila przez SMTP lub zapis do bazy
            MessageSent = true; // Pokazuje alert po wys³aniu
        }
    }
}