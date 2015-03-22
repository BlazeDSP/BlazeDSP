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

// -- EasyForm
Ember.EasyForm.Config.registerWrapper('bootstrap', {
    // Define the custom template
    inputTemplate: 'bootstrap-input',

    // Define a custom config used by the template
    controlsWrapperClass: 'col-lg-10',

    // Define the classes for the form, label, error...
    formClass: 'form-horizontal',
    fieldErrorClass: 'has-error',
    errorClass: 'help-block',
    hintClass: 'help-block',
    labelClass: 'col-lg-2 control-label',
    inputClass: 'form-group',
    buttonClass: 'btn btn-primary',
    cancelClass: 'btn btn-default'
});

// -- Add Bootstrap v3 class to form elements
Ember.TextSupport.reopen({
    classNames: ['form-control']
});

Ember.Select.reopen({
    classNames: ['form-control']
});