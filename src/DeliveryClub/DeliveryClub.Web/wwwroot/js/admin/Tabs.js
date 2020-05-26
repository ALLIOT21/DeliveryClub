function ChangeTabs() {
    var pillsInfoTab = document.getElementById("pills-info-tab");
    var pillsMenuTab = document.getElementById("pills-menu-tab");
    var pillInfo = document.getElementById("pills-info");
    var pillMenu = document.getElementById("pills-menu");

    pillsInfoTab.classList.toggle("active");
    pillsMenuTab.classList.toggle("active");

    pillInfo.classList.toggle("show");
    pillInfo.classList.toggle("active");

    pillMenu.classList.toggle("show");
    pillMenu.classList.toggle("active");
}