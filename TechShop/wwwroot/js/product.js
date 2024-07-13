function updateItemsPerPage() {
            var pageSize = document.getElementById('itemsPerPage').value;
            var currentUrl = new URL(window.location.href);
            currentUrl.searchParams.set('PageNumber', 1);
            currentUrl.searchParams.set('PageSize', pageSize);
            window.location.href = currentUrl.href;
}

function setActiveSlide(index) {
    var carousel = new bootstrap.Carousel(document.querySelector('#productCarousel'));
    carousel.to(index);
}

function closeCategoryModal() {
    $(document).ready(function () {
        $('#addCategoryModal').on('hidden.bs.modal', function () {
            $('#categoryModalBody').empty();
        });

        $('.close').click(function () {
            $('#addCategoryModal').modal('hide');
        });
    });
}

$(document).on('click', '.delete_product_button', function (e) {
    e.preventDefault();
    var id = $(this).data('product-id');
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
                url: '/Product/Delete/' + id,
                type: 'DELETE',
                success: function (result) {
                    Swal.fire({
                        title: 'Deleted!',
                        text: 'Product has been deleted.',
                        icon: 'success',
                        timer: 1500,
                    }).then(() => {
                        if (window.location.href.includes('GetById')) {
                            window.location.href = '/Admin/AdminPanel';
                            localStorage.setItem('activeTab', "nav-products-tab");
                        } else {
                            location.reload();
                        }
                    });
                },
                error: function (xhr, status, error) {
                    Swal.fire(
                        'Error!',
                        'There was an error deleting the product.',
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