function SwitchTabsAndTables() {
    var adminTab = document.getElementById("administrator-tab");
    var dispatcherTab = document.getElementById("dispatcher-tab");
    var administrators = document.getElementById("administrators");
    var dispatchers = document.getElementById("dispatchers");

    adminTab.classList.toggle("active");
    dispatcherTab.classList.toggle("active");       
    administrators.classList.toggle("active");
    dispatchers.classList.toggle("active");    

    var createSpecialUserA = document.getElementById("createSpecialUser-a");

    if (adminTab.classList.contains("active"))
        createSpecialUserA.setAttribute("href", "/SuperUser/CreateAdmin");    
    else
        createSpecialUserA.setAttribute("href", "/SuperUser/CreateDispatcher");
}