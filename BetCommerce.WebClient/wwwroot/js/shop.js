function addItemToCart(itemId, element) {
    $(element).addClass("loading");
    $.ajax({
        type: "GET",
        url: "/cart/additem/" + itemId,
        success: function (msg) {
            toastr.success(msg);
        },
        error: function (xhr, status, error) {
            console.log(error);
            toastr.error("An rrror occurred while adding item to cart : Error: " + error);
        },
        complete: function (data) {
            $(element).removeClass("loading");
            refreshCartItemsCount();
        }

    });
}
function removeItemFromCart(itemId, element) {
    if (!confirm('Are you sure you want to remove this item from your shopping cart ?')) { return false; }
    $(element).addClass("loading");
    $.ajax({
        type: "GET",
        url: "/cart/removeitem/" + itemId,
        success: function (msg) {
            toastr.success(msg);
            location.reload();
        },
        error: function (xhr, status, error) {
            console.log(error);
            toastr.error("An rrror occurred while trying to remove item from cart : Error: " + error);
        },
        complete: function (data) {
            refreshCartItemsCount();
        }
    });
}
function refreshCartItemsCount() {
    $.ajax({
        type: "GET",
        url: "/cart/getcartcount/",
        success: function (msg) {
            $(".shopping-cart-count").html(msg);
        },
        error: function (xhr, status, error) {
            console.log(error);
        },
        complete: function (data) {
        }
    });
}
