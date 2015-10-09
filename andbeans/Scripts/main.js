var AndBeans = (function ($, window) {

    var _fetch,
        _init;

    _fetch = function (url, data, callback) {
        $.ajax({
            url: url,
            data: data,
            dataType: 'json',
            type: 'GET'
        }).done(callback).fail(function (request, status, error) {
            console.log(status + ': ' + error);
        });
    };

    _init = function () {
        //https://api.datamarket.azure.com/Bing/Search/v1/Image?Query=%27baked%2Bbeans%27
        _fetch('https://api.datamarket.azure.com/Bing/Search/v1/Image', {
            Query: 'baked+beans'
        }, function (data) {
            console.log(data);
        });
    };

    return {
        init: _init
    };

}(jQuery, window));

$(function () {

    AndBeans.init();

});