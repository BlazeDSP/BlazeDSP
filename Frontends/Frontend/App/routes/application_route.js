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

App.ApplicationRoute = Ember.Route.extend({
    actions: {
        // Catch API Errors
        error: function(error, transition) {
            switch (error.status) {
                case 0:
                    // No Internet (timeout, etc)
                    //return this.transitionTo('no_internet');
                    toastr.error('Error: No Internet Connection');
                    break;
                case 401:
                    // 401 Unauthorized
                    toastr.error('Error: 401 Unauthorized');
                    break;
                case 403:
                    // 403 Forbidden
                    toastr.error('Error: 403 Forbidden');
                    break;
                case 404:
                    // 404 Not Found
                    return this.transitionTo('not_found', transition.intent.url.replace(/^(\/)/, ''));
                case 405:
                    // 405 Method Not Allowed
                    toastr.error('Error: 405 Method Not Allowed');
                    break;
                case 500:
                    // 500 Internal Server Error
                    toastr.error('Error: 500 Internal Server Error');
                    break;
                case 501:
                    // 501 Not Implemented
                    toastr.error('Error: 501 Not Implemented');
                    break;
                case 502:
                    // 502 Bad Gateway
                    toastr.error('Error: 502 Bad Gateway');
                    break;
                case 503:
                    // 503 Service Unavailable
                    toastr.error('Error: 503 Service Unavailable');
                    break;
                case 504:
                    // 504 Gateway Timeout
                    toastr.error('Error: 504 Gateway Timeout');
                    break;
            }
            // Bubble Error
            return true;
        }
    }
});