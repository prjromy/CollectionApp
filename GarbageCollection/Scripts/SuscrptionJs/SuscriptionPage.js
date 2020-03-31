﻿
function getPageSizeChangeData(url, container, me) {
    debugger;
    var parent = $(me).closest("#paging");
    var pageSize = $(me).val();
   
    var elementRowCount = container.find("#pageSize");   
    var pageSize = parseInt(elementRowCount.val());
    var eror = false;
    if (pageSize < 1) {
        eror = true;
        pageSize = 5;
    }
    var controller = $(parent).attr('controller');
    var action = $(parent).attr('action');
    var currentUrl = '/' + controller + '/' + action;

    if (url != "") {
        currentUrl = currentUrl + url + "&pageNo=" + 1 + "&pageSize=" + pageSize;
    } else {
        currentUrl = currentUrl + "?pageNo=" + 1 + "&pageSize=" + pageSize;

    }
    $.ajax({
        type: "GET",
        url: currentUrl,
        dataType: "Html",
        success: function (data) { SuccessCall(data, pageNo, pageSize, container); },
        error: function (xhr, status, error) { alert(error); },

    });

    function SuccessCall(data, pageNo, pageSize, container) {
        debugger;
        $(container).html('');
        if (eror == true) {
            $(container).append(data).find('#erormsgpagesize').show();

        } else {
            $(container).append(data).find('#erormsgpagesize').hide();

        }
        $(me).val(pageSize);

    }
}

function getPageNumberChangeData(url, container,me,eventValue) {
    debugger;
    var parent = $(me).closest("#paging");
    var erorDivClass = $(parent).find('#erormsg').attr('class');
    var elementCurrentPage = container.parents().find("#pageNo");
    var elementRowCount = container.parents().find("#pageSize");
    var pageCount = $(parent).attr('pageCount');

    var controller = $(parent).attr('controller');
    var action = $(parent).attr('action');
    var currentUrl = '/' + controller + '/' + action;
    var ele;
    var pageSize = parseInt(elementRowCount.val())
    var eror = false;
    var pageNo = parseInt(elementCurrentPage.val());
    if (pageNo > pageCount || pageNo < 1) {
        eror = true;
        pageNo = 1;

    }
    if (url != "") {
        currentUrl = currentUrl + url + "&pageNo=" + pageNo + "&pageSize=" + pageSize;
    } else {
        currentUrl = currentUrl + "?pageNo=" + pageNo + "&pageSize=" + pageSize;

    }
    $.ajax({
        type: "GET",
        url: currentUrl,
        dataType: "Html",
        success: function (data) { SuccessCall(data, container); },
        error: function (xhr, status, error) { alert(error); },
    });
    function SuccessCall(data, container) {
        debugger;
        $(container).html('');
        if (eror == true) {
            $(container).append(data).find('#erormsg').show();

        } else {
            $(container).append(data).find('#erormsg').hide();

        }

    }


}
////for all button////

function getPageNumberBtnAction(url, container, me) {
    debugger;
    var parent = $(me).closest("#paging");
    var id = $(me).attr('id');

    var controller = $(parent).attr('controller');
    var action = $(parent).attr('action');
    var pageCount = $(parent).attr('pageCount');
    var currentUrl = '/' + controller + '/' + action;
    var elementCurrentPage = container.find("#pageNo");
    var elementRowCount = container.find("#pageSize");
    var currentPageNo = 1;
    if (id == "btnNxt") {
        currentPageNo = parseInt(elementCurrentPage.val()) + 1;

    }
    else if (id == "btnPrev") {
        currentPageNo = parseInt(elementCurrentPage.val()) - 1;
    }
    else if (id == "btnFirst") {
        currentPageNo = 1;
    }
    else if (id == "btnLast") {
        currentPageNo = pageCount;
    }
    var pageSize = parseInt(elementRowCount.val())
    if (url != "") {
        currentUrl = currentUrl + url + "&pageNo=" + currentPageNo + "&pageSize=" + pageSize;
    } else {
        currentUrl = currentUrl + "?pageNo=" + currentPageNo + "&pageSize=" + pageSize;

    }

    $.ajax({
        type: "GET",
        url: currentUrl,
        dataType: "Html",
        traditional: true,
        success: function (data) { SuccessCall(data, container); },
        error: function (xhr, status, error) { alert(error); },
    });
    function SuccessCall(data, container) {
        $(container).html('');
        $(container).append(data);
        // $('#search').val(search);
    }
}


//for CustomerSearch
function getPageNumberBtnActionCustomerSearch(searchOption, searchParam, container, me, mode, custType) {
    debugger;
    var parent = $(me).closest("#paging");
    var id = $(me).attr('id');

    var controller = $(parent).attr('controller');
    var action = $(parent).attr('action');
    var pageCount = $(parent).attr('pageCount');
    var currentUrl = '/' + controller + '/' + action;
    var elementCurrentPage =$(container).find("#pageNo");
    var elementRowCount =$(container).find("#pageSize");
    var currentPageNo = 1;
    if (id == "btnNxt") {
        currentPageNo = parseInt(elementCurrentPage.val()) + 1;

    }
    else if (id == "btnPrev") {
        currentPageNo = parseInt(elementCurrentPage.val()) - 1;
    }
    else if (id == "btnFirst") {
        currentPageNo = 1;
    }
    else if (id == "btnLast") {
        currentPageNo = pageCount;
    }
    var pageSize = parseInt(elementRowCount.val())
    $.ajax({
        type: "GET",
        url: currentUrl,
        data: {
            searchParam: searchParam,
            searchOption: searchOption,
           
            mode: mode,
            custType: custType,
            pageNo: currentPageNo,
            pageSize: pageSize
        },
        dataType: "Html",
        traditional: true,
        success: function (data) { SuccessCall(data, container); },
        error: function (xhr, status, error) { alert(error); },
    });
    function SuccessCall(data, container) {
        $(container).html('');
        $(container).append(data);
    }
}

function getPageSizeChangeDataCustomerSearch(searchOption, searchParam, container, me, mode,type) {
    debugger;
    var parent = $(me).closest("#paging");
    var pageSize = $(me).val();
    var eror = false;
    if (pageSize < 1) {
        eror = true;
        pageSize = 5;
    }
    var controller = $(parent).attr('controller');
    var action = $(parent).attr('action');
    var currentUrl = '/' + controller + '/' + action;

   
    $.ajax({
        type: "GET",
        url: currentUrl,
        data: {
            searchParam: searchParam,
            searchOption: searchOption,
           
            mode: mode,
            custType: type,
            pageNo: 1,
            pageSize: pageSize
        },
        traditional: true,
        dataType: "Html",
        success: function (data) { SuccessCall(data, pageNo, pageSize, container); },
        error: function (xhr, status, error) { alert(error); },

    });

    function SuccessCall(data, pageNo, pageSize, container) {
        debugger;
        $(container).html('');
        if (eror == true) {
            $(container).append(data).find('#erormsgpagesize').show();

        } else {
            $(container).append(data).find('#erormsgpagesize').hide();

        }
        $(me).val(pageSize);

    }
}

function getPageNumberChangeDataCustomerSearch(searchOption, searchParam, container, me, mode,type) {
    debugger;
    var parent = $(me).closest("#paging");
    var erorDivClass = $(parent).find('#erormsg').attr('class');
    var elementCurrentPage = container.find("#pageNo");
    var elementRowCount = container.find("#pageSize");
    var pageCount = $(parent).attr('pageCount');

    var controller = $(parent).attr('controller');
    var action = $(parent).attr('action');
    var currentUrl = '/' + controller + '/' + action;
    var ele;
    var pageSize = parseInt(elementRowCount.val())
    var eror = false;
    var pageNo = parseInt(elementCurrentPage.val());
    if (pageNo > pageCount || pageNo < 1) {
        eror = true;
        pageNo = 1;

    }
    $.ajax({
        type: "GET",
        url:currentUrl,
        data: {
            searchParam: searchParam,
            searchOption: searchOption,
            
            mode: mode,
            custType: type,
            pageNo: pageNo,
            pageSize: pageSize
        },
        traditional: true,
        dataType: "Html",
        success: function (data) { SuccessCall(data, container); },
        error: function (xhr, status, error) { alert(error); },
    });
    function SuccessCall(data, container) {
        $(container).html('');
        if (eror == true) {
            $(container).append(data).find('#erormsg').show();

        } else {
            $(container).append(data).find('#erormsg').hide();

        }

    }


}