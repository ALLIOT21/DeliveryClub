function addPortionPriceDiv(portionPriceDiv, portionPricesDiv, portionPriceDivButtons) {
    insertPortionPriceDivBeforeDiv(portionPriceDiv, portionPricesDiv, portionPriceDivButtons);
}

function addPortionPriceButtonsDiv(portionPricesDiv, portionPriceDivButtons) {
    portionPricesDiv.appendChild(portionPriceDivButtons);
    return portionPriceDivButtons;
}

function addPortionPrices() {
    var portionPricesDiv = createDivWithClass("portion-prices");
    portionPricesDiv.classList.toggle("form-group");

    var portionPriceDivButtons = createPortionPriceButtonsDiv(createPortionPricePair_Product, deletePortionPricePair_Product);
    addPortionPriceButtonsDiv(portionPricesDiv, portionPriceDivButtons);

    var portionPriceDiv = createPortionPriceDiv(portionPricesDiv);
    addPortionPriceDiv(portionPriceDiv, portionPricesDiv, portionPriceDivButtons);
       
    var formCreateProduct = document.getElementById("form-product"); 
    formCreateProduct.insertBefore(portionPricesDiv, document.getElementsByClassName("product-uploading-image")[0]);

    SetPortionPricedProductGroupProperty("false");
}

function deletePortionPrices() {
    var portionPricesDiv = getPortionPricesDiv();
    if (portionPricesDiv != null) {
        portionPricesDiv.remove();
    }
}

function getPortionPricesDiv() {
    return document.getElementsByClassName("portion-prices")[0]
}

function hidePortionPrices() {
    var portionPricesDiv = getPortionPricesDiv();
    if (!portionPricesDiv.classList.contains("non-visible"))
        portionPricesDiv.classList.add("non-visible");

    SetPortionPricedProductGroupProperty("true");
}

function showPortionPrices() {
    var portionPricesDiv = getPortionPricesDiv();
    if (portionPricesDiv.classList.contains("non-visible"))
        portionPricesDiv.classList.remove("non-visible");

    SetPortionPricedProductGroupProperty("false");
}

function documentHasPortionPrices() {
    var portionPricesDiv = getPortionPricesDiv();
    if (portionPricesDiv) 
        return true;    
    return false;
}

function createPortionPricePair_Product() {
    var portionPricesDiv = getPortionPricesDiv();
    insertPortionPriceDivBeforeDiv(createPortionPriceDiv(portionPricesDiv), portionPricesDiv, getPortionPriceButtonsDiv(portionPricesDiv));
}

function deletePortionPricePair_Product() {
    deletePortionPriceDiv(getPortionPricesDiv());
}

function SetPortionPricedProductGroupProperty(value) {
    if (document.querySelector(`[name="ProductGroupPortionPriced"]`))
        document.querySelector(`[name="ProductGroupPortionPriced"]`).setAttribute("value", value);
}