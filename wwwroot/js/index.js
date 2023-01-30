$(document).ready(function () {
    var x = 0;
    var s = "";

    console.log("Hello pluralsight");

    var theForm = $("#theForm");
    theForm.hide();

    var buyButton = $("#buyButton");
    buyButton.on("click", function () {
        console.log("Buying item");
    });

    var productInfo = $(".product-props li");
    productInfo.on("click", function () {
        console.log("You clicked on " + $(this).text());
    });

    var $loginToggle = $("#loginToggle");
    var $popupForm = $(".popup-form");

    $loginToggle.on("click", function () {
        $popupForm.slideToggle(100);
    });
});
