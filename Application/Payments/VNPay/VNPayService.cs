using Data.EF.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Policy;
using ViewModel.Order;
using ViewModel.Payment;

namespace Application.Payments.VNPay
{
	public class VNPayService : IVNPayService
	{
		private readonly IConfiguration _config;
		public VNPayService(IConfiguration config)
		{
			_config = config;
		}
		public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
		{
			
			var vnpay = new VNPayLibrary();
			vnpay.AddRequestData("vnp_Version", _config["VNPay:vnp_Version"]);
			vnpay.AddRequestData("vnp_Command", _config["VNPay:vnp_Command"]);
			vnpay.AddRequestData("vnp_TmnCode", _config["VNPay:vnp_TmnCode"]);
			vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());

			vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
			vnpay.AddRequestData("vnp_CurrCode", _config["VNPay:vnp_CurrCode"]);
			vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
			vnpay.AddRequestData("vnp_Locale", _config["VNPay:vnp_Locale"]);
			vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + model.OrderId);
			vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
			vnpay.AddRequestData("vnp_ReturnUrl", _config["VNPay:return_Url"]);
			vnpay.AddRequestData("vnp_TxnRef", model.OrderId);
			var paymentUrl = vnpay.CreateRequestUrl(_config["VNPay:vnp_Url"], _config["VNPay:vnp_HashSecret"]);
			return paymentUrl;
		}
		public VnPaymentResponseModel PaymentExcute(IQueryCollection collections)
		{
			var vnpay = new VNPayLibrary();
			foreach (var (key, value) in collections)
			{
				if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
				{
					vnpay.AddResponseData(key, value.ToString());
				}
			}
			var vnpay_orderID = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
			long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
			string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
			string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
			string vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
			var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
			long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
			bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VNPay:vnp_HashSecret"]);
			if (!checkSignature)
			{
				return new VnPaymentResponseModel
				{
					Success = false,
				};
			}
			return new VnPaymentResponseModel
			{
				Success = true,
				PaymentMethod = "VnPay",
				OrderDescription = vnp_OrderInfo,
				OrderId = vnpay_orderID.ToString(),
				TransactionId = vnpayTranId.ToString(),
				Token = vnp_SecureHash,
				VnPayResponseCode = vnp_ResponseCode,
			};
		}


	}
}
