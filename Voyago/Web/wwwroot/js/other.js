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
	if (this.dataSource.total() == 0) {
		$("#allProductsListView").append("<h4 class='no-product-msg'>This category is out of stock at the moment.</h4>");
	}
	//showResultCount(e);
	distinguishFavorites();
}

function distinguishFavorites() {
	var favButtons = $("[id^=addToFavoritesButton_]");
	favButtons.each(function () {
		var currentButton = $(this);
		var icon = currentButton.find(".k-icon");
		var productId = this.id.split("_")[1];
		$.get("/Account/ProductIsInFavorites?productId=" + productId, function (data) {
			if (data) {
				if (currentButton.find(".k-button-text")) {
					currentButton.find(".k-button-text").text("Added to favorites");
				}
				icon.removeClass("k-i-heart-outline");
				icon.addClass("k-i-heart");
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
	let category = e.sender.text();
	if (category != "") {
		if (category == "Clothes") {
			location.href = "/Home/Clothing";
		} else location.href = "/Home/" + category;
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

function changeUserCountry(e) {
	let stateDDL = $("#State").data("kendoDropDownList");
	if (this.value() == "US") {
		stateDDL.enable(true);
	} else {
		stateDDL.value("");
		stateDDL.enable(false);
    }
}

function productCatalogClick() {
	location.href="/Home/ProductCatalog";
}

function onContactsFormSubmit(e) {
	e.preventDefault();
	$("#contacts-submit-success").html("<div class='k-messagebox k-messagebox-success'>Your message is sent.</div>");
}

function onContactsFormValidate() {
	$("#contacts-submit-success").html("");
}