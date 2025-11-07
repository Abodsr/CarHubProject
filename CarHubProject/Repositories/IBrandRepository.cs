using CarHubProject.Models;

namespace CarHubProject.Repositories
{
    public interface IBrandRepository
    {
        void Add(Brand brand);
        List<Brand> GetAll();
        Brand? GetById(int brandId);
        void Update(Brand brand);

        void Delete(Brand brand);

        void Save();
    }
}
