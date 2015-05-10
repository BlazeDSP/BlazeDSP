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

App.DashboardCampaignReadFlightView = Ember.View.extend({
    chart: null,

    willAnimateIn: function() {
        this.$().css('opacity', 0);
    },
    animateIn: function(done) {
        var self = this;

        //// #region amCharts
        //// generate random data
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
        //            color: "#578ebe",
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
        //// #endregion

        // #region amMap
        // create AmMap object
        var map = new window.AmCharts.AmMap();
        // set path to images
        map.pathToImages = '//www.amcharts.com/lib/3/images/';

        /* create data provider object
            map property is usually the same as the name of the map file.

            getAreasFromMap indicates that amMap should read all the areas available
            in the map data and treat them as they are included in your data provider.
            in case you don't set it to true, all the areas except listed in data
            provider will be treated as unlisted.
        */
        var dataProvider = {
            map: 'worldLow',
            getAreasFromMap: true
        };
        // pass data provider to the map object
        map.dataProvider = dataProvider;

        /* create areas settings
            * autoZoom set to true means that the map will zoom-in when clicked on the area
            * selectedColor indicates color of the clicked area.
            */
        map.areasSettings = {
            autoZoom: false,
            color: '#578ebe',
            unlistedAreasColor: '#578ebe',
            selectedColor: '#4B77BE',
            rollOverOutlineColor: '#4B77BE',
            rollOverColor: '#4B77BE'
        };

        map.zoomControl = {
            "zoomControlEnabled": false,
            panControlEnabled: false
        };

        // let's say we want a small map to be displayed, so let's create it
        map.smallMap = new AmCharts.SmallMap();

        // write the map to container div
        map.write('mapdiv');
        // #endregion

        // DATATABLES
        this.set('controller.datatable',
            this.$('#events_table').DataTable({
                order: [[1, 'desc']],

                autoWidth: false,
                //scrollY: '100px',
                //scrollCollapse: false,

                processing: true,

                //deferRender: true,

                ajax: {
                    url: '/api/events?flight=' + self.get('controller.params.fid'),
                    dataSrc: function(json) {
                        json.data = [];
                        for (var i = 0, ien = json.events.length; i < ien; i++) {
                            var intarr = [
                                json.events[i].id,
                                //json.events[i].time,
                                moment.utc(json.events[i].time).local().format('dddd, MMMM Do YYYY, h:mm:ss a'),
                                json.events[i].user_id,
                                json.events[i].user_agent,
                                json.events[i].user_host_address,
                                json.events[i].user_proxy_address,
                                json.events[i].flight,
                                json.events[i].destination
                            ];
                            json.data.push(intarr);
                        }
                        return json.data;
                    }
                }
            }));


        this.$().fadeTo(App.ViewFadeTime, 1, done);
    },
    animateOut: function(done) {
        //this.chart.clear();

        this.get('controller.datatable').destroy();

        this.$().fadeTo(App.ViewFadeTime, 0, done);
    }
});