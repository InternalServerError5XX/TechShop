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
