using CarHubProject.Models;

namespace CarHubProject.Repositories
{

    public class BrandRepository : IBrandRepository
    {
        private readonly AppDbContext _context;
        public BrandRepository(AppDbContext context) {
        
            _context = context;
        }
        public void Add(Brand brand)
        {
          _context.Brands.Add(brand);
        }

        public void Delete(Brand brand)
        {
            Brand? DelBrand = GetById(brand.BrandId);
            if (DelBrand != null && DelBrand.IsDeleted == false)
            {
                DelBrand.IsDeleted = true;
            }
        }

        public Brand? GetById(int brandId)
        {
            return _context.Brands.FirstOrDefault(x=>x.BrandId==brandId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Brand brand)
        {
           _context.Update(brand);
        }
    }
}
