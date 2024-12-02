
$("#main-price").change(function () {
    if (checkHasValue()) {
        calculatefunction();
    }
});

$("#additional-cost").change(function () {
    if (checkHasValue()) {
        calculatefunction();
    }
});

$("#discounted-cost").change(function () {
    if (checkHasValue()) {
        calculatefunction();
    }
});

$("#area-id-sender").change(function () {
    if (checkHasValue()) {
        calculatefunction();
    }
});

$.ajax({
    url: '/Admin/Values/GetTradersFullData',
    dataType: "json",
    success: function (Data) {
        if ($("#trader-edit-val").val()) {
            $(Data).each(function (index, value) {
                if (value.TraderId == $("#trader-edit-val").val()) {
                    $("#trader-Id").append($("<option selected data-value='" + value.AreaId + "' data-subtext='( " + value.AreaName + ")'></option>").val(value.TraderId).html(value.TraderName)).selectpicker('refresh');
                } else {
                    $("#trader-Id").append($("<option data-value='" + value.AreaId + "' data-subtext='( " + value.AreaName + ")'></option>").val(value.TraderId).html(value.TraderName)).selectpicker('refresh');
                }
            })
        } else {
            $(Data).each(function (index, value) {
                $("#trader-Id").append($("<option data-value='" + value.AreaId + "' data-subtext='( " + value.AreaName + ")'></option>").val(value.TraderId).html(value.TraderName)).selectpicker('refresh');
            })
        }
    },
    complete: function (Data) {
        if (checkHasValue()) {
            calculatefunction();
        }
    }
});

$('#trader-Id').on('change', function () {
    //const userId = $(this).find(":selected").val();
    //if (userId) {
    //    jQuery.ajax({
    //        type: 'GET',
    //        url: '/Admin/BaseOrders/GetUserPriceType?userId=' + userId,
    //        success: function (response) {
    //            console.log("response :" + response);
    //            if (response != "-1") {
    //                $('#trader-id-title').html("الزيادة : " + response);
    //            }
    //        }, complete: function (response) {
    //            calculatefunction();
    //        }
    //    });
    //} else {
    //    $('#trader-id-title').html("");
    //}
    const areaId = $('option:selected', this).attr('data-value');
    if (areaId) {
        $("#area-id-sender").val(areaId).selectpicker('refresh');
        if (checkHasValue()) {
            calculatefunction();
        }
    }
});

$('#area-id-receiver').on('change', function () {
    if ($(this).find(":selected").val()) {
        if (checkHasValue()) {
            calculatefunction();
        }
    }
});

function calculatefunction() {
    jQuery.ajax({
        type: 'POST',
        url: '/Admin/BaseOrders/CalculateOrderPrice',
        data: {
            "mainPrice": $('#main-price').val(),
            "additionalCost": $('#additional-cost').val(),
            "discountedCost": $('#discounted-cost').val(),
            "traderId": $('#trader-Id').val(),
            "areaIdReceiver": $('#area-id-receiver').val(),
            "areaIdSender": $('#area-id-sender').val()
        },
        success: function (response) {
            if (response) {
                if (response == "-1" || response == "0") {
                    $('#total-price-calculate').html("0");
                    $('#total-delivary-calculate').html("0");
                } else {
                    $('#total-price-calculate').html(response);
                    $('#total-delivary-calculate').html(parseFloat(response) - parseFloat($('#main-price').val()));
                }
            }
        }
    });
}

function checkHasValue() {

    console.log("tue");

    if ($('#main-price').val()
        && $('#trader-Id').val()
        && $('#area-id-receiver').val()
        && $('#area-id-sender').val()) {
        return true;
    } else {
        return false;
    }
}