using BookReviewHub.Data;
using BookReviewHub.Models;
using BookReviewHub.Services.Interfaces;
using BookReviewHub.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookReviewHub.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;

        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AuthorViewModel>> GetAllAsync()
        {
            return await _context.Authors
                .Select(a => new AuthorViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Nationality = a.Nationality,
                    BirthYear = a.BirthYear,
                    Biography = a.Biography,
                    BookCount = a.Books.Count
                })
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        public async Task<AuthorViewModel?> GetByIdAsync(int id)
        {
            return await _context.Authors
                .Where(a => a.Id == id)
                .Select(a => new AuthorViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Nationality = a.Nationality,
                    BirthYear = a.BirthYear,
                    Biography = a.Biography,
                    BookCount = a.Books.Count
                })
                .FirstOrDefaultAsync();
        }

        public async Task<AuthorFormModel?> GetFormModelByIdAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return null;

            return new AuthorFormModel
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                Nationality = author.Nationality,
                BirthYear = author.BirthYear
            };
        }

        public async Task CreateAsync(AuthorFormModel model)
        {
            var author = new Author
            {
                Name = model.Name,
                Biography = model.Biography,
                Nationality = model.Nationality,
                BirthYear = model.BirthYear
            };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(AuthorFormModel model)
        {
            var author = await _context.Authors.FindAsync(model.Id);
            if (author == null) return false;

            author.Name = model.Name;
            author.Biography = model.Biography;
            author.Nationality = model.Nationality;
            author.BirthYear = model.BirthYear;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null) return false;

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> HasBooksAsync(int id)
            => await _context.Books.AnyAsync(b => b.AuthorId == id);

        public async Task<IEnumerable<SelectListItem>> GetSelectListAsync()
        {
            return await _context.Authors
                .OrderBy(a => a.Name)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name
                })
                .ToListAsync();
        }
    }
}