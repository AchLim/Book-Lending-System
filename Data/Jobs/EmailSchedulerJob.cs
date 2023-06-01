using Quartz;
using Microsoft.EntityFrameworkCore;

namespace Book_Lending_System.Data.Jobs
{
    public class EmailSchedulerJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        public EmailSchedulerJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<Book_Lending_SystemContext>();
                    var emailSender = scope.ServiceProvider.GetService<IEmailSender>();

                    var RequestsToCheck = dbContext!.LendRequest.Include(lr => lr.User).ThenInclude(u => u.User).Include(lr => lr.Book).Where(lr => lr.Status == BookLendingStatus.Approved).ToList();
                    foreach (var request in RequestsToCheck)
                    {
                        var utcNow = DateTime.UtcNow.Date;
                        if ((utcNow.AddDays(1)) == request.EndDate)
                        {
                            var userEmail = request.User!.User!.Email!;

                            var subject = "Book Lending Due Date Reminder";
                            var body = $"""
                                <p>Dear {request.User.Name}</p>
                                <p>The book: {request.Book.Title} you have borrowed will reach the due date tomorrow. Please return the book as soon as possible</p>
                            """;

                            emailSender!.SendEmailAsync(userEmail, subject, body);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return Task.FromResult(true);
        }
    }
}
