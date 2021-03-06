﻿define(function (require, exports, module) {
    var $ = require("jquery"),
        Backbone = require("backbone"),
        StyleModule = require("master"),
        PeiHuoModule = require("detail");
    var StyleCollection = StyleModule.StyleCollection,
        PeiHuoCollection = PeiHuoModule.PeiHuoCollection;

    var SearchModel = Backbone.Model.extend({
        defaults: {
            dh: ""
        },
        initialize: function () {
            this.on("invalid", function (model, error) {
                $("#msg_error").html(error);
                $("#dialog-message").dialog({
                    modal: true,
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            });

            //$("#draggable").draggable();
        },
        //验证
        validate: function (attrs, optons) {
            if (!attrs.dh) {
                return "请输入生产制造单号";
            }
        },
        //url: '/api/ph/GetZDHById'
    });
    var styles = new StyleCollection;
    var peihuos = new PeiHuoCollection;
    var SearchView = Backbone.View.extend({
        el: "#search_frm",
        inputzhd: $("#inputzhd"),//获取输入表单
        table_header: $("#table_header"),//加载制造单用的
        accd: $("#accd"),
        showMsg: $("#showMsg"),//显示提示信息用的
        initialize: function () {
            this.listenTo(styles, "add", this.addOneStyle);
            this.listenTo(peihuos, "add", this.addOnePeiHuo);
        },
        events: {
            "click .btn-default": "search_click"
        },
        search_click: function (e) {
            e.preventDefault();

            //this.table_header.nextAll().remove();
            this.accd.html("");
            //console.log(this.showMsg);
            this.showMsg.show("1000");
            var d = new SearchModel({ dh: this.inputzhd[0].value });
            d.save(null, {
                url: '/api/ph/GetZDHById?dh=' + d.get('dh'),
                success: function (model, resp, option) {

                    $(this.showMsg).hide("1000");
                    //console.log(resp);
                    styles.add(resp);
                    $(".accordion").accordion({
                        collapsible: true, active: false, icons: false, heightStyle: "content", activate: function (event, ui) {
                            if (ui.newHeader[0]) {
                                var tmp = $(ui.newHeader[0]).data("style");
                                $("body").data('style', tmp);
                                //console.log($("#id_" + tmp).html().length);
                                if (!$("#id_" + tmp).html()) {
                                    $.post("/api/ph/GetPeiHuos?zdid=" + d.get('dh') + "&style=" + $(ui.newHeader[0]).data("style"), function (data) {
                                        peihuos.add(data);

                                        $("#id_" + $("body").data("style")).sortable();

                                    });
                                }


                            }
                        }
                    });
                    $(this.accd).sortable();

                },
                error: function (model, resp, option) {
                    console.log(resp.responseText);
                }
            });
        },
        addOneStyle: function (item) {
            var styleView = new StyleModule.StyleView({ model: item });
            //this.table_header.after(styleView.render().el);
            this.accd.append(styleView.render().el);

        },

        addOnePeiHuo: function (item) {
            //sconsole.log($("body").data("style"));
            
            var peiHuoView = new PeiHuoModule.PeiHuoView({ model: item });
            $("#id_" + $("body").data("style")).append(peiHuoView.render().el);

            //console.log(peiHuoView.render().el);


        }


    });

    module.exports = {
        "SearchView": SearchView
    };
});