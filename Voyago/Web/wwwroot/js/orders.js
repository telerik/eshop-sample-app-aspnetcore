function passOrderNumber() {
    let urlParams = new URLSearchParams(window.location.search);
    return {
        orderNumber: urlParams.get('orderNumber')
    }
}

function viewInvoiceClick() {
    let urlParams = new URLSearchParams(window.location.search);
    let getUrl = window.location.href.indexOf('fluent-eshop-voyago') > 0 ? window.location.host + "/fluent-eshop-voyago/Orders/Invoice?orderNumber=" : "/Orders/Invoice?orderNumber=";
    location.href = getUrl + urlParams.get('orderNumber');
}
