function selectAlternateProductVersion(modelId) {
	var size = $("#sizePicker").data("kendoDropDownList").value();
	var color = $("#colorPicker").data("kendoRadioGroup").value();

	if (size != null && color != null) {
		selectProductByColorAndSize(modelId, color, size);
	}
	else if (size != null) {
		selectProductBySize(modelId, size);
	}
	else if (color != null) {
		selectProductByColor(modelId, color);
	}
}

function selectProductByColor(modelId, color) {
	let getUrl = window.location.href.indexOf('fluent-eshop-voyago') > 0 ? window.location.host + "/fluent-eshop-voyago/Products/GetProductIdByModelAndColor?modelId=" : "/Products/GetProductIdByModelAndColor?modelId=";
	$.get(getUrl + modelId + "&color=" + color, function (data) {
		if (data != null) {
			location.href = window.location.href.indexOf('fluent-eshop-voyago') > 0 ? window.location.host + "/fluent-eshop-voyago/Products/Details?productId=" + data : "/Products/Details?productId=" + data;
		}
	});
}

function selectProductBySize(modelId, size) {
	let getUrl = window.location.href.indexOf('fluent-eshop-voyago') > 0 ? window.location.host + "/fluent-eshop-voyago/Products/GetProductIdByModelAndSize?modelId=" : "/Products/GetProductIdByModelAndSize?modelId=";
	$.get(getUrl + modelId + "&size=" + size, function (data) {
		if (data != null) {
			location.href = window.location.href.indexOf('fluent-eshop-voyago') > 0 ? window.location.host + "/fluent-eshop-voyago/Products/Details?productId=" + data : "/Products/Details?productId=" + data;
		}
	});
}

function selectProductByColorAndSize(modelId, color, size) {
	let getUrl = window.location.href.indexOf('fluent-eshop-voyago') > 0 ? window.location.host + "/fluent-eshop-voyago/Products/GetProductIdByModelSizeAndColor?modelId=" : "/Products/GetProductIdByModelSizeAndColor?modelId=";
	$.get(getUrl + modelId + "&size=" + size + "&color=" + color, function (data) {
		if (data != null) {
			location.href = window.location.href.indexOf('fluent-eshop-voyago') > 0 ? window.location.host + "/fluent-eshop-voyago/Products/Details?productId=" + data : "/Products/Details?productId=" + data;
		}
	});
}