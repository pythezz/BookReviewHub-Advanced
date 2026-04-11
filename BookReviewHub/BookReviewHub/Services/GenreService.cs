using BookReviewHub.Data;
using BookReviewHub.Models;
using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookReviewHub.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;

        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GenreViewModel>> GetAllAsync()
        {
            return await _context.Genres
                .Select(g => new GenreViewModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    BookCount = g.Books.Count
                })
                .ToListAsync();
        }

        public async Task<GenreViewModel?> GetByIdAsync(int id)
        {
            var genre = await _context.Genres
                .Include(g => g.Books)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (genre == null) return null;

            return new GenreViewModel
            {
                Id = genre.Id,
                Name = genre.Name,
                BookCount = genre.Books.Count
            };
        }

        public async Task<GenreFormModel?> GetFormModelByIdAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null) return null;

            return new GenreFormModel
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public async Task CreateAsync(GenreFormModel model)
        {
            var genre = new Genre
            {
                Name = model.Name
            };

            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(GenreFormModel model)
        {
            var genre = await _context.Genres.FindAsync(model.Id);

            if (genre == null) return false;

            genre.Name = model.Name;

            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);

            if (genre == null) return false;

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Genres.AnyAsync(g => g.Id == id);
        }

        public async Task<bool> HasBooksAsync(int id)
        {
            return await _context.Books.AnyAsync(b => b.GenreId == id);
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListAsync()
        {
            return await _context.Genres
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .ToListAsync();
        }
    }
}
