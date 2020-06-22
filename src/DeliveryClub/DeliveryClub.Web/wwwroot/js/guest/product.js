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