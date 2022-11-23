function renderCaption(data) {
    const elementId = '#tbl-detail-caption';
    $(elementId).text(data.ContractAddress);
}

function renderTable(data) {
    const elementId = '#tbl-detail-body';
    let row = '<td>Price:</td>'
        + '<td class="price"> $ ' + data.Price + '</td>';
    $('<tr/>', { html: row }).appendTo($(elementId));

    row = '<td>Total Supply:</td>'
        + '<td>' + `${data.TotalSupply} ${data.Symbol}` + '</td>';
    $('<tr/>', { html: row }).appendTo($(elementId));

    row = '<td>Total Holders:</td>'
        + '<td>' + data.TotalHolders + '</td>';
    $('<tr/>', { html: row }).appendTo($(elementId));

    row = '<td>Name:</td>'
        + '<td>' + data.Name + '</td>';
    $('<tr/>', { html: row }).appendTo($(elementId));
}

function getDetail() {
    const params = new Proxy(new URLSearchParams(window.location.search), {
        get: (searchParams, prop) => searchParams.get(prop),
    });
    const symbol = params.id;
    if (!symbol) return;
    const url = `api/token/${symbol}`;
    $.getJSON(url,
        function (data) {
            //console.log('### data', data);
            if (!data) return;
            renderCaption(data);
            renderTable(data);
        });
}

$(document).ready(() => {
    $('#tbl-detail-caption').empty(); // Clear the table body.
    $('#tbl-detail-body').empty(); // Clear the table body.
    getDetail();
});