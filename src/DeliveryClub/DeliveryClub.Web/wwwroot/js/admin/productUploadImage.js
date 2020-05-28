function onChangeImgInput() {
    getFileName(event.target);
    changeImage(event.target);    
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