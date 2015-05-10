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

App.DashboardCampaignReadFlightController = Ember.ObjectController.extend({
    flight_test: function() {
        return 'http://' + App.get('system.node_tracking_domain') + '/t?__fid=' + this.get('params.fid') + '&__debug=1';
    }.property('params.fid'),

    actions: {
        fullscreen: function() {
            // FULLSCREEN
            //this.$('.fullscreen').on('click', function(e) {
            //    e.preventDefault();
            //    var portlet = $(this).closest('.portlet');
            //    if (portlet.hasClass('portlet-fullscreen')) {
            //        $(this).removeClass('on');
            //        portlet.removeClass('portlet-fullscreen');
            //        $('body').removeClass('page-portlet-fullscreen');
            //        portlet.children('.portlet-body').css('height', 'auto');
            //    } else {
            //        var height = Metronic.getViewPort().height -
            //            portlet.children('.portlet-title').outerHeight() -
            //            parseInt(portlet.children('.portlet-body').css('padding-top')) -
            //            parseInt(portlet.children('.portlet-body').css('padding-bottom'));

            //        $(this).addClass('on');
            //        portlet.addClass('portlet-fullscreen');
            //        $('body').addClass('page-portlet-fullscreen');
            //        portlet.children('.portlet-body').css('height', height);
            //    }
            //});

            //Ember.$('#fullscreener').toggleClass('portlet-fullscreen');

            //Ember.$('.dataTables_scrollBody').css('height', '750px');
        }
    }
});