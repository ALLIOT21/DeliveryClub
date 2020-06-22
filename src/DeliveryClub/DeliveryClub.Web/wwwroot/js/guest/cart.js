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