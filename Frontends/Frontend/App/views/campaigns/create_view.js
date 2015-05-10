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

App.CampaignsCreateView = Ember.View.extend({
    willAnimateIn: function() {
        this.$().css('opacity', 0);
    },
    animateIn: function (done) {
        var self = this;

        this.$('input[type="checkbox"]').uniform();

        //this.$('input[type="checkbox"],input[type="radio"]').uniform();

        window.autosize(this.$('textarea'));

        this.$('[name="start_date"]').datetimepicker();
        if (!Ember.isEmpty(this.get('controller.model.start_date'))) {
            this.$('[name="start_date"]').data('DateTimePicker').date(this.get('controller.model.start_date'));
        }

        this.$('[name="end_date"]').datetimepicker();
        if (!Ember.isEmpty(this.get('controller.model.end_date'))) {
            this.$('[name="end_date"]').data('DateTimePicker').date(this.get('controller.model.end_date'));
        }

        this.$('[name="tags"]')
        //this.$('#tags')
            .tagsInput({
                //'autocomplete_url': '/api/tags',
                //'autocomplete': { option: value, option: value },
                'height': 'auto',
                'width': 'auto',
                //'interactive': true,
                //'defaultText': 'add a tag',
                //'onAddTag': function(tag) {
                //    alert('onAddTag: ' + tag);
                //},
                //'onRemoveTag': function(tag) {
                //    alert('onRemoveTag: ' + tag);
                //},
                //'onChange': function(tag) {
                //    alert('onChange: ' + tag);
                //},
                //'delimiter': [',', ';'],
                //'removeWithBackspace': true,
                //'minChars': 0,
                //'maxChars': 0, //if not provided there is no limit
                //'placeholderColor': '#666666'
            });
        //this.get('controller.tags').forEach(function(item) {
        //    self.$('#tags').addTag(item.get('name'));
        //});
        this.$('[name="tags"]').importTags('google,facebook,display');

        this.$().fadeTo(App.ViewFadeTime, 1, done);
    },
    animateOut: function(done) {
        var self = this;
        this.$().fadeTo(App.ViewFadeTime, 0, function() {
            self.$().uniform.restore('input[type="checkbox"]');

            //self.$().uniform.restore('input[type="checkbox"],input[type="radio"]');

            done();
        });
    }
});