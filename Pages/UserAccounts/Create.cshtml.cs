using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Book_Lending_System.Data;
using Book_Lending_System.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Book_Lending_System.Pages.UserAccounts
{
    public class CreateModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private ILogger<CreateModel> _logger;

        public CreateModel(Book_Lending_System.Data.Book_Lending_SystemContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserAccount UserAccount { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.UserAccount == null || UserAccount == null)
            {
                return Page();
            }

            _context.UserAccount.Add(UserAccount);
            await _context.SaveChangesAsync();

            ICollection<Role> roles = UserAccount.GetRoleChanges(_context);

            if (_context.Role != null)
            {
                await _context.Role.AddRangeAsync(roles);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
