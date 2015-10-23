var AndBeans = (function ($, window) {

    var _rnd,
        _fetch,
        _checkImage,
        _init,
        _ui;

    _ui = {
        search: null,
        beans: null
    };

    _rnd = function (min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    };

    _checkImage = function (url, success, error) {
        var img = new Image();
        img.onload = success;
        img.onerror = error;
        img.src = url;
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
        _ui.search = $('div.search > div.image-container');
        _ui.beans = $('div.beans > div.image-container');

        $.when(
            _fetch('/Main/GetImages', { Query: 'fish' }),
            _fetch('/Main/GetBeans')
        ).done(function (data1, data2) {
            var search = data1[0].Results;
            var beans = data2[0].Results;

            var searchImage = search[_rnd(0, search.length -1)];
            var beansImage = beans[_rnd(0, beans.length - 1)];

            _checkImage(searchImage.MediaUrl, function () {
                console.log('Image Loaded: ' + searchImage.MediaUrl);
                _ui.search.css('background-image', 'url(' + searchImage.MediaUrl + ')');
            }, function () {
                console.log('Image Inaccessible: ' + searchImage.MediaUrl);
            });

            _checkImage(beansImage.MediaUrl, function () {
                console.log('Image Loaded: ' + beansImage.MediaUrl);
                _ui.beans.css('background-image', 'url(' + beansImage.MediaUrl + ')');
            }, function () {
                console.log('Image Inaccessible: ' + beansImage.MediaUrl);
            });
        });
    };

    return {
        init: _init
    };

}(jQuery, window));

$(function () {

    AndBeans.init();

});