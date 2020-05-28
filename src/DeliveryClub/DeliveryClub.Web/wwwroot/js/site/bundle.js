"use strict"

function setEditModeIndex() {
    const toEditList = document.getElementsByClassName("to-edit");
    toggleVisibilityClassesInListIndex(toEditList);

    const checkboxDisabledList = document.getElementsByClassName("checkbox-disabled");
    setDisabledAttributeToList(checkboxDisabledList, false);

    saveCheckboxValues(getSpecializationList());
    saveCheckboxValues(getPaymentList());

    fillEditFields();
}

function setSaveModeIndex() {
    const toEditList = document.getElementsByClassName("to-edit");
    toggleVisibilityClassesInListIndex(toEditList);

    const checkboxDisabledList = document.getElementsByClassName("checkbox-disabled");
    setDisabledAttributeToList(checkboxDisabledList, true);

    setCheckboxValues(getSpecializationList());
    setCheckboxValues(getPaymentList());
}

function toggleVisibilityClassesInListIndex(list)
{
    const listLength = list.length;
    for (let i = 0; i < listLength; i++)
    {
        var item = list.item(i);
        item.classList.toggle("is-visible");
        item.classList.toggle("inline");
        item.classList.toggle("non-visible");
    }
}

function setDisabledAttributeToList(list, value)
{
    for (let i = 0; i < list.length; i++)
    {
        var item = list.item(i);
        item.disabled = value;
    }
}

function saveCheckboxValues(list) {
    for (let i = 0; i < list.length; i++) {
        let l = list.item(i);
        if (l.checked) 
            sessionStorage.setItem(l.id, "checked");        
        else 
            sessionStorage.setItem(l.id, "unchecked");        
    }
}

function setCheckboxValues(list) {
    for (let i = 0; i < list.length; i++) {
        let l = list.item(i);
        var checked = sessionStorage.getItem(l.id);
        if (checked == "checked") {
            l.checked = true;
        }
        else
            l.checked = false;
    }
}

function getSpecializationList() {
    return document.getElementsByClassName("specialization");
}

function getPaymentList() {
    return document.getElementsByClassName("payment");
}

function fillEditFields() {
    document.getElementById("edit-name").value = document.getElementById("display-name").innerHTML;
    document.getElementById("edit-deliverycost").value = document.getElementById("display-deliverycost").innerHTML;
    document.getElementById("edit-minorderprice").value = document.getElementById("display-minorderprice").innerHTML;
    document.getElementById("edit-description").value = document.getElementById("display-description").innerHTML;
    document.getElementById("edit-deliverymaxtime").value = document.getElementById("display-deliverymaxtime").innerHTML;
    document.getElementById("edit-ordertimebegin").value = document.getElementById("display-ordertimebegin").innerHTML;
    document.getElementById("edit-ordertimeend").value = document.getElementById("display-ordertimeend").innerHTML;
}

function createMenuGroupFormDiv() {
    var divCreateProductGroup = getActiveDivCreateProductGroup();
    var formCreateProductGroup = createFormCreateProductGroup();
    hideButtonCreateMenuGroup();    

    var divName = createDivWithClass("div-name");
    var divLabelName = createDivWithClass("div-name-label");
    var divInputName = createDivWithClass("div-name-input");    
    var divButtonCreate = createDivWithClass("div-button-create");    

    var labelInputName = createLabelInputName();
    var inputName = createInputName();

    var portionPriceButtonsDiv = createPortionPriceButtonsDiv(createPortionPricePair_Create, deletePortionPricePair_Create);    

    var buttonCreate = createButtonCreate();   
    var buttonDecline = createButtonDecline();
    
    divLabelName.appendChild(labelInputName);
    divInputName.appendChild(inputName);
    divName.appendChild(divLabelName);
    divName.appendChild(divInputName);

    
    divButtonCreate.appendChild(buttonCreate);
    divButtonCreate.appendChild(buttonDecline);

    formCreateProductGroup.appendChild(divName);
    formCreateProductGroup.appendChild(portionPriceButtonsDiv);
    formCreateProductGroup.appendChild(divButtonCreate);
    divCreateProductGroup.appendChild(formCreateProductGroup);    
}

function createPortionPriceDiv(currentDiv, index = -1, portion = "", price = 0) {
    
    var divPortionPrice = createDivWithClass("div-portion-price");

    var divPortion = createDivWithClass("div-portion");   
    var divPortionLabel = createDivWithClass("div-portion-price-label");   
    var divPortionInput = createDivWithClass("div-portion-price-input"); 

    var divPrice = createDivWithClass("div-price");
    var divPriceLabel = createDivWithClass("div-portion-price-label"); 
    var divPriceInput = createDivWithClass("div-portion-price-input");   

    var portionLabel = document.createElement("label");
    var portionInput = document.createElement("input");
    var priceLabel = document.createElement("label");
    var priceInput = document.createElement("input");

    let divPortionPriceNumber;
    if (index == -1)
        divPortionPriceNumber = currentDiv.getElementsByClassName("div-portion-price").length;
    else
        divPortionPriceNumber = index;

    portionInput.name = `PortionPrices[${divPortionPriceNumber}].Portion`;
    priceInput.name = `PortionPrices[${divPortionPriceNumber}].Price`;
    portionInput.classList.toggle("form-control");
    priceInput.classList.toggle("form-control");

    priceInput.type = "number";
    
    portionLabel.innerHTML = "Portion: ";
    priceLabel.innerHTML = "Price: ";

    portionInput.value = portion;
    priceInput.value = price;
    priceInput.step = "any";

    divPortionLabel.appendChild(portionLabel);
    divPortionInput.appendChild(portionInput);
    divPriceLabel.appendChild(priceLabel);
    divPriceInput.appendChild(priceInput);

    divPortion.appendChild(divPortionLabel);
    divPortion.appendChild(divPortionInput);
    divPrice.appendChild(divPriceLabel);
    divPrice.appendChild(divPriceInput);

    divPortionPrice.appendChild(divPortion);
    divPortionPrice.appendChild(divPrice);

    return divPortionPrice;
}

function deletePortionPriceDiv(sourceDiv) {
    var divs = sourceDiv.getElementsByClassName("div-portion-price");
    var divsLength = divs.length;
    if (divsLength != 0) {
        divs[divsLength - 1].remove();
        var name = `[name="PortionPrices[${divsLength - 1}].Id"]`;
        var idInput = sourceDiv.querySelector(name);
        console.log(idInput);

        if (idInput != null) {
            idInput.remove();
        }        
    }
}

function submitForm() {
    document.getElementById("form-create-product-group").submit();
}

function createFormCreateProductGroup() {
    var formCreateProductGroup = document.createElement("form");
    formCreateProductGroup.setAttribute("method", "post");
    formCreateProductGroup.action = "/Admin/CreateProductGroup";
    formCreateProductGroup.id = "form-create-product-group";
    return formCreateProductGroup;
}

function createButtonCreatePortionPrice() {
    var buttonCreatePortionPrice = document.createElement("button");
    buttonCreatePortionPrice.classList.add("btn");
    buttonCreatePortionPrice.classList.add("btn-secondary");
    buttonCreatePortionPrice.classList.add("button-portion_price");
    buttonCreatePortionPrice.classList.add("button-portion_price-create");
    buttonCreatePortionPrice.innerHTML = "Create a portion-price pair";
    buttonCreatePortionPrice.setAttribute("form", "");
    return buttonCreatePortionPrice;
}

function createButtonDeletePortionPrice()
{
    var buttonDeletePortionPrice = document.createElement("button");
    buttonDeletePortionPrice.classList.add("btn");
    buttonDeletePortionPrice.classList.add("btn-secondary");
    buttonDeletePortionPrice.classList.add("button-portion_price");
    buttonDeletePortionPrice.classList.add("button-potrion_price-delete");
    buttonDeletePortionPrice.innerHTML = "Delete a portion-price pair";
    buttonDeletePortionPrice.setAttribute("form", "");
    return buttonDeletePortionPrice; 
}

function createButtonCreate()
{
    var buttonCreate = document.createElement("button")
    buttonCreate.classList.add("btn");
    buttonCreate.classList.add("btn-primary");
    buttonCreate.classList.add("button-form");
    buttonCreate.innerHTML = "Create";
    buttonCreate.onclick = submitForm;
    return buttonCreate;
}

function createButtonDecline() {
    var buttonDecline = document.createElement("button")
    buttonDecline.classList.add("btn");
    buttonDecline.classList.add("btn-danger");
    buttonDecline.classList.add("button-form");
    buttonDecline.innerHTML = "Decline";
    buttonDecline.onclick = declineCreateProductGroupForm;
    return buttonDecline; 
}

function createDivWithClass(divClass)
{
    var div = document.createElement("div");
    div.classList.toggle(divClass);
    return div;
}

function createLabelInputName() {
    var labelInputName = document.createElement("label");
    labelInputName.setAttribute("for", "input-name");
    labelInputName.innerHTML = "Name: ";
    return labelInputName;
}

function createInputName() {
    var inputName = document.createElement("input");
    inputName.id = "input-name";
    inputName.name = "Name";
    inputName.classList.toggle("form-control");
    return inputName;
}

function createPortionPriceButtonsDiv(onclickCreateFunc, onclickDeleteFunc) {
    var portionPriceButtonsDiv = createDivWithClass("div-buttons-portion_price");
    portionPriceButtonsDiv.id = "div-buttons-portion_price";

    var buttonCreatePortionPrice = createButtonCreatePortionPrice();
    var buttonDeletePortionPrice = createButtonDeletePortionPrice();

    setPortionPriceButtonsDivFuncs(buttonCreatePortionPrice, buttonDeletePortionPrice, onclickCreateFunc, onclickDeleteFunc);

    portionPriceButtonsDiv.appendChild(buttonCreatePortionPrice);
    portionPriceButtonsDiv.appendChild(buttonDeletePortionPrice);
    return portionPriceButtonsDiv;
}

function setPortionPriceButtonsDivFuncs(buttonCreate, buttonDelete, onclickCreateFunc, onclickDeleteFunc) {
    buttonCreate.onclick = onclickCreateFunc;
    buttonDelete.onclick = onclickDeleteFunc;    
}

function declineCreateProductGroupForm() {
    var formCreateProductGroup = document.getElementById("form-create-product-group");
    formCreateProductGroup.remove();

    var divCreateProductGroup = document.getElementById("div-create-product-group");
    divCreateProductGroup.classList.toggle("menu-content-create_group_active");

    var buttonCreateMenuGroup = document.getElementById("btn-create-menu-group");
    buttonCreateMenuGroup.classList.toggle("display-none");
}

function insertPortionPriceDivBeforeDiv(divPortionPrice, sourceDiv, nextDiv) {
    sourceDiv.insertBefore(divPortionPrice, nextDiv);
}

function appendPortionPriceDiv(divPortionPrice, sourceDiv) {
    sourceDiv.appendChild(divPortionPrice);
}

function getActiveDivCreateProductGroup() {
    var divCreateProductGroup = document.getElementById("div-create-product-group");
    divCreateProductGroup.classList.toggle("menu-content-create_group_active");
    return divCreateProductGroup;
}

function hideButtonCreateMenuGroup() {
    var buttonCreateMenuGroup = document.getElementById("btn-create-menu-group");
    buttonCreateMenuGroup.classList.toggle("display-none");
}

function createPortionPricePair_Create() {
    insertPortionPriceDivBeforeDiv(createPortionPriceDiv(document.getElementById("form-create-product-group")), document.getElementById("form-create-product-group"), document.getElementById("div-buttons-portion_price"));
}

function deletePortionPricePair_Create() {
    deletePortionPriceDiv(document.getElementById("form-create-product-group"));
}

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

function ChangeTabs() {
    var pillsInfoTab = document.getElementById("pills-info-tab");
    var pillsMenuTab = document.getElementById("pills-menu-tab");
    var pillInfo = document.getElementById("pills-info");
    var pillMenu = document.getElementById("pills-menu");

    pillsInfoTab.classList.toggle("active");
    pillsMenuTab.classList.toggle("active");

    pillInfo.classList.toggle("show");
    pillInfo.classList.toggle("active");

    pillMenu.classList.toggle("show");
    pillMenu.classList.toggle("active");
}
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

function switchPrice() {
    var selectId = event.target.id;
    var optionId = $(`#${selectId} option:selected`).attr('id');
    
    var priceId = optionId.replace("portion", "price");
    var productDiv = findDivByElement("product");

    var prices = productDiv.getElementsByClassName("price");
    console.log(prices);
    

    for (let i = 0; i < prices.length; i++) {
        if (prices[i].classList.contains("is-visible")) {
            prices[i].classList.toggle("non-visible");
            prices[i].classList.toggle("is-visible");  
        }
        if (prices[i].id == priceId)
        {
            prices[i].classList.toggle("non-visible");
            prices[i].classList.toggle("is-visible");            
        }        
    }
}


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