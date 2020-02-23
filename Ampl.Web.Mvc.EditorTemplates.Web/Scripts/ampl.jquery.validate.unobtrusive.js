/// <reference path="_references.js" />

$(function()
{
    var oldDateValidator = $.validator.methods.date;
    $.validator.methods.date = function(value, element)
    {
        //
        // <input type="text" data-val-date="..." data-val-datetime="..." .... />
        // ignore date validation if there is datetime validation
        //
        if ($(element).data('val-datetime'))
        {
            return true;
        }
        return oldDateValidator.call(this, value, element);
    };

    $.validator.addMethod(
        "datetime",
        function(value, element)
        {
            value = value.replace(' ', ', ');
            return this.optional(element) ||
                Globalize.parseDate(value, { skeleton: 'yMdHms' }) !== null ||
                Globalize.parseDate(value, { skeleton: 'yMdhms' }) !== null;
        });
});

$.validator.unobtrusive.adapters.addBool("datetime");
