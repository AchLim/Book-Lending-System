namespace Book_Lending_System.Models
{
    public class Publisher
    {
        public Publisher() {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<BookPublisher>? BookPublishers { get; set; }
    }
}