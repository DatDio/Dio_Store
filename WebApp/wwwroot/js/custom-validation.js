﻿$(document).ready(function () {
    $("#Test-Click").click(function () {
        alert("Clicked");
    });
    $.extend($.validator.messages, {
        required: "Trường này là bắt buộc.",
        remote: "Vui lòng sửa trường này.",
        email: "Vui lòng nhập địa chỉ email hợp lệ.",
        url: "Vui lòng nhập URL hợp lệ.",
        date: "Vui lòng nhập ngày hợp lệ.",
        dateISO: "Vui lòng nhập ngày hợp lệ (ISO).",
        number: "Vui lòng nhập số hợp lệ.",
        digits: "Vui lòng chỉ nhập chữ số.",
        creditcard: "Vui lòng nhập số thẻ tín dụng hợp lệ.",
        equalTo: "Vui lòng nhập lại giá trị này.",
        maxlength: $.validator.format("Vui lòng không nhập quá {0} ký tự."),
        minlength: $.validator.format("Vui lòng nhập ít nhất {0} ký tự."),
        rangelength: $.validator.format("Vui lòng nhập giá trị có độ dài từ {0} đến {1} ký tự."),
        range: $.validator.format("Vui lòng nhập giá trị từ {0} đến {1}."),
        max: $.validator.format("Vui lòng nhập giá trị nhỏ hơn hoặc bằng {0}."),
        min: $.validator.format("Vui lòng nhập giá trị lớn hơn hoặc bằng {0}.")
    });
});


