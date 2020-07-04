(function createHubConnection() {
    var dispatcherNotificationHub = new signalR.HubConnectionBuilder()
        .withUrl("/DispatcherNotification")
        .build();

    dispatcherNotificationHub.on("ReceiveOrder", function (data) {
        
    });

    dispatcherNotificationHub.start().then(() => {
        dispatcherNotificationHub.invoke("JoinGroup");
    });
}());