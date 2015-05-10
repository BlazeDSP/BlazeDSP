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

App.DashboardIndexView = Ember.View.extend({
    willAnimateIn: function() {
        this.$().css('opacity', 0);
    },
    animateIn: function(done) {
        window.AmCharts.makeChart('chart_top_countries',
        {
            "type": 'pie',

            "pathToImages": 'http://cdn.amcharts.com/lib/3/images/',
            "balloonText": '[[title]]<br><span style=\'font-size:14px\'><b>[[value]]</b> ([[percents]]%)</span>',

            "innerRadius": '60%',

            //"angle": 60,
            //"depth3D": 4,

            "marginTop": -40,
            "marginBottom": -50,
            //"marginLeft": 0,
            //"marginRight": 0,

            "labelsEnabled": false,

            "pullOutDuration": 0,

            "sequencedAnimation": false,

            "startDuration": 0,

            "titleField": 'category',
            "valueField": 'column-1',

            "allLabels": [],

            //"amExport": {
            //    "buttonAlpha": 1,
            //    "exportJPG": true,
            //    "exportPDF": true,
            //    "exportSVG": true,
            //    "imageFileName": 'country_chart'
            //},

            "balloon": {},

            "titles": [],

            "dataProvider": [
                {
                    "category": 'United States',
                    "column-1": 125125
                },
                {
                    "category": 'Cananda',
                    "column-1": 65656
                },
                {
                    "category": 'Australia',
                    "column-1": 35635
                },
                {
                    "category": 'France',
                    "column-1": 23232
                },
                {
                    "category": 'Other',
                    "column-1": 12011
                }
            ]
        });

        window.AmCharts.makeChart('chart_top_os',
        {
            "type": 'pie',

            "pathToImages": 'http://cdn.amcharts.com/lib/3/images/',
            "balloonText": '[[title]]<br><span style=\'font-size:14px\'><b>[[value]]</b> ([[percents]]%)</span>',

            "innerRadius": '60%',

            //"angle": 60,
            //"depth3D": 4,

            "marginTop": -40,
            "marginBottom": -50,
            //"marginLeft": 0,
            //"marginRight": 0,

            "labelsEnabled": false,

            "pullOutDuration": 0,

            "sequencedAnimation": false,

            "startDuration": 0,

            "titleField": 'category',
            "valueField": 'column-1',

            "allLabels": [],

            //"amExport": {
            //    "buttonAlpha": 1,
            //    "exportJPG": true,
            //    "exportPDF": true,
            //    "exportSVG": true,
            //    "imageFileName": 'country_chart'
            //},

            "balloon": {},

            "titles": [],

            "dataProvider": [
                {
                    "category": 'Windows',
                    "column-1": 125125
                },
                {
                    "category": 'Apple',
                    "column-1": 65656
                },
                {
                    "category": 'Linux',
                    "column-1": 35635
                },
                {
                    "category": 'BeeOS',
                    "column-1": 23232
                },
                {
                    "category": 'Other',
                    "column-1": 12011
                }
            ]
        });

        this.$('.scroller')
            .slimScroll({
                height: '250px'
            });

        //this.$('input[type="checkbox"],input[type="radio"]').uniform();
        this.$('input[type="checkbox"]').uniform();

        this.$().fadeTo(App.ViewFadeTime, 1, done);
    },
    animateOut: function(done) {
        //var self = this;
        this.$().fadeTo(App.ViewFadeTime, 0, function() {
            //self.$().uniform.restore('input[type="checkbox"], input[type="radio"]');
            done();
        });
    }
});