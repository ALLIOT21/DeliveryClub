function onChangeImgInput() {
    if (isFileBig(event.target)) {
        hideFileSizeError();
        getFileName(event.target);
        changeImage(event.target);
    }
    else {
        showFileSizeError();
    }
}

function getFileName(fileInput) {
    let fileName = $(fileInput).val().split("\\").pop();
    $(fileInput).next('.custom-file-label').html(fileName);
    return $(fileInput).val();
}

function changeImage(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#product_image-img')
                .attr('src', e.target.result)
                .width(200)
                .height(200);
        };

        reader.readAsDataURL(input.files[0]);
    }    
}

function isFileBig(input) {
    if (input.files[0].size < 2097152) {
        return true;
    }
    return false;
}

function hideFileSizeError() {
    var errorLabel = document.getElementById("file-size-error");
    if (!errorLabel.classList.contains("non-visible")) {
        errorLabel.classList.add("non-visible");
    }
}

function showFileSizeError() {
    var errorLabel = document.getElementById("file-size-error");
    if (errorLabel.classList.contains("non-visible")) {
        errorLabel.classList.remove("non-visible");
    }
    
}