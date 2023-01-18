function addProductToFavorites(e) {
	var element = e.sender.element;
	var productId = element[0].id.split('_')[1];
	var icon = $(element[0]).find(".k-icon");
	var getUrl = window.location.href.indexOf('eshop') > 0 ? "/aspnet-core/eshop/Account/ProductIsInFavorites?productId=" : "/Account/ProductIsInFavorites?productId=";
	var addUrl = window.location.href.indexOf('eshop') > 0 ? "/aspnet-core/eshop/Account/AddProductToFavorites?productId=" : "/Account/AddProductToFavorites?productId=";
	var removeUrl = window.location.href.indexOf('eshop') > 0 ? "/aspnet-core/eshop/Account/RemoveProductFromFavorites?productId=" : "/Account/RemoveProductFromFavorites?productId=";

	$.get(getUrl + productId, function (data) {
		if (!data) {
			$.post(addUrl + productId, function () {
				getFavoritesCount();
				if ($(element[0]).find(".k-button-text")) {
					$(element[0]).find(".k-button-text").text("Added to favorites");
                }
				icon.removeClass("k-i-heart-outline");
				icon.addClass("k-i-heart");
			});
		}
		else {
			$.post(removeUrl + productId, function () {
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
	var removeUrl = window.location.href.indexOf('eshop') > 0 ? "/aspnet-core/eshop/Account/RemoveProductFromFavorites?productId=" : "/Account/RemoveProductFromFavorites?productId=";
	$.post(removeUrl + productId, function () {
		$("#favoritesListView").data("kendoListView").dataSource.read();
		getFavoritesCount();
	});
}

function getFavoritesCount() {
	var getUrl = window.location.href.indexOf('eshop') > 0 ? "/aspnet-core/eshop/Account/GetFavoritesCount" : "/Account/GetFavoritesCount";
	$.get(getUrl, function (data) {
		$("#favoritesBadge").data("kendoBadge").text(data);
	});
}

function favouritesReportClick() {
	location.href = window.location.href.indexOf('eshop') > 0 ? "/aspnet-core/eshop/Account/FavouritesReport" : "/Account/FavouritesReport";
}

function onListViewDataBound(e) {
	if (e.sender.dataSource.data().length == 0) {
		$("#favouritesReportBtn").data("kendoButton").enable(false);
	} else $("#favouritesReportBtn").data("kendoButton").enable(true);
}