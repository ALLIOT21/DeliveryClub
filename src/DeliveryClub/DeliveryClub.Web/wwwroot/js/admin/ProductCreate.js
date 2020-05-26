function addPortionPriceDiv(portionPriceDivButtons) {
    var portionPricesDiv = getPortionPricesDiv_Product();
    var portionPriceDiv = createPortionPriceDiv(portionPricesDiv);
    insertPortionPriceDivBeforeDiv(portionPriceDiv, portionPricesDiv, portionPriceDivButtons);
}

function addPortionPriceButtonsDiv(portionPriceDivButtons) {
    var portionPricesDiv = getPortionPricesDiv_Product();
    portionPricesDiv.appendChild(portionPriceDivButtons);
    return portionPriceDivButtons;
}

function getPortionPricesDiv_Product() {
    return document.getElementsByClassName("portion-prices")[0];
}

function addPortionPrices() {
    var portionPriceDivButtons = createPortionPriceButtonsDiv(createPortionPricePair_Product, deletePortionPricePair_Product);
    addPortionPriceButtonsDiv(portionPriceDivButtons);
    addPortionPriceDiv(portionPriceDivButtons);
}

function createPortionPricePair_Product() {
    var portionPricesDiv = getPortionPricesDiv_Product();
    insertPortionPriceDivBeforeDiv(createPortionPriceDiv(portionPricesDiv), portionPricesDiv, getPortionPriceButtonsDiv(portionPricesDiv));
}

function deletePortionPricePair_Product() {
    deletePortionPriceDiv(getPortionPricesDiv_Product());
}
