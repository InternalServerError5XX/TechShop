function closeUserModal() {
    $(document).ready(function () {
        $('#addUserModal').on('hidden.bs.modal', function () {
            $('#addUserModalBody').empty();
        });

        $('.close').click(function () {
            $('#addUserModal').modal('hide');
        });
    });
}

function closeUpdateUserModal() {
    $(document).ready(function () {
        $('#updateUserModal').on('hidden.bs.modal', function () {
            $('#updateUserModalBody').empty();
        });

        $('.close').click(function () {
            $('#updateUserModal').modal('hide');
        });
    });
}

function closeGetCategoryModal() {
    $(document).ready(function () {
        $('#getCategoryModal').on('hidden.bs.modal', function () {
            $('#getCategoryModalBody').empty();
        });

        $('.close').click(function () {
            $('#getCategoryModal').modal('hide');
        });
    });
}

function closeAddRoleModal() {
    $(document).ready(function () {
        $('#addRoleModal').on('hidden.bs.modal', function () {
            $('#addRoleModalBody').empty();
        });

        $('.close').click(function () {
            $('#addRoleModal').modal('hide');
        });
    });
}

$(document).on('click', '.delete_category_button', function (e) {
    e.preventDefault();
    var id = $(this).data('category-id');
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
                url: '/Product/DeleteCategory/' + id,
                type: 'DELETE',
                success: function (result) {
                    Swal.fire({
                        title: 'Deleted!',
                        text: 'Category has been deleted.',
                        icon: 'success',
                        timer: 1500,
                    }).then(() => {
                        location.reload();
                    });
                },
                error: function (xhr, status, error) {
                    Swal.fire(
                        'Error!',
                        'There was an error deleting the category.',
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

$(document).on('click', '.delete_role_button', function (e) {
    e.preventDefault();
    var id = $(this).data('role-id');
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
                url: '/Role/Delete/' + id,
                type: 'DELETE',
                success: function (result) {
                    Swal.fire({
                        title: 'Deleted!',
                        text: 'Role has been deleted.',
                        icon: 'success',
                        timer: 1500,
                    }).then(() => {
                        location.reload();
                    });
                },
                error: function (xhr, status, error) {
                    Swal.fire(
                        'Error!',
                        'There was an error deleting the category.',
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

$(document).on('click', '.delete_user_button', function (e) {
    e.preventDefault();
    var id = $(this).data('user-id');
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
                url: '/User/Delete/' + id,
                type: 'DELETE',
                success: function (result) {
                    Swal.fire({
                        title: 'Deleted!',
                        text: 'User has been deleted.',
                        icon: 'success',
                        timer: 1500,
                    }).then(() => {
                        location.reload();
                    });
                },
                error: function (xhr, status, error) {
                    Swal.fire(
                        'Error!',
                        'There was an error deleting the category.',
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

$(document).on('click', '.delete_order_button', function (e) {
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
                url: '/Order/DeleteOrder/' + id,
                type: 'DELETE',
                success: function (result) {
                    Swal.fire({
                        title: 'Deleted!',
                        text: 'Order has been deleted.',
                        icon: 'success',
                        timer: 1500,
                    }).then(() => {
                        location.reload();
                    });
                },
                error: function (xhr, status, error) {
                    Swal.fire(
                        'Error!',
                        'There was an error deleting the order.',
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

document.addEventListener('DOMContentLoaded', function () {
    var statsForm = document.getElementById('statsForm');

    statsForm.addEventListener('submit', function (event) {
        event.preventDefault();
        updateDateStats();
    });

    function updateDateStats() {
        var startDate = document.getElementById('startDate').value;
        var endDate = document.getElementById('endDate').value;
        var currentUrl = new URL(window.location.href);

        if (currentUrl.pathname.includes("GetOrdersStats")) {
            currentUrl.searchParams.set('start', startDate);
            currentUrl.searchParams.set('end', endDate);
            window.location.href = currentUrl.href;
        } else {
            var newUrl = new URL("/Admin/GetOrdersStats", window.location.origin);
            newUrl.searchParams.set('start', startDate);
            newUrl.searchParams.set('end', endDate);
            window.location.href = newUrl.href;
        }
    }

    var currentUrl = new URL(window.location.href);
    var startDateInput = currentUrl.searchParams.get('start');
    var endDateInput = currentUrl.searchParams.get('end');
    if (startDateInput) {
        document.getElementById('startDate').value = startDateInput;
    }
    if (endDateInput) {
        document.getElementById('endDate').value = endDateInput;
    }
});