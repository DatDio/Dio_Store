using ViewModel.Payment;

namespace Application.Payments.VNPay
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(HttpContext content, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExcute(IQueryCollection collections);
    }
}
