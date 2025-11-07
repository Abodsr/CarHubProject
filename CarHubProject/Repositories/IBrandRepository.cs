using CarHubProject.Models;

namespace CarHubProject.Repositories
{
    public interface IBrandRepository
    {
        void Add(Brand brand);
        Brand? GetById(int brandId);
        void Update(Brand brand);

        void Delete(Brand brand);

        void Save();
    }
}
