(function createHubConnection() {
    const dispatcherNotificationHub = new signalR.HubConnectionBuilder()
                                        .withUrl("/DispatcherNotification")
                                        .build();

    dispatcherNotificationHub.on("ReceiveOrder", function (data) {
        showNotification("Success", data);
        ion.sound({
            sounds: [
                { name: "bell_ring" }
            ],
            path: "/js/site/ion-sound/sounds/",
            preload: true,
            volume: 0.3
        });
        ion.sound.play("bell_ring");
        console.log("tut");
    });

    dispatcherNotificationHub.start().then(() => {
        dispatcherNotificationHub.invoke("JoinGroup");
    });
}());