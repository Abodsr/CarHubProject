using CarHubProject.Models;

namespace CarHubProject.Repositories
{
    public interface IPaymentRepository
    {
        //CRUD
        List<Payment> GetAll();
        void Add(Payment payment);
        Payment? GetById(int paymentId);
        void Update(Payment payment);
        void Delete(Payment payment);
        void Save();
    }
}
