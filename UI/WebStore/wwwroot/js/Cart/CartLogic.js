Cart = {
    _properties: {
        getCartViewLink: "",
        addToCartLink: "",
        removeFromCartLink: "",
        decrementItemLink: "",
        incrementItemLink: ""
    },
    init: function (properties) {
        $.extend(Cart._properties, properties);
        $(".add-to-cart").click(Cart.addToCart);
        $(".cart_quantity_up").click(Cart.incrementItem);
        $(".cart_quantity_down").click(Cart.decrementItem);
        $(".cart_quantity_delete").click(Cart.removeFromCart);
    },
    
    addToCart: function (event) {
        event.preventDefault();
        var button = $(this);
        const id = button.data("id");
        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function (response){
                Cart.showToolTip(button);
                Cart.refreshCartView();
                console.log(response.message);
            })
            .fail(function (){console.log("addToCart failed");});
    },
    decrementItem: function (event) {
        event.preventDefault();
        var button = $(this);
        const id = button.data("id");
        var tr = button.closest("tr");
        $.get(Cart._properties.decrementItemLink + "/" + id)
            .done(function (response) {
                const count = parseInt($(".cart_quantity_input",tr).val());
                if(count > 1)
                {
                    $(".cart_quantity_input",tr).val(count - 1);
                    Cart.refreshPrice(tr);
                }
                else
                {
                    tr.remove();
                    Cart.refreshTotalPrice();
                }
                Cart.refreshCartView();
                console.log(response.message);
            })
            .fail(function (){console.log("decrementItem failed");});
    },
    incrementItem: function (event) {
        event.preventDefault();
        var button = $(this);
        const id = button.data("id");
        var tr = button.closest("tr");
        $.get(Cart._properties.incrementItemLink + "/" + id)
            .done(function (response) {
                const count = parseInt($(".cart_quantity_input",tr).val());
                $(".cart_quantity_input",tr).val(count + 1);
                Cart.refreshPrice(tr);
                Cart.refreshCartView();
                console.log(response.message);
            })
            .fail(function (){console.log("decrementItem failed");});
    },
    removeFromCart: function (event) {
        event.preventDefault();
        var button = $(this);
        const id = button.data("id");
        $.get(Cart._properties.removeFromCartLink + "/" + id)
            .done(function (response) {
                button.closest("tr").remove();
                Cart.refreshTotalPrice();
                Cart.refreshCartView();
                console.log(response.message);
            })
            .fail(function (){console.log("remove failed");});
    },
    
    showToolTip: function (button) {
        button.tooltip({title: "Added to Cart"}).tooltip("show");
        setTimeout(function (){
            button.tooltip("destroy");
        },500);
    },
    
    refreshCartView: function () {
        $.get(Cart._properties.getCartViewLink)
            .done(function (cartHtml){
                $("#cart-container").html(cartHtml);
            })
            .fail(function () {console.log("refreshCartView failed");});
    },
    refreshPrice: function (tr) {
        const count = parseInt($(".cart_quantity_input",tr).val());
        const price = parseFloat($(".cart_price",tr).data("price"));
        const totalPrice = count * price;
        var value = totalPrice.toLocaleString("ru-RU", {style: "currency", currency: "RUB"});
        var cartTotalPrice = $(".cart_total_price",tr);
        cartTotalPrice.html(value);
        cartTotalPrice.data("price",totalPrice);
        Cart.refreshTotalPrice();
    },
    refreshTotalPrice: function () {
        var totalPrice = 0;
        $(".cart_total_price").each(function () {
            const price = parseFloat($(this).data("price"));
            totalPrice += price;
        })
        var value = totalPrice.toLocaleString("ru-RU", {style: "currency", currency: "RUB"});
        $("#total-order-price").html(value);
    }
}