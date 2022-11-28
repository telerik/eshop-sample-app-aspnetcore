function passOrderNumber() {
    let urlParams = new URLSearchParams(window.location.search);
    return {
        orderNumber: urlParams.get('orderNumber')
    }
}

function viewInvoiceClick() {
    let urlParams = new URLSearchParams(window.location.search);
    location.href = "/Orders/Invoice?orderNumber=" + urlParams.get('orderNumber')
}
