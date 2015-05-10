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

// -- Ember EasyForm | Cancel Button
Ember.EasyForm.Config.registerInputType('cancel', Ember.EasyForm.Cancel);
Ember.EasyForm.Cancel = Ember.EasyForm.BaseView.extend({
    tagName: 'button',
    template: Ember.Handlebars.compile('{{text}}'),
    attributeBindings: ['type', 'disabled'],
    classNameBindings: ['wrapperConfig.cancelClass'],
    type: 'button',
    click: function() {
        this.get('controller').send('cancel');
    },
    init: function() {
        this._super();
        this.set('formForModel.text', this.value);
    }
});

Ember.Handlebars.registerHelper('cancel', function(value, options) {
    if (typeof (value) === 'object') {
        options = value;
        value = undefined;
    }

    options.hash.context = this;
    options.hash.value = value || 'Cancel';

    Ember.Handlebars.helpers.view.call(this, Ember.EasyForm.Cancel, options);
});