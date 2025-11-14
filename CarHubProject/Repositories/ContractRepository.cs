using CarHubProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CarHubProject.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly AppDbContext _context;
        public ContractRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(Contract contract)
        {
            _context.Contracts.Add(contract);
        }

        public void Delete(Contract contract)
        {
            Contract? DelContract = GetById(contract.ContractId);
            if (DelContract != null && DelContract.IsDeleted == false)
            {
                DelContract.IsDeleted = true;
            }
        }

        public List<Contract> GetAll()
        {
            return _context.Contracts.Where(x => !x.IsDeleted).ToList();
        }

        public List<Contract> GetContractsByCarId(int carId)
        {
            return _context.Contracts.Where(c => c.CarId == carId && !c.IsDeleted).ToList();
        }

        public Contract? GetById(int contractId)
        {
            return _context.Contracts.FirstOrDefault(x => x.ContractId == contractId);
        }

        public void Update(Contract contract)
        {
            _context.Update(contract);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
