using Bogus;
using Library.Domain;

namespace Library.MVC.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Look for any books already in the database.
            if (context.Books.Any())
                return;
            // Create a faker for books
            var bookFaker = new Faker<Books>()
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
                .RuleFor(b => b.Author, f => f.Name.FullName())
                .RuleFor(b => b.ISBN, f => f.Random.Replace("###-#-##-######-#"))
                .RuleFor(b => b.Category, f => f.Commerce.Categories(1)[0])
                .RuleFor(b => b.IsAvailable, true);
            // Generate 20 fake books
            var books = bookFaker.Generate(20);

            // Create a faker for members
            var memberFaker = new Faker<Member>()
                .RuleFor(m => m.FullName, f => f.Name.FullName())
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.PhoneNumber, f => f.Phone.PhoneNumber());
            // Generate 10 fake members
            var members = memberFaker.Generate(10);

            
            context.Books.AddRange(books);
            context.Members.AddRange(members);
            context.SaveChanges();

            var random = new Random();

            var loans = new List<Loan>();

            for (int i = 0; i < 15; i++)
            {
                var book = books[random.Next(books.Count)];
                var member = members[random.Next(members.Count)];

                loans.Add(new Loan
                {
                    BookID = book.Id,
                    MemberID = member.Id,
                    LoanDate = DateTime.Now.AddDays(-random.Next(30)),
                    DueDate = DateTime.Now.AddDays(random.Next(10)),
                    ReturnDate = random.Next(2) == 0 ? null : DateTime.Now.AddDays(-random.Next(5))
                });
            }

            context.Loans.AddRange(loans);
            context.SaveChanges();
        }
    }
}