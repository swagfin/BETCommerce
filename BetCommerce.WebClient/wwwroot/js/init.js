$(document).ready(function () {
    $('.ui.rating').rating();
    $('.ui.dropdown').dropdown();
    $('.ui.loading').removeClass("loading");
    //Refresh Cart Count
    refreshCartItemsCount();
});
