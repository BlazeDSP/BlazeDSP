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

App.Router.reopen({
    location: 'history'
});

App.Router.map(function() {
    this.resource('dashboard', { path: '/' }, function() {
        this.route('live', { path: '/dashboard/live' });

        this.route('campaign', { path: '/dashboard/campaigns' }, function () {
            this.route('read', { path: '/:id/view' }, function() {
                this.route('flight', { path: '/:fid/flight' });
            });
        });

        this.route('help');
        this.route('user');
    });

    this.resource('campaigns', function() {
        this.route('create');
        this.route('update', { path: '/:id/edit' });
    });

    this.resource('flights', function() {
        this.route('create');
        this.route('update', { path: '/:id/edit' });
    });

    this.resource('destinations', function() {
        this.route('create');
        this.route('update', { path: '/:id/edit' });
    });

    this.resource('networks', function() {
        this.route('create');
        this.route('update', { path: '/:id/edit' });
    });

    this.resource('offers', function () {
        this.route('create');
        this.route('update', { path: '/:id/edit' });
    });

    this.resource('system', function() {
        this.route('settings');
        this.route('management');
        this.route('nodes');
    });

    /* NO INTERNET */
    //this.route('no_internet');

    /* CATCH ALL */
    this.route('not_found', { path: '/*path' }); // Catch 404
});