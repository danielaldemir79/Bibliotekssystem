using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Bibliotekssystem.Core.Models;
using Bibliotekssystem.Web.Components.Shared;

namespace Biblioteksystem.Tests.Components
{
    // Testar BookCard-komponenten med bUnit (renderar Blazor-komponenter i minnet)
    public class BookCardTests : BunitContext
    {
        [Fact]
        public void BookCard_ShouldDisplayBookTitle()
        {
            var book = new Book
            {
                Id = 1,
                ISBN = "978-TEST",
                Title = "Testbok",
                Author = "Testförfattare",
                PublishedYear = 2024,
                IsAvailable = true
            };

            var cut = Render<BookCard>(parameters =>
                parameters.Add(p => p.Book, book));

            cut.Find("h5").MarkupMatches("<h5 class=\"card-title mb-1\">Testbok</h5>");
        }

        [Fact]
        public void BookCard_AvailableBook_ShouldShowGreenBadge()
        {
            var book = new Book
            {
                Id = 1,
                ISBN = "978-TEST",
                Title = "Testbok",
                Author = "Författare",
                PublishedYear = 2024,
                IsAvailable = true
            };

            var cut = Render<BookCard>(parameters =>
                parameters.Add(p => p.Book, book));

            var badge = cut.Find(".badge");
            Assert.Contains("bg-success", badge.GetAttribute("class"));
            Assert.Contains("Tillgänglig", badge.TextContent);
        }

        [Fact]
        public void BookCard_UnavailableBook_ShouldShowRedBadge()
        {
            var book = new Book
            {
                Id = 1,
                ISBN = "978-TEST",
                Title = "Utlånad bok",
                Author = "Författare",
                PublishedYear = 2024,
                IsAvailable = false
            };

            var cut = Render<BookCard>(parameters =>
                parameters.Add(p => p.Book, book));

            var badge = cut.Find(".badge");
            Assert.Contains("bg-danger", badge.GetAttribute("class"));
            Assert.Contains("Utlånad", badge.TextContent);
        }

        [Fact]
        public void BookCard_ShouldDisplayAuthorAndYear()
        {
            var book = new Book
            {
                Id = 1,
                ISBN = "978-TEST",
                Title = "Testbok",
                Author = "Astrid Lindgren",
                PublishedYear = 1945,
                IsAvailable = true
            };

            var cut = Render<BookCard>(parameters =>
                parameters.Add(p => p.Book, book));

            var text = cut.Find(".text-muted").TextContent;
            Assert.Contains("Astrid Lindgren", text);
            Assert.Contains("1945", text);
        }
    }
}
