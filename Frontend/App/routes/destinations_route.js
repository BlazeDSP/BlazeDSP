App.DestinationsRoute = Ember.Route.extend({
    model: function() {
        return this.store.filter('destination', {}, function (model) {
            return !(model.get('isNew'));
        });
    }
});