$(document).ready(function () {
    pageSetUp();
    var responsiveHelper_dt_basic = undefined;
    var responsiveHelper_datatable_fixed_column = undefined;
    var responsiveHelper_datatable_col_reorder = undefined;
    var responsiveHelper_datatable_tabletools = undefined;

    var breakpointDefinition = {
        tablet: 1024,
        phone: 480
    };


});


$.fn.serializeObject = function (options) {
    options = jQuery.extend({}, options);

    var self = this,
        json = {},
        pushCounters = {},
        patterns = {
            "validate": /^[a-zA-Z][a-zA-Z0-9_]*(?:\[(?:\d*|[a-zA-Z0-9_]+)\])*$/,
            "key": /[a-zA-Z0-9_]+|(?=\[\])/g,
            "push": /^$/,
            "fixed": /^\d+$/,
            "named": /^[a-zA-Z0-9_]+$/
        };


    this.build = function (base, key, value) {
        base[key] = value;
        return base;
    };

    this.push_counter = function (key) {
        if (pushCounters[key] === undefined) {
            pushCounters[key] = 0;
        }
        return pushCounters[key]++;
    };

    jQuery.each(jQuery(this).serializeArray(), function () {

        // skip invalid keys
        if (!patterns.validate.test(this.name)) {
            return;
        }

        var k,
            keys = this.name.match(patterns.key),
            merge = this.value,
            reverseKey = this.name;

        while ((k = keys.pop()) !== undefined) {

            // adjust reverse_key
            reverseKey = reverseKey.replace(new RegExp("\\[" + k + "\\]$"), '');

            // push
            if (k.match(patterns.push)) {
                merge = self.build([], self.push_counter(reverseKey), merge);
            }

                // fixed
            else if (k.match(patterns.fixed)) {
                merge = self.build([], k, merge);
            }

                // named
            else if (k.match(patterns.named)) {
                merge = self.build({}, k, merge);
            }
        }

        json = jQuery.extend(true, json, merge);
    });


    return json;
};