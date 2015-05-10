// The MIT License (MIT)
// 
// Copyright (c) 2015 Daniel Franklin. http://blazedsp.com/
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

App.DashboardCampaignReadView = Ember.View.extend({
    chart: null,

    willAnimateIn: function() {
        this.$().css('opacity', 0);
    },
    animateIn: function(done) {
        // #region amCharts
        // generate random data
        //function generateChartData() {
        //    var chartData = [];
        //    var firstDate = new Date(2012, 0, 1);
        //    firstDate.setDate(firstDate.getDate() - 1000);
        //    firstDate.setHours(0, 0, 0, 0);

        //    for (var i = 0; i < 1000; i++) {
        //        var newDate = new Date(firstDate);
        //        newDate.setHours(0, i, 0, 0);

        //        var a = Math.round(Math.random() * (40 + i)) + 100 + i;
        //        var b = Math.round(Math.random() * 100000000);

        //        chartData.push({
        //            date: newDate,
        //            value: a,
        //            volume: b
        //        });
        //    }
        //    return chartData;
        //}
        //var chartDatas = generateChartData();
        //this.chart = AmCharts.makeChart("chartdiv", {
        //    type: "stock",
        //    "theme": "none",
        //    pathToImages: "//www.amcharts.com/lib/3/images/",
        //    categoryAxesSettings: {
        //        minPeriod: "mm"
        //    },
        //    dataSets: [
        //        {
        //            color: "#b0de09",
        //            fieldMappings: [
        //                {
        //                    fromField: "value",
        //                    toField: "value"
        //                }, {
        //                    fromField: "volume",
        //                    toField: "volume"
        //                }
        //            ],
        //            dataProvider: chartDatas,
        //            categoryField: "date"
        //        }
        //    ],
        //    panels: [
        //        {
        //            showCategoryAxis: false,
        //            title: "Value",
        //            percentHeight: 70,
        //            stockGraphs: [
        //                {
        //                    id: "g1",
        //                    valueField: "value",
        //                    type: "smoothedLine",
        //                    lineThickness: 2,
        //                    bullet: "round"
        //                }
        //            ],
        //            stockLegend: {
        //                valueTextRegular: " ",
        //                markerType: "none"
        //            }
        //        },
        //        {
        //            title: "Volume",
        //            percentHeight: 30,
        //            stockGraphs: [
        //                {
        //                    valueField: "volume",
        //                    type: "column",
        //                    cornerRadiusTop: 2,
        //                    fillAlphas: 1
        //                }
        //            ],
        //            stockLegend: {
        //                valueTextRegular: " ",
        //                markerType: "none"
        //            }
        //        }
        //    ],
        //    chartScrollbarSettings: {
        //        graph: "g1",
        //        usePeriod: "10mm",
        //        position: "bottom"
        //    },
        //    chartCursorSettings: {
        //        valueBalloonsEnabled: true
        //    },
        //    periodSelector: {
        //        position: "top",
        //        dateFormat: "YYYY-MM-DD JJ:NN",
        //        inputFieldWidth: 150,
        //        periods: [
        //            {
        //                period: "hh",
        //                count: 1,
        //                label: "1 hour",
        //                selected: true

        //            }, {
        //                period: "hh",
        //                count: 2,
        //                label: "2 hours"
        //            }, {
        //                period: "hh",
        //                count: 5,
        //                label: "5 hour"
        //            }, {
        //                period: "hh",
        //                count: 12,
        //                label: "12 hours"
        //            }, {
        //                period: "MAX",
        //                label: "MAX"
        //            }
        //        ]
        //    },
        //    panelsSettings: {
        //        usePrefixes: true
        //    }
        //});
        // #endregion

        this.$().fadeTo(App.ViewFadeTime, 1, done);
    },
    animateOut: function(done) {
        //this.chart.clear();

        this.$().fadeTo(App.ViewFadeTime, 0, done);
    }
});