function switchPrice() {
    var selectId = event.target.id;
    var optionId = $(`#${selectId} option:selected`).attr('id');
    
    var priceId = optionId.replace("portion", "price");
    var productDiv = findDivByElement("product");

    var prices = productDiv.getElementsByClassName("price");
    console.log(prices);
    

    for (let i = 0; i < prices.length; i++) {
        if (prices[i].classList.contains("is-visible")) {
            prices[i].classList.toggle("non-visible");
            prices[i].classList.toggle("is-visible");  
        }
        if (prices[i].id == priceId)
        {
            prices[i].classList.toggle("non-visible");
            prices[i].classList.toggle("is-visible");            
        }        
    }
}

