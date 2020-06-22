function menuIsActive() {
    let orderTimeBegin = document.getElementById("span_order-time-begin").innerHTML;
    let orderTimeEnd = document.getElementById("span_order-time-end").innerHTML;

    let addProductButtons = document.getElementsByClassName("btn btn-outline-danger");

    let currentTime = new Date();
    let currentTimeHours = currentTime.getHours();
    let currentTimeMinutes = currentTime.getMinutes();

    let currentTimeString = createTimeString(currentTimeHours, currentTimeMinutes)
    
    if ((currentTimeString > orderTimeBegin) && (currentTimeString < orderTimeEnd)) {
        
    }
    else {
        setButtonsDisabled(addProductButtons);
        swal("Closed", "The restaurant is closed now :(", "error");
    }
}

// return "12:28"
function createTimeString(hours, minutes) {
    return createDoubleDigitTimeValue(hours) + ':' + createDoubleDigitTimeValue(minutes);
}

function setButtonsDisabled(buttons) {
    console.log(buttons);
    for (let i = 0; i < buttons.length; i++) {
        buttons[i].setAttribute("disabled", "disabled");
    }
}

function createDoubleDigitTimeValue(timeValue) {
    if (timeValue < 10) {
        return "0" + timeValue;
    }
    return timeValue.toString();
}