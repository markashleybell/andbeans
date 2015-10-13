var AndBeans = (function ($, window) {

    var _fetch,
        _init;

    _fetch = function (url, data) {
        return $.ajax({
            url: url,
            data: data,
            dataType: 'json',
            type: 'GET'
        });
    };

    _init = function () {
        $.when(
            _fetch('/Main/GetImagesTest', { Query: 'fish' }),
            _fetch('/Main/GetBeansTest', { Query: 'baked+beans' })
        ).done(function (data1, data2) {
            console.log(data1[0]);
            console.log(data2[0]);
        });
    };

    return {
        init: _init
    };

}(jQuery, window));

$(function () {

    AndBeans.init();

});