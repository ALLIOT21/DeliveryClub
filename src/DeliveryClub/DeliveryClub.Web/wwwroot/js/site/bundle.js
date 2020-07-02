"use strict"

function setEditModeIndex() {
    const toEditList = document.getElementsByClassName("to-edit");
    toggleVisibilityClassesInListIndex(toEditList);

    const checkboxDisabledList = document.getElementsByClassName("checkbox-disabled");
    setDisabledAttributeToList(checkboxDisabledList, false);

    enableImageInput();
    saveImageSrc();

    saveCheckboxValues(getSpecializationList());
    saveCheckboxValues(getPaymentList());

    fillEditFields();
}

function setSaveModeIndex() {
    const toEditList = document.getElementsByClassName("to-edit");
    toggleVisibilityClassesInListIndex(toEditList);

    const checkboxDisabledList = document.getElementsByClassName("checkbox-disabled");
    setDisabledAttributeToList(checkboxDisabledList, true);

    disableImageInput();
    returnImageSrc();

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

function saveImageSrc() {
    sessionStorage.setItem("image", document.getElementById("image").getAttribute("src"))
}

function returnImageSrc() {
    document.getElementById("image").setAttribute("src", sessionStorage.getItem("image")) 
}

function enableImageInput() {
    document.getElementById("image-input").removeAttribute("disabled");
}

function disableImageInput() {
    document.getElementById("image-input").removeAttribute("src");
    document.getElementById("image-label").innerHTML = "";
    document.getElementById("image-input").setAttribute("disabled", "disabled");
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

function createPortionPriceDiv(currentDiv, portion = "", price = 0, index = -1) {
    
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
        var portionPriceDiv = createPortionPriceDiv(divPortionPrices, portion.innerHTML, price.innerHTML, i);
        
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

function ChangeTabs1() {
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
function switchPrice() {
    var selectId = event.target.id;
    var optionId = $(`#${selectId} option:selected`).attr('id');
    
    var priceId = optionId.replace("portion", "price");
    var productDiv = findDivByElement("product-portion-price");

    var prices = productDiv.getElementsByClassName("price");

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
        url: `/api/productgroups/${sel.value}/portionpriced`,
        type: "GET",
        dataType: "json",
        success: function (data) {
            if (data) {
                hidePortionPrices();
            }
            else {
                if (documentHasPortionPrices()) 
                    showPortionPrices();                
                else
                    addPortionPrices();
            }
        }
        })
    return ajaxResult;
}

function getProductGroupName(){
    return document.getElementById("product-group");
}
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
            $('#image')
                .attr('src', e.target.result);
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
function showNotification(type, message) {
    switch (type) {
        case "Success":
            {
                iziToast.success({
                    title: message,
                    position: 'topRight'
                })
                break;
            };
        case "Error":        
            {
                iziToast.error({
                    title: message,
                    position: 'topRight'
                })
                break;
            };
        case "Warning":
            {                
                iziToast.warning({
                    title: message,
                    position: 'topRight'
                })      
                break;
            };
    }
}
function toggleGrayBackground() {
    event.target.classList.toggle("gray-background");
}

function toggleGrayBackgroundParent() {
    event.target.classList.toggle("gray-background");
    event.target.parentNode.classList.toggle("gray-background");
}
function Cart() {
    this.orders = [];
}

function addProductToCart() {
    let cart = getCart();
    if (cart == null) {
        cart = createCart();
    }

    if (cart != "empty")
        cart = JSON.parse(getCart());
    else
        cart = null;


    let order = null;
    let orderIndex;
    if (cart != null) {
        if (cart.orders != null) {
            var result = getOrder(cart.orders, getRestaurantId());
            if (result != null) {
                orderIndex = result.orderIndex;
                order = result.order;
            }
            else {
                order = createOrder(null, null);
            }
        }
        else {
            order = createOrder(null, null);
        }
    }
    else
    {
        cart = new Cart();
        order = createOrder(null, null);
    }

    if (order.restaurant == null) {
        order.restaurant = createRestaurant();
        addProductToOrderProducts(createProduct(), order.products);
        cart.orders.push(order);
        addOrderView(order);
    }
    else {
        let product = createProduct();
        if (!addingProductIsInCart(product, order.products, orderIndex)) {
            addProductToOrderProducts(product, order.products);
            addProductView(product, orderIndex);
        }
    }

    setCart(cart);
}

function createCart() {
    localStorage.setItem("delivery-club-cart", "empty");
}

function deleteCart() {
    localStorage.setItem("delivery-club-cart", "empty");
    var cartMain = getCartMain();
    while (cartMain.firstChild) {
        cartMain.removeChild(cartMain.firstChild)
    }
}

function getCart() {
    return localStorage.getItem("delivery-club-cart");
}

function setCart(cart) {
    localStorage.setItem("delivery-club-cart", JSON.stringify(cart));
}

function addingProductIsInCart(product, products, orderIndex) {
    for (let i = 0; i < products.length; i++) {
        if ((product.id == products[i].id) && (product.portion == products[i].portion)) {
            products[i].amount++;
            incrementProductView(products[i], i, orderIndex);
            return true;
        }
    }
    return false;
}

function fillCart() {
    let cart = getCart();
    if (cart != null) {
        cart = JSON.parse(cart);
        for (let i = 0; i < cart.orders.length; i++) {
            addOrderView(cart.orders[i]);
        }
    }
}
function menuIsActive() {
    let orderTimeBegin = document.getElementById("span_order-time-begin").innerHTML;
    let orderTimeEnd = document.getElementById("span_order-time-end").innerHTML;

    let addProductButtons = document.getElementsByClassName("btn btn-outline-danger");

    let currentTime = new Date();
    let currentTimeHours = currentTime.getHours();
    let currentTimeMinutes = currentTime.getMinutes();

    let currentTimeString = createTimeString(currentTimeHours, currentTimeMinutes)
    
    if ((currentTimeString > orderTimeBegin) && (currentTimeString < orderTimeEnd)) {
        
    }
    else {
        setButtonsDisabled(addProductButtons);
        swal("Closed", "The restaurant is closed now :(", "error");
    }
}

// return "12:28"
function createTimeString(hours, minutes) {
    return createDoubleDigitTimeValue(hours) + ':' + createDoubleDigitTimeValue(minutes);
}

function setButtonsDisabled(buttons) {
    console.log(buttons);
    for (let i = 0; i < buttons.length; i++) {
        buttons[i].setAttribute("disabled", "disabled");
    }
}

function createDoubleDigitTimeValue(timeValue) {
    if (timeValue < 10) {
        return "0" + timeValue;
    }
    return timeValue.toString();
}
function Order(restaurant, product) {
    this.restaurant = restaurant
    if (product != null) {
        this.products = [product];
    }
    else
        this.products = [];
}

function getOrder(orders, restId) {
    console.log(orders);
    for (let i = 0; i < orders.length; i++) {
        console.log(orders[i]);
        if (orders[i].restaurant.id == restId) {                
            var result = {
                orderIndex: i,
                order: orders[i],
            };
            console.log(result);
            return result;
        }
    }    
}

function createOrder(restaurant, product) {
    let order = new Order(restaurant, product);
    return order;
}
function Product(id, name, portion, price) {
    this.id = id;
    this.name = name;
    this.amount = 1;
    this.portion = portion;
    this.price = price;
}

function createProduct() {
    var productDiv = findDivByElement("user-restaurant-full_product");

    let productDivId = productDiv.id;

    let id = document.getElementById(productDivId + "_id").getAttribute("value");
    let name = document.getElementById(productDivId + "_name").innerHTML;

    let selectDivId = productDivId + "_select";
    let portion;
    let portionId;
    var selectDiv = document.getElementById(selectDivId);
    if (selectDiv != null) {
        portionId = $(`#${selectDivId} option:selected`).attr("id");
        portion = $(`#${selectDivId} option:selected`).html();
    }
    else {
        portion = $(`#${productDivId}_portion-0`).html();
        portionId = productDivId + "_portion-0";
    }

    let priceId = portionId.replace("portion", "price");
    let price = document.getElementById(priceId).innerHTML;
    price = price.substr(0, price.indexOf(" "));

    let product = new Product(id, name, portion, price);

    return product;
}

function getProduct(products) {
    
}

function addProductToOrderProducts(newProduct, productList) {
    productList.push(newProduct);
    return productList.length - 1;
}
function Restaurant(id, name, deliveryCost, minOrderPrice) {
    this.id = id;
    this.name = name;
    this.deliveryCost = deliveryCost;
    this.minOrderPrice = minOrderPrice;
}

function createRestaurant() {
    let id = getRestaurantId();
    let name = document.getElementById("h1_name").innerHTML;
    let deliveryCost = document.getElementById("span_delivery-cost").innerHTML;
    let minOrderPrice = document.getElementById("span_minimal-order-price").innerHTML;

    let restaurant = new Restaurant(id, name, deliveryCost, minOrderPrice);

    return restaurant;
}

function getRestaurantId() {
    return document.getElementById("restaurant-id").getAttribute("value");
}
function addOrderView(order) {
    var cartMain = getCartMain();
    cartMain.appendChild(createOrderView(order));
}

function addProductView(product, orderIndex) {
    var cartMain = getCartMain();
    var orderView = cartMain.getElementsByClassName("order")[orderIndex];

    var orderProducts = orderView.getElementsByClassName("order_products")[0];
    orderProducts.appendChild(createProductDiv(product));
}

function incrementProductView(product, productIndex, orderIndex) {
    var cartMain = getCartMain();
    var orderView = cartMain.getElementsByClassName("order")[orderIndex];
    var orderProducts = orderView.getElementsByClassName("order_products")[0];
    var productDiv = orderProducts.getElementsByClassName("products_product")[productIndex];

    var amountDiv = productDiv.getElementsByClassName("product_amount")[0];
    amountDiv.getElementsByTagName("span")[0].innerHTML++;

    var price = productDiv.getElementsByClassName("product-price")[0];
    console.log(price);
    let newPrice = product.price * product.amount;
    console.log(newPrice);
    price.innerHTML = `${newPrice} BYN`;
}

function getCartMain() {
    return $(`.cart-main`)[0];
}

function createOrderView(order) {
    var orderDiv = createDivWithClass("order");
    var orderDivRestaurant = createOrderDivRestaurant(order.restaurant);   
    var orderDivProducts = createOrderDivProducts(order.products);
    var orderDivDelivery = createOrderDivDelivery(order.restaurant.deliveryCost);

    orderDiv.appendChild(orderDivRestaurant);
    orderDiv.appendChild(orderDivProducts);
    orderDiv.appendChild(orderDivDelivery);  

    return orderDiv;
}

function createOrderDivRestaurant(restaurant) {
    var orderDivRestaurant = createDivWithClass("order_restaurant");
    var restaurantName = document.createElement("h4");
    restaurantName.innerHTML = restaurant.name;
    var restaurantId = document.createElement("input");
    restaurantId.type = "hidden";
    restaurantId.value = restaurant.id;

    orderDivRestaurant.appendChild(restaurantName);
    orderDivRestaurant.appendChild(restaurantId);

    return orderDivRestaurant;
}

function createOrderDivProducts(products) {
    var orderDivProducts = createDivWithClass("order_products");
    var productDiv;
    for (let i = 0; i < products.length; i++) {
        productDiv = createProductDiv(products[i]);
        orderDivProducts.appendChild(productDiv);
    }
    return orderDivProducts;
}

function createOrderDivDelivery(deliveryCost) {
    var orderDivDelivery = createDivWithClass("order_delivery");
    var deliveryTag = document.createElement("h6");
    deliveryTag.innerHTML = "Delivery";
    var deliveryCostSpan = document.createElement("span");
    deliveryCostSpan.innerHTML = deliveryCost + " BYN";

    orderDivDelivery.appendChild(deliveryTag);
    orderDivDelivery.appendChild(deliveryCostSpan);

    return orderDivDelivery;
}

function createProductDiv(product) {
    var productDiv = createDivWithClass("products_product");
    var productName = document.createElement("h6");
    productName.innerHTML = product.name;

    var productDivAmount = createDivWithClass("product_amount");

    var productDecrementAmount = document.createElement("div");
    productDecrementAmount.classList.toggle("btn-change-amount");
    productDecrementAmount.innerHTML = "-";

    var productAmount = document.createElement("span");
    productAmount.innerHTML = product.amount;

    var productIncrementAmount = document.createElement("div");
    productIncrementAmount.classList.toggle("btn-change-amount");
    productIncrementAmount.innerHTML = "+";    

    productDivAmount.appendChild(productDecrementAmount);
    productDivAmount.appendChild(productAmount);
    productDivAmount.appendChild(productIncrementAmount);

    var productPortion = document.createElement("span");
    productPortion.classList.toggle("product-portion");
    productPortion.innerHTML = product.portion;

    var productPrice = document.createElement("span");
    productPrice.classList.toggle("product-price");
    productPrice.innerHTML = `${product.price * product.amount} BYN`;

    productDiv.appendChild(productName);
    productDiv.appendChild(productDivAmount);
    productDiv.appendChild(productPortion);
    productDiv.appendChild(productPrice);        
    return productDiv;
}
function toggleActivity(id) {
    jQuery.ajax({
        url: `/api/dispatcher/${id}/active`,
        type: "POST",
        dataType: "json",
        success: function () {
            var checkbox = document.getElementById(`toggle-activity-${id}`);
            checkbox.setAttribute("checked", !checkbox.getAttribute("checked"));
        }
    })
}