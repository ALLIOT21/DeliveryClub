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