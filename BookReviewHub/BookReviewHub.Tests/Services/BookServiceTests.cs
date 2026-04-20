using BookReviewHub.Services;
using BookReviewHub.Tests.Helpers;
using BookReviewHub.ViewModels;
using NUnit.Framework;

namespace BookReviewHub.Tests.Services
{
    [TestFixture]
    public class BookServiceTests
    {
        private BookService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            var context = TestDbContextFactory.CreateWithSeed(
                $"BookDb_{TestContext.CurrentContext.Test.Name}");
            _sut = new BookService(context);
        }

        [Test]
        public async Task GetPagedAsync_NoFilter_ReturnsAllBooks()
        {
            var result = await _sut.GetPagedAsync(null, null, null, 1, 10);
            Assert.That(result.TotalCount, Is.EqualTo(3));
        }

        [Test]
        public async Task GetPagedAsync_SearchByTitle_ReturnsMatch()
        {
            var result = await _sut.GetPagedAsync("1984", null, null, 1, 10);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Title, Is.EqualTo("1984"));
        }

        [Test]
        public async Task GetPagedAsync_SearchByAuthor_ReturnsAllByAuthor()
        {
            var result = await _sut.GetPagedAsync("Orwell", null, null, 1, 10);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetPagedAsync_FilterByGenre_ReturnsOnlyThatGenre()
        {
            var result = await _sut.GetPagedAsync(null, 1, null, 1, 10);
            Assert.That(result.All(b => b.GenreName == "Fiction"), Is.True);
        }

        [Test]
        public async Task GetPagedAsync_Pagination_ReturnsCorrectPage()
        {
            var page1 = await _sut.GetPagedAsync(null, null, null, 1, 2);
            var page2 = await _sut.GetPagedAsync(null, null, null, 2, 2);

            Assert.That(page1.Count, Is.EqualTo(2));
            Assert.That(page2.Count, Is.EqualTo(1));
            Assert.That(page1.TotalCount, Is.EqualTo(3));
        }

        [Test]
        public async Task GetPagedAsync_NoMatch_ReturnsEmpty()
        {
            var result = await _sut.GetPagedAsync("zzz-no-match", null, null, 1, 10);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsBook()
        {
            var result = await _sut.GetByIdAsync(1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Title, Is.EqualTo("1984"));
        }

        [Test]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            var result = await _sut.GetByIdAsync(9999);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByIdAsync_CalculatesAverageRating()
        {
            var result = await _sut.GetByIdAsync(1);
            Assert.That(result!.AverageRating, Is.EqualTo(5.0));
        }

        [Test]
        public async Task CreateAsync_ValidModel_CreatesBook()
        {
            var model = new BookFormModel
            {
                Title = "Brave New World",
                Description = "A dystopian social science fiction novel.",
                PublicationYear = 1932,
                GenreId = 2,
                AuthorId = 1
            };

            await _sut.CreateAsync(model);

            var all = await _sut.GetPagedAsync(null, null, null, 1, 100);
            Assert.That(all.TotalCount, Is.EqualTo(4));
        }

        [Test]
        public async Task UpdateAsync_ValidModel_UpdatesBook()
        {
            var model = new BookFormModel
            {
                Id = 1,
                Title = "1984 – Updated",
                Description = "Updated description for this classic novel.",
                PublicationYear = 1949,
                GenreId = 2,
                AuthorId = 1
            };

            var result = await _sut.UpdateAsync(model);

            Assert.That(result, Is.True);
            var updated = await _sut.GetByIdAsync(1);
            Assert.That(updated!.Title, Is.EqualTo("1984 – Updated"));
        }

        [Test]
        public async Task UpdateAsync_InvalidId_ReturnsFalse()
        {
            var model = new BookFormModel
            {
                Id = 9999,
                Title = "Ghost",
                Description = "Does not exist in the database.",
                PublicationYear = 2000,
                GenreId = 1,
                AuthorId = 1
            };

            var result = await _sut.UpdateAsync(model);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAsync_ValidId_DeletesBook()
        {
            var result = await _sut.DeleteAsync(3);
            Assert.That(result, Is.True);
            Assert.That(await _sut.ExistsAsync(3), Is.False);
        }

        [Test]
        public async Task DeleteAsync_InvalidId_ReturnsFalse()
        {
            var result = await _sut.DeleteAsync(9999);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ExistsAsync_ExistingBook_ReturnsTrue()
        {
            Assert.That(await _sut.ExistsAsync(1), Is.True);
        }

        [Test]
        public async Task ExistsAsync_MissingBook_ReturnsFalse()
        {
            Assert.That(await _sut.ExistsAsync(9999), Is.False);
        }
    }
}