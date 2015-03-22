App.NetworksRoute = Ember.Route.extend({
    model: function() {
        return this.store.filter('network', {}, function(model) {
            return !(model.get('isNew'));
        });
    }
});