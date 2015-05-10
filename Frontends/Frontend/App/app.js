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

window.blazed = window.blazed || false;

App = Ember.Application.create({
    LOG_STACKTRACE_ON_DEPRECATION: window.blazed,
    LOG_BINDINGS: window.blazed,
    LOG_TRANSITIONS: window.blazed,
    LOG_TRANSITIONS_INTERNAL: window.blazed,
    LOG_VIEW_LOOKUPS: window.blazed,
    LOG_ACTIVE_GENERATION: window.blazed,

    // Globals
    ViewFadeTime: 250
});

App.ApplicationAdapter = DS.RESTAdapter.extend({
    namespace: 'api'
});


// Loading Message
//Ember.Application.initializer({
//    name: 'loadingText',
//    after: 'store',
//    initialize: function(container) {
//        Ember.$('#loadingText').text('Loading Realtime & Preloading Data');
//    }
//});

// Init SignalR
Ember.Application.initializer({
    name: 'initSignalR',
    after: 'store',
    initialize: function(container) {
        var store = container.lookup('store:main');

        // Block App from loading
        App.deferReadiness();

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
        //jQuery.connection.hub.connectionSlow(function() {
        //    //toastr.error('Real-time events response is slow.');
        //});
        //jQuery.connection.hub.reconnecting(function() {
        //    //toastr.error('Real-time events reconnecting.');
        //});
        //jQuery.connection.hub.disconnected(function() {
        //    //toastr.error('Real-time events disconnected');
        //    //setTimeout(function() {
        //    //    jQuery.connection.hub.start();
        //    //}, 5000); // Restart connection after 5 seconds.
        //});

        jQuery.connection
            .eventHub
            .client
            .newEvent = function(message) {
                store.push('liveEvent', message);
            };
        jQuery.connection
            .conversionHub
            .client
            .newConversion = function(message) {
                store.push('liveConversion', message);
            };
        jQuery.connection.hub.logging = window.blazed;
        jQuery.connection
            .hub
            .start()
            .done(function() {
                // Rlease Block
                App.advanceReadiness();
            })
            .fail(function() {
                // Raise Error
                toastr.error('Failed connecting to realtime events');
                // Rlease Block
                App.advanceReadiness();
            });

        //jQuery.connection.hub.stop();

        //Ember.$('#loadingText').html('Realtime Loaded Successfully');
    }
});

// Preload Data
Ember.Application.initializer({
    name: 'preloadData',
    after: 'store',
    initialize: function(container) {
        var store = container.lookup('store:main');

        // Global Data
        App.deferReadiness();
        store.find('account', 1).then(function(item) {
            App.set('account', item);
            App.advanceReadiness();
        });

        App.deferReadiness();
        store.find('system', 1).then(function(item) {
            App.set('system', item);
            App.advanceReadiness();
        });

        // TODO: This isn't the best way to handle loading of this data
        // Account Related Data
        App.deferReadiness();
        store.find('campaign').then(function() {
            App.advanceReadiness();
        });

        App.deferReadiness();
        store.find('flight').then(function() {
            App.advanceReadiness();
        });

        App.deferReadiness();
        store.find('destination').then(function() {
            App.advanceReadiness();
        });

        App.deferReadiness();
        store.find('network').then(function() {
            App.advanceReadiness();
        });

        // Misc
        App.deferReadiness();
        store.find('bid-type').then(function() {
            App.advanceReadiness();
        });
        App.deferReadiness();
        store.find('creative-type').then(function() {
            App.advanceReadiness();
        });
        App.deferReadiness();
        store.find('destination-type').then(function() {
            App.advanceReadiness();
        });
    }
});