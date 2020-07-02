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

    refreshCartOverallPrice(cart);
    refreshProductOverallAmount(cart);
    setCart(cart);
}

function createCart() {
    localStorage.setItem("delivery-club-cart", "empty");
}

function decrementProductAmount() {
    var productDiv = findDivByElement("products_product");
    var productId = productDiv.getElementsByTagName("input")[0].getAttribute("value");

    var cart = JSON.parse(getCart());
    decrementProductInCart(cart, productId);

    refreshCartOverallPrice(cart);
    refreshProductOverallAmount(cart);
    setCart(cart);
}

function incrementProductAmount() {
    var productDiv = findDivByElement("products_product");
    var productId = productDiv.getElementsByTagName("input")[0].getAttribute("value");

    var cart = JSON.parse(getCart());
    incrementProductInCart(cart, productId);

    refreshCartOverallPrice(cart);
    refreshProductOverallAmount(cart);
    setCart(cart);
}

function deleteCart() {
    localStorage.setItem("delivery-club-cart", "empty");
    var cartMain = getCartMain();
    while (cartMain.firstChild) {
        cartMain.removeChild(cartMain.firstChild)
    }

    document.getElementById("cart-product-overall-amount").innerHTML = 0;
    document.getElementById("cart-overall-price").innerHTML = "0 BYN";
}

function getCart() {
    return localStorage.getItem("delivery-club-cart");
}

function setCart(cart) {
    if (cart != "empty")
        localStorage.setItem("delivery-club-cart", JSON.stringify(cart));
    else
        localStorage.setItem("delivery-club-cart", "empty");
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
    if (cart != "empty") {
        if (cart != null) {
            cart = JSON.parse(cart);
            for (let i = 0; i < cart.orders.length; i++) {
                addOrderView(cart.orders[i]);
            }
        }
    }
    else {
        cart = null;
    }

    refreshCartOverallPrice(cart);
    refreshProductOverallAmount(cart);
}

function incrementProductInCart(cart, id) {
    for (let i = 0; i < cart.orders.length; i++) {
        for (let j = 0; j < cart.orders[i].products.length; j++) {
            if (cart.orders[i].products[j].id == id) {
                cart.orders[i].products[j].amount++;
                incrementProductView(cart.orders[i].products[j], j, i);
            }
        }
    }
}

function decrementProductInCart(cart, id) {
    for (let i = 0; i < cart.orders.length; i++) {
        let isProductFound = false;
        for (let j = 0; j < cart.orders[i].products.length; j++) {
            if (cart.orders[i].products[j].id == id) {
                cart.orders[i].products[j].amount--;
                if (cart.orders[i].products[j].amount == 0) {
                    cart.orders[i].products.splice(j, 1);
                    deleteProductView(i, j);
                    if (cart.orders[i].products.length == 0) {
                        cart.orders.splice(i, 1);
                        if (cart.orders.length == 0) {
                            cart = "empty";
                        }
                        deleteOrderView(i);
                    }
                }
                else {
                    decrementProductView(cart.orders[i].products[j], i, j);
                }
                isProductFound = true;   
                break;
            }
        }
        if (isProductFound) {
            break;
        }
    }
}

function refreshCartOverallPrice(cart) {
    let overallPrice = 0;
    if (cart != null) {
        let orders = cart.orders;
        if (orders != null) {
            for (let i = 0; i < orders.length; i++) {
                let products = orders[i].products;
                overallPrice += parseFloat(orders[i].restaurant.deliveryCost);
                for (let j = 0; j < products.length; j++) {
                    overallPrice += products[j].amount * parseFloat(products[j].price);
                }
            }
        }
    }

    document.getElementById("cart-overall-price").innerHTML = `${getPriceView(overallPrice)} BYN`;
}

function refreshProductOverallAmount(cart) {
    let overallAmount = 0;
    if (cart != null) { 
        let orders = cart.orders;
        if (orders != null) {
            for (let i = 0; i < orders.length; i++) {
                overallAmount += orders[i].products.length;
            }
        }
    }

    document.getElementById("cart-product-overall-amount").innerHTML = overallAmount.toString();
}

function placeAnOrder() {
    let cart = getCart(); 
    if (cart != "empty") {
        cart = JSON.parse(cart);
        let errorMessage = "";
        for (let i = 0; i < cart.orders.length; i++) {
            let orderProductPrice = 0;
            for (let j = 0; j < cart.orders[i].products.length; j++) {
                orderProductPrice += cart.orders[i].products[j].amount * cart.orders[i].products[j].price;
            }
            console.log(orderProductPrice);
            if (orderProductPrice < cart.orders[i].restaurant.minOrderPrice) {
                errorMessage += `Minimal order price in ${cart.orders[i].restaurant.name} is ${cart.orders[i].restaurant.minOrderPrice} BYN.`
            }
        }
        if (errorMessage != "")
            swal("You can't place an order", errorMessage, "error");
        else
            swal("Do you really want to place an order?", {
                buttons: {
                    no: {
                        "text": "No",
                        "value": "No"
                    },
                    yes: {
                        "text": "Yes",
                        "value": "Yes"
                    }
                }
            }).then(value => {
                switch (value) {
                    case "Yes": {
                        addRestaurantIds(cart, document.getElementById("form-create-order"));
                        document.getElementById("form-create-order").submit();
                        break;
                    }
                    case "No": {
                        break;
                    }
                }
            })
    }
    else
        swal("The cart is empty.", "", "error");
}
