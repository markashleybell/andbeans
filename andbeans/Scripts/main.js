var AndBeans = (function ($, window) {

    var _rnd,
        _fetch,
        _checkImage,
        _getRandomImage,
        _init,
        _ui;

    _ui = {
        search: null,
        beans: null,
        searchForm: null
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

    _getRandomImage = function (data) {
        var images = data[0].Results;
        return images[_rnd(0, images.length - 1)];
    };

    _init = function () {
        _ui.search = $('div.search > div.image-container');
        _ui.beans = $('div.beans > div.image-container');
        _ui.searchForm = $('div.search form');

        _ui.searchForm.on('submit', function (e) {
            e.preventDefault();

            _ui.search.css('background-image', 'none');
            _ui.beans.css('background-image', 'none');

            $.when(
                _fetch('/Main/GetImages', { Query: $.trim($(this).find('input').val()) }),
                _fetch('/Main/GetBeans')
            ).done(function (data1, data2) {
                var searchImage = _getRandomImage(data1);
                var beansImage = _getRandomImage(data2);

                _checkImage(searchImage.MediaUrl, function () {
                    _ui.search.css('background-image', 'url(' + searchImage.MediaUrl + ')');
                }, function () {
                    console.log('Image Inaccessible: ' + searchImage.MediaUrl);
                });

                _checkImage(beansImage.MediaUrl, function () {
                    _ui.beans.css('background-image', 'url(' + beansImage.MediaUrl + ')');
                }, function () {
                    console.log('Image Inaccessible: ' + beansImage.MediaUrl);
                });
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