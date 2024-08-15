function getShoppingCartGrid() {
	return $("#shoppingCartGrid").data("kendoGrid");
}

function shoppingCartGridOnDataBound(e) {
	var grid = e.sender;
	var items = e.sender.items();

	items.each(function (e) {
		var dataItem = grid.dataItem(this);
		var ntb = $(this).find('.quantity-editor');
		var ntbComponent = $(this).find('.quantity-editor[data-role="numerictextbox"]')
		if (ntbComponent.data("kendoNumericTextBox")) {
			return;
		}
		$(ntb).kendoNumericTextBox({
			change: updateShoppingCartChanges,
			value: dataItem.Quantity,
			"decimals": 0,
			"format": "\\#",
			"max": 100,
			"min": 1,
			"rounded": "none"
		});
	});

	calculateShoppingCartTotal();
	getShoppingCartItemsCount();
}

function removeItemFromShoppingCart(itemId) {
	var rowToRemove = $("#remove_" + itemId).closest('tr');
	var grid = getShoppingCartGrid();

	grid.removeRow(rowToRemove);
	grid.dataSource.sync();
}

function updateShoppingCartChanges(e) {
	var shoppingCartGrid = $('#shoppingCartGrid').data('kendoGrid');
	var currentRow = $(e.sender.element).closest('tr');
	var editDataItem = shoppingCartGrid.dataItem(currentRow);
	var newQtyValue = e.sender.value();
	editDataItem.set('Quantity', newQtyValue);
	editDataItem.set('Total', newQtyValue * editDataItem.ProductPrice);
	getShoppingCartGrid().dataSource.sync();
}

function checkoutShoppingCart() {
	kendo.ui.progress($("#checkoutButton"), true);
	location.href = window.location.href.indexOf('eshop') > 0 ? "/aspnet-core/eshop/Account/CheckoutShoppingCart" : "/Account/CheckoutShoppingCart";
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
	let getUrl = window.location.href.indexOf('eshop') > 0 ? "/aspnet-core/eshop/Account/AddProductToShoppingCart?productId=" : "/Account/AddProductToShoppingCart?productId=";

	$.post(getUrl + productId, function () {
		getShoppingCartItemsCount();
	});
}
function getShoppingCartItemsCount() {
	let getUrl = window.location.href.indexOf('eshop') > 0 ? "/aspnet-core/eshop/Account/GetShoppingCartItemsCount" : "/Account/GetShoppingCartItemsCount";
	$.get(getUrl, function (data) {
		$('#shoppingCartBadge').data('kendoBadge').text(data);
	});
}