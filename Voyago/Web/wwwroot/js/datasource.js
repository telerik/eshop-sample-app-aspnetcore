var discountToggle = "1";

function toggleDiscountPicker(e) {
    var checkBoxGroup = e.sender;
    var targetBox = e.target;
    var clickedValue = targetBox[0].defaultValue;

    checkBoxGroup.checkAll(false);
    if (discountToggle == clickedValue) {
        checkBoxGroup.value([discountToggle]);
        checkBoxGroup.trigger("select");
        return;
    }

    if (discountToggle == "1") {
        discountToggle = "2";
        checkBoxGroup.value([discountToggle]);
        checkBoxGroup.trigger("select")
    }
    else {
        discountToggle = "1";
        checkBoxGroup.value([discountToggle]);
        checkBoxGroup.trigger("select")
    }

    filterDataSource();
}

function getSearchFilter() {
    var value = $("#searchBar").data("kendoAutoComplete").value();
    if (value == "") {
        return null;
    }

    var searchFilter = {
        logic: "or",
        filters: [
            { field: "Name", operator: "contains", value: value },
            { field: "Model", operator: "contains", value: value },
        ]
    };

    return searchFilter;
}

function getDiscountFilter() {
    var values = $("#discountPicker").data("kendoCheckBoxGroup").value();
    if (!values.length || values.length > 1) {
        return null;
    }
    if (values[0] == "1") {
        return {
            logic: "and",
            filters: [
                { field: "DiscountPct", operator: "gte", value: 0 }
            ]
        };
    }
    else {
        return {
            logic: "and",
            filters: [
                { field: "DiscountPct", operator: "gt", value: 0 }
            ]
        };
    }
}

function getCategoryFilter() {
    var value = $("#subCategory").html();
    var filters = new Array();
    filters.push({ field: "SubCategory", operator: "eq", value: value });

    var categoryFilter = {
        logic: "or",
        filters: filters
    };

    return categoryFilter;
}

function getModelFilter() {
    var modelPicker = $("#modelPicker").data("kendoCheckBoxGroup");
    if (!modelPicker) {
        return null;
    }

    var values = modelPicker.value();
    if (!values.length) {
        return null;
    }

    var filters = new Array();
    values.forEach(value => {
        filters.push({ field: "Model", operator: "eq", value: value });
    })

    var modelFilter = {
        logic: "or",
        filters: filters
    };

    return modelFilter;
}

function getSizeFilter() {
    var sizePicker = $("#sizePicker").data("kendoCheckBoxGroup");
    if (!sizePicker) {
        return null;
    }

    var values = sizePicker.value();
    if (!values.length) {
        return null;
    }

    var filters = new Array();
    values.forEach(value => {
        filters.push({ field: "Size", operator: "eq", value: value });
    })

    var sizeFilter = {
        logic: "or",
        filters: filters
    };

    return sizeFilter;
}

function getRatingFilter() {
    var values = $("#ratingPicker").data("kendoCheckBoxGroup").value();
    if (!values.length) {
        return null;
    }

    var filters = new Array();
    values.forEach(value => {
        var inner = {
            logic: "and",
            filters: [
                { field: "AverageRating", operator: "gt", value: +value - 1 },
                { field: "AverageRating", operator: "lt", value: +value + 1 }
            ]
        };
        filters.push(inner);
    })

    var ratingFilter = {
        logic: "or",
        filters: filters
    };

    return ratingFilter;
}

function getColorFilter() {
    var colorPicker = $("#colorPicker").data("kendoCheckBoxGroup");
    if (!colorPicker) {
        return null;
    }

    var values = colorPicker.value();
    if (!values.length) {
        return null;
    }

    var filters = new Array();
    values.forEach(value => {
        filters.push({ field: "Color", operator: "eq", value: value });
    })

    var colorFilter = {
        logic: "or",
        filters: filters
    };

    return colorFilter;
}
function getPriceFilter() {
    var priceSlider = $("#priceSlider").data("kendoRangeSlider");
    var start = priceSlider.value()[0];
    var end = priceSlider.value()[1];

    var priceFilter = {
        logic: "and",
        filters: [
            { field: "Price", operator: "gte", value: start },
            { field: "Price", operator: "lte", value: end }
        ]
    };

    return priceFilter;
}

function getWeightFilter() {
    var weightSlider = $("#weightSlider").data("kendoRangeSlider");
    var start = weightSlider.value()[0];
    var end = weightSlider.value()[1];

    var weightFilter = {
        logic: "and",
        filters: [
            { field: "Weight", operator: "gte", value: start },
            { field: "Weight", operator: "lte", value: end }
        ]
    };

    return weightFilter;
}

function filterDataSource() {
    var searchFilter = getSearchFilter();
    var discountFilter = getDiscountFilter();

    var ratingFilter = getRatingFilter();
    var priceFilter = getPriceFilter();
    var weightFilter = getWeightFilter();

    var filters = new Array();
    if (searchFilter) { filters.push(searchFilter); }
    if (discountFilter) { filters.push(discountFilter); }
    if (ratingFilter) { filters.push(ratingFilter); }
    if (priceFilter) { filters.push(priceFilter); }
    if (weightFilter) { filters.push(weightFilter); }

    if (location.href.includes("/Products/Category")) {
        var categoryFilter = getCategoryFilter();
        var modelFilter = getModelFilter();
        var sizeFilter = getSizeFilter();
        var colorFilter = getColorFilter();
        if (categoryFilter) { filters.push(categoryFilter); }
        if (modelFilter) { filters.push(modelFilter); }
        if (sizeFilter) { filters.push(sizeFilter); }
        if (colorFilter) { filters.push(colorFilter); }
    }

    var total = {
        logic: "and",
        filters: filters
    };

    var ds = $("#allProductsListView").data("kendoListView").dataSource;
    ds.filter(total);

    var currentSort = ds.sort();
    if (currentSort) {
        ds.sort(currentSort);
    }
}

function sortDataSource(e) {
    var parameter = e.sender.value();
    var ds = $("#allProductsListView").data("kendoListView").dataSource;
    switch (parameter) {
        case '1':
            ds.sort({ field: "FinalPrice", dir: "asc" })
            break;
        case '2':
            ds.sort({ field: "FinalPrice", dir: "desc" })
            break;
        case '3':
            ds.sort({ field: "Name", dir: "asc" })
            break;
        case '4':
            ds.sort({ field: "Name", dir: "desc" })
            break;
        default:
            break;
    }
}