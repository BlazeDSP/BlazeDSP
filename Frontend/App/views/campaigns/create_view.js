App.CampaignsCreateView = Ember.View.extend({
    willAnimateIn: function() {
        this.$().css('opacity', 0);
    },
    animateIn: function(done) {
        this.$('input[type="checkbox"]').uniform();
        //this.$('input[type="checkbox"],input[type="radio"]').uniform();
        this.$().fadeTo(App.ViewFadeTime, 1, done);
    },
    animateOut: function(done) {
        var self = this;
        this.$().fadeTo(App.ViewFadeTime, 0, function() {
            self.$().uniform.restore('input[type="checkbox"]');
            //self.$().uniform.restore('input[type="checkbox"],input[type="radio"]');
            done();
        });
    }
});