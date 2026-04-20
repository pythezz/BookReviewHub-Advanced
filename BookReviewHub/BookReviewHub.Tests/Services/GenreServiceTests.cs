using BookReviewHub.Services;
using BookReviewHub.Tests.Helpers;
using BookReviewHub.ViewModels;
using NUnit.Framework;

namespace BookReviewHub.Tests.Services
{
    [TestFixture]
    public class GenreServiceTests
    {
        private GenreService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            var context = TestDbContextFactory.CreateWithSeed(
                $"GenreDb_{TestContext.CurrentContext.Test.Name}");
            _sut = new GenreService(context);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllGenres()
        {
            var genres = await _sut.GetAllAsync();
            Assert.That(genres.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsync_ValidId_ReturnsGenreWithBookCount()
        {
            var genre = await _sut.GetByIdAsync(2);
            Assert.That(genre, Is.Not.Null);
            Assert.That(genre!.BookCount, Is.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsync_InvalidId_ReturnsNull()
        {
            Assert.That(await _sut.GetByIdAsync(9999), Is.Null);
        }

        [Test]
        public async Task CreateAsync_AddsNewGenre()
        {
            await _sut.CreateAsync(new GenreFormModel { Name = "Horror" });
            var all = await _sut.GetAllAsync();
            Assert.That(all.Any(g => g.Name == "Horror"), Is.True);
        }

        [Test]
        public async Task UpdateAsync_ValidId_UpdatesAndReturnsTrue()
        {
            var result = await _sut.UpdateAsync(new GenreFormModel
            {
                Id = 1,
                Name = "Literary Fiction"
            });

            Assert.That(result, Is.True);
            var updated = await _sut.GetByIdAsync(1);
            Assert.That(updated!.Name, Is.EqualTo("Literary Fiction"));
        }

        [Test]
        public async Task UpdateAsync_InvalidId_ReturnsFalse()
        {
            var result = await _sut.UpdateAsync(new GenreFormModel
            {
                Id = 9999,
                Name = "Ghost"
            });
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task HasBooksAsync_GenreWithBooks_ReturnsTrue()
        {
            Assert.That(await _sut.HasBooksAsync(2), Is.True);
        }

        [Test]
        public async Task ExistsAsync_ExistingId_ReturnsTrue()
        {
            Assert.That(await _sut.ExistsAsync(1), Is.True);
        }

        [Test]
        public async Task ExistsAsync_MissingId_ReturnsFalse()
        {
            Assert.That(await _sut.ExistsAsync(9999), Is.False);
        }
    }
}