$(document).on('click', '.cancel_order_button', function (e) {
    e.preventDefault();
    var id = $(this).data('order-id');
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#28a745",
        cancelButtonColor: "#d33",
        confirmButtonText: "Confirm",
        customClass: {
            confirmButton: 'confirmSave',
            cancelButton: 'confirmCancel'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Order/CancelOrder/' + id,
                type: 'PATCH',
                success: function (result) {
                    Swal.fire({
                        title: 'Canceled!',
                        text: 'Order has been canceled',
                        icon: 'success',
                        timer: 1500,
                    }).then(() => {
                        location.reload();
                    });
                },
                error: function (xhr, status, error) {
                    Swal.fire(
                        'Error!',
                        'There was an error canceling the order.',
                        'error'
                    );
                }
            });
        }
    });

    var confirmButton = document.querySelector('.confirmSave');
    var cancelButton = document.querySelector('.confirmCancel');

    if (confirmButton && cancelButton) {
        confirmButton.style.fontSize = '17px';
        cancelButton.style.fontSize = '17px';
    }
});

$(document).on('click', '#confirm_order', function (e) {
    e.preventDefault();
    Swal.fire({
        title: "Are you sure?",
        text: "Order will be created with selected items",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#28a745",
        cancelButtonColor: "#d33",
        confirmButtonText: "Confirm",
        customClass: {
            confirmButton: 'confirmSave',
            cancelButton: 'confirmCancel'
        }
    }).then((result) => {
        if (result.isConfirmed) {
            var formData = $('#confirm_order_form').serialize();
            $.ajax({
                url: '/Order/ConfirmOrder/',
                type: 'POST',
                data: formData,
                success: function (response) {
                    if (response.redirectUrl) {
                        Swal.fire({
                            title: 'Success!',
                            text: 'Order has been created',
                            icon: 'success',
                            timer: 1500,
                        }).then(() => {
                            window.location.href = response.redirectUrl;
                        });
                    } else {
                        Swal.fire(
                            'Error!',
                            'Unexpected response from server.',
                            'error'
                        );
                    }
                },
                error: function (xhr) {
                    if (xhr.status === 400) {
                        var errors = xhr.responseJSON.Errors;
                        var errorList = '<ul>';
                        $.each(errors, function (index, error) {
                            errorList += '<li>' + error + '</li>';
                        });
                        errorList += '</ul>';

                        Swal.fire(
                            'Error!',
                            'Fill the whole infirmation fields',
                            'error');
                    } else {
                        Swal.fire(
                            'Error!',
                            'There was an error creating the order.',
                            'error'
                        );
                    }
                }
            });
        }
    });

    var confirmButton = document.querySelector('.confirmSave');
    var cancelButton = document.querySelector('.confirmCancel');

    if (confirmButton && cancelButton) {
        confirmButton.style.fontSize = '17px';
        cancelButton.style.fontSize = '17px';
    }
});
