using BookReviewHub.Services;
using BookReviewHub.Tests.Helpers;
using BookReviewHub.ViewModels;
using NUnit.Framework;

namespace BookReviewHub.Tests.Services
{
    [TestFixture]
    public class ReviewServiceTests
    {
        private ReviewService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            var context = TestDbContextFactory.CreateWithSeed(
                $"ReviewDb_{TestContext.CurrentContext.Test.Name}");
            _sut = new ReviewService(context);
        }

        [Test]
        public async Task GetByBookAsync_BookWithReviews_ReturnsReviews()
        {
            var reviews = await _sut.GetByBookAsync(1);
            Assert.That(reviews.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetByBookAsync_BookWithNoReviews_ReturnsEmpty()
        {
            var reviews = await _sut.GetByBookAsync(3);
            Assert.That(reviews, Is.Empty);
        }

        [Test]
        public async Task GetByUserAsync_UserWithReviews_ReturnsTheirReviews()
        {
            var reviews = await _sut.GetByUserAsync("test-user-id-1");
            Assert.That(reviews.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task CreateAsync_ValidData_CreatesReview()
        {
            var model = new ReviewFormModel
            {
                BookId = 3,
                Content = "A brilliant political allegory that still resonates today.",
                Rating = 5
            };

            var id = await _sut.CreateAsync(model, "test-user-id-1");

            Assert.That(id, Is.GreaterThan(0));
            var review = await _sut.GetByIdAsync(id);
            Assert.That(review!.Rating, Is.EqualTo(5));
        }

        [Test]
        public async Task UpdateAsync_OwnerUpdates_ReturnsTrueAndPersists()
        {
            var model = new ReviewFormModel
            {
                Id = 1,
                BookId = 1,
                Content = "On reflection, an absolute masterpiece of literature.",
                Rating = 5
            };

            var result = await _sut.UpdateAsync(model, "test-user-id-1");

            Assert.That(result, Is.True);
            var updated = await _sut.GetByIdAsync(1);
            Assert.That(updated!.Content, Does.Contain("absolute masterpiece"));
        }

        [Test]
        public async Task UpdateAsync_WrongUser_ReturnsFalse()
        {
            var model = new ReviewFormModel
            {
                Id = 1,
                BookId = 1,
                Content = "Attempting to edit someone else's review.",
                Rating = 1
            };

            var result = await _sut.UpdateAsync(model, "wrong-user");
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAsync_OwnerDeletes_ReturnsTrue()
        {
            var result = await _sut.DeleteAsync(1, "test-user-id-1", false);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteAsync_AdminDeletes_ReturnsTrue()
        {
            var result = await _sut.DeleteAsync(1, "any-other-user", true);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task DeleteAsync_WrongUser_ReturnsFalse()
        {
            var result = await _sut.DeleteAsync(1, "wrong-user", false);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task UserHasReviewedBookAsync_HasReview_ReturnsTrue()
        {
            var result = await _sut.UserHasReviewedBookAsync(1, "test-user-id-1");
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task UserHasReviewedBookAsync_NoReview_ReturnsFalse()
        {
            var result = await _sut.UserHasReviewedBookAsync(3, "test-user-id-1");
            Assert.That(result, Is.False);
        }
    }
}