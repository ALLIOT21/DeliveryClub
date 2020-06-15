function showNotification(type, message) {
    switch (type) {
        case "Success":
            {
                iziToast.success({
                    title: message,
                    position: 'topRight'
                })
                break;
            };
        case "Error":        
            {
                iziToast.error({
                    title: message,
                    position: 'topRight'
                })
                break;
            };
        case "Warning":
            {                
                iziToast.warning({
                    title: message,
                    position: 'topRight'
                })      
                break;
            };
    }
}