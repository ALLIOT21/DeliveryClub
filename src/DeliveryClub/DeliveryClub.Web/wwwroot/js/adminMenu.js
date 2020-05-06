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
