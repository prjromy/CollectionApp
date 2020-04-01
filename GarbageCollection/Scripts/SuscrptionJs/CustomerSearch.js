var _globalObject;

$(document).on("click", "#btn-customer-search", function (e) {
    debugger;
    e.stopImmediatePropagation();
    debugger
    var searchValue = $("#SearchParameter").val();
    var selectedOption = $("#SearchOption option:selected").val();
    var listBox = $(this).parents().find(".listBox").find(".multiselect").val();
    var mode = $("#Mode").val();
    var custType = $("#CustomerType").val();
   
    $.ajax({
        type: 'GET',
        url: '/Customer/_CustomerInfoList',
        data: {
            searchParam: searchValue,
            searchOption: selectedOption,
            listBox: listBox,
            mode: mode,
            custType: custType
        },
        traditional: true,
        success: function (result) {
            $("#customerDetails").html("");
            $("#customerDetails").html(result);
        },
    });

});
$(document).on("keypress", "#SearchParameter", function (e) {
    debugger;
    if (e.which == 13) {
        e.stopImmediatePropagation();
        debugger
        var searchValue = $("#SearchParameter").val();
        var selectedOption = $("#SearchOption option:selected").val();
        var listBox = $(this).parents().find(".listBox").find(".multiselect").val();
        var mode = $("#Mode").val();
        var custType = $("#CustomerType").val();

        $.ajax({
            type: 'GET',
            url: '/Customer/_CustomerInfoList',
            data: {
                searchParam: searchValue,
                searchOption: selectedOption,
                listBox: listBox,
                mode: mode,
                custType: custType
            },
            traditional: true,
            success: function (result) {
                $("#customerDetails").html("");
                $("#customerDetails").html(result);
            },
        });
    }
});

$(document).on('click', '.table-click-customer table tr', function (e) {
    debugger;
    e.stopImmediatePropagation();
    var closestTr = $(this).closest('tr');
    var objCheck = $(closestTr).find('.Ischeck');
    var isChecked = $(closestTr).find('.Ischeck').prop("checked");
    var listBox = $(this).parents().find(".listBox").find(".multiselect")
    var mode = $(this).parents().find("#Mode").val();
    var custType = $(this).parents().find("#CustomerType").val();
    
    var me = $(this);
    if (isChecked == true) {
        //  objCheck.prop('checked', false);
        //  $(closestTr).removeAttr('style');
        //  var customerName = $(closestTr).find('.Customername').text();

        //  var chosenValueName = $('select.multiselect option:contains("' + customerName + '")').remove()
        //var a=  listBox.chosen().trigger("chosen:updated");
        return;
    } else {

        var customerId = objCheck.val();
        var listBoxIDs = listBox.val();
        $.ajax({
            type: 'GET',
            url: '/Customer/GetSelectedCustomer',
            data: {
                customerId: customerId,
                listBox: listBoxIDs,
                mode: mode,
                custType: custType
            },
            traditional: true,
            success: function (result) {

            
                        //else if (mode == 'VerifiedRegisteredLoanList') {
                        //    debugger;


                        //    var customerName = $('#CustomerId').find(':selected').text();
                        //        $.ajax({
                        //            type: 'GET',
                        //            url: '/Credit/_VerifiedRegisteredLoanAccounts',
                        //            data: {
                        //                customerName: customerName,

                        //            },
                        //            success: function (result) {
                        //                debugger;
                        //                if (result.Success) {
                        //                    $.ajax({
                        //                        url: "/Credit/VerifiedRegisteredLoanAccountsIndex",
                        //                        type: 'GET',
                        //                        async: false,
                        //                        success: function (result) {
                        //                            debugger;
                        //                            $('section.content').html(result);
                        //                        }
                        //                    })
                        //                    //$('#registered-loan-accountnumber-list').html(result);
                        //                }

                        //            else{

                        //        }
                        //            },

                        //        });


                        //    objCheck.prop('checked', true);
                        //    $(closestTr).css('background-color', '#c2c4c3');
                        //    $(listBox).append('<option value="' + result.CID + '" selected>' + result.Name + '</option>');
                        //    $(listBox).trigger("chosen:updated");

                        //}
                if (":not(objCheck)") {
                    $(".table-click-customer table tr:not(objCheck)").prop('checked', false);
                    $(".table-click-customer table tr:not(objCheck)").css('background-color', '#ffffff');
                } 
                        objCheck.prop('checked', true);
                        $(closestTr).css('background-color', '#c2c4c3');
                        $('.CommonSearchDiv').find('#CustomerName').val("");
                        $('.cust-id').val("");
                        $('.CommonSearchDiv').find('#CustomerName').val(result.CustomerName);
                        $('.cust-id').val(result.CustNo);
                        //$('.customer-add').find('#CustId').append( result.Name );
                        //$(listBox).trigger("chosen:updated");

              


            },
        });
    }

});
$(document).on("click", ".addandClose", function (e) {
    debugger;
    e.stopImmediatePropagation();
    debugger;
    var Parent = $(this).parents();
    var id = _globalObject;
    //var closestTr = $('.table-click-customer table tr').closest('tr');
    //var objCheck = $('.table-click-customer table tr').find('.Ischeck');
    //var isChecked = $(closestTr).find('.Ischeck').prop("checked");
   
  
    var AccountName = "";
    var customerId = $('.cust-id').val();
    $.ajax({
        type: 'GET',
        url: '/Customer/GetMultipleSelectedCustomer',
        data: {
            listBox: customerId
        },
        traditional: true,
        success: function (result) {
            debugger;
            //$($(Parent).find(".multiselectCustomer#" + id + " option:selected")).each(function () {
            //    $("select.multiselectCustomer#" + id + " option:contains('" + $(this).text() + "')").remove();
            //    $(".multiselectCustomer#" + id).trigger("chosen:updated");
            //});
            //$(result).each(function (index, element) {
            //    $(multiSelectCustomer).append('<option value="' + element.CID + '" selected>' + element.Name + '</option>');
            //    $(multiSelectCustomer).trigger("chosen:updated");
            //    if (mode == 'AccountOpen' || mode == 'VerifiedRegisteredLoanList') {
            //        if (AccountName != '') {
            //            AccountName = AccountName + ',' + element.Name;
            //        } else {
            //            AccountName = element.Name;
            //        }

            //    }
            //});
            //if (mode == 'AccountOpen') {
            //    $(Parent).find("#Aname").val("");
            //    $(Parent).find("#Aname").val(AccountName);
            //}

            //if (mode == 'VerifiedRegisteredLoanList') {
            //    debugger;
            //    $(Parent).find("#Aname").val("");
            //    $(Parent).find("#Aname").val(AccountName);
               
            //       var customerName=AccountName;
            //    $.ajax({
            //        type: 'get',
            //        url: '/Credit/_VerifiedRegisteredLoanAccounts',
            //        data: {
            //            customerName: customerName,

            //        },
            //        success: function (result) {
            //            debugger;
                   

            //            if (result == null) {
            //                $('#verified-registered-loan').text("No Item to Display");
            //            }
            //            else {
            //                $('#verified-registered-loan').html(result);

            //            }
                          
                      

                    
            //        },

            //    });
            //}
            $('.CommonSearchDiv').find('#CustomerName').val("");
            $('.cust-id').val("");
            $('.CommonSearchDiv').find('#CustomerName').val(result.CustomerName);
            $('.cust-id').val(result.CustNo);
            $('#CustomerName').trigger('keyup');
            //}
        },
    });
    
    



});

$(' #CustomerName').on('keyup', function () {
    var mode = $("#btncustomersearch").attr("data-mode");
    
        var customerId = $('#CustId').val();
        $.ajax({
            type: 'GET',
            url: '/Customer/GetDetail',
            data: {
                customerId: customerId
            },

            success: function (result) {
                debugger;
               
                $('.customer-detail').html(result);

            },
        });
        if (mode == "Suscription") {
              $.ajax({
            type: 'GET',
            url: '/Suscription/List',
            data: {
                customerId: customerId,
               
            },

            success: function (result) {
                debugger;
               
                    $('.suscription-detail').html(result);
                
               
                 
                
            },
        });
        }
        if (mode == "Collection") {
            $.ajax({
                type: 'GET',
                url: '/Collection/CollectionEntry',
                data: {
                    customerId: customerId,

                },

                success: function (result) {
                    debugger;

                   $('.collection-detail').html(result);



                },
            });
        }
      
   
  
        


})
//$(document).ready(function () {

$(".btncustomersearch").on('click', function (e) {
    debugger;
    e.stopImmediatePropagation();
    var Parent = $(this).parents();
    var mode = $(this).attr("mode");
    var CustomerType = $(this).attr("customerType")
    var multiSelectCustomer = $(Parent).find(".multiselectCustomer");
    _globalObject = $(this).closest("div.CommonSearchDiv").find(".multiselectCustomer").attr("id");

    var multiSelectCustomerList = $(multiSelectCustomer).val();
    $('.img-container').hide();
    $('.view-history-image').hide();
    $.ajax({
        type: 'GET',
        url: "/Customer/CustomerInfoList",
        data: {
            listBox: multiSelectCustomerList,
            mode: mode,
            custType: CustomerType
        },
        traditional: true,
        success: function (result) {

            $('#pop-up-div').html(result).modal({
                'show': true,
                'backdrop': 'static'
            });

        },
        error: function () {

        }
    });



});
//});


//$(document).ready(function () {
//    debugger;
//    $(".btncustomersearch").on('click', function (e) {
//        e.stopImmediatePropagation();
//        var Parent = $(this).parents();
//        var mode = $(this).attr("mode");
//        var multiSelectCustomer = $(Parent).find(".multiselectCustomer");
//        _globalObject = $(this).closest("div.CommonSearchDiv").find(".multiselectCustomer").attr("id");

//        var multiSelectCustomerList = $(multiSelectCustomer).val();
//        $.ajax({
//            type: 'GET',
//            url: "/Customer/CustomerInfoList",
//            data: {
//                listBox: multiSelectCustomerList,
//                mode: mode
//            },
//            traditional: true,
//            success: function (result) {

//                $('#pop-up-div').html(result).modal({
//                    'show': true,
//                    'backdrop': 'static'
//                });

//            },
//            error: function () {

//            }
//        });
//    });
//});