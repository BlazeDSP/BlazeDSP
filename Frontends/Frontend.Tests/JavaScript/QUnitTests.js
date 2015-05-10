/// <reference path="../../frontend/app/app.js" />

QUnit.module('Frontend.Tests.JavaScript', {
    beforeEach: function(assert) {
        assert.ok(true, 'one extra assert per test');
    },
    afterEach: function(assert) {
        assert.ok(true, 'and one extra assert after each test');
    }
});

QUnit.test('hello test', function (assert) {
    assert.ok(1 == '1', 'Passed!');
});