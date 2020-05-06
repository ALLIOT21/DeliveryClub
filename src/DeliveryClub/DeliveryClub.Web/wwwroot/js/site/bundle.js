"use strict"

function setEditMode() {
    const toEditList = document.getElementsByClassName("to-edit");
    toggleVisibilityClassesInList(toEditList);

    const checkboxDisabledList = document.getElementsByClassName("checkbox-disabled");
    setDisabledAttributeToList(checkboxDisabledList, false);

    saveCheckboxValues(getSpecializationList());
    saveCheckboxValues(getPaymentList());

    fillEditFields();
}

function setSaveMode() {
    const toEditList = document.getElementsByClassName("to-edit");
    toggleVisibilityClassesInList(toEditList);

    const checkboxDisabledList = document.getElementsByClassName("checkbox-disabled");
    setDisabledAttributeToList(checkboxDisabledList, true);

    setCheckboxValues(getSpecializationList());
    setCheckboxValues(getPaymentList());
}

function toggleVisibilityClassesInList(list)
{
    const listLength = list.length;
    for (let i = 0; i < listLength; i++)
    {
        var item = list.item(i);
        item.classList.toggle("is-visible");
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

function CreateMenuGroupFormDiv() {
    var divCreateProductGroup = document.getElementById("div-create-product-group");
    divCreateProductGroup.classList.toggle("menu-content-create_group_active");

    var formCreateProductGroup = document.createElement("form");
    formCreateProductGroup.setAttribute("method", "post");
    formCreateProductGroup.action = "/Admin/CreateProductGroup";
    formCreateProductGroup.id = "form-create-product-group";

    var buttonCreateMenuGroup = document.getElementById("btn-create-menu-group");
    buttonCreateMenuGroup.classList.toggle("display-none");

    var divName = document.createElement("div");
    var divLabelName = document.createElement("div");
    var divInputName = document.createElement("div");
    var divButtonPortionPrice = document.createElement("div");
    var divButtonCreate = document.createElement("div");

    divName.classList.toggle("div-name");
    divLabelName.classList.toggle("div-name-label");
    divInputName.classList.toggle("div-name-input");
    divButtonPortionPrice.classList.toggle("div-button-portion_price");
    divButtonCreate.classList.toggle("div-button-create");
    divButtonPortionPrice.id = "div-button-portion_price";

    var labelInputName = document.createElement("label");
    labelInputName.setAttribute("for", "input-name");
    labelInputName.innerHTML = "Name: ";
    
    var inputName = document.createElement("input");
    inputName.id = "input-name";
    inputName.name = "Name";

    var buttonCreatePortionPrice = document.createElement("div");
    buttonCreatePortionPrice.classList.add("btn");
    buttonCreatePortionPrice.classList.add("btn-secondary");
    buttonCreatePortionPrice.innerHTML = "Create a portion-price pair";
    buttonCreatePortionPrice.onclick = CreatePortionPriceDiv;
    buttonCreatePortionPrice.form = "";

    var buttonCreate = document.createElement("button")
    buttonCreate.classList.add("btn");
    buttonCreate.classList.add("btn-primary");
    buttonCreate.id = "button-create";
    buttonCreate.innerHTML = "Create";
    buttonCreate.onclick = submitForm;    
    
    divLabelName.appendChild(labelInputName);
    divInputName.appendChild(inputName);
    divName.appendChild(divLabelName);
    divName.appendChild(divInputName);

    divButtonPortionPrice.appendChild(buttonCreatePortionPrice);
    divButtonCreate.appendChild(buttonCreate);

    formCreateProductGroup.appendChild(divName);
    formCreateProductGroup.appendChild(divButtonPortionPrice);
    formCreateProductGroup.appendChild(divButtonCreate);
    divCreateProductGroup.appendChild(formCreateProductGroup);
}

function CreatePortionPriceDiv() {
    let divPortionPriceNumber = document.getElementsByClassName("div-portion-price").length + 1;

    var divPortionPrice = document.createElement("div");
    divPortionPrice.classList.toggle("div-portion-price");

    var divPortion = document.createElement("div");   
    var divPortionLabel = document.createElement("div");   
    var divPortionInput = document.createElement("div");   

    var divPrice = document.createElement("div");
    var divPriceLabel = document.createElement("div");   
    var divPriceInput = document.createElement("div");   

    divPortion.classList.toggle("div-portion");
    divPrice.classList.toggle("div-price");

    var portionLabel = document.createElement("label");
    var portionInput = document.createElement("input");
    var priceLabel = document.createElement("label");
    var priceInput = document.createElement("input");

    portionLabel.id = "portion-label-" + divPortionPriceNumber;
    portionInput.id = "portion-input-" + divPortionPriceNumber;
    priceLabel.id = "price-label-" + divPortionPriceNumber;
    priceInput.id = "price-input-" + divPortionPriceNumber;

    portionInput.name = `PortionPrices[${divPortionPriceNumber - 1}].Portion`;
    priceInput.name = `PortionPrices[${divPortionPriceNumber - 1}].Price`;

    priceInput.type = "number";

    divPortionLabel.classList.toggle("div-portion-price-label");
    divPortionInput.classList.toggle("div-portion-price-input");
    divPriceLabel.classList.toggle("div-portion-price-label");
    divPriceInput.classList.toggle("div-portion-price-input");

    portionLabel.innerHTML = "Portion: ";
    priceLabel.innerHTML = "Price: ";

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
    
    var divButtonPortionPrice = document.getElementById("div-button-portion_price");
    var formCreateProductGroup = document.getElementById("form-create-product-group");
    formCreateProductGroup.insertBefore(divPortionPrice, divButtonPortionPrice);
}

function submitForm() {
    document.getElementById("form-create-product-group").submit();
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