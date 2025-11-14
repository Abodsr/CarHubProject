using CarHubProject.Models;

namespace CarHubProject.Repositories
{
    public interface IReviewRepository
    {
        //CRUD
        List<Review> GetAll();
        void Add(Review review);
        Review? GetById(int reviewId);
        void Update(Review review);
        void Delete(Review review);
        void Save();
    }
}
