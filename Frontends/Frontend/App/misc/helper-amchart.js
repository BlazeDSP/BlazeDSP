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

//Ember.Handlebars.helper('amchart-stock', Ember.View.extend({
//    tagName: 'div',

//    template: Ember.Handlebars.compile('Chart Not Loaded'),

//    attributeBindings: ['style'],
//    style: '',

//    chartData: [],

//    chartObject: null,

//    didInsertElement: function() {
//            this.chartObject = AmCharts.makeChart(this.get('elementId'), {
//                type: 'stock',
//                theme: 'none',
//                pathToImages: '//www.amcharts.com/lib/3/images/',
//                categoryAxesSettings: {
//                    minPeriod: 'mm'
//                },
//                dataSets: [
//                    {
//                        color: '#578ebe',
//                        fieldMappings: [
//                            {
//                                fromField: 'value',
//                                toField: 'value'
//                            }, {
//                                fromField: 'volume',
//                                toField: 'volume'
//                            }
//                        ],
//                        dataProvider: this.get('chartData'),
//                        categoryField: 'date'
//                    }
//                ],
//                panels: [
//                    {
//                        showCategoryAxis: false,
//                        title: 'Value',
//                        percentHeight: 70,
//                        stockGraphs: [
//                            {
//                                id: 'g1',
//                                valueField: 'value',
//                                type: 'smoothedLine',
//                                lineThickness: 2,
//                                bullet: 'round'
//                            }
//                        ],
//                        stockLegend: {
//                            valueTextRegular: ' ',
//                            markerType: 'none'
//                        }
//                    },
//                    {
//                        title: 'Volume',
//                        percentHeight: 30,
//                        stockGraphs: [
//                            {
//                                valueField: 'volume',
//                                type: 'column',
//                                cornerRadiusTop: 2,
//                                fillAlphas: 1
//                            }
//                        ],
//                        stockLegend: {
//                            valueTextRegular: ' ',
//                            markerType: 'none'
//                        }
//                    }
//                ],
//                chartScrollbarSettings: {
//                    graph: 'g1',
//                    usePeriod: '10mm',
//                    position: 'bottom'
//                },
//                chartCursorSettings: {
//                    valueBalloonsEnabled: true
//                },
//                periodSelector: {
//                    position: 'top',
//                    dateFormat: 'YYYY-MM-DD JJ:NN',
//                    inputFieldWidth: 150,
//                    periods: [
//                        {
//                            period: 'fff',
//                            count: 1,
//                            label: '1 millisecond'
//                        }, {
//                            period: 'ss',
//                            count: 1,
//                            label: '1 second',
//                            selected: true

//                        }, {
//                            period: 'mm',
//                            count: 1,
//                            label: '1 minute'
//                        }, {
//                            period: 'hh',
//                            count: 5,
//                            label: '5 hour'
//                        }, {
//                            period: 'hh',
//                            count: 12,
//                            label: '12 hours'
//                        }, {
//                            period: 'MAX',
//                            label: 'MAX'
//                        }
//                    ]
//                },
//                panelsSettings: {
//                    usePrefixes: true
//                }
//            });
//    },
//    willDestroyElement: function() {
//        if (this.chartObject !== null) {
//            this.chartObject.clear();
//        }
//    }
//}));

// TODO: Clean up this CODE!
Ember.Handlebars.helper('amchart-live', Ember.View.extend({
    tagName: 'div',

    template: Ember.Handlebars.compile('Chart Not Loaded'),

    attributeBindings: ['style'],
    style: '',

    chartImpression: [],
    chartEvent: [],
    chartConversion: [],

    chartData: [],

    totalEvents: function() {
        return 50; // Total number of events to show on chart
    }.property(),
    loopInterval: function() {
        return 1 * 1000; // in milliseconds
    }.property(),

    moment: function() {
        return new Date(new Date().getTime() + this.get('loopInterval'));
    }.property(),

    impressionCounter: 0,
    eventCounter: 0,
    conversionCounter: 0,

    runloop: function() {
        // NOTE: Not needed for/when prepop'in
        //if (this.get('liveChart').dataProvider.length >= this.get('totalEvents')) {
        //    this.get('liveChart').dataProvider.shift();
        //}
        this.get('liveChart').dataProvider.shift();

        this.get('liveChart')
            .dataProvider
            .push({
                date: this.get('moment'),
                impressions: this.impressionCounter,
                events: this.eventCounter,
                conversions: this.conversionCounter
            });

        this.get('liveChart').validateData();

        var loopInterval = this.get('loopInterval');

        this.set('moment', new Date(new Date().getTime() + loopInterval));

        this.impressionCounter = 0;
        this.eventCounter = 0;
        this.conversionCounter = 0;

        this.set('timer', Ember.run.later(this, this.runloop, loopInterval));
    },

    impressionWatcher: function() {
        // TODO: Test this (there *may* be some minor data loss)
        if (new Date() < this.get('moment')) {
            this.incrementProperty('impressionCounter');
        }
    }.observes('chartImpression.@each'),

    eventWatcher: function() {
        // TODO: Test this (there *may* be some minor data loss)
        if (new Date() < this.get('moment')) {
            this.incrementProperty('eventCounter');
        }
    }.observes('chartEvent.@each'),

    conversionWatcher: function() {
        // TODO: Test this (there *may* be some minor data loss)
        if (new Date() < this.get('moment')) {
            this.incrementProperty('conversionCounter');
        }
    }.observes('chartConversion.@each'),

    didInsertElement: function() {
        // Get settings
        var totalEvents = this.get('totalEvents');
        var loopInterval = this.get('loopInterval');

        // TODO: Populate from store/cache (if it has data)
        // Prepopulate chart with empty data
        var date = new Date(new Date().getTime() - (loopInterval * totalEvents));
        for (var i = 0; i < totalEvents; i++) {
            this.chartData.push({
                date: date,
                events: 0,
                conversions: 0
            });
            date = new Date(date.getTime() + loopInterval);
        }


        // #region amChart
        //AmCharts.ready(function() {
        // SERIAL CHART
        this.set('liveChart', new window.AmCharts.AmSerialChart());
        var chartObject = this.get('liveChart');

        chartObject.pathToImages = 'http://www.amcharts.com/lib/images/';
        chartObject.marginTop = 0;
        chartObject.marginRight = 10;
        chartObject.autoMarginOffset = 5;
        chartObject.zoomOutButton = {
            backgroundColor: '#000000',
            backgroundAlpha: 0.15
        };
        chartObject.dataProvider = this.chartData;
        chartObject.categoryField = 'date';

        // AXES
        // category
        var categoryAxis = chartObject.categoryAxis;
        //categoryAxis.equalSpacing = true; // Evens out realtime chart data
        categoryAxis.parseDates = true;
        categoryAxis.minPeriod = 'ss';
        categoryAxis.dashLength = 2;
        categoryAxis.gridAlpha = 0.15;
        categoryAxis.axisColor = '#dadada';

        // value                
        var valueAxis = new window.AmCharts.ValueAxis();
        valueAxis.axisAlpha = 0.2;
        valueAxis.dashLength = 2;
        valueAxis.precision = 0;
        chartObject.addValueAxis(valueAxis);

        // GRAPH - Conversions
        var graphConversions = new window.AmCharts.AmGraph();
        graphConversions.title = 'Conversions';
        graphConversions.valueField = 'conversions';
        graphConversions.bullet = 'round';
        graphConversions.bulletBorderColor = '#ffffff';
        graphConversions.bulletBorderThickness = 2;
        graphConversions.lineThickness = 2;
        graphConversions.lineColor = '#35aa47';     // Green
        //graphConversions.lineColor = '#20a46d';   // Red
        //graphConversions.negativeLineColor = '#b5030d';
        //graphConversions.hideBulletsCount = totalEvents;
        chartObject.addGraph(graphConversions);

        // GRAPH - Conversions
        var graphImpressions = new window.AmCharts.AmGraph();
        graphImpressions.title = 'Impressions';
        graphImpressions.valueField = 'impressions';
        graphImpressions.bullet = 'round';
        graphImpressions.bulletBorderColor = '#ffffff';
        graphImpressions.bulletBorderThickness = 2;
        graphImpressions.lineThickness = 2;
        graphImpressions.lineColor = '#555555';     // Dark Grey
        //graphImpressions.negativeLineColor = '#b5030d';
        //graphImpressions.hideBulletsCount = totalEvents;
        chartObject.addGraph(graphImpressions);

        // GRAPH - Events
        var graphEvents = new window.AmCharts.AmGraph();
        graphEvents.title = 'Events';
        graphEvents.valueField = 'events';
        graphEvents.bullet = 'round';
        graphEvents.bulletBorderColor = '#ffffff';
        graphEvents.bulletBorderThickness = 2;
        graphEvents.lineThickness = 2;
        graphEvents.lineColor = '#4b77be';          // Blue
        //graphEvents.lineColor = '#e87e04';        // Yellow Gold
        //graphEvents.negativeLineColor = '#b5030d';
        //graphEvents.hideBulletsCount = totalEvents;
        chartObject.addGraph(graphEvents);

        // CURSOR
        var chartCursor = new window.AmCharts.ChartCursor();
        chartCursor.categoryBalloonDateFormat = 'JJ:NN:SS';
        chartCursor.cursorPosition = 'mouse';
        chartObject.addChartCursor(chartCursor);

        //// SCROLLBAR
        //var chartScrollbar = new AmCharts.ChartScrollbar();
        //chartScrollbar.graph = graph;
        //chartScrollbar.scrollbarHeight = 40;
        //chartScrollbar.color = '#FFFFFF';
        //chartScrollbar.autoGridCount = true;
        //chart.addChartScrollbar(chartScrollbar);

        // WRITE
        chartObject.write(this.get('elementId'));
        //});
        // #endregion


        // Setup runloop
        this.set('timer', Ember.run.later(this, this.runloop, loopInterval));
    },

    willDestroyElement: function() {
        if (!Ember.isEmpty(this.get('liveChart'))) {
            this.get('liveChart').clear();
        }
        // Kill runloop
        if (!Ember.run.cancel(this.get('timer'))) {
            console.error('Can not dispose "runloop"');
        }
    }
}));