window.setupAging = (id, chartTitle, workItems, columnInfo, percentiles) => {
    var ctx = document.getElementById(id).getContext('2d');
    var config = {
        type: 'scatter',
        data: {
            datasets: [
                {
                    label: 'Aging',
                    data: workItems,
                    borderColor: 'blue',
                    backgroundColor: 'blue',
                    parsing: {
                        xAxisKey: 'columnOrder',
                        yAxisKey: 'age',
                    }
                },
            ]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: chartTitle,
                    font: {
                        size: 10
                    }
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            let label = 'WI ' + context.raw.workItemId + ', ' + context.parsed.y + ' days';
                            return label;
                        }
                    }
                },
                annotation: {
                    annotations: {
                        line1: {
                            type: 'line',
                            yMin: percentiles.eightyFifth,
                            yMax: percentiles.eightyFifth,
                            borderColor: 'blue'
                        }
                    }
                }
            },
            scales: {
                x: {
                    ticks: {
                        callback: function (val, index) {
                            return columnInfo[val];
                        }
                    }
                }
            }
        }
    };
    var chart = Chart.getChart(id);
    if (chart != undefined) {
        chart.destroy();
    }
    new Chart(ctx, config);
}

window.setupCycleTime = (id, chartTitle, dataArray, annotation) => {
    var ctx = document.getElementById(id).getContext('2d');
    var percentiles = (dataArray == null || dataArray.length == 0) ? null : dataArray[0];
    var ninetyFifth = (percentiles == null) ? 0 : percentiles.ninetyFifth;
    var eightyFifth = (percentiles == null) ? 0 : percentiles.eightyFifth;
    var seventieth = (percentiles == null) ? 0 : percentiles.seventieth;
    var fiftieth = (percentiles == null) ? 0 : percentiles.fiftieth;
    var thirtieth = (percentiles == null) ? 0 : percentiles.thirtieth;
    var config = {
        data: {
            datasets: [
                {
                    type: 'scatter',
                    label: 'Cycle times',
                    data: dataArray,
                    borderColor: 'blue',
                    backgroundColor: 'blue',
                    parsing: {
                        yAxisKey: 'y'
                    }
                },
                {
                    type: 'line',
                    label: '95th: ' + ninetyFifth + ' days',
                    pointRadius: 0,
                    data: dataArray,
                    borderColor: 'green',
                    backgroundColor: 'green',
                    parsing: {
                        yAxisKey: 'ninetyFifth'
                    }

                },
                {
                    type: 'line',
                    label: '85th: ' + eightyFifth + ' days',
                    pointRadius: 0,
                    data: dataArray,
                    borderColor: 'greenyellow',
                    backgroundColor: 'greenyellow',
                    parsing: {
                        yAxisKey: 'eightyFifth'
                    }

                },
                {
                    type: 'line',
                    label: '70th: ' + seventieth + ' days',
                    pointRadius: 0,
                    data: dataArray,
                    borderColor: 'yellow',
                    backgroundColor: 'yellow',
                    parsing: {
                        yAxisKey: 'seventieth'
                    }

                },
                {
                    type: 'line',
                    label: '50th: ' + fiftieth + ' days',
                    pointRadius: 0,
                    data: dataArray,
                    borderColor: 'orange',
                    backgroundColor: 'orange',
                    parsing: {
                        yAxisKey: 'fiftieth'
                    }

                },
                {
                    type: 'line',
                    label: '30th: ' + thirtieth + ' days',
                    pointRadius: 0,
                    data: dataArray,
                    borderColor: 'red',
                    backgroundColor: 'red',
                    parsing: {
                        yAxisKey: 'thirtieth'
                    }

                }
            ]
        },
        options: {
            responsive: true,
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'day'
                    }
                }
            },
            plugins: {
                title: {
                    display: true,
                    text: chartTitle,
                    font: {
                        size: 18
                    }
                },
                legend: {
                    position: 'left'
                },
                annotation: {
                    annotations: {
                        label1: {
                            type: 'label',
                            xValue: annotation.xPosition,
                            yValue: annotation.yPosition,
                            position: { x: 'end', y: 'start' },
                            content: annotation.queryDetails,
                            font: {
                                size: 14
                            }
                        }
                    }
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            let label = 'WI ' + context.raw.workItemId + ', ' + context.parsed.y + ' days';
                            return label;
                        }
                    }
                }
            }
        }
    };
    var chart = Chart.getChart(id);
    if (chart != undefined) {
        chart.destroy();
    }
    new Chart(ctx, config);
}

window.setupHowManyDays = (id, chartTitle, data, annotation) => {
    var ctx = document.getElementById(id).getContext('2d');
    var config = {
        type: 'bar',
        data: {
            datasets: [
                {
                    data: data.simulations,
                    label: 'Outcomes',
                    borderColor: 'blue',
                    backgroundColor: 'blue'
                },
                {
                    data: data.thirtieth,
                    label: '30th: ' + data.thirtieth[0].x + ' days',
                    borderColor: 'red',
                    backgroundColor: 'red'
                },
                {
                    data: data.fiftieth,
                    label: '50th: ' + data.fiftieth[0].x + ' days',
                    borderColor: 'orange',
                    backgroundColor: 'orange'
                },
                {
                    data: data.seventieth,
                    label: '70th: ' + data.seventieth[0].x + ' days',
                    borderColor: 'yellow',
                    backgroundColor: 'yellow'
                },
                {
                    data: data.eightyFifth,
                    label: '85th: ' + data.eightyFifth[0].x + ' days',
                    borderColor: 'greenyellow',
                    backgroundColor: 'greenyellow'
                },
                {
                    data: data.ninetyFifth,
                    label: '95th: ' + data.ninetyFifth[0].x + ' days',
                    borderColor: 'green',
                    backgroundColor: 'green'
                },
            ]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: chartTitle,
                    font: {
                        size: 18
                    }
                },
                legend: {
                    position: 'left'
                },
                annotation: {
                    annotations: {
                        label1: {
                            type: 'label',
                            xValue: annotation.xPosition,
                            yValue: annotation.yPosition,
                            position: { x: 'end', y: 'start' },
                            content: annotation.queryDetails,
                            font: {
                                size: 14
                            }
                        }
                    }
                }
            }
        }
    }
    var chart = Chart.getChart(id);
    if (chart != undefined) {
        chart.destroy();
    }
    new Chart(ctx, config);
}

window.setupHowManyStories = (id, chartTitle, data, annotation) => {
    var ctx = document.getElementById(id).getContext('2d');
    var config = {
        type: 'bar',
        data: {
            datasets: [
                {
                    data: data.simulations,
                    label: 'Outcomes',
                    borderColor: 'blue',
                    backgroundColor: 'blue'
                },
                {
                    data: data.ninetyFifth,
                    label: '95th: ' + data.ninetyFifth[0].x + ' stories',
                    borderColor: 'green',
                    backgroundColor: 'green'
                },
                {
                    data: data.eightyFifth,
                    label: '85th: ' + data.eightyFifth[0].x + ' stories',
                    borderColor: 'yellowgreen',
                    backgroundColor: 'yellowgreen'
                },
                {
                    data: data.seventieth,
                    label: '70th: ' + data.seventieth[0].x + ' stories',
                    borderColor: 'yellow',
                    backgroundColor: 'yellow'
                },
                {
                    data: data.fiftieth,
                    label: '50th: ' + data.fiftieth[0].x + ' stories',
                    borderColor: 'orange',
                    backgroundColor: 'orange'
                },
                {
                    data: data.thirtieth,
                    label: '30th: ' + data.thirtieth[0].x + ' stories',
                    borderColor: 'red',
                    backgroundColor: 'red'
                },
            ]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: chartTitle,
                    font: {
                        size: 18
                    }
                },
                legend: {
                    position: 'left'
                },
                annotation: {
                    annotations: {
                        label1: {
                            type: 'label',
                            xValue: annotation.xPosition,
                            yValue: annotation.yPosition,
                            position: { x: 'end', y: 'start' },
                            content: annotation.queryDetails,
                            font: {
                                size: 14
                            }
                        }
                    }
                }
            }
        }
    }
    var chart = Chart.getChart(id);
    if (chart != undefined) {
        chart.destroy();
    }
    new Chart(ctx, config);
}

window.setupDeliveryEfficiency = (id, chartTitle, data95, data85, data70, data50, data30, annotation) => {
    var ctx = document.getElementById(id).getContext('2d');
    var config = {
        type: 'line',
        data: {
            datasets: [
                {
                    data: data95.percentileData,
                    label: data95.description,
                    borderColor: 'green',
                    backgroundColor: 'green'
                },
                {
                    data: data85.percentileData,
                    label: data85.description,
                    borderColor: 'yellowgreen',
                    backgroundColor: 'yellowgreen'
                },
                {
                    data: data70.percentileData,
                    label: data70.description,
                    borderColor: 'yellow',
                    backgroundColor: 'yellow'
                },
                {
                    data: data50.percentileData,
                    label: data50.description,
                    borderColor: 'orange',
                    backgroundColor: 'orange'
                },
                {
                    data: data30.percentileData,
                    label: data30.description,
                    borderColor: 'red',
                    backgroundColor: 'red'
                },
            ]
        },
        options: {
            responsive: true,
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'day'
                    }
                }
            },
            plugins: {
                title: {
                    display: true,
                    text: chartTitle,
                    font: {
                        size: 18
                    }
                },
                legend: {
                    position: 'left'
                },
                annotation: {
                    annotations: {
                        label1: {
                            type: 'label',
                            xValue: annotation.xPosition,
                            yValue: annotation.yPosition,
                            position: { x: 'end', y: 'start' },
                            content: annotation.queryDetails,
                            font: {
                                size: 14
                            }
                        }
                    }
                }
            }
        }
    }
    var chart = Chart.getChart(id);
    if (chart != undefined) {
        chart.destroy();
    }
    new Chart(ctx, config);
}

window.setupFeatureProgress = (id, title, closedItems, resolvedItems, newItems, activeItems, theRestItems) => {
    var ctx = document.getElementById(id).getContext('2d');
    var config = {
        type: 'bar',
        data: {
            datasets: [
                {
                    data: closedItems,
                    label: 'Closed',
                    borderColor: 'blue',
                    backgroundColor: 'blue'
                },
                {
                    data: resolvedItems,
                    label: 'Resolved',
                    borderColor: 'green',
                    backgroundColor: 'green'
                },
                {
                    data: theRestItems,
                    label: 'Other',
                    borderColor: 'yellow',
                    backgroundColor: 'yellow'
                },
                {
                    data: activeItems,
                    label: 'Active',
                    borderColor: 'orange',
                    backgroundColor: 'orange'
                },
                {
                    data: newItems,
                    label: 'New',
                    borderColor: 'red',
                    backgroundColor: 'red'
                },
            ]
        },
        options: {
            responsive: true,
            plugins: {
                title: {
                    display: true,
                    text: title,
                    font: {
                        size: 18
                    }
                },
                legend: {
                    position: 'left'
                }
            },
            scales: {
                x: {
                    stacked: true,
                    type: 'time',
                    time: {
                        unit: 'day'
                    }
                },
                y: {
                    stacked: true
                }
            }
        }
    }
    var chart = Chart.getChart(id);
    if (chart != undefined) {
        chart.destroy();
    }
    new Chart(ctx, config);
}

window.setupCumulativeFlow = (id, chartTitle, data) => {
    var ctx = document.getElementById(id).getContext('2d');
    var config = {
        type: 'line',
        data: {
            datasets: data
        },
        options: {
            responsive: true,
            scales: {
                x: {
                    type: 'time',
                    time: {
                        unit: 'day'
                    }
                },
                y: {
                    stacked: true,
                }
            },
            plugins: {
                title: {
                    display: true,
                    text: chartTitle,
                    font: {
                        size: 18
                    }
                },
                legend: {
                    position: 'left'
                }
            }
        }
    }
    var chart = Chart.getChart(id);
    if (chart != undefined) {
        chart.destroy();
    }
    new Chart(ctx, config);
}

window.DownloadCsv = (filename, contentType, content) => {
    const exportUrl = URL.createObjectURL(file);
    const a = document.createElement("a");
    document.body.appendChild(a);
    a.href = exportUrl;
    a.download = filename;
    a.target = "_self";
    a.click();
}

window.DownloadImage = (filename, id) => {
    var img = "";
    var canvas = document.getElementById(id);
    var ctx = canvas.getContext('2d');
    ctx.globalCompositeOperation = 'destination-over';
    ctx.fillStyle = "white";
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    img = canvas.toDataURL("image/png");
    var d = document.createElement("a");
    d.href = img;
    d.download = filename;
    d.click();
}
