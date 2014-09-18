define(function (require, exports, module) {
    var $ = require("jquery"),
        Backbone = require("backbone"),
        _ = require("underscore"),
        StyleModule = require("master"),
        PeiHuoModule = require("detail");
    var StyleCollection = StyleModule.StyleCollection,
        PeiHuoCollection = PeiHuoModule.PeiHuoCollection,
        PeiHuoModel = PeiHuoModule.PeiHuoModel;

    var SearchModel = Backbone.Model.extend({
        defaults: {
            dh: ""
        },
        initialize: function () {
            this.on("invalid", function (model, error) {
                $("#msg_error").html(error);
                $("#showMsg").hide();
                $("#dialog-message").dialog({
                    modal: true,
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });
            });
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
            this.row_template = $("#draggable");
            this.row_template.draggable({ revert: "valid" });
            $.get("/api/Ph/Get_Man_No", function (data) {
                //console.log(data);
                $("#inputzhd").autocomplete({
                    source: data,
                    //minLength: 0
                    //select: function (event, ui) {
                    //    //console.log(ui.item.value);
                    //    var val = ui.item.value;
                    //    console.log(val.substr(0,val.indexOf("(")));
                    //    return val.substr(0, val.indexOf("("));
                    //}
                });
            });
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
            var val = this.inputzhd[0].value;
            val = val.substr(0, val.indexOf("("));
            var d = new SearchModel({ dh: val });
            //更具制单查询需要进行配货的信息
            d.save(null, {
                url: '/api/ph/GetZDHById?dh=' + d.get('dh'),
                success: function (model, resp, option) {

                    $(this.showMsg).hide("1000");
                    //console.log(resp);
                    styles.add(resp);

                    $("#out_excel").attr("href", "/PeiHuo/OutputExcel?zdid=" + val).removeClass("disabled");

                    //console.log($("#out_excel").attr("href"));



                    $("body").data("styles", styles.toJSON());//缓存起来，给后面计算使用
                    $(".accordion").accordion({
                        collapsible: true, active: false, icons: false, heightStyle: "content", activate: function (event, ui) {
                            if (ui.newHeader[0]) {
                                var tmp = $(ui.newHeader[0]).data("style");
                                $("body").data('style', tmp);

                                if (!$("#id_" + tmp).html()) {
                                    $.post("/api/ph/GetPeiHuos?zdid=" + d.get('dh') + "&style=" + $(ui.newHeader[0]).data("style"), function (data) {
                                        peihuos.add(data);

                                        $("#id_" + $("body").data("style")).sortable();
                                        //检验输入框中的数字是否合法
                                        $("input.input-sm").unbind('change').change(function () {
                                            calculate(this);
                                        });
                                        //添加删除事件
                                        $(".btn-danger").click(function () {
                                            delph(this);
                                        });

                                        //添加保存事件
                                        $(".btn-success").click(function () {
                                            saveph(this);
                                        });
                                    });
                                }
                            }
                        }
                    });
                    $(this.accd).sortable();//添加排序功能
                    $("[id^='id']").droppable({
                        drop: function (event, ui) {
                            if (ui.draggable[0].id === "draggable") {
                                //添加配货店铺
                                var dps = $("body").data("dps");
                                if (dps === undefined) {
                                    $.ajax({
                                        url: '/api/ph/GetDianpus',
                                        type: "GET",
                                        async: false,
                                        success: function (data) {
                                            dps = data;
                                        }
                                    });
                                }

                                var tmpl = $("body").data("tmpl");
                                if (tmpl === undefined) {
                                    tmpl = _.template($("#row_template").html());
                                }
                                $(this).append(tmpl({ dps: dps, style: this.id }));
                                //change事件(每一次都要计算一边，看填入的数字是否准确)
                                //var total
                                $("input.input-sm").unbind('change').change(function () {
                                    calculate(this);
                                });

                                //添加删除事件
                                $(".btn-danger").click(function () {
                                    delph(this);
                                });

                                //添加保存事件
                                $(".btn-info").click(function () {
                                    saveph(this);
                                });
                            }
                        }
                    });

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
            var peiHuoView = new PeiHuoModule.PeiHuoView({ model: item });
            $("#id_" + $("body").data("style")).append(peiHuoView.render().el);

        },




    });
    //计算输入的数字是否符合要求
    var calculate = function (obj) {
        var tmp = $("body").data("styles"), parent = $($(obj).parent().parent()[0]), st = parent.data("style"), total = 0, id = $(obj).data("id"), unt_pr = 0;
        //st = st.substring(3);
        if (st.indexOf("id") >= 0) {
            st = st.substring(3);
        }

        $.each(tmp, function (index, value) {
            if (value.sty_no + value.col_no === st) {
                switch (id) {
                    case "s105"://尺码S：105
                        total = value.s105;
                        unt_pr = value.unt_pr;
                        break;
                    case "m120":
                        total = value.m120;
                        unt_pr = value.unt_pr;
                        break;
                    case "l130":
                        total = value.l130;
                        unt_pr = value.unt_pr;
                        break;
                    case "xl140":
                        total = value.xl140;
                        unt_pr = value.unt_pr;
                        break;
                    case "xxl155":
                        total = value.xxl155;
                        unt_pr = value.unt_pr;
                        break;
                    default:
                        total = 0;
                        unt_pr = 0;
                        break;
                }
            }
        });


        var ips = parent.parent().find("input." + id), c_total = 0;
        $.each(ips, function (index, input) {
            c_total = c_total + parseInt(input.value === "" ? 0 : input.value);
        });

        //console.log("c_total:" + c_total);
        //console.log("total:" + total);
        if (c_total > total) {//恢复到正确的值
            $(obj).val(parseInt($(obj).val() - (c_total - total)));
        }

        //计算总数目 和总金额
        ips = parent.find("input.form-control");
        var sm = ips.slice(0, 5), label = parent.find("label");

        c_total = 0;

        $.each(sm, function (index, input) {
            c_total = parseInt(c_total) + parseInt(input.value === "" ? 0 : input.value);
        });
        label[0].innerText = c_total;
        label[1].innerText = unt_pr;
        label[2].innerText = c_total * unt_pr;


        c_total = 0;
    }
    //删除配货信息
    var delph = function (obj) {
        //console.log($($(obj).parent().parent()[0]).data("style"));
        var p = $(obj).parent().parent(),
            parent = $(p[0]),
            style = parent.data("style"),
            flag = false;

        if (style.indexOf("id") >= 0) {
            style = style.substring(3);
            flag = true;
        }
        var select = parent.find("select.form-control")[0];
        if (!flag) {
            //url 区分大小写
            $.post("/api/Ph/DelPh?style=" + style + "&use_id=" + select.value, function (data) {
                if (data !== "success") {
                    $("#msg_error").html("删除失败！");
                    $("#dialog-message").attr("title", "提示信息").dialog({
                        modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });


                }
                $(p).remove();
            });
        }
        $(p).remove();

        //$($(obj).parent().parent()).remove();
    }
    //保存配货信息
    var saveph = function (ob) {
        var obj = $(ob),
            parent = $(obj.parent().parent()[0]);

        //console.log(parent);
        var style = parent.data("style").substring(3),//款式+颜色
            inputs = parent.find(".form-control"),
            labels = parent.find("label");

        var ph = new PeiHuoModel({
            use_id: inputs[0].value,//店铺id
            style: style,//款式+颜色
            s105: inputs[1].value,//S:尺码数量
            m120: inputs[2].value,//M:尺码数量
            l130: inputs[3].value,//L:尺码数量
            xl140: inputs[4].value,//XL:尺码数量
            xxl155: inputs[5].value,//XXL:尺码数量
            total_num: labels[0].innerText,//总数目
            unt_pr: labels[1].innerText,//吊牌价
            total_money: labels[2].innerText,//总金额

        });
        ph.save(null, {
            // url: '/api/ph/saveph',
            success: function (model, resp, option) {
                if (resp !== "success") {
                    //alert("保存失败！");
                    $("#msg_error").html("保存失败！");
                    $("#dialog-message").attr("title", "提示信息").dialog({
                        modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                } else {
                    $("#msg_error").html("保存成功！");
                    $("#dialog-message").attr("title", "提示信息").dialog({
                        modal: true,
                        buttons: {
                            Ok: function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                    $(obj.find("span")[0]).removeClass("glyphicon glyphicon-star-empty").addClass("glyphicon glyphicon-star");
                    obj.removeClass("btn-info").addClass("btn-success");
                    parent.removeClass("ui-priority-primary").addClass("ui-priority-primary");
                }
            }
        });
    }

    module.exports = {
        "SearchView": SearchView
    };
});