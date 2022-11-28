function getShoppingCartGrid() {
	return $("#shoppingCartGrid").data("kendoGrid");
}

function shoppingCartGridOnDataBound() {
	$(".templateCell").each(function () {
		eval($(this).children("script").last().html());
	});

	var rows = this.tbody.children();
	var dataItems = this.dataSource.view();
	for (var i = 0; i < dataItems.length; i++) {
		kendo.bind(rows[i], dataItems[i]);
	}

	calculateShoppingCartTotal();
	getShoppingCartItemsCount();
}

function removeItemFromShoppingCart(itemId) {
	var rowToRemove = $("#remove_" + itemId).parent().parent();
	var grid = getShoppingCartGrid();
	var refresh = grid.dataSource.data().length == 1;

	grid.removeRow(rowToRemove);
	grid.dataSource.sync();

	if (refresh) {
		location.href = "/Account/ShoppingCart";
    }
}

function updateShoppingCartChanges() {
	getShoppingCartGrid().dataSource.sync();
}

function checkoutShoppingCart() {
	kendo.ui.progress($("#checkoutButton"), true);
	location.href = "/Account/CheckoutShoppingCart";
}

function calculateShoppingCartTotal() {
	var data = getShoppingCartGrid().dataSource.data();

	var total = 0;
	for (i = 0; i < data.length; ++i) {
		total += data[i].Total;
	}

	total = parseFloat(total).toFixed(2);
	$("#subTotalValue").html("$ " + total);
}

function addProductToShoppingCart(e) {
	var productId = e.sender.element[0].id.split('_')[1];

	$.post("/Account/AddProductToShoppingCart?productId=" + productId, function () {
		getShoppingCartItemsCount();
	});
}

function getShoppingCartItemsCount() {
	$.get("/Account/GetShoppingCartItemsCount", function (data) {
		$("#shoppingCartBadge").data("kendoBadge").text(data);
	});
}