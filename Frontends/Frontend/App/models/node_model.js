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

App.Node = DS.Model.extend({
    label: DS.attr('string'),
    location: DS.attr('string'),
    status: DS.attr('string'),

    node_deployments: DS.hasMany('node_deployment')
});

App.NodeDeployment = DS.Model.extend({
    name: DS.attr('string'),
    label: DS.attr('string'),
    deployment_slot: DS.attr('string'),
    sdk_version: DS.attr('string'),
    status: DS.attr('string'),
    created_time: DS.attr('date'),
    last_modified_time: DS.attr('date'),

    node_instances: DS.hasMany('node_instance')
});

App.NodeInstance = DS.Model.extend({
    name: DS.attr('string'),
    size: DS.attr('string'),
    state_details: DS.attr('string'),
    status: DS.attr('string'),
    power_state: DS.attr('string'),
    role_name: DS.attr('string')
});