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
App.DashboardLiveController = Ember.ObjectController.extend({
    // TODO: Pull this value from user settings.
    eventDisplayLimit: 10,
    conversionDisplayLimit: 10,

    initialize: function() {
        var self = this;
        this.get('liveEvents')
            .addArrayObserver({
                arrayWillChange: Ember.K,
                arrayDidChange: function(array, start, removedCount, addedCount) {
                    if (removedCount > 0 && addedCount < 1) {
                        return;
                    }

                    // TODO: This may have issues, since the number of items to add maybe > 1
                    self.incrementProperty('cost', self.get('liveEvents.lastObject.user_click_cost'));

                    self.incrementProperty('clicks');

                    if (array.content.length > self.get('eventDisplayLimit')) {
                        array.removeObject(array.content.objectAt(0));
                    }
                }
            });
        this.get('liveConversions')
            .addArrayObserver({
                arrayWillChange: Ember.K,
                arrayDidChange: function(array, start, removedCount, addedCount) {
                    if (removedCount > 0 && addedCount < 1) {
                        return;
                    }

                    // TODO: This may have issues, since the number of items to add maybe > 1
                    self.incrementProperty('revenue', self.get('liveConversions.lastObject.value'));

                    self.incrementProperty('conversions');

                    if (array.content.length > self.get('conversionDisplayLimit')) {
                        array.removeObject(array.content.objectAt(0));
                    }
                }
            });
    }.observes('model'),

    // TODO: Clean this up (convert to mixin)
    impressions: 0,
    cost: 0,
    clicks: 0,
    conversions: 0,
    revenue: 0,

    profit: function() {
        return (this.get('revenue') - this.get('cost'));
    }.property('revenue', 'cost'),
    textColorProfit: function() {
        if (this.get('profit') < 0) {
            return 'font-red';
        }
        if (this.get('profit') > 0) {
            return 'font-green';
        }
        return '';
    }.property('profit'),

    roi: function() {
        // (return - cost) / cost
        // ($250 - $200) / $200
        if (this.get('revenue') === 0 && this.get('cost') === 0) {
            return 0;
        }
        if (this.get('cost') === 0) {
            return 1;
        }
        return ((this.get('revenue') - this.get('cost')) / this.get('cost'));
    }.property('revenue', 'cost'),
    textColorRoi: function() {
        if (this.get('roi') < 0) {
            return 'font-red';
        }
        if (this.get('roi') > 0) {
            return 'font-green';
        }
        return '';
    }.property('roi'),


    ctr: function() {
        // CTR = Clicks / Impressions
        if (this.get('impressions') === 0) {
            return 0;
        }
        if (this.get('clicks') > this.get('impressions')) {
            console.error('clicks > impressions');
            return 0;
        }
        return (this.get('clicks') / this.get('impressions'));
    }.property('clicks', 'impressions'),

    cpc: function() {
        // [IF CPM] CPC = CPM / 1,000 x CTR
        // [IF CPC/CPV] return cost
        return 0.25;
    }.property('ctr'),

    conversion_rate: function() {
        // CR = Conversions / Clicks
        if (this.get('clicks') === 0) {
            return 0;
        }
        return (this.get('conversions') / this.get('clicks'));
    }.property('conversions', 'clicks'),

    cpa: function() {
        // CPA = CPC / CR
        if (this.get('conversion_rate') === 0) {
            return 0;
        }
        return (this.get('cpc') / this.get('conversion_rate'));
    }.property('cpc', 'conversion_rate'),

    // NOTE: DEBUG
    actions: {
        increment: function() {
            this.incrementProperty('impressions', 132);
        }
    }
});