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

App.DestinationsCreateController = Ember.ObjectController.extend({
    showUnsavedMessage: function() {
        return this.get('isDirty') && !this.get('isSaving');
    }.property('isDirty', 'isSaving'),

    actions: {
        submit: function() {
            Ember.$('#form-block').block();

            var self = this;

            this.get('model')
                .save()
                .then(function() {
                    self.transitionToRoute('destinations');
                }, function(error) {
                    toastr.error('Error Saving Destination');

                    Ember.$('#form-block').unblock();
                });
        },
        cancel: function() {
            // TODO: Use modal
            if (this.get('isDirty') && !confirm('Abandon unsaved changes?')) {
                return;
            }

            this.get('model').rollback();

            this.transitionToRoute('destinations');
        },

        delete: function() {
            if (!confirm('Delete this destination?')) {
                return;
            }

            var self = this,
                model = this.get('model');
            model.deleteRecord();
            model.save()
                .then(function() {
                    self.transitionToRoute('destinations');
                }, function(error) {
                    toastr.error('Error Deleting Destination');

                    Ember.$('#form-block').unblock();
                });
        }
    }
});