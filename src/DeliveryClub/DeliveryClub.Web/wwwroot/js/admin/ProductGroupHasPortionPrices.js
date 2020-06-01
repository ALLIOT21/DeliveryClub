function HasPortionPrices(sel) {
    let ajaxResult;
    jQuery.ajax({
        url: `/api/productgroups/${sel.value}/portionpriced`,
        type: "GET",
        dataType: "json",
        success: function (data) {
            if (data) {
                deletePortionPrices();
            }
            else {
                deletePortionPrices();
                addPortionPrices();
            }
        }
        })
    return ajaxResult;
}

function getProductGroupName()
{
    return document.getElementById("product-group");
}