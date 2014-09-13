define(function (require, exports, module) {
    var $ = require("jquery");
    var _ = require("underscore");
    var Backbone = require("backbone");

    var StyleModel = Backbone.Model.extend({
        defaults: {
            col_dr: "",
            col_no: "",
            com_nm: "",
            com_pr: 0,
            com_qu: 0,
            l130: 0,
            m120: 0,
            s105: 0,
            sty_no: "",
            unt_pr: 0,
            xl140: 0,
            xxl155: 0
        }
    });

    var StyleCollection = Backbone.Collection.extend({
        model: StyleModel,

    });


    var StyleView = Backbone.View.extend({
        tagName: "div class='accordion'",
        initialize: function () {
            $("#accordion").accordion({ collapsible: true, active: false });
            //this.listenTo(this.model, "add", this.render);
        },
        template: _.template($("#style_template").html()),
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));

            return this;
        }
    });
    //return StyleView;

    module.exports = {
        "StyleModel": StyleModel,
        "StyleCollection": StyleCollection,
        "StyleView": StyleView
    }
});