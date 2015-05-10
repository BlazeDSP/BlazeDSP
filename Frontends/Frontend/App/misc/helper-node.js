Ember.Handlebars.helper('node-portlet', Ember.View.extend({
    tagName: 'div',
    classNames: ['portlet', 'box'],
    classNameBindings: ['portletStyle'],
    portletValue: null,
    portletStatus: null,
    portletStyle: function() {
        var value = this.get('portletValue');
        var status = this.get('portletStatus');

        if (status === 'BusyRole') {
            return 'grey-cascade';
        }
        //if (status === 'RoleStateUnknown') {
        //    return 'yellow-crusta';
        //}

        if (value === 'starting') {
            return 'blue-madison';
        }
        if (value === 'started') {
            return 'green-seagreen';
        }
        if (value === 'stopped') {
            return 'grey-cascade';
        }
        if (value === 'stopping') {
            return 'grey-cascade';
        }
        if (value === 'unknown') {
            return 'yellow-crusta';
        }

        return '';
    }.property('portletValue', 'portletStatus')
}));

Ember.Handlebars.helper('node-portlet-title', Ember.View.extend({
    tagName: 'div',
    classNames: ['portlet-title', 'pointer'],
    click: function(evt) {
        Ember.$(this.get('element.nextElementSibling')).toggle();
    }
}));

Ember.Handlebars.helper('node-status-badge', Ember.View.extend({
    tagName: 'span',
    classNames: ['badge', 'badge-roundless'],
    classNameBindings: ['badgeStyle'],
    template: Ember.Handlebars.compile('{{view.badgeValue}}'),
    badgeValue: null,
    badgeStyle: function() {
        var value = this.get('badgeValue');

        if (value === 'deploying') {
            return 'badge-info';
        }

        if (value === 'starting') {
            return 'badge-primary';
        }

        if (value === 'running' || value === 'started') {
            return 'bg-green-seagreen';
        }

        if (value === 'deleting') {
            return 'badge-danger';
        }

        if (value === 'suspending' || value === 'stopping') {
            return 'badge-warning';
        }
        if (value === 'suspended' || value === 'stopped') {
            return 'badge-warning';
        }

        if (value === 'runningTransitioning') {
            return 'bg-grey-cascade';
        }
        if (value === 'suspendedTransitioning') {
            return 'bg-grey-cascade';
        }

        if (value === 'unknown') {
            return 'bg-grey-cascade';
        }

        return '';
    }.property('badgeValue')
}));