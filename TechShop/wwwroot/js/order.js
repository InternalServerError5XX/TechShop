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
