jQuery(document).ready(function ($) {
    cosen = {};
    cosen["body"] = $("body");
    cosen["sltdp"] = $("#sltdp");
    cosen["dptb"] = $("#dptb");
    cosen["pagenum"] = 1;
    cosen["dpid"] = ""
    cosen["msg"] = $("#msg");
    cosen["ckball"] = $("#ckball");
    cosen["dpcount"] = $("#dpcount");//设置推荐数目

    cosen["hasdpcount"] = 0;//已经推荐的数目
    cosen["jzcount"] = $("#jzcount");//加载数目
    cosen["savetj"] = $("#savetj");
    cosen["showdate"] = $("#showdate");
    cosen["search"] = $("#search");
    cosen["tjdate"] = $("#tjdate");//推荐日期
    cosen["date"] = "";
    cosen["swdate"] = $("#swdate");
    cosen["tjrows"] = $("#tjrows");//获取已经推荐的信息根据店铺
    cosen["showdp"] = $("#showdp");//显示推荐的搭配图片
    cosen["dpcontent"] = $("#dpcontent");

    cosen["filterCond"] = $("#filterCond");//筛选条件
    cosen["filterValue"] = $("#filterValue");//筛选的值
    cosen["remark"] = $("#remark");//推荐添加备注信息
    //cosen["bigimg"]=$("#bigimg");
    //cosen["showbigimg"]=$("#showbigimg");
    var date = new Date(), month = date.getMonth() + 1, day = date.getDate();
    month = month < 10 ? "0" + month : month;
    day = day < 10 ? "0" + day : day;
    var hour = date.getHours(), minute = date.getMinutes(), second = date.getSeconds(), time = hour + ":" + minute + ":" + second;
    cosen["date"] = date.getFullYear() + "-" + month + "-" + day;
    cosen["time"] = time;
    cosen["tjdate"].val(cosen["date"]);




    //根据店铺的库存获取搭配信息
    //var opts = cosen.sltdp.children('option');
    //console.log(opts);

    cosen.sltdp.change(function () {
        $(document).unbind("scroll");
        var dpid = $(this).val();
        if (dpid === "0") {
            alert("请选择店铺");
        } else {

            cosen["dptb"].html("");
            cosen["dpcount"].text("0");
            cosen["hasdpcount"] = 0
            cosen["jzcount"].text("0");
            cosen["pagenum"] = 1;
            cosen["dpid"] = $(this).val();
            cosen["showdate"].modal({ show: true, backdrop: "static" });
        }
    });


    //cosen["sltdp"].children("option").click(function (event) {
    //    $(document).unbind("scroll");
    //    var dpid = $(this).val();
    //    if (dpid === "0") {
    //        alert("请选择店铺");
    //    } else {

    //        cosen["dptb"].html("");
    //        cosen["dpcount"].text("0");
    //        cosen["hasdpcount"] = 0
    //        cosen["jzcount"].text("0");
    //        cosen["pagenum"] = 1;
    //        cosen["dpid"] = $(this).val();
    //        cosen["showdate"].modal("show");
    //    }
    //});

    //全选事件
    cosen["ckball"].click(function () {
        var ckbs = cosen["dptb"].find("input:checkbox"), obj = $(this), sts = obj.prop("checked");

        if (sts) {
            cosen["dpcount"].text(ckbs.length);
        } else {
            cosen["dpcount"].text("0");
        }
        $.each(ckbs, function (index, ckb) {
            $(ckb).prop("checked", sts);
            if (sts) {

                $($(ckb).parent().parent()).addClass("danger");
            } else {

                $($(ckb).parent().parent()).removeClass("danger");
            }
        });
    });

});


//checkbox事件
function ckbchangecolor(obj) {
    var ckb = $(obj);
    if (ckb.prop("checked") === true) {

        $(ckb.parent().parent()).addClass("danger");
        cosen["dpcount"].text(parseInt(cosen["dpcount"].text()) + 1);
    } else {

        $(ckb.parent().parent()).removeClass("danger");
        cosen["dpcount"].text(parseInt(cosen["dpcount"].text()) - 1);
    }
}
//显示组图的大图
function showbigimg(e) {
    var evt = window.event ? window.event : e;
    var imgid = evt.target.getAttribute("alt"), url = "/static/images/composebig/" + imgid + "_bg.jpg";
    var x = 10, y = 20, tooltip = "<div id='tooltip'><img src='" + url + "' class='img-circle'/></div>";
    cosen["body"].append(tooltip);
    $("#tooltip").css({ "position": "absolute", "z-index": 10011, "top": (evt.pageY + y) + "px", "left": (evt.pageX + x) + "px" }).show("fast");


}
//隐藏组图的大图显示
function hidebigimg(obj) {
    $("#tooltip").remove();
}
//鼠标在图片上移动
function mousemoveonimg(e) {
    var evt = window.event ? window.event : e, x = 10, y = 10;
    $("#tooltip").css({ "position": "absolute", "z-index": 10011, "top": (evt.pageY + y) + "px", "left": (evt.pageX + x) + "px" }).show("fast");
}
//显示搭配（左侧边上的更具时间查询用的）
function showdp(obj) {
    cosen["showdp"].modal("show");
    cosen["dpcontent"].html("<tr><td>加载数据...</td></tr>");
    var tjdate = $(obj).attr("title");
    $.post("/api/DpApi/LookUpTjDp/?dpid=" + cosen["dpid"] + "&tjdate=" + tjdate, function (data) {
        if (data.length === 0) {
            cosen["dpcontent"].html("<tr><td>没有查询到推荐信息</td></tr>");
        } else {
            var html = [];
            if (data[0].remark !== null) {
                html.push("<tr><td colspan='9'>备注：" + data[0].remark + "</td></tr>");
            }
            $.each(data, function (index, value) {
                html.push("<tr><td>" + (index + 1) + "</td><td><img width='60' height='60' alt='" + value.cbpicture + "' onmouseover='showbigimg(event);' onmouseout='hidebigimg(event);' onmousemove='mousemoveonimg(event);'  class='img-circle' src='/static/images/compose/" + value.cbpicture + ".jpg'/></td>");
                if (value.masterstyle !== "None" && value.masterstyle !== "") {
                    html.push("<td><img width='60' height='60' title='" + value.masterstyle + "' src='/static/images/single/" + value.masterstyle + ".jpg' class='img-circle'/><br/>" + value.mqu + "</td>");
                }
                else {
                    html.push("<td></td>");
                }

                if (value.legging1 !== "None" && value.legging1 !== "") {
                    html.push("<td><img width='60' height='60' class='img-circle' title='" + value.legging1 + "' src='/static/images/single/" + value.legging1 + ".jpg'/><br/>" + value.lqu1 + "</td>");
                }
                else {
                    html.push("<td></td>");
                }
                if (value.legging2 !== "None" && value.legging2 !== "") {
                    html.push("<td><img width='60' height='60' class='img-circle' title='" + value.legging2 + "' src='/static/images/single/" + value.legging2 + ".jpg'/><br/>" + value.lqu2 + "</td>");
                }
                else {
                    html.push("<td></td>");
                }
                if (value.bottom1 !== "None" && value.bottom1 !== "") {
                    html.push("<td><img width='60' height='60' class='img-circle' title='" + value.bottom1 + "' src='/static/images/single/" + value.bottom1 + ".jpg'/><br/>" + value.bqu1 + "</td>");
                }
                else {
                    html.push("<td></td>");
                }
                if (value.bottom2 !== "None" && value.bottom2 !== "") {
                    html.push("<td><img width='60' height='60' class='img-circle' title='" + value.bottom2 + "' src='/static/images/single/" + value.bottom2 + ".jpg'/><br/>" + value.bqu2 + "</td>");
                }
                else {
                    html.push("<td></td>");
                }
                if (value.accessory1 !== "None" && value.accessory1 !== "") {
                    html.push("<td><img width='60' height='60' class='img-circle' title='" + value.accessory1 + "' src='/static/images/single/" + value.accessory1 + ".jpg'/><br/>" + value.aqu1 + "</td>");
                }
                else {
                    html.push("<td></td>");
                }
                if (value.accessory2 !== "None" && value.accessory2 !== "") {
                    html.push("<td><img width='60' height='60' class='img-circle' title='" + value.accessory2 + "' src='/static/images/single/" + value.accessory2 + ".jpg'/><br/>" + value.aqu2 + "</td>");
                }
                else {
                    html.push("<td></td>");
                }
                html.push("</tr>");
            });
            cosen["dpcontent"].html(html.join(""));
        }

    });
}



//knock out

function TjDate() {
    var self = this;
    self.tjdates = ko.observableArray();//初始化一个空数组

    //获取搭配的图片信息(所有的搭配，推荐之前，为推荐准备的)
    self.getdp = function getdp(dpid, pagenum, tjdate, filterType, filterValue) {
        if (cosen["msg"].html() !== "已经加载完所有数据") {
            cosen["msg"].html("加载数据...").show();
            $.getJSON('/api/DpApi/GetDaiPei?sltDp=' + dpid + "&pageNum=" + pagenum + "&tjDate=" + tjdate + "&filterType=" + filterType + "&filterValue=" + filterValue, function (data, textStatus, xhr) {
                var html = [];
                cosen["msg"].hide();
                var rows = data.rows, tjrows = data.tjrows;
                if (rows.length === 0) {
                    if (pagenum === 1) {
                        html.push("<tr><td colspan='12'>没有获取到搭配信息...</td></tr>")
                    } else {
                        cosen["msg"].html("已经加载完所有数据").show();
                    }
                } else {
                    cosen["jzcount"].text(parseInt(cosen["jzcount"].text()) + rows.length);
                    $.each(rows, function (index, value) {

                        if (value.isdp !== "null" && value.isdp !== "" && value.isdp !== null) {
                            html.push("<tr class='danger'><td><input checked='checked' onclick='ckbchangecolor(this);' type='checkbox' id='" + value.cbpicture + "@" + value.mqu + "@" + value.lqu1 + "@" + value.lqu2 + "@" + value.bqu1 + "@" + value.bqu2 + "@" + value.aqu1 + "@" + value.aqu2 + "@" + cosen["dpid"] + "@" + cosen["tjdate"].val() + "'></td><td>" + (index + 1 + (pagenum - 1) * 10) + "</td><td><img width='60' height='60' src='/static/images/compose/" + value.cbpicture + ".jpg' alt='" + value.cbpicture + "' onmouseover='showbigimg(event);' onmouseout='hidebigimg(event);' onmousemove='mousemoveonimg(event);' class='img-circle'/></td>");
                        } else {

                            html.push("<tr><td><input  onclick='ckbchangecolor(this);' type='checkbox' id='" + value.cbpicture + "@" + value.mqu + "@" + value.lqu1 + "@" + value.lqu2 + "@" + value.bqu1 + "@" + value.bqu2 + "@" + value.aqu1 + "@" + value.aqu2 + "@" + cosen["dpid"] + "@" + cosen["tjdate"].val() + "'></td><td>" + (index + 1 + (pagenum - 1) * 10) + "</td><td><img width='60' height='60' src='/static/images/compose/" + value.cbpicture + ".jpg' alt='" + value.cbpicture + "' onmouseover='showbigimg(event);' onmouseout='hidebigimg(event);' onmousemove='mousemoveonimg(event)' class='img-circle'/></td>");
                        }

                        if (value.masterstyle !== "None" && value.masterstyle !== "" && value.masterstyle !== null) {
                            html.push("<td><img width='60' height='60' title='" + value.masterstyle + "' src='/static/images/single/" + value.masterstyle + ".jpg' class='img-circle'/></br>" + value.mqu + "*" + (value.mdate == null ? "" : value.mdate) + "</td>");
                        }
                        else {
                            html.push("<td></td>")
                        }

                        if (value.legging1 !== "None" && value.legging1 !== "" && value.legging1 !== null) {
                            html.push("<td><img width='60' height='60' class='img-circle' title='" + value.legging1 + "' src='/static/images/single/" + value.legging1 + ".jpg'/></br>" + value.lqu1 + "*" + (value.datel1 == null ? "" : value.datel1) + "</td>");
                        }
                        else {
                            html.push("<td></td>")
                        }
                        if (value.legging2 !== "None" && value.legging2 !== "" && value.legging2 !== null) {
                            html.push("<td><img width='60' height='60' class='img-circle' title='" + value.legging2 + "' src='/static/images/single/" + value.legging2 + ".jpg'/></br>" + value.lqu2 + "*" + (value.datel2 == null ? "" : value.datel2) + "</td>");
                        }
                        else {
                            html.push("<td></td>")
                        }
                        if (value.bottom1 !== "None" && value.bottom1 !== "" && value.bottom1 !== null) {
                            html.push("<td><img width='60' height='60' class='img-circle' title='" + value.bottom1 + "' src='/static/images/single/" + value.bottom1 + ".jpg'/></br>" + value.bqu1 + "*" + (value.dateb1 == null ? "" : value.dateb1) + "</td>");
                        }
                        else {
                            html.push("<td></td>")
                        }
                        if (value.bottom2 !== "None" && value.bottom2 !== "" && value.bottom2 !== null) {
                            html.push("<td><img width='60' height='60' class='img-circle' title='" + value.bottom2 + "' src='/static/images/single/" + value.bottom2 + ".jpg'/></br>" + value.bqu2 + "*" + (value.dateb2 == null ? "" : value.dateb2) + "</td>");
                        }
                        else {
                            html.push("<td></td>")
                        }
                        if (value.accessory1 !== "None" && value.accessory1 !== "" && value.accessory1 !== null) {
                            html.push("<td><img width='60' height='60' class='img-circle' title='" + value.accessory1 + "' src='/static/images/single/" + value.accessory1 + ".jpg'/></br>" + value.aqu1 + "*" + (value.datea1 == null ? "" : value.datea1) + "</td>");
                        }
                        else {
                            html.push("<td></td>")
                        }
                        if (value.accessory2 !== "None" && value.accessory2 !== "" && value.accessory2 !== null) {
                            html.push("<td><img width='60' height='60' class='img-circle' title='" + value.accessory2 + "' src='/static/images/single/" + value.accessory2 + ".jpg'/><br/>" + value.aqu2 + "*" + (value.datea2 == null ? "" : value.datea2) + "</td>");
                        }
                        else {
                            html.push("<td></td>")
                        }
                        html.push("</tr>")

                    });
                }
                if (pagenum === 1) {
                    cosen["dptb"].html("");
                    cosen["dpcount"].text(data.total);
                    if (tjrows.length === 0) {
                        //cosen["tjrows"].html("");
                        self.tjdates([]);
                    }
                    else {
                        self.tjdates(tjrows);
                    }
                }
                cosen["dptb"].append(html.join(""));

            });
        }
    }


    //search 确定查询事件
    self.search = function () {
        try {
            cosen["msg"].html('');
            cosen["swdate"].text(cosen["tjdate"].val());
            cosen["dptb"].html("<tr><td colspan='12'>正在获取...</td></tr>")
            self.getdp(cosen["dpid"], cosen["pagenum"], cosen["tjdate"].val(), cosen.filterCond.val(), cosen.filterValue.val());//调用方法
            $(document).bind("scroll", function () {
                self.scrollHandler();
            });
            cosen["pagenum"] += 1;
            cosen["showdate"].modal("hide");
        } catch (e) {
            console.log(e);
        }

    };

    //scroll 滚动条事件（因为数据时分页加载的）
    self.scrollHandler = function () {
        var totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
        if ($(document).height() <= totalheight) {
            new TjDate().getdp(cosen["dpid"], cosen["pagenum"], cosen["tjdate"].val(), cosen.filterCond.val(), cosen.filterValue.val());
            cosen["pagenum"] = cosen["pagenum"] + 1;
        }
    };

    //save tj 保存推荐事件
    self.saveTj = function () {
        var ckbs = cosen["dptb"].find("input:checkbox:checked");
        if (ckbs.length === 0) {
            cosen["msg"].html("请选择要推荐的搭配").show();
        } else {
            cosen["msg"].hide();
            dps = [];
            $.each(ckbs, function (index, ckb) {
                dps.push($(ckb).attr("id"));
            });
            cosen["msg"].html("正在保存...").show();
            var datetime = cosen.tjdate.val() + " " + cosen.time;
            $.post('/api/DpApi/SaveTuiJianDp?dpid=' + cosen["dpid"] + '&tjdate=' + datetime + '&tjdp=' + dps.join("$") + "&remark=" + cosen.remark.val(), function (data) {
                if (data.indexOf("成功") >= 0) {
                    //cosen["tjdate"].val();
                    self.tjdates.push({ tjdate: datetime });
                }
                cosen["msg"].html(data).show();
            });
        }
    }
    //筛选事件
    self.filter = function () {

        cosen["pagenum"] = 1;
        self.search();
    }
    //keydown 事件
    self.kdown = function () {

        $('#filterValue').keypress(function (e) {
            if (e.keyCode === 13) {
                self.filter();
            }
        });
    }

    self.kdown();



}

ko.applyBindings(new TjDate());