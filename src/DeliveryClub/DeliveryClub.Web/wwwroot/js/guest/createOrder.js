function addCartToOrder() {
    var cart = getCart();
    cart = JSON.parse(cart);

    var form = document.getElementById("form-create-order");
    let orders = cart.orders;
    for (let i = 0; i < orders.length; i++) {
        var restaurantIdInput = createInput(`Orders[${i}].RestaurantId`, orders[i].restaurant.id);
        form.appendChild(restaurantIdInput);
        let products = orders[i].products;
        for (let j = 0; j < products.length; j++) {
            var productIdInput = createInput(`Orders[${i}].Products[${j}].Id`, products[j].id);
            var productAmountInput = createInput(`Orders[${i}].Products[${j}].Amount`, products[j].amount);
            var productPortionId = createInput(`Orders[${i}].Products[${j}].PortionId`, products[j].portionId);
            form.appendChild(productIdInput);
            form.appendChild(productAmountInput);
            form.appendChild(productPortionId);
        }
    }
    
    const hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/DispatcherNotification")
        .build();
    hubConnection.invoke("Send", "New order!");
    hubConnection.start();
    deleteCart();
    form.submit();
}

function createInput(name, value) {
    var input = document.createElement("input");
    input.type = "hidden";
    input.name = name;
    input.value = value;
    return input;
}