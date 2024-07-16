function updateItemsPerPage() {
    var pageSize = document.getElementById('itemsPerPage').value;
    var currentUrl = new URL(window.location.href);
    currentUrl.searchParams.set('PageNumber', 1);
    currentUrl.searchParams.set('PageSize', pageSize);
    window.location.href = currentUrl.href;
}

function updateSearchTerm(event) {
    event.preventDefault();
    var searchTerm = document.getElementById('searchTerm').value;
    var currentUrl = new URL(window.location.href);

    if (currentUrl.pathname.includes("GetAll")) {
        currentUrl.searchParams.set('searchTerm', searchTerm);
        window.location.href = currentUrl.href;
    } else {
        currentUrl.pathname = "Product/GetAll";
        currentUrl.searchParams.set('searchTerm', searchTerm);
        window.location.href = currentUrl.href;
    }
}

function updateSortOption() {
    var sortOption = document.getElementById('sortOptions').value;
    var currentUrl = new URL(window.location.href);

    if (currentUrl.pathname.includes("GetAll")) {
        currentUrl.searchParams.set('orderBy', sortOption);
    } else {
        currentUrl.pathname = "Product/GetAll";
        currentUrl.searchParams.set('orderBy', sortOption);
    }

    window.location.href = currentUrl.href;
}

function updateCategoryFilters() {
    var checkboxes = document.querySelectorAll('.form-check-input');
    var selectedCategories = [];

    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            selectedCategories.push(checkbox.value);
        }
    });

    var currentUrl = new URL(window.location.href);
    var baseUrl = currentUrl.origin + currentUrl.pathname;
    var searchParams = new URLSearchParams(currentUrl.search);

    searchParams.delete('categories');
    if (selectedCategories.length > 0) {
        var queryString = Array.from(searchParams.entries())
            .map(pair => `${pair[0]}=${pair[1]}`)
            .concat(`categories=${selectedCategories.join(',')}`)
            .join('&');
        window.location.href = `${baseUrl}?${queryString}`;
    } else {
        window.location.href = `${baseUrl}?${searchParams.toString()}`;
    }
}

document.addEventListener("DOMContentLoaded", function () {
    var currentUrl = new URL(window.location.href);

    var searchTerm = currentUrl.searchParams.get('searchTerm');
    if (searchTerm) {
        document.getElementById('searchTerm').value = searchTerm;
    }

    var sortOption = currentUrl.searchParams.get('orderBy');
    if (sortOption) {
        document.getElementById('sortOptions').value = sortOption;
    }

    var categoriesParam = currentUrl.searchParams.get('categories');
    if (categoriesParam) {
        var selectedCategories = categoriesParam.split(',');
        selectedCategories.forEach(function (category) {
            var checkbox = document.querySelector('.form-check-input[value="' + category.trim() + '"]');
            if (checkbox) {
                checkbox.checked = true;
            }
        });
    }

    var paginationLinks = document.querySelectorAll('.pagination a.page-link');
    paginationLinks.forEach(function (link) {
        link.addEventListener('click', function (event) {
            event.preventDefault();
            var pageUrl = new URL(link.href);

            currentUrl.searchParams.forEach(function (value, key) {
                if (!pageUrl.searchParams.has(key)) {
                    pageUrl.searchParams.set(key, value);
                }
            });

            window.location.href = pageUrl.href;
        });
    });
});


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