using CarHubProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CarHubProject.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(Review review)
        {
            _context.Reviews.Add(review);
        }

        public void Delete(Review review)
        {
            Review? DelReview = GetById(review.ReviewId);
            if (DelReview != null && DelReview.IsHidden == false)
            {
                DelReview.IsHidden = true;
            }
        }

        public List<Review> GetAll()
        {
            return _context.Reviews.Where(x => !x.IsHidden).ToList();
        }

        public Review? GetById(int reviewId)
        {
            return _context.Reviews.FirstOrDefault(x => x.ReviewId == reviewId);
        }

        public void Update(Review review)
        {
            _context.Update(review);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
