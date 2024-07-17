
$(document).ready(function () {
    $('.btn-add-to-cart').click(function (event) {
        event.preventDefault();
        var productId = $(this).data('product-id');
        $.ajax({
            url: '/Cart/AddToCart',
            type: 'POST',
            data: { productId: productId, quantity: 1 },
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                    console.log(response.cart);
                    $('#cart-count').text(response.cart.items.length);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                toastr.error('Có lỗi xảy ra!');
            }
        });
    });
    $('.price-format').each(function () {
        var priceText = $(this).text(); // Lấy văn bản chứa giá tiền
        var price = parseFloat(priceText.replace(/\D/g, '')); // Lấy giá trị số từ văn bản và loại bỏ các ký tự không phải số
        if (!isNaN(price)) {
            var formattedPrice = price.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' }).replace('₫', ''); // Định dạng và loại bỏ ký tự ₫
            $(this).text(formattedPrice); // Thay đổi nội dung của phần tử thành giá trị đã định dạng
        }
    });
   
});
toastr.options = {
    "closeButton": true,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}
