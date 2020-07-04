(function createHubConnection() {
    var dispatcherNotificationHub = new signalR.HubConnectionBuilder()
                                        .withUrl("/DispatcherNotification")
                                        .build();

    dispatcherNotificationHub.on("ReceiveOrder", function (data) {
        showNotification("Success", data);
        ion.sound({
            sounds: [
                { name: "bell_ring" }
            ],
            path: "sounds/",
            preload: true,
            volume: 1.0
        });
        ion.sound.play("bell_ring");
    });

    dispatcherNotificationHub.start().then(() => {
        dispatcherNotificationHub.invoke("JoinGroup");
    });
}());