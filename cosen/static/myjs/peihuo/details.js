define(function (require, exports, module) {
    var $ = require("jquery"),
     Backbone = require("backbone"),
     _ = require("underscore");

    //对每一个店铺的配货
    var PeiHuoModel = Backbone.Model.extend({
        defaults: {
            use_id: "",//店铺id
            style: "",//款式+颜色
            s105: 0,//S:尺码数量
            m120: 0,//M:尺码数量
            l130: 0,//L:尺码数量
            xl140: 0,//XL:尺码数量
            xxl155: 0,//XXL:尺码数量
            total_num: 0,//总数目
            unt_pr: 0,//吊牌价
            total_money: 0,//总金额
            remark: ""//备注
        }
    });

    var PeiHuoCollection = Backbone.Collection.extend({
        model: PeiHuoModel,
    });

    var PeiHuos = new PeiHuoCollection;

    var PeiHuoView = Backbone.View.extend({
        tagName: "div class='row ui-priority-primary ui-state-default'",
        template: _.template($("#peihuo_template").html()),
        render: function () {
            //console.log(this.model.toJSON());
            this.$el.data("style", this.model.get('style')).data("use_id", this.model.get("use_id"));

            this.$el.html(this.template(this.model.toJSON()));
            return this;
        }
    });

    module.exports = {
        "PeiHuoView": PeiHuoView,
        "PeiHuoCollection": PeiHuoCollection
    };
});