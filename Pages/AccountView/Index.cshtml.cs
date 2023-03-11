using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using Book_Lending_System.Models;
using Microsoft.AspNetCore.Authorization;

namespace Book_Lending_System.Pages.AccountView
{
    public class IndexModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(Book_Lending_System.Data.Book_Lending_SystemContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Account> Account { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Account != null)
            {
                Account = await _context.Account.ToListAsync();
            }
        }
    }
}
