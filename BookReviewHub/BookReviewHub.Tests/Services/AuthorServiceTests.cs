using BookReviewHub.Services;
using BookReviewHub.Tests.Helpers;
using BookReviewHub.ViewModels;
using NUnit.Framework;

namespace BookReviewHub.Tests.Services
{
    [TestFixture]
    public class AuthorServiceTests
    {
        private AuthorService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            var context = TestDbContextFactory.CreateWithSeed(
                $"AuthorDb_{TestContext.CurrentContext.Test.Name}");
            _sut = new AuthorService(context);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllAuthors()
        {
            var authors = await _sut.GetAllAsync();
            Assert.That(authors.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsAuthorWithBookCount()
        {
            var author = await _sut.GetByIdAsync(1);
            Assert.That(author, Is.Not.Null);
            Assert.That(author!.Name, Is.EqualTo("George Orwell"));
            Assert.That(author.BookCount, Is.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            Assert.That(await _sut.GetByIdAsync(9999), Is.Null);
        }

        [Test]
        public async Task CreateAsync_AddsAuthor()
        {
            await _sut.CreateAsync(new AuthorFormModel
            {
                Name = "Aldous Huxley",
                Nationality = "British",
                BirthYear = 1894
            });

            var all = await _sut.GetAllAsync();
            Assert.That(all.Any(a => a.Name == "Aldous Huxley"), Is.True);
        }

        [Test]
        public async Task UpdateAsync_ValidId_UpdatesAndReturnsTrue()
        {
            var result = await _sut.UpdateAsync(new AuthorFormModel
            {
                Id = 1,
                Name = "Eric Arthur Blair"
            });

            Assert.That(result, Is.True);
            var updated = await _sut.GetByIdAsync(1);
            Assert.That(updated!.Name, Is.EqualTo("Eric Arthur Blair"));
        }

        [Test]
        public async Task UpdateAsync_InvalidId_ReturnsFalse()
        {
            var result = await _sut.UpdateAsync(new AuthorFormModel
            {
                Id = 9999,
                Name = "Nobody"
            });
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task HasBooksAsync_AuthorWithBooks_ReturnsTrue()
        {
            Assert.That(await _sut.HasBooksAsync(1), Is.True);
        }

        [Test]
        public async Task DeleteAsync_InvalidId_ReturnsFalse()
        {
            Assert.That(await _sut.DeleteAsync(9999), Is.False);
        }
    }
}