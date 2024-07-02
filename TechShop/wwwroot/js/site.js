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