$
    .when(
        $.getJSON("/Scripts/cldr/supplemental/likelySubtags.json"),
        $.getJSON("/Scripts/cldr/supplemental/numberingSystems.json"),
        $.getJSON("/Scripts/cldr/supplemental/timeData.json"),
        $.getJSON("/Scripts/cldr/supplemental/weekData.json"),

        $.getJSON("/Scripts/cldr/main/en/numbers.json"),
        $.getJSON("/Scripts/cldr/main/en/ca-gregorian.json"),
        $.getJSON("/Scripts/cldr/main/en/timeZoneNames.json"),

        $.getJSON("/Scripts/cldr/main/ru/numbers.json"),
        $.getJSON("/Scripts/cldr/main/ru/ca-gregorian.json"),
        $.getJSON("/Scripts/cldr/main/ru/timeZoneNames.json")
    )
    .then(
        function()
        {
            return [].slice.apply(arguments, [0]).map(function(result)
            {
                return result[0];
            });
        })
    .then(Globalize.load)
    .then(
        function()
        {
            Globalize.locale("ru");
        });