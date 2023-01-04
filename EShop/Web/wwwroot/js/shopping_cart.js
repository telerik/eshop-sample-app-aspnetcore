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

	grid.removeRow(rowToRemove);
	grid.dataSource.sync();

	grid.dataSource.bind("sync", function (e) {
		if (e.sender.data().length == 0) {
			location.href = window.location.href.indexOf('eshop') > 0 ? "/eshop/Account/ShoppingCart" : "/Account/ShoppingCart";
		}
	});
}

function updateShoppingCartChanges() {
	getShoppingCartGrid().dataSource.sync();
}

function checkoutShoppingCart() {
	kendo.ui.progress($("#checkoutButton"), true);
	location.href = window.location.href.indexOf('eshop') > 0 ?  "/eshop/Account/CheckoutShoppingCart" : "/Account/CheckoutShoppingCart";
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
	let getUrl = window.location.href.indexOf('eshop') > 0 ? "/eshop/Account/AddProductToShoppingCart?productId=" : "/Account/AddProductToShoppingCart?productId=";
	$.post(getUrl + productId, function () {
		getShoppingCartItemsCount();
	});
}

function getShoppingCartItemsCount() {
	let getUrl = window.location.href.indexOf('eshop') > 0 ? "/eshop/Account/GetShoppingCartItemsCount" : "/Account/GetShoppingCartItemsCount";
	$.get(getUrl, function (data) {
		$("#shoppingCartBadge").data("kendoBadge").text(data);
	});
}