function hideErrorMessage() {
    var errorMessage = $('#errorMessage');
    if (errorMessage.length > 0) {
        errorMessage.delay(3000).fadeOut(1000);
    }
}

function closeErrorMessage() {
    $(document).ready(function () {
        $(".close").click(function () {
            $("#errorMessage").alert('close');
        });
    });
}

function closeWishlistModal() {
    $(document).ready(function () {
        $('#wishlistModal').on('hidden.bs.modal', function () {
            $('#wishlistModalBody').empty();
        });

        $('.close').click(function () {
            $('#wishlistModal').modal('hide');
        });
    });
}

function closeBasketModal() {
    $(document).ready(function () {
        $('#basketModal').on('hidden.bs.modal', function () {
            $('#basketModalBody').empty();
        });

        $('.close').click(function () {
            $('#basketModal').modal('hide');
        });
    });
}

function closeProfileModal() {
    $(document).ready(function () {
        $('#profileModal').on('hidden.bs.modal', function () {
            $('#profileModalBody').empty();
        });

        $('.close').click(function () {
            $('#profileModal').modal('hide');
        });
    });
}

function getBasketFromWishlist() {
    $(document).ready(function () {
        $('#wishlistModal .btn-primary').click(function () {
            $('#wishlistModal').modal('hide');
            $('#wishlistModal').on('hidden.bs.modal', function () {
                $('#basketModal').modal('show');
            });
        });
    });

}