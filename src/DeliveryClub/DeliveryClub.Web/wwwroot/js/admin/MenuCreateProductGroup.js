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
