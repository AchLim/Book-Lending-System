namespace Book_Lending_System.Models
{
    public class Author
    {
        public Author() {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public ICollection<BookAuthor>? BookAuthors { get; set; }
    }
}
