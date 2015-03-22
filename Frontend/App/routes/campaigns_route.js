App.CampaignsRoute = Ember.Route.extend({
    model: function() {
        return this.store.filter('campaign', {}, function(model) {
            return !(model.get('isNew'));
        });
    }
});