App.ApplicationRoute = Ember.Route.extend({
    setupController: function(controller, model) {
        // TODO: Update to Baerer Tokens
        // NOTE: ID is just to return a single record, it's not the actual user ID.
        this.store.find('account', 1).then(function(item) {
            App.set('account', item);
        });
        // NOTE: Pull system settings
        this.store.find('setting', 1).then(function(item) {
            App.set('settings', item);
        });

        // TODO: Remove preloading
        this.store.find('campaign');
        this.store.find('flight');
        this.store.find('destination');
    },

    actions: {
        // Catch API Errors
        error: function(error, transition) {
            switch (error.status) {
            case 0:
                // No Internet (timeout, etc)
                break;
            case 401:
                // 401 Unauthorized
                // TODO: Clean this up
                alert('401 Unauthorized');
                break;
            case 403:
                // 403 Forbidden
                break;
            case 404:
                // 404 Not Found
                return this.transitionTo('not_found', transition.intent.url.replace(/^(\/)/, ''));
            case 405:
                // 405 Method Not Allowed
                break;
            case 500:
                // 500 Internal Server Error
                break;
            case 501:
                // 501 Not Implemented
                break;
            case 502:
                // 502 Bad Gateway
                break;
            case 503:
                // 503 Service Unavailable
                break;
            case 504:
                // 504 Gateway Timeout
                break;
            }
            // Bubble Error
            return true;
        }
    }
});