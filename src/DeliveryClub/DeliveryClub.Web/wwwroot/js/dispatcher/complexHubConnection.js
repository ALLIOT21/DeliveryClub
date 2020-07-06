(function createHubConnection() {
    var dispatcherNotificationHub = new signalR.HubConnectionBuilder()
        .withUrl("/DispatcherNotification")
        .build();

    dispatcherNotificationHub.on("ReceiveOrder", function (notificationData) {
        notificationData = JSON.parse(notificationData);
        showNotification("Success", notificationData.Message);
        ion.sound({
            sounds: [
                { name: "bell_ring" }
            ],
            path: "/js/site/ion-sound/sounds/",
            preload: true,
            volume: 0.5
        });
        ion.sound.play("bell_ring");

        createDispatcherOrder(notificationData.DispatcherOrderModel);
    });

    dispatcherNotificationHub.start().then(() => {
        dispatcherNotificationHub.invoke("JoinGroup");
    });
}());

function createDispatcherOrder(dispatcherOrder) {
    var orderTableBody = document.getElementById("order-table-body");
    var tr = document.createElement("tr");
    var tdId = document.createElement("td");
    var tdName = document.createElement("td");
    var tdPhone = document.createElement("td");
    var tdDelivery = document.createElement("td");
    var tdOptions = document.createElement("td");

    var formDecline = document.createElement("button");
    formDecline.formAction = "/Dispatcher/SetOrderStatus";
    formDecline.formMethod = "post";
    var formAccept = document.createElement("button");
    formAccept.formAction = "/Dispatcher/SetOrderStatus";
    formAccept.formMethod = "post";
    var inputIdDecline = document.createElement("input");
    inputIdDecline.name = "Id";
    inputIdDecline.value = dispatcherOrder.Id;
    inputIdDecline.type = "hidden";
    var inputStatusDecline = document.createElement("input");
    inputStatusDecline.name = "OrderStatus";
    inputStatusDecline.value = "Declined";
    inputStatusDecline.type = "hidden";
    var inputBtnDecline = document.createElement("button");
    inputBtnDecline.classList.toggle("btn");
    inputBtnDecline.classList.toggle("btn-danger");
    inputBtnDecline.innerHTML = "Decline";
    var inputIdAccept = document.createElement("input");
    inputIdAccept.name = "Id";
    inputIdAccept.value = dispatcherOrder.Id;
    inputIdAccept.type = "hidden";
    var inputStatusAccept = document.createElement("input");
    inputStatusAccept.name = "OrderStatus";
    inputStatusAccept.value = "Accepted";
    inputStatusAccept.type = "hidden";
    var inputBtnAccept = document.createElement("button");
    inputBtnAccept.classList.toggle("btn");
    inputBtnAccept.classList.toggle("btn-success");
    inputBtnAccept.innerHTML = "Accept";

    formDecline.appendChild(inputIdDecline);
    formDecline.appendChild(inputStatusDecline);
    formDecline.appendChild(inputBtnDecline);
    formAccept.appendChild(inputIdAccept);
    formAccept.appendChild(inputStatusAccept);
    formAccept.appendChild(inputBtnAccept);

    tdOptions.appendChild(formDecline);
    tdOptions.appendChild(formAccept);

    tdId.innerHTML = dispatcherOrder.Id;
    tdName.innerHTML = dispatcherOrder.Name;
    tdPhone.innerHTML = dispatcherOrder.PhoneNumber;
    tdDelivery.innerHTML = dispatcherOrder.DeliveryAddress;

    tr.appendChild(tdId);
    tr.appendChild(tdName);
    tr.appendChild(tdPhone);
    tr.appendChild(tdDelivery);
    tr.appendChild(tdOptions);

    tr.classList.toggle("new-order");

    orderTableBody.prepend(tr);
}