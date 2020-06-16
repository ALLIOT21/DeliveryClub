"use strict"

function setEditModeIndex() {
    const toEditList = document.getElementsByClassName("to-edit");
    toggleVisibilityClassesInListIndex(toEditList);

    const checkboxDisabledList = document.getElementsByClassName("checkbox-disabled");
    setDisabledAttributeToList(checkboxDisabledList, false);

    enableImageInput();
    saveImageSrc();

    saveCheckboxValues(getSpecializationList());
    saveCheckboxValues(getPaymentList());

    fillEditFields();
}

function setSaveModeIndex() {
    const toEditList = document.getElementsByClassName("to-edit");
    toggleVisibilityClassesInListIndex(toEditList);

    const checkboxDisabledList = document.getElementsByClassName("checkbox-disabled");
    setDisabledAttributeToList(checkboxDisabledList, true);

    disableImageInput();
    returnImageSrc();

    setCheckboxValues(getSpecializationList());
    setCheckboxValues(getPaymentList());
}

function toggleVisibilityClassesInListIndex(list)
{
    const listLength = list.length;
    for (let i = 0; i < listLength; i++)
    {
        var item = list.item(i);
        item.classList.toggle("is-visible");
        item.classList.toggle("inline");
        item.classList.toggle("non-visible");
    }
}

function setDisabledAttributeToList(list, value)
{
    for (let i = 0; i < list.length; i++)
    {
        var item = list.item(i);
        item.disabled = value;
    }
}

function saveCheckboxValues(list) {
    for (let i = 0; i < list.length; i++) {
        let l = list.item(i);
        if (l.checked) 
            sessionStorage.setItem(l.id, "checked");        
        else 
            sessionStorage.setItem(l.id, "unchecked");        
    }
}

function setCheckboxValues(list) {
    for (let i = 0; i < list.length; i++) {
        let l = list.item(i);
        var checked = sessionStorage.getItem(l.id);
        if (checked == "checked") {
            l.checked = true;
        }
        else
            l.checked = false;
    }
}

function saveImageSrc() {
    sessionStorage.setItem("image", document.getElementById("image").getAttribute("src"))
}

function returnImageSrc() {
    document.getElementById("image").setAttribute("src", sessionStorage.getItem("image")) 
}

function enableImageInput() {
    document.getElementById("image-input").removeAttribute("disabled");
}

function disableImageInput() {
    document.getElementById("image-input").removeAttribute("src");
    document.getElementById("image-label").innerHTML = "";
    document.getElementById("image-input").setAttribute("disabled", "disabled");
}

function getSpecializationList() {
    return document.getElementsByClassName("specialization");
}

function getPaymentList() {
    return document.getElementsByClassName("payment");
}

function fillEditFields() {
    document.getElementById("edit-name").value = document.getElementById("display-name").innerHTML;
    document.getElementById("edit-deliverycost").value = document.getElementById("display-deliverycost").innerHTML;
    document.getElementById("edit-minorderprice").value = document.getElementById("display-minorderprice").innerHTML;
    document.getElementById("edit-description").value = document.getElementById("display-description").innerHTML;
    document.getElementById("edit-deliverymaxtime").value = document.getElementById("display-deliverymaxtime").innerHTML;
    document.getElementById("edit-ordertimebegin").value = document.getElementById("display-ordertimebegin").innerHTML;
    document.getElementById("edit-ordertimeend").value = document.getElementById("display-ordertimeend").innerHTML;
}
