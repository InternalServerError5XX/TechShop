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