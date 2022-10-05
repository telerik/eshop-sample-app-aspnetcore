function onRecentlyViewedDataBound(e) {
	hideRecentlyViewedIfEmpty(e);
	distinguishFavorites();
}

function hideRecentlyViewedIfEmpty(e) {
	var ds = e.sender.dataSource;
	if (ds.total() == 0) {
		$(".recently-viewed").hide();
	}
}

function onCategoryDataBound(e) {
	showResultCount(e);
	distinguishFavorites();
}

function distinguishFavorites() {
	var favButtons = $("[id^=addToFavoritesButton_]");
	favButtons.each(function () {
		var currentButton = $(this);
		var productId = this.id.split("_")[1];
		$.get("/Account/ProductIsInFavorites?productId=" + productId, function (data) {
			if (data) {
				currentButton.css("background-color", "#ffdf73");
			}
		});
	});
}

function showResultCount(e) {
	var count = e.sender.dataSource.total();
	$("#resultCount").html(count + " results in ");
}

function addRatingVisual() {
	for (i = 1; i <= 5; ++i) {
		$(".rating-" + i).append("<input id='rating" + i + "'></input>");
		$("#rating" + i).kendoRating({
			readonly: true,
			label: false,
			min: 1,
			max: 5,
			precision: "half",
			value: Number(i) - 0.5
		});
	}
}

function goToCategoryPage(e) {
	var category = e.sender.value();
	if (category != "") {
		location.href = "/Products/Category?subCategory=" + category;
	}
}

function onSummaryDataBound(e) {
	showSearchResult(e);
	showCategories(e);
	distinguishFavorites();
}

function showSearchResult(e) {
	var count = e.sender.dataSource.total();
	var searchParam = $("#searchBar").data("kendoAutoComplete").value();
	if (searchParam != "") {
		$("#searchResultCount").html(count + " results for " + searchParam);
		return;
	}
	$("#searchResultCount").html(count + " results");
}

function showCategories(e) {
	var searchParam = $("#searchBar").data("kendoAutoComplete").value();
	var data = e.sender.dataSource._data;

	$("#availableCategories").html("");
	for (i = 0; i < data.length; ++i) {
		var value = data[i].value;
		var count = data[i].items.length;
		var categoriesElement = $("#availableCategories");
		categoriesElement.append("<a href='/Products/Category?subCategory=" + value + "&searchParam=" + searchParam + "' ><p style='color: black;' ><strong>" + value + "</strong> (" + count + " results)</p></a>");
	}
}

function onAdditionalData() {
	return {
		text: $("#searchBar").val()
	};
}

function onSearchSelect(e) {
	e.preventDefault();

	var productName = e.dataItem.ProductName;
	var subCategory = e.dataItem.SubCategory;

	var searchParam = productName;
	if (subCategory != null) {
		searchParam = searchParam + "; " + subCategory;
	}

	e.sender.value(searchParam);
	e.sender.trigger("change");
}

function searchProducts(e) {
	var params = e.sender.value().split("; ");
	if (params.length == 1) {
		searchByName(params[0]);
	}
	else {
		searchByNameAndCategory(params[0], params[1]);
	}
}

function searchByNameAndCategory(name, subCategory) {
	location.href = "/Products/Category?searchParam=" + name + "&subCategory=" + subCategory;
}

function searchByName(name) {
	if (location.pathname.includes("/Products/")) {
		filterDataSource();
	}
	else {
		location.href = "/Products/Summary?searchParam=" + name;
	}
}