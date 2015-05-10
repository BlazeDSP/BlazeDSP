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

App.SystemManagementView = Ember.View.extend({
    dzone: null,
    willAnimateIn: function() {
        this.$().css('opacity', 0);

        var self = this;

        this.dzone = new window.Dropzone('#dropzone', {
            url: '/api/managementcertificates',
            maxFilesize: 1,
            acceptedFiles: '.publishsettings',
            dictDefaultMessage: 'Drop publish settings files (.publishsettings) here to upload'
        });
        this.dzone.on('error', function(file, message) {
            toastr.error('Error Uploading File');
        });
        this.dzone.on('complete', function(file) {
            if (file.status !== 'success') {
                return;
            }

            var data = JSON.parse(file.xhr.response);
            if (data.management_certificates) {
                data.management_certificates
                    .forEach(function(item) {
                        var store = self.get('controller.store');
                        store.push('managementCertificate', store.normalize('managementCertificate', item));
                    });
            }
        });
    },
    animateIn: function(done) {
        this.$().fadeTo(App.ViewFadeTime, 1, done);
    },
    animateOut: function(done) {
        if (this.dzone !== null) {
            this.dzone.destroy();
        }
        this.$().fadeTo(App.ViewFadeTime, 0, done);
    }
});