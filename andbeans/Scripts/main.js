var AndBeans = (function ($, window) {

    var _rnd,
        _fetch,
        _init,
        _ui;

    _ui = {
        search: null,
        beans: null
    };

    _rnd = function (min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    };

    _fetch = function (url, data) {
        var options = {
            url: url,
            dataType: 'json',
            type: 'GET'
        };
        if (data) {
            options.data = data;
        }
        return $.ajax(options);
    };

    _init = function () {
        _ui.search = $('div.search');
        _ui.beans = $('div.beans');

        $.when(
            _fetch('/Main/GetImages', { Query: 'fish' }),
            _fetch('/Main/GetBeans')
        ).done(function (data1, data2) {
            var search = data1[0].Results;
            var beans = data2[0].Results;

            var searchImage = search[_rnd(0, search.length -1)];
            var beansImage = beans[_rnd(0, beans.length -1)];

            _ui.search.css('background-image', 'url(' + searchImage.MediaUrl + ')');
            _ui.beans.css('background-image', 'url(' + beansImage.MediaUrl + ')');
        });
    };

    return {
        init: _init
    };

}(jQuery, window));

$(function () {

    AndBeans.init();

});