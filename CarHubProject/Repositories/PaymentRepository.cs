using CarHubProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CarHubProject.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(Payment payment)
        {
            _context.Payments.Add(payment);
        }

        public void Delete(Payment payment)
        {
            Payment? DelPayment = GetById(payment.PaymentId);
            if (DelPayment != null && DelPayment.IsDeleted == false)
            {
                DelPayment.IsDeleted = true;
            }
        }

        public List<Payment> GetAll()
        {
            return _context.Payments.Where(x => !x.IsDeleted).ToList();
        }

        public Payment? GetById(int paymentId)
        {
            return _context.Payments.FirstOrDefault(x => x.PaymentId == paymentId);
        }

        public void Update(Payment payment)
        {
            _context.Update(payment);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
