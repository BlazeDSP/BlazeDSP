Ember.Handlebars.helper('tab-li', Ember.View.extend({
    tagName: 'li',
    classNameBindings: ['active'],
    active: function() {
        return this.get('childViews.firstObject.active');
    }.property('childViews.firstObject.active')
}));

Ember.Handlebars.helper('tab-a', Ember.View.extend({
    tagName: 'a',
    classNames: ['uppercase'],
    attributeBindings: ['href', 'toggle:data-toggle'],
    toggle: 'tab',
    link: null,
    href: function() {
        return '#' + this.get('link');
    }.property('link')
}));