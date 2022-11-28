function addProductToFavorites(e) {
	var element = e.sender.element;
	var productId = element[0].id.split('_')[1];
	var icon = $(element[0]).find(".k-icon");

	$.get("/Account/ProductIsInFavorites?productId=" + productId, function (data) {
		if (!data) {
			$.post("/Account/AddProductToFavorites?productId=" + productId, function () {
				getFavoritesCount();
				if ($(element[0]).find(".k-button-text")) {
					$(element[0]).find(".k-button-text").text("Added to favorites");
                }
				icon.removeClass("k-i-heart-outline");
				icon.addClass("k-i-heart");
			});
		}
		else {
			$.post("/Account/RemoveProductFromFavorites?productId=" + productId, function () {
				getFavoritesCount();
				if ($(element[0]).find(".k-button-text")) {
					$(element[0]).find(".k-button-text").text("Add to favorites");
				}
				icon.removeClass("k-i-heart");
				icon.addClass("k-i-heart-outline");
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
		$("#favoritesBadge").data("kendoBadge").text(data);
	});
}

function favouritesReportClick() {
	location.href = "/Account/FavouritesReport";
}

function onListViewDataBound(e) {
	if (e.sender.dataSource.data().length == 0) {
		$("#favouritesReportBtn").data("kendoButton").enable(false);
	} else $("#favouritesReportBtn").data("kendoButton").enable(true);
}