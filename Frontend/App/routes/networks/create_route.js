App.NetworksCreateRoute = Ember.Route.extend({
    model: function() {
        return this.store.createRecord('network');
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