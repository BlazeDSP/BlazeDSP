App.CampaignsCreateRoute = Ember.Route.extend({
    model: function() {
        return this.store.createRecord('campaign');
    },
    setupController: function(controller, model) {
        controller.set('model', model);
        this.store.find('network').then(function(item) {
            controller.set('networkList', item);
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