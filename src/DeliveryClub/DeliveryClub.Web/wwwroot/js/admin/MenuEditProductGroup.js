function setEditModeMenu() {
    var divProductGroupTitle = findDivByElement("product-group-title");

    const toEditList = divProductGroupTitle.getElementsByClassName("to-edit");
    toggleVisibilityClassesInListMenu(toEditList);

    fillEditFieldsMenu(divProductGroupTitle);

    var divPortionPrices = divProductGroupTitle.getElementsByClassName("product-group-title-info-portion_prices")[0];
    var divPortionPricesButtons = createPortionPriceButtonsDiv(createPortionPricePair_Edit, deletePortionPricePair_Edit);

    divPortionPrices.appendChild(divPortionPricesButtons);
}

function setSaveModeMenu() {
    var divProductGroupTitle = findDivByElement("product-group-title");
    const toEditList = divProductGroupTitle.getElementsByClassName("to-edit");
    toggleVisibilityClassesInListMenu(toEditList);

    var portionPricesDiv = getPortionPricesDiv_Edit();
    deletePortionPriceDivs(portionPricesDiv);
    deletePortionPriceButtonsDiv(portionPricesDiv);
}

function findDivByElement(className) {
    var domObject = event.target;
    while (!domObject.classList.contains(className)) {
        domObject = domObject.parentNode;
    }
    return domObject;
}

function toggleVisibilityClassesInListMenu(list) {
    const listLength = list.length;
    for (let i = 0; i < listLength; i++)
    {
        var item = list.item(i);
        item.classList.toggle("is-visible");
        item.classList.toggle("non-visible");
    }
}

function fillEditFieldsMenu(currentDiv) {
    var name = currentDiv.getElementsByClassName("product-group-title-info-name")
    var nameInput = name[0].getElementsByTagName("input");
    var nameH2 = name[0].getElementsByTagName("h2");
    nameInput[0].value = nameH2[0].innerHTML;

    var portionPricesDiv = currentDiv.getElementsByClassName("product-group-title-info-portion_prices")[0];

    var portionPriceList = SetPortionPricesToEdit();
    for (let i = 0; i < portionPriceList.length; i++) {
        insertPortionPriceDivBeforeDiv(portionPriceList[i], portionPricesDiv, getPortionPriceButtonsDiv(getPortionPricesDiv_Edit()));
    }
}

function createPortionPricePair_Edit() {
    insertPortionPriceDivBeforeDiv(createPortionPriceDiv(findDivByElement("product-group-title")), getPortionPricesDiv_Edit(), getPortionPriceButtonsDiv(getPortionPricesDiv_Edit()));
}

function deletePortionPricePair_Edit() {
    var portionPriceDivList = getPortionPricesDiv_Edit();
    deletePortionPriceDiv(portionPriceDivList);
}

function deletePortionPriceButtonsDiv(portionPricesDiv) {
    getPortionPriceButtonsDiv(portionPricesDiv).remove();
}

function getPortionPriceButtonsDiv(portionPricesDiv) {
    var divButtons = portionPricesDiv.getElementsByClassName("div-buttons-portion_price");
    return divButtons[0];
}

function getPortionPricesDiv_Edit() {
    var divProductGroupTitle = findDivByElement("product-group-title");
    var divPortionPrices = divProductGroupTitle.getElementsByClassName("product-group-title-info-portion_prices");
    return divPortionPrices[0];
}

function SetPortionPricesToEdit() {
    var divPortionPrices = getPortionPricesDiv_Edit();
    
    var portionPriceList = divPortionPrices.getElementsByClassName("product-group-title-info-portion_price");
    var portionPricesToEdit = [];
    for (let i = 0; i < portionPriceList.length; i++) {
        var portion = portionPriceList[i].getElementsByClassName("span-edit-product_group-portion")[0];
        var price = portionPriceList[i].getElementsByClassName("span-edit-product_group-price")[0];
        var portionPriceDiv = createPortionPriceDiv(divPortionPrices, i, portion.innerHTML, price.innerHTML);
        
        portionPricesToEdit.push(portionPriceDiv);       
    }
    return portionPricesToEdit;
}

function deletePortionPriceDivs(currentDiv) {
    var portionPriceDivs = currentDiv.getElementsByClassName("div-portion-price");
    let portionPriceListLength = portionPriceDivs.length;
    for (let i = 0; i < portionPriceListLength; i++) {
        portionPriceDivs[0].remove();
    }
}
