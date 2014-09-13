define(function (require, exports, module) {

    var $ = require("jquery");
    var Backbone = require("backbone");
    var masterModule = require("master");
    var searchModule = require("search");
    var AppView = Backbone.View.extend({
        el: "body",
        initialize: function () {
            this.styleView = new masterModule.StyleView();
            this.searchView = new searchModule.SearchView();
            //console.log(this.searchView);
        }
    });

    return AppView;
});