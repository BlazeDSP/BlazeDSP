App.CampaignsUpdateRoute = Ember.Route.extend({
    model: function(params) {
        return this.store.find('campaign', params.id);
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
    },
    renderTemplate: function(controller, model) {
        this.render('campaigns.create', {
            controller: 'campaigns.create',
            model: model
            //view: 'items.edit'
        });
    }
});