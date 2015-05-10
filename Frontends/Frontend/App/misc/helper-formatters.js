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

Ember.Handlebars.registerBoundHelper('format-date', function(date, format) {
    // NOTE: Converts UTC (system default) to local time.
    if (format !== null && typeof format === 'string') {
        return moment.utc(date).local().format(format);
    }
    return moment.utc(date).local().format('dddd, MMMM Do YYYY, h:mm:ss a');
});

Ember.Handlebars.registerBoundHelper('format-date-now', function(date) {
    return moment.utc(date).fromNow();
});

Ember.Handlebars.registerBoundHelper('format-number', function(number, format) {
    if (format !== null && typeof format === 'string') {
        return numeral(number).format(format);
    }
    return numeral(number).format('0,0.[0000]');
});

Ember.Handlebars.registerBoundHelper('format-currency', function(number, raw) {
    // TODO: Clean this
    if (raw !== null && typeof raw === 'boolean' && raw) {
        return numeral(number).format('$0,0.00[00]').replace('$', '');
    }
    return numeral(number).format('$0,0.00[00]');
});

Ember.Handlebars.registerBoundHelper('format-percent', function(number, raw) {
    // TODO: Clean this
    if (raw !== null && typeof raw === 'boolean' && raw) {
        return numeral(number).format('0.[0000]%').replace('%', '');
    }
    return numeral(number).format('0.[0000]%');
});

//Ember.Handlebars.helper('portlet', Ember.View.extend({
//    tagName: 'div',
//    classNames: ['portlet-body', 'h3'],
//    classNameBindings: ['positive:text-success', 'negative:text-danger'],
//    positive: false,
//    negative: false,
//    value: 0,
//    calculate: function() {
//        if (this.get('value') < 0) {
//            this.set('positive', false);
//            this.set('negative', true);
//        } else if (this.get('value') > 0) {
//            this.set('positive', true);
//            this.set('negative', false);
//        } else {
//            this.set('positive', false);
//            this.set('negative', false);
//        }
//    }.property('value')
//}));