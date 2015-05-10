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

App.DashboardView = Ember.View.extend({
    willAnimateIn: function() {
        this.$().css('opacity', 0);
    },
    animateIn: function (done) {
        //this.$('#fixedscroll').scrollToFixed({
        //    marginTop: 30
        //});


        AmCharts.makeChart("sparklinechart", {
            "type": "serial",
            "theme": "none",
            "pathToImages": "http://www.amcharts.com/lib/3/images/",
            "dataProvider": [{
                "lineColor": "#b7e021",
                "date": "2012-01-01",
                "duration": 408
            }, {
                "date": "2012-01-02",
                "duration": 482
            }, {
                "date": "2012-01-03",
                "duration": 562
            }, {
                "date": "2012-01-04",
                "duration": 379
            }, {
                "lineColor": "#fbd51a",
                "date": "2012-01-05",
                "duration": 501
            }, {
                "date": "2012-01-06",
                "duration": 443
            }, {
                "date": "2012-01-07",
                "duration": 405
            }, {
                "date": "2012-01-08",
                "duration": 309,
                "lineColor": "#2498d2"
            }, {
                "date": "2012-01-09",
                "duration": 287
            }, {
                "date": "2012-01-10",
                "duration": 485
            }, {
                "date": "2012-01-11",
                "duration": 890
            }, {
                "date": "2012-01-12",
                "duration": 810
            }],
            "balloon": {
                "cornerRadius": 6
            },
            "graphs": [{
                "bullet": "square",
                "bulletBorderAlpha": 1,
                "bulletBorderThickness": 1,
                "fillAlphas": 0.3,
                "fillColorsField": "lineColor",
                "lineColorField": "lineColor",
                "valueField": "duration"
            }],
            "dataDateFormat": "YYYY-MM-DD",
            "categoryField": "date",
            "categoryAxis": {
                "labelsEnabled": false,
                "axisAlpha": 0,
                "gridAlpha": 0
            },
            "valueAxes": [
                {
                    "labelsEnabled": false,
                    "axisAlpha": 0,
                    "gridAlpha": 0
                }
            ]
        });


        this.$().fadeTo(App.ViewFadeTime, 1, done);
    },
    animateOut: function(done) {
        this.$().fadeTo(App.ViewFadeTime, 0, done);
    }
});