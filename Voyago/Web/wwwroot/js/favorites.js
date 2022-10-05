function addProductToFavorites(e) {
	var element = e.sender.element;
	var productId = element[0].id.split('_')[1];

	$.get("/Account/ProductIsInFavorites?productId=" + productId, function (data) {
		if (!data) {
			$.post("/Account/AddProductToFavorites?productId=" + productId, function () {
				getFavoritesCount();
				$(element[0]).css("background-color", "#ffdf73");
			});
		}
		else {
			$.post("/Account/RemoveProductFromFavorites?productId=" + productId, function () {
				getFavoritesCount();
				$(element[0]).css("background-color", "#ffffff");
			});
		}
	});	
}

function removeProductFromFavorites(e) {
	var productId = e.sender.element[0].id.split('_')[1];

	$.post("/Account/RemoveProductFromFavorites?productId=" + productId, function () {
		$("#favoritesListView").data("kendoListView").dataSource.read();
		getFavoritesCount();
	});
}

function getFavoritesCount() {
	$.get("/Account/GetFavoritesCount", function (data) {
		$("#favoritesBadge").html(data);
	});
}