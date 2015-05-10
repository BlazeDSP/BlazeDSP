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

App.Flight = DS.Model.extend(Ember.Validations.Mixin, {
    validations: {
        name: {
            presence: true
        },
        description: {
            presence: true
        }

        //start_date: {
        //    presence: true
        //},
        //end_date: {
        //    presence: true
        //}
    },

    added_date: DS.attr('date'), // Readonly
    modified_date: DS.attr('date'), // Readonly

    name: DS.attr('string'),
    description: DS.attr('string'),

    start_date: DS.attr('date'),
    end_date: DS.attr('date'), // Null for indefinite

    active: DS.attr('boolean'),

    bid_cost: DS.attr('number'), // Decimal
    //bid_type: DS.belongsTo('bidType', { async: true }),
    bid_type: DS.belongsTo('bidType'),

    //creative: DS.belongsTo('creative', { async: true }), // TODO: Create Model

    //network: DS.belongsTo('network', { async: true }),
    network: DS.belongsTo('network'),

    //campaign: DS.belongsTo('campaign', { async: true }),
    campaign: DS.belongsTo('campaign'),

    //destinations: DS.hasMany('destination', { async: true })
    destinations: DS.hasMany('destination')

    //targets: DS.hasMany('target', { async: true }),
    //events: DS.hasMany('event', { async: true }),
    //actions: DS.hasMany('action', { async: true }),
    //tags: DS.hasMany('tag', { async: true }),
    //notes: DS.hasMany('note', { async: true })
});