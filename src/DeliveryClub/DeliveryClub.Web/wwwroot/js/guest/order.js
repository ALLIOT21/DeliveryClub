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