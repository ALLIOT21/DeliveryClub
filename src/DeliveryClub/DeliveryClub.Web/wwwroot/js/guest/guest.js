function toggleGrayBackground() {
    event.target.classList.toggle("gray-background");
}

function toggleGrayBackgroundParent() {
    event.target.classList.toggle("gray-background");
    event.target.parentNode.classList.toggle("gray-background");
}