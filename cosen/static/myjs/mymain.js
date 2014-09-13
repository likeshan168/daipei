require.config({
    //baseUrl: "/static/myjs",
    shim: {
        underscore: {
            exports: '_'
        },
    },
    paths: {
        jquery: "/Scripts/jquery-1.8.2.min",
        jquery_ui: "/Scripts/jquery-ui.min-1.11.1",
        underscore: "/Scripts/underscore.min",
        backbone: "/Scripts/backbone.min",
        master: "/static/myjs/peihuo/master",
        detail: "/static/myjs/peihuo/details",
        search:"/static/myjs/peihuo/search",
        app: '/static/myjs/peihuo/app'
    }

});

require(['jquery', 'jquery_ui', 'underscore', 'backbone', 'app'], function ($, $ui, _, Backbone, AppView) {
    var appView = new AppView;
});

