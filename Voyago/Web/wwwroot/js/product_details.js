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
	$.get("/Products/GetProductIdByModelAndColor?modelId=" + modelId + "&color=" + color, function (data) {
		if (data != null) {
			location.href = "/Products/Details?productId=" + data;
		}
	});
}

function selectProductBySize(modelId, size) {
	$.get("/Products/GetProductIdByModelAndSize?modelId=" + modelId + "&size=" + size, function (data) {
		if (data != null) {
			location.href = "/Products/Details?productId=" + data;
		}
	});
}

function selectProductByColorAndSize(modelId, color, size) {
	$.get("/Products/GetProductIdByModelSizeAndColor?modelId=" + modelId + "&size=" + size + "&color=" + color, function (data) {
		if (data != null) {
			location.href = "/Products/Details?productId=" + data;
		}
	});
}