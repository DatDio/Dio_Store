$(document).ready(function () {
    $('.quantity-input').change(function () {
        var productId = $(this).data('product-id');
        var newQuantity = parseInt($(this).val());

        if (newQuantity > 0) {
            updateCart(productId, newQuantity);
        } else {
            toastr.error('Số lượng phải lớn hơn 0!');
            $(this).val(1); // Reset value to 1 if input is invalid
        }
    });

    $('.remove-from-cart').click(function () {
        var productId = $(this).data('product-id');
        removeFromCart(productId);
    });

    function updateCart(productId, quantity) {
        $.ajax({
            url: '/Cart/UpdateCart',
            type: 'POST',
            data: { productId: productId, quantity: quantity },
            success: function (response) {
                if (response.success) {
                    var formattedPrice = formatCurrency(response.singlePrice); // Định dạng giá thành tiền tệ VND
                    $(`#total-price-${productId}`).html(`<span class="price-format">${formattedPrice}</span>`);
                    updateCartUI(response.cart);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('Có lỗi xảy ra!');
            }
        });
    }

    function removeFromCart(productId) {
        $.ajax({
            url: '/Cart/RemoveFromCart',
            type: 'POST',
            data: { productId: productId },
            success: function (response) {
                if (response.success) {
                    $(`.row-cart-index-${productId}`).remove();
                    $('#cart-count').text(response.cart.items.length);
                    updateCartUI(response.cart);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('Có lỗi xảy ra!');
            }
        });
    }

    function updateCartUI(cart) {
        $('#cart-count').text(cart.items.length);
        // Update total price and any other necessary UI elements
    }
    function formatCurrency(amount) {
        return amount.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' }).replace('₫', '');
    }
});
