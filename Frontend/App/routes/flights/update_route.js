App.FlightsUpdateRoute = Ember.Route.extend({
    model: function(params) {
        return this.store.find('flight', params.id);
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
    },
    renderTemplate: function(controller, model) {
        this.render('flights.create', {
            controller: 'flights.create',
            model: model
            //view: 'items.edit'
        });
    }
});