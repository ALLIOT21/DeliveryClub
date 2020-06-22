﻿function addOrderView(order) {
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