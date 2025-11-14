using CarHubProject.Models;

namespace CarHubProject.Repositories
{
    public interface IContractRepository
    {
        //CRUD
        List<Contract> GetAll();
        List<Contract> GetContractsByCarId(int carId);
        void Add(Contract contract);
        Contract? GetById(int contractId);
        void Update(Contract contract);
        void Delete(Contract contract);
        void Save();
    }
}