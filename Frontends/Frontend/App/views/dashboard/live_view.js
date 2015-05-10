// The MIT License (MIT)
// 
// Copyright (c) 2015 Daniel Franklin. http://blazedsp.com/
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

App.DashboardLiveView = Ember.View.extend({
    //gridstack: null,
    willAnimateIn: function() {
        this.$().css('opacity', 0);
    },
    animateIn: function(done) {
        // Uniform
        this.$('input[type="checkbox"]').uniform();


        // Gridstack
        //this.gridstack = this.$('.grid-stack').gridstack({
        //    cell_height: 80,
        //    vertical_margin: 30
        //});


        // fade effects
        this.$().fadeTo(App.ViewFadeTime, 1, done);
    },
    animateOut: function(done) {
        var self = this;
        this.$().fadeTo(App.ViewFadeTime, 0, function() {
            // Dispose Dridstack
            // TODO: reinit or destroying?
            //self.$('.grid-stack').gridstack().destroy();
            //self.gridstack.destroy();
            //self.gridstack.remove_all();

            // Dispose Uniform
            //self.$().uniform.restore('input[type="checkbox"]');

            done();
        });
    }
});