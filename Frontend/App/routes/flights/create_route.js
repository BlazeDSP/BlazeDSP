App.FlightsCreateRoute = Ember.Route.extend({
    model: function() {
        return this.store.createRecord('flight');
    },
    setupController: function(controller, model) {
        controller.set('model', model);

        this.store.find('campaign').then(function(item) {
            controller.set('campaignList', item);
        });

        this.store.find('network').then(function(item) {
            controller.set('networkList', item);
        });

        this.store.find('bid_type').then(function(item) {
            controller.set('bidtypeList', item);
        });
    },
    actions: {
        willTransition: function(transition) {
            if (this.controller.get('isDirty') && !confirm('Abandon unsaved changes?')) {
                return transition.abort();
            }
            this.controller.get('model').rollback();
            return true;
        }
    }
});