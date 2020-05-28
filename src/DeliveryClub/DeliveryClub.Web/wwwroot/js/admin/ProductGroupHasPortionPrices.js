function HasPortionPrices(sel) {
    let ajaxResult;
    jQuery.ajax({
        url: `/api/Admin/HasPortionPrices?productGroupName=${sel.value}`,
        type: "GET",
        dataType: "json",
        success: function (data) {
            console.log(data)
        }
        })
    console.log(ajaxResult);
    return ajaxResult;
}

function getProductGroupName()
{
    return document.getElementById("product-group");
}