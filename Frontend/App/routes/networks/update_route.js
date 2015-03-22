App.NetworksUpdateRoute = Ember.Route.extend({
    model: function(params) {
        return this.store.find('network', params.id);
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
        this.render('networks.create', {
            controller: 'networks.create',
            model: model
            //view: 'items.edit'
        });
    }
});