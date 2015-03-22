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

App.ApplicationView = Ember.View.extend({
    animateIn: function(done) {

        // TODO: Clean this up
        var offset = 300,
            duration = 500;

        if (navigator.userAgent.match(/iPhone|iPad|iPod/i)) { // ios supported
            Ember.$(window).bind('touchend touchcancel touchleave', function(e) {
                if (Ember.$(this).scrollTop() > offset) {
                    Ember.$('.scroll-to-top').fadeIn(duration);
                } else {
                    Ember.$('.scroll-to-top').fadeOut(duration);
                }
            });
        } else { // general 
            Ember.$(window).scroll(function() {
                if (Ember.$(this).scrollTop() > offset) {
                    Ember.$('.scroll-to-top').fadeIn(duration);
                } else {
                    Ember.$('.scroll-to-top').fadeOut(duration);
                }
            });
        }


        var store = this.get('controller.store');

        // SignalR (order of priority, init this last)
        // TODO: Catch connection errors and report to user
        /*
            starting: Raised before any data is sent over the connection.
            received: Raised when any data is received on the connection. Provides the received data.
            connectionSlow: Raised when the client detects a slow or frequently dropping connection.
            reconnecting: Raised when the underlying transport begins reconnecting.
            reconnected: Raised when the underlying transport has reconnected.
            stateChanged: Raised when the connection state changes. Provides the old state and the new state (Connecting, Connected, Reconnecting, or Disconnected).
            disconnected: Raised when the connection has disconnected.
         */
        jQuery.connection.hub.connectionSlow(function() {
            //toastr.error('Real-time events response is slow.');
        });
        jQuery.connection.hub.reconnecting(function() {
            //toastr.error('Real-time events reconnecting.');
        });
        jQuery.connection.hub.disconnected(function() {
            //toastr.error('Real-time events disconnected');
            //setTimeout(function() {
            //    jQuery.connection.hub.start();
            //}, 5000); // Restart connection after 5 seconds.
        });

        var hub = jQuery.connection.eventHub;
        hub.client
            .newEvent = function(message) {
                store.push('liveEvent', message);
            };
        jQuery.connection.hub.logging = true;
        jQuery.connection.hub
            .start()
            .done(function() {
                // Hub started successfully
            })
            .fail(function() {
                // Hub failed to connect
            });


        done();
    },
    animateOut: function(done) {
        // SignalR Close Connection (priority, close this first)
        jQuery.connection.hub.stop();


        done();
    }
});