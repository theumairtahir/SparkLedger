var cartItemsPs;
function blockPage(color, text) {
    KTApp.blockPage({
        overlayColor: '#000000',
        state: color,
        message: text
    });
}
function unblockUI() {
    KTApp.unblockPage();
}
function showSearchDropDown(toggle, dropdown, wrapper) {
    if (!KTUtil.hasClass(dropdown, 'show')) {
        $(toggle).dropdown('toggle');
        $(toggle).dropdown('update');
        $(wrapper).show();
    }
}
function hideSearchDropDown(toggle, dropdown, wrapper) {
    if (KTUtil.hasClass(dropdown, 'show')) {
        $(toggle).dropdown('toggle');
        $(wrapper).hide();
    }
}
function showSearchSpinner() {
    KTLayoutSearch().showSearchProgress();
}
function hideSearchSpinner() {
    KTLayoutSearch().hideSearchProgress();
}
function processSearch() {
    KTLayoutSearch().processSearch();
}
function handleSearch() {
    KTLayoutSearch().handleSearch();
}

function addClass(e, c) {
    $(e).addClass(c + '');
}

function removeClass(e, c) {
    $(e).removeClass(c, '');
}

function show(e) {
    $(e).show();
}

function hide(e) {
    $(e).hide();
}

function disableFormSubmit(e) {
    $(e).on('keyup keypress', function (e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode === 13) {
            e.preventDefault();
            return false;
        }
    });
}
function checkAll(table, selectAll) {
    var e = $(table);
    $('td input:checkbox', e).prop('checked', selectAll.checked);
    $(table).on('change', 'td input:checkbox', function () {
        if ($(this).is(":checked")) {
            var isAllChecked = 0;

            $(".checkSingle").each(function () {
                if (!this.checked)
                    isAllChecked = 1;
            });

            if (isAllChecked == 0) {
                $("#checkedAll").prop("checked", true);
            }
        }
        else {
            $("#checkedAll").prop("checked", false);
        }
    });
}

function loadMainSalesChart(element, data, curSymbol) {
    //var element = document.getElementById("kt_mixed_widget_6_chart");
    data = JSON.parse(data);
    var height = parseInt(KTUtil.css(element, 'height'));

    if (!element) {
        return;
    }

    var options = {
        series: data.series,
        chart: {
            type: 'bar',
            height: height,
            toolbar: {
                show: false
            },
            sparkline: {
                enabled: true
            },
        },
        plotOptions: {
            bar: {
                horizontal: false,
                columnWidth: ['30%'],
                endingShape: 'rounded'
            },
        },
        legend: {
            show: false
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            show: true,
            width: 1,
            colors: ['transparent']
        },
        xaxis: {
            categories: data.xLabels,
            axisBorder: {
                show: false,
            },
            axisTicks: {
                show: false
            },
            labels: {
                style: {
                    colors: KTApp.getSettings()['colors']['gray']['gray-500'],
                    fontSize: '12px',
                    fontFamily: KTApp.getSettings()['font-family']
                }
            }
        },
        yaxis: {
            min: data.min,
            max: data.max,
            labels: {
                style: {
                    colors: KTApp.getSettings()['colors']['gray']['gray-500'],
                    fontSize: '12px',
                    fontFamily: KTApp.getSettings()['font-family']
                }
            }
        },
        fill: {
            type: ['solid', 'solid'],
            opacity: [0.5, 1]
        },
        states: {
            normal: {
                filter: {
                    type: 'none',
                    value: 0
                }
            },
            hover: {
                filter: {
                    type: 'none',
                    value: 0
                }
            },
            active: {
                allowMultipleDataPointsSelection: false,
                filter: {
                    type: 'none',
                    value: 0
                }
            }
        },
        tooltip: {
            style: {
                fontSize: '12px',
                fontFamily: KTApp.getSettings()['font-family']
            },
            y: {
                formatter: function (val) {
                    return curSymbol + " " + val;
                }
            },
            marker: {
                show: false
            }
        },
        colors: data.colors,
        grid: {
            borderColor: KTApp.getSettings()['colors']['gray']['gray-200'],
            strokeDashArray: 4,
            yaxis: {
                lines: {
                    show: true
                }
            },
            padding: {
                left: 20,
                right: 20
            }
        }
    };

    var chart = new ApexCharts(element, options);
    chart.render();
}

function showConfirmAlert(title, message, confirmButtonText, cancelButtonText, callback, cancelCallback = "") {
    Swal.fire({
        title: title,
        text: message,
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: confirmButtonText,
        reverseButtons: true,
        cancelButtonText: cancelButtonText,
    }).then(function (result) {
        if (result.isConfirmed) {
            invokeMethod(callback);
        }
        else {
            if (cancelCallback && cancelCallback != "") {
                DotNet.invokeMethodAsync('StoreManagerApp', callback);
            }
        }
    });
}

function invokeMethod(method) {
    DotNet.invokeMethodAsync('StoreManagerApp', method);
}

function showSuccessMessage(title, message) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": false,
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
    };

    toastr.success(message, title);
}
function showWarningMessage(title, message) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": false,
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
    };

    toastr.warning(message, title);
}
function showErrorMessage(title, message) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": false,
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
    };

    toastr.error(message, title);
}

function addFormValidation(form, validationFields) {
    FormValidation.formValidation(
        form,
        {
            fields: validationFields,

            plugins: {
                trigger: new FormValidation.plugins.Trigger(),
                // Bootstrap Framework Integration
                bootstrap: new FormValidation.plugins.Bootstrap(),
                // Validate fields when clicking the Submit button
                submitButton: new FormValidation.plugins.SubmitButton(),
                // Submit the form when all fields are valid
                defaultSubmit: new FormValidation.plugins.DefaultSubmit(),
            }
        }
    );
}
function showModal(e) {
    $(e).modal('show');
}
function hideModal() {
    $(e).modal('hide');
}
function isFormValid(e) {
    var flag = $(e).valid();
    return flag;
}

function onFormSubmit(e, callback) {
    $(e).on('submit', function () {
        e.preventDefault();
        invokeMethod(callback);
        return false;
    });
}