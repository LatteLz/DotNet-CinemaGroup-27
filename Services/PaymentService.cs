using CinemaManagement.Models;

namespace CinemaManagement.Services
{
    public class PaymentService
    {
        public decimal CalculateChange(decimal totalAmount, decimal cashGiven)
        {
            if (cashGiven < totalAmount)
                throw new BusinessException(
                    $"Số tiền khách đưa ({cashGiven:N0} đ) chưa đủ để thanh toán {totalAmount:N0} đ.");
            return cashGiven - totalAmount;
        }

        public Payment Process(decimal totalAmount, string paymentMethod, decimal cashGiven)
        {
            if (totalAmount <= 0)
                throw new BusinessException("Tổng tiền thanh toán không hợp lệ.");

            decimal given = paymentMethod == PaymentMethods.Cash ? cashGiven : totalAmount;
            decimal change = CalculateChange(totalAmount, given);

            return new Payment
            {
                Amount = totalAmount,
                CashGiven = given,
                ChangeReturned = change,
                PaymentMethod = paymentMethod,
                Status = PaymentStatus.Success,
                PaymentDate = DateTime.Now
            };
        }
    }
}
