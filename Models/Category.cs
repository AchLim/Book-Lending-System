using Microsoft.EntityFrameworkCore;

namespace Book_Lending_System.Models 
{
    // Book Category
    public class Category
    {
        public Category()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public required string Name { get; set; }

        public ICollection<BookCategory>? BookCategories { get; set; }
    }
}
