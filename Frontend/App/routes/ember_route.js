Ember.Route.reopen({
    render: function(name, options) {
        window.scrollTo(0, 0);
        // TODO: This is buggy when it fires but user tries to scroll
        //Ember.$('html, body').animate({ scrollTop: 0 }, 'slow');

        if (name != null) {
            return this._super(name, options);
        }
        return this._super();
    }
});