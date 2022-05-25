function archiveData() {
    isArchive = isArchive ? true : false;
}

getOrderStatus();

function getOrderStatus() {
    $.ajax({
        type: "POST",
        url: "/Admin/Values/GetOrderStatus",
        success: function (data) {
            $("#orderStatus-dshboard").html('');
            $.each(data, function (key, val) {
                $("#orderStatus-dshboard").append(`
                                        <span class='rounded shadow-sm border border-secondary m-badge--wide badge-status'>
                                           <label class="kt-checkbox kt-checkbox--bold kt-checkbox--dark">
                                               <input type="checkbox" class="check-status" value="`+ val.Id + `"> ` + val.TitleView + `
                                                    <span class="kt-badge kt-badge--brand">` + val.CountOrders + `</span>
                                               <span></span>
                                           </label>
                                        </span>
                                        ` );
            });
        },
        error: function (data) {

        }
    });
}

function columnfooterfunction() {
    var sum = 0;
    var sumDelivery = 0;
    $('input[name="total-price-value"]').each(function () {
        sum += parseInt($(this).val());
    });
    $('input[name="total-delivery-value"]').each(function () {
        sumDelivery += parseInt($(this).val());
    });
    console.log("sum : " + sum);
    console.log("sumDelivery : " + sumDelivery);

    var data = '<b class="mr-4">اجمالي التحصيل : <b style="color:#0a640b">' + sum + '</b> ₪</b>';
    data += '<b class="mr-4">اجمالي المنتجات : <b style="color:#0a640b">' + (sum - sumDelivery) + '</b>  ₪</b>';
    data += '<b class="mr-4">اجمالي التوصيل : <b style="color:#0a640b">' + sumDelivery + '</b> ₪</b>';

    $("#columnfooter").html(data);
}

$("#check-all-rows").change(function () {
    if (this.checked) {
        $(".check-row").each(function () {
            $(this).prop('checked', true);
        });
    }
    else {
        $(".check-row").each(function () {
            $(this).prop('checked', false);
        });
    }
});

$('#btn-print-billpolicy').click(function () {
    var rowSelectedIds = [];
    $(".check-row").each(function () {
        if (this.checked) {
            rowSelectedIds.push(parseInt($(this).attr("data-id")));
            var input = $("<input>").attr("type", "hidden").attr("name", "id").val(parseInt($(this).attr("data-id")));
            $('#form-billpolicy').append(input);
        }
    });
    if (rowSelectedIds.length <= 0) {
        return ShowMessage("w: يرجى اختيار طلب او اكثر");
    } else {
        $("#form-billpolicy").submit();
    }
});

$('#btn-print-billersalia').click(function () {
    var rowSelectedIds = [];
    $(".check-row").each(function () {
        if (this.checked) {
            rowSelectedIds.push(parseInt($(this).attr("data-id")));
            var input = $("<input>").attr("type", "hidden").attr("name", "id").val(parseInt($(this).attr("data-id")));
            $('#form-billersalia').append(input);
        }
    });
    if (rowSelectedIds.length <= 0) {
        return ShowMessage("w: يرجى اختيار طلب او اكثر");
    } else {
        $("#form-billersalia").submit();
    }
});

$('#btn-print-billtahsil').click(function () {
    var rowSelectedIds = [];
    $(".check-row").each(function () {
        if (this.checked) {
            rowSelectedIds.push(parseInt($(this).attr("data-id")));
        }
    });
    if (rowSelectedIds.length <= 0) {
        return ShowMessage("w: يرجى اختيار طلب او اكثر");
    } else {
        $('#PopUp-Custom').modal('show');
    }
});

$('#btn-tahsil-form').click(function () {
    if (!$('#trader-select-tahsil').val()) {
        return ShowMessage("w: يرجى اختيار التاجر");
    } else {
        printTahsil();
    }
});

function printTahsil() {
    var rowSelectedIds = [];
    $(".check-row").each(function () {
        if (this.checked) {
            rowSelectedIds.push(parseInt($(this).attr("data-id")));
            var input = $("<input>").attr("type", "hidden").attr("name", "id").val(parseInt($(this).attr("data-id")));
            $('#form-billtahsil').append(input);
        }
    });
    if (rowSelectedIds.length <= 0) {
        return ShowMessage("w: يرجى اختيار طلب او اكثر");
    } else {
        var inputTraderId = $("<input>").attr("type", "hidden").attr("name", "traderId").val($('#trader-select-tahsil').find(":selected").val());
        $('#form-billtahsil').append(inputTraderId);

        var inputNote = $("<input>").attr("type", "hidden").attr("name", "noteTahsil").val($('#note-tahsil').val());
        $('#form-billtahsil').append(inputNote);

        $("#form-billtahsil").submit();
        removeInputAppend("form-billtahsil");
    }
}

$("#change-orders-status").on("change", function () {
    if ($(this).val() == "-1") {
        return ShowMessage("w: يرجى اختيار حالة");
    }
    var rowSelectedIds = [];
    $(".check-row").each(function () {
        if (this.checked) {
            rowSelectedIds.push(parseInt($(this).attr("data-id")));
        }
    });
    if (rowSelectedIds.length <= 0) {
        $(this).val("-1").selectpicker('refresh');
        return ShowMessage("w: يرجى اختيار طلب او اكثر");
    } else {
        var orderStatusId = $(this).val()
        var url = "/Admin/BaseOrders/SetOrdersStatus";
        $.ajax({
            type: "POST",
            url: url,
            data: { id: rowSelectedIds, orderStatusId: orderStatusId },
            success: function (data) {
                if (data == "ok") {
                    dataSourceAjaxServer.init();
                    return ShowMessage("s: تم تغيير الحالة بنجاح");
                }
            },
            error: function (data) {
                return ShowMessage("e: عذرا!حدث خلل ما");
            },
            complete: function () {
                getOrderStatus();
            }
        });
    }
});

$("#set-driver-to-order").on("change", function () {
    if ($(this).val() == "-1") {
        return ShowMessage("w: يرجى اختيار تاجر");
    }
    var rowSelectedIds = [];
    $(".check-row").each(function () {
        if (this.checked) {
            rowSelectedIds.push(parseInt($(this).attr("data-id")));
        }
    });
    if (rowSelectedIds.length <= 0) {
        $(this).val("-1").selectpicker('refresh');
        return ShowMessage("w: يرجى اختيار طلب او اكثر");
    } else {
        var driverId = $(this).val()
        var url = "/Admin/BaseOrders/SetOrdersDriver";
        $.ajax({
            type: "POST",
            url: url,
            data: { id: rowSelectedIds, driverId: driverId },
            success: function (data) {
                if (data == "ok") {
                    dataSourceAjaxServer.init();
                    return ShowMessage("s: تم اضافة التاجر للطلبات بنجاح");
                }
            },
            error: function (data) {
                return ShowMessage("e: عذرا!حدث خلل ما");
            }
        });
    }
});

function removeInputAppend(formId) {
    $("form#" + formId + " : input").each(function () {
        var input = $(this);
        console.log("1");
    });
}