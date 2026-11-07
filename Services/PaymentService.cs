using System;
using CinemaManagement.Models;
using CinemaManagement.Data;

namespace CinemaManagement.Services
{
    public class PaymentService
    {
        /// <summary>
        /// Xử lý tính toán tiền thừa cho giao dịch
        /// </summary>
        public decimal CalculateChange(decimal totalAmount, decimal cashGiven)
        {
            if (cashGiven < totalAmount)
            {
                throw new InvalidOperationException("Số tiền khách đưa không đủ để thanh toán.");
            }
            return cashGiven - totalAmount;
        }

        /// <summary>
        /// Thực hiện giao dịch thanh toán giả lập (luôn thành công) và lưu vào CSDL
        /// </summary>
        public bool ProcessPayment(int bookingId, decimal totalAmount, decimal cashGiven, string paymentMethod = "Tiền mặt")
        {
            try
            {
                decimal change = CalculateChange(totalAmount, cashGiven);

                using var context = new CinemaDbContext();
                var payment = new Payment
                {
                    BookingId = bookingId,
                    Amount = totalAmount,
                    CashGiven = cashGiven,
                    ChangeReturned = change,
                    PaymentMethod = paymentMethod,
                    Status = "Success",
                    PaymentDate = DateTime.Now
                };

                context.Payments.Add(payment);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi xử lý thanh toán: " + ex.Message);
            }
        }
    }
}