//console.log('Products Data:', productsData);
console.log('Turnover Data:', turnoverData);
console.log('Growth Data:', growthData);

// Load Charts and the corechart and barchart packages.
google.charts.load('current', { 'packages': ['corechart'] });

// Draw the pie chart and bar chart when Charts is loaded.
google.charts.setOnLoadCallback(drawProductChart);
google.charts.setOnLoadCallback(drawTurnoverChart);
//google.charts.setOnLoadCallback(drawLineChart);

// Biểu đồ cơ cấu sản phẩm
function drawProductChart() {
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Tên sản phẩm');
    data.addColumn('number', 'Số lượng');
    console.log('Products Data:', productsData);
    productsData.forEach(function (item) {
        data.addRow([item.name, item.quantity]);
    });

    var piechart_options = { title: 'Cơ cấu sản phẩm đã bán', width: 400, height: 300 };
    var piechart = new google.visualization.PieChart(document.getElementById('piechartOfProducts'));
    piechart.draw(data, piechart_options);

    var barchart_options = { title: 'Thống kê sản phẩm đã bán', width: 300, height: 300, legend: 'none' };
    var barchart = new google.visualization.BarChart(document.getElementById('barchartOfProducts'));
    barchart.draw(data, barchart_options);
}

// Biểu đồ cơ cấu doanh thu
function drawTurnoverChart() {
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Sản phẩm');
    data.addColumn('number', 'Doanh thu');
    turnoverData.forEach(function (item) {
        data.addRow([item.product, item.revenue]);
    });

    console.log('Data:', data);

    var _3Doptions = { title: 'Cơ cấu theo danh thu', width: 400, height: 300, is3D: true };
    var TurnoverChart = new google.visualization.PieChart(document.getElementById('donutchartOfTurnover'));
    TurnoverChart.draw(data, _3Doptions);
}

// Biểu đồ tăng trưởng doanh thu và số lượng
function drawLineChart() {
    var data = new google.visualization.DataTable();
    data.addColumn('string', 'Tháng');
    data.addColumn('number', 'Tổng sản phẩm');
    data.addColumn('number', 'Tổng doanh thu');
    growthData.forEach(function (item) {
        data.addRow([item.month, item.totalProducts, item.totalRevenue]);
    });

    console.log('Data:', data);

    var materialOptions = {
        title: 'Biểu đồ tăng trưởng về số lượng sản phẩm và doanh thu theo thời gian',
        curveType: 'function',
        legend: { position: 'bottom' },
        series: {
            0: { targetAxisIndex: 0 },
            1: { targetAxisIndex: 1 }
        },
        vAxes: {
            0: { title: 'Số lượng sản phẩm' },
            1: { title: 'Tổng doanh thu (VNĐ)' }
        }
    };

    var linechart = new google.visualization.LineChart(document.getElementById('growthchart'));
    linechart.draw(data, materialOptions);
}

