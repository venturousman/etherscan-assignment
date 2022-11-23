let myChart;

function bindChartData(labels, data) {
    if (!labels || !data) return;
    if (myChart) myChart.destroy();
    const ctx = document.getElementById('myChart');
    myChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels,
            datasets: [{
                label: 'Token Dataset',
                data,
                backgroundColor: [
                    '#4dc9f6',
                    '#f67019',
                    '#f53794',
                    '#537bc4',
                    '#acc236',
                    '#166a8f',
                    '#00a950',
                    '#58595b',
                    '#8549ba',
                ],
                hoverOffset: 4
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    //display: false,
                    position: 'top',
                },
                title: {
                    display: true,
                    text: 'Token Statistics by Total Supply'
                },
                //outlabels: {
                //    //text: '%l',
                //    color: 'black',
                //    stretch: 35,
                //    font: {
                //        resizable: true,
                //        minSize: 12,
                //        maxSize: 18,
                //    },
                //},
            }
        },
    });
}

function renderChart() {
    const url = 'api/report/total-supply-statistic';
    $.getJSON(url,
        function (res) {
            if (!res) return;
            bindChartData(res.labels, res.data);
        });
}

function selectToEdit(token) {
    //const token = JSON.parse(str);
    console.log('### editing', token, typeof token);
    if (!token) return;
    $('#inputId').val(token.Id);
    $('#inputName').val(token.Name);
    $('#inputSymbol').val(token.Symbol);
    $('#inputContractAddress').val(token.ContractAddress);
    $('#inputTotalSupply').val(token.TotalSupply);
    $('#inputTotalHolders').val(token.TotalHolders);
}

function renderTable(tokens) {
    const elementId = '#tbl-body-tokens';
    $(elementId).empty();
    // Loop through the list of tokens.
    $.each(tokens, function (index, val) {
        // Add a table row for the token.
        const rank = index + 1;
        const percent = (val.TotalSupplyPercentage * 100).toFixed(5) + '%';
        const link = `<a href="/Detail.aspx?id=${val.Symbol}">${val.Symbol}</a>`;
        const dataJSON = JSON.stringify(val);
        const editClick = `javascript:selectToEdit(${dataJSON})`;
        const editLink = `<a href"#" onclick=${editClick}>Edit</a>`;
        const row = '<td>' + rank + '</td>'
            + '<td>' + link + '</td>'
            + '<td>' + val.Name + '</td>'
            + '<td>' + val.ContractAddress + '</td>'
            + '<td>' + val.TotalHolders + '</td>'
            + '<td>' + val.TotalSupply + '</td>'
            + '<td>' + percent + '</td>'
            + '<td>' + editLink + '</td>';

        $('<tr/>', { html: row }).appendTo($(elementId));
    });
}

function getPagedData(page, callback) {
    //console.log('###', page);
    const url = `api/token?pageNumber=${page}&pageSize=${10}`;
    $.getJSON(url,
        function (data) {
            if (!data) return;
            const tokens = data.Results || [];
            // render table
            renderTable(tokens);
            // render pagination
            renderPagination(data);
            // render chart
            const chartLabels = tokens.map(x => x.Name);
            const chartData = tokens.map(x => x.TotalSupply);
            bindChartData(chartLabels, chartData);
            // do some stuffs.
            if (callback) callback(data);
        });
}

function renderPagination(data) {
    const elementId = '#pagination';
    $(elementId).empty();

    let prevCss = '';
    let prevClick = `javascript:getPagedData(${data.CurrentPage - 1})`;
    if (data.CurrentPage === 1) {
        prevCss = 'disabled';
        prevClick = '#'
    }
    const prevBtn = `<li class=${prevCss}><a href=${prevClick} aria-label="Previous"><span aria-hidden="true">&laquo;</span></a></li>`;
    $(elementId).append(prevBtn);

    for (let page = 1; page <= data.PageCount; page++) {
        let css = '';
        if (page === data.CurrentPage) {
            css = 'active'
        }
        const column = `<li class=${css}><a href="javascript:getPagedData(${page})">${page}</a></li>`;
        $(elementId).append(column);
    }

    let nextCss = '';
    let nextClick = `javascript:getPagedData(${data.CurrentPage + 1})`;
    if (data.CurrentPage === data.PageCount) {
        nextCss = 'disabled';
        nextClick = '#';
    }
    const nextBtn = `<li clas=${nextCss}><a href=${nextClick} aria-label="Next"><span aria-hidden="true">&raquo;</span></a></li>`;
    $(elementId).append(nextBtn);
}

function resetForm() {
    //console.log('### reset');
    $('#inputId').val('');
    $('#inputName').val('');
    $('#inputSymbol').val('');
    $('#inputContractAddress').val('');
    $('#inputTotalSupply').val('');
    $('#inputTotalHolders').val('');
}

function validateIsRequired(elementId) {
    // true is valid, false is invalid
    if (!elementId) return true;
    const value = $(elementId).val().trim();
    if (!value) return false;
    return true;
}

function validate() {
    let isAllValid = true;
    const elements = ['#inputName', '#inputSymbol', '#inputContractAddress', '#inputTotalSupply', '#inputTotalHolders'];
    // validate
    for (var i = 0; i < elements.length; i++) {
        const element = elements[i];
        const isValid = validateIsRequired(element);
        const eHelper = element.replace('input', 'helper');
        $(eHelper).text('');
        if (!isValid) {
            $(eHelper).text('This field is required');
            $(element).closest(".form-group").addClass('has-error');
            isAllValid = false;
        } else {
            $(element).closest(".form-group").removeClass('has-error');
        }
    }
    return isAllValid;
}

function onInputChange(event) {
    console.log('### onInputChange', event);
    validate();
}

function create() {
    const name = $('#inputName').val();
    const symbol = $('#inputSymbol').val();
    const contractAddress = $('#inputContractAddress').val();
    const totalSupply = $('#inputTotalSupply').val();
    const totalHolders = $('#inputTotalHolders').val();
    var dto = { name, symbol, contractAddress, totalSupply, totalHolders };
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "api/token",
        data: JSON.stringify(dto),
        datatype: "json",
        success: function (result) {
            alert('Created successfully!');
            const currentPage = $('#pagination li.active a').text();
            getPagedData(parseInt(currentPage));
            resetForm();
        },
        error: function (xmlhttprequest, textstatus, errorthrown) {
            console.log("error: " + errorthrown);
            alert('Error!');
        }
    });
}

function update(id) {
    const name = $('#inputName').val();
    const symbol = $('#inputSymbol').val();
    const contractAddress = $('#inputContractAddress').val();
    const totalSupply = $('#inputTotalSupply').val();
    const totalHolders = $('#inputTotalHolders').val();
    var dto = { name, symbol, contractAddress, totalSupply, totalHolders };
    const url = `api/token/${id}`;
    $.ajax({
        type: "PUT",
        contentType: "application/json; charset=utf-8",
        url,
        data: JSON.stringify(dto),
        datatype: "json",
        success: function (result) {
            alert('Updated successfully!');
            const currentPage = $('#pagination li.active a').text();
            getPagedData(parseInt(currentPage));
            resetForm();
        },
        error: function (xmlhttprequest, textstatus, errorthrown) {
            console.log("error: " + errorthrown);
            alert('Error!');
        }
    });
}

function submitForm() {
    //console.log('### submit');
    const isValid = validate();
    //console.log('### isValid', isValid);
    if (isValid) {
        const id = $('#inputId').val();
        if (!id) {
            create();
        } else {
            update(id);
        }
    }
}

$(document).ready(() => {
    $('#myChart').empty(); // Clear.
    $('#pagination').empty();
    getPagedData(1);
    //getPagedData(1, (data) => {
    //    const tokens = data.Results || [];
    //    // render chart.
    //    const chartLabels = tokens.map(x => x.Name);
    //    const chartData = tokens.map(x => x.TotalSupply);
    //    bindChartData(chartLabels, chartData);
    //});
    //renderChart();
});