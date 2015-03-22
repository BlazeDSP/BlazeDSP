App.FlightsRoute = Ember.Route.extend({
    model: function() {
        return this.store.filter('flight', {}, function(model) {
            return !(model.get('isNew'));
        });
    }
});