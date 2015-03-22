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

// NOTE: This controller is responsible for the display of data (realtime or aggerated) for all campaigns (inlcuding flight data of campaigns).
App.DashboardIndexController = Ember.ArrayController.extend({
    // TODO: Use a filter w/ sort properties and move the realtime events to a named model.
    sortProperties: ['time'],
    sortAscending: false,

    // TODO: Pull this value from user settings.
    eventLimit: 10,

    // TODO: [OBSOLETE] Buffer results so there are 50 in the buffer but only show selected max (10, 25, 50)
    // NOTE: No need for buffer if the total number to display is pulled from user settings.
    contentArrayDidChange: function(array, idx, removedCount, addedCount) {
        this._super(array, idx, removedCount, addedCount);

        if (array.content.length > this.eventLimit) {
            array.removeObject(array.content.objectAt(0));
        }
    },


    // TODO: Clean this up (convert to mixin)
    clicks: 0,
    conversions: 0,
    cost: 0,
    revenue: 0,

    profit: function(asd, dsa) {
        if (this.get('revenue') === 0) {
            return 0;
        }
        return (this.get('revenue') - this.get('cost'));
    }.property('revenue', 'cost'),

    textColorProfit: function(asd) {
        if (this.get('profit') < 0) {
            return 'font-red';
        }
        if (this.get('profit') > 0) {
            return 'font-green';
        }
        return '';
    }.property('profit'),

    roi: function(asd, dsa) {
        if (this.get('revenue') === 0) {
            return 0;
        }
        // (return - cost) / cost
        // ($250 - $200) / $200
        return ((this.get('revenue') - this.get('cost')) / this.get('cost'));
    }.property('revenue', 'cost'),

    textColorRoi: function(asd) {
        if (this.get('roi') < 0) {
            return 'font-red';
        }
        if (this.get('roi') > 0) {
            return 'font-green';
        }
        return '';
    }.property('roi')
});