
//knockout js
var trMenu = $("#trMenu"), topHeader = $("#topheader");
$(document).ready(function () {

    $.get('/promotion/GetPromotionJson', function (data) {
        $.data(document.body, "pros", data);

        var pms = [], bds = {};
        $.each(data, function (index, value) {//p_id,p_content
            pms.push("<li id='" + value.p_id + "'>" + value.p_content + "</li>");
            bds[value.p_id] = function () { getSortData({ id: "cxinfo" }, value.p_id) };
        });

        $("#cxli").html(pms.join(''));
        //getSortData({ id: 'ssdate' }, flag);
        $('#cxinfo').contextMenu('myMenu3', {
            bindings: bds
        });

    });

    $("#cosenfixed").stickyTableHeaders({ fixedOffset: 37 });



    //$('#rknum').contextMenu('myMenu1', {
    //    bindings: {
    //        'desc': desc,
    //        'asc': asc,
    //        'num1': function (t) { number(t, 'num1') },
    //        'num2': function (t) { number(t, 'num2') },
    //        'num3': function (t) { number(t, 'num3') },
    //        'num4': function (t) { number(t, 'num4') },
    //        'num5': function (t) { number(t, 'num5') }
    //    }
    //});

    $('#nsty_no').contextMenu('myMenu5', {
        bindings: {
            'desc': desc,
            'asc': asc,

        }
    });
    $('#sty_nm').contextMenu('myMenu4', {
        //bindings: {
        //    'search_name': desc
        //}
    });
    $('#ssdate').contextMenu('myMenu2', {
        bindings: {
            'empty': function () { filter('empty'); },
            'm_01': function () { filter('m_01'); },
            'm_02': function () { filter('m_02'); },
            'm_03': function () { filter('m_03'); },
            'm_04': function () { filter('m_04'); },
            'm_05': function () { filter('m_05'); },
            'm_06': function () { filter('m_06'); },
            'm_07': function () { filter('m_07'); },
            'm_08': function () { filter('m_08'); },
            'm_09': function () { filter('m_09'); },
            'm_10': function () { filter('m_10'); },
            'm_11': function () { filter('m_11'); },
            'm_12': function () { filter('m_12'); },
            'all': function () { filter('all'); }
        }
    });

    //$('#chnum').contextMenu('myMenu1', {
    //    bindings: {
    //        'desc': desc,
    //        'asc': asc,
    //        'num1': function (t) { number(t, 'num1') },
    //        'num2': function (t) { number(t, 'num2') },
    //        'num3': function (t) { number(t, 'num3') },
    //        'num4': function (t) { number(t, 'num4') },
    //        'num5': function (t) { number(t, 'num5') }
    //    }
    //});

    //$('#dbnum').contextMenu('myMenu1', {
    //    bindings: {
    //        'desc': desc,
    //        'asc': asc,
    //        'num1': function (t) { number(t, 'num1') },
    //        'num2': function (t) { number(t, 'num2') },
    //        'num3': function (t) { number(t, 'num3') },
    //        'num4': function (t) { number(t, 'num4') },
    //        'num5': function (t) { number(t, 'num5') }
    //    }
    //});

    //$('#thnum').contextMenu('myMenu1', {
    //    bindings: {
    //        'desc': desc,
    //        'asc': asc,
    //        'num1': function (t) { number(t, 'num1') },
    //        'num2': function (t) { number(t, 'num2') },
    //        'num3': function (t) { number(t, 'num3') },
    //        'num4': function (t) { number(t, 'num4') },
    //        'num5': function (t) { number(t, 'num5') }
    //    }
    //});

    //$('#xsnum').contextMenu('myMenu1', {
    //    bindings: {
    //        'desc': desc,
    //        'asc': asc,
    //        'num1': function (t) { number(t, 'num1') },
    //        'num2': function (t) { number(t, 'num2') },
    //        'num3': function (t) { number(t, 'num3') },
    //        'num4': function (t) { number(t, 'num4') },
    //        'num5': function (t) { number(t, 'num5') }
    //    }
    //});

    $('#cknum').contextMenu('myMenu1', {
        bindings: {
            'desc': desc,
            'asc': asc,
            'num1': function (t) { number(t, 'num1') },
            'num2': function (t) { number(t, 'num2') },
            'num3': function (t) { number(t, 'num3') },
            'num4': function (t) { number(t, 'num4') },
            'num5': function (t) { number(t, 'num5') }
        }
    });

    $("#search_name").keypress(function (e) {
        if (e.keyCode === 13) {

            $("body").click();
            //alert($(this).val());
            getSortData({ id: 'search_name' }, $(this).val());
        }
    });

    $("#search_value").keypress(function (e) {
        if (e.keyCode === 13) {
            $("body").click();
            getSortData({ id: 'search_style' }, $(this).val());
        }
    });
});


function desc(t) {
    trMenu.find("img").remove();
    $(t).append("<img src='http://cdn.staticfile.org/jquery.tablesorter/2.17.3/css/images/green-desc.gif'/>");

    getSortData(t, "desc");
}



function asc(t) {
    trMenu.find("img").remove();
    $(t).append("<img src='http://cdn.staticfile.org/jquery.tablesorter/2.17.3/css/images/green-asc.gif'/>");

    getSortData(t, "asc");
}

function number(t, flag) {
    //console.log(t.id);
    getSortData(t, flag);
}

function filter(flag) {
    // console.log(flag);
    //alert(t.id);
    getSortData({ id: 'ssdate' }, flag);
}

function getSortData(t, sortT) {
    //topHeader.hide();
    var vm = new ReportViewModel();
    vm.cosen["hdpagenum"].val("1");
    vm.options.data = {
        sltType: $("#sltcond").val(),
        sort: t.id,//按那个字段进行排序
        sortT: sortT,//升序还是降序
        dps: $("body").data("dps"),
        startDate: $("body").data("startDate"),
        endDate: $("body").data("endDate"),
        styleNo: $("body").data("styleNo")
    };
    //console.log(vm.options);
    $('#cosenFrm').ajaxSubmit(vm.options);
    $("body").data("opts", vm.options);
    vm.cosen.dpmodal.modal('hide');
    var pagenum = parseInt(vm.cosen["hdpagenum"].val());
    //if (pagenum === 1) {
    //    $(window).unbind("scroll");
    //    $(window).scroll(function () {
    //        //topHeader.hide();
    //        var totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
    //        if ($(document).height() <= totalheight) {
    //            if (vm.cosen["finished"] !== "true") {

    //                $('#cosenFrm').ajaxSubmit(vm.options);
    //            }
    //        }
    //    });
    //}
}

function ReportViewModel() {
    var self = this;
    //根据选择查询条件不同显示不同的输入库
    self.changeCond = ko.observable(false);
    //查询条件 1：入库时间  2:上市日期
    self.sltcond = "1";//默认按入库查询
    //店铺
    self.dianpus = ko.observableArray();
    //拼接字符串用的
    self.htmlStr = [];
    //加载店铺数据
    self.loadData = function () {
        var html = $.data(document.body, "dianpu")
        if (html === undefined) {

            $.get('/api/DpApi/GetDianpus', function (data) {
                self.dianpus(data);

                $.data(document.body, "dianpu", data);
                self.renderDp();
            });


        } else {
            self.dianpus(html);
            self.renderDp();
        };

    }
    //渲染店铺
    self.renderDp = function () {
        self.htmlStr.push("<table class='table table-striped table-hover table-condensed text-left'><tr>");

        $.each(self.dianpus(), function (index, value) {
            if ((index) % 6 === 0 && index !== 0) {
                self.htmlStr.push("</tr><tr><td><label><input type='checkbox' checked='checked' value='" + value.Use_id + "'/>" + value.Use_nm + "</label></td>");
            } else {
                self.htmlStr.push("<td><label><input type='checkbox' checked='checked' value='" + value.Use_id + "'/>" + value.Use_nm + "</label></td>");
            }
        });
        self.htmlStr.push("</tr><tfoot><tr><td colspan='6' class='text-center'><label><input type='checkbox' checked='checked' name='selectdp' id='sltall' onclick='selectall(this);'/>选择全部</label></td></tr></tfoot></table>");
        self.cosen.dpcont.html(self.htmlStr.join(''));
    };
    self.cosen = {
        cosenModal: $("#cosenModal"),
        cosenProgress: $("#cosenProgress"),
        cosenResult: $("#cosenResult"),
        report: $("#report"),
        count: 1,
        dpmodal: $("#dpmodal"),
        dpcont: $("#dpcont"),
        hdpagenum: $("#hdpagenum"),
        finished: "false",
        startdate: $("#startdate"),
        enddate: $("#enddate"),
        startmonth: $("#startmonth"),
        endmonth: $("#endmonth"),
        styleno: $("#styleno"),
        loadmodal: $("#loadmodal"),
    };

    //ajax 请求之前
    self.showRequest = function (formData, jqForm, options) {
        var queryString = $.param(formData);
        if (self.cosen["hdpagenum"].val() === "1") {
            self.cosen.report.html("<tr><td colspan='15'>正在查询...</td></tr>")
        }
        self.cosen.loadmodal.modal({ show: true, backdrop: 'static' });
        return true;
    }
    //ajax 请求之后
    self.showResponse = function (responseText, statusText) {

        var pagenum = parseInt(self.cosen["hdpagenum"].val());
        if (pagenum === 1) {
            self.cosen.report.html("");
            //    $(window).scroll(function () {
            //        var totalheight = parseFloat($(window).height()) + parseFloat($(window).scrollTop());
            //        if ($(document).height() <= totalheight) {
            //            if (self.cosen["finished"] !== "true") {
            //                $('#cosenFrm').ajaxSubmit(self.options);
            //            }
            //        }
            //    });
        }
        if (responseText.length === 0) {
            $("#tfoot").hide();
            //self.cosen["report"].append("<tr><td colspan='12'>查询完成</td></tr>");
            if (pagenum === 1) {
                self.cosen.report.html("<tr><td colspan='15'>没有查到数据</td></tr>");
            }
            self.cosen["finished"] = "true";

        } else {
            self.htmlStr = [];
            //var options = createSelect();
            var pindex = (pagenum - 1) * 10;
            $.each(responseText, function (index, value) {
                //查询所有的
                //self.htmlStr.push("<tr><td>" + (pindex + index + 1) +
                //    "</td><td>" + value.nsty_no + "</td><td>" + value.com_nm +
                //    "</td><td><input ondblclick='editDate(this);' id='" + value.nsty_no + "' type='number' max='12' min='01' class='form-control input-sm' readonly value='" + (value.editionhandle === null ? "" : value.editionhandle) + "'/></td>" +
                //    "<td><select ondblclick='editDate(this);' id='cx_" + value.nsty_no + "'    class='form-control input-sm' readonly value='" + (value.p_id === null ? "" : value.p_id) + "'>" + createSelect(value.p_id) + "</select></td><td><img src='/static/images/single/" + value.nsty_no +
                //    ".jpg' width='60' height='60'/></td><td title='入库' style='color:#0099CC;font-weight:bold'>" + value.rknum + "</td><td title='出货' style='color:#663300;font-weight:bold'>" + value.chnum +
                //    "</td><td style='color:#66CC00;font-weight:bold' title='调拨'>" + value.dbnum + "</td><td title='退货' style='color:#006633;font-weight:bold'>" + value.thnum +
                //    "</td><td style='color:#CCCC00;font-weight:bold' title='销售'>" + value.xsnum + "</td><td title='库存' style='color:#9933FF;font-weight:bold'>" + value.cknum +
                //    "</td><td title='单价'>" + value.unitprice + "</td><td title='金额'>" + value.xsmoney +
                //    "</td><td><input type='button' onclick='saveDate(this);' id='btn_" + value.nsty_no + "' class='btn btn-primary btn-sm'  value='保存'/><input type='button' onclick='cancelSave(this);' id='cancel_" + value.nsty_no + "' class='btn btn-warning btn-sm'  value='取消'/></td></tr>");

                //下面的只是查询库存
                self.htmlStr.push("<tr><td>" + (pindex + index + 1) +
                    "</td><td>" + value.nsty_no + "</td><td>" + value.com_nm +
                    "</td><td><input ondblclick='editDate(this);' id='" + value.nsty_no + "' type='number' max='12' min='01' class='form-control input-sm' readonly value='" + (value.editionhandle === null ? "" : value.editionhandle) + "'/></td>" +
                    "<td><select ondblclick='editDate(this);' id='cx_" + value.nsty_no + "'    class='form-control input-sm' readonly value='" + (value.p_id === null ? "" : value.p_id) + "'>" + createSelect(value.p_id) + "</select></td><td><img src='/static/images/single/" + value.nsty_no +
                    ".jpg' width='60' height='60'/></td><td title='库存' style='color:#9933FF;font-weight:bold'>" + value.cknum +
                    "</td><td><input type='button' onclick='saveDate(this);' id='btn_" + value.nsty_no + "' class='btn btn-primary btn-sm'  value='保存'/><input type='button' onclick='cancelSave(this);' id='cancel_" + value.nsty_no + "' class='btn btn-warning btn-sm'  value='取消'/></td></tr>");
            });
            self.cosen.report.append(self.htmlStr.join(''));
            self.cosen["hdpagenum"].val(parseInt(self.cosen["hdpagenum"].val()) + 1);
            self.cosen["finished"] = "false";

            $("#tfoot").show();
        }
        self.cosen.loadmodal.modal('hide');
    }

    //ajax 请求的参数
    self.options = {
        beforeSubmit: self.showRequest,  // pre-submit callback
        success: self.showResponse,  // post-submit callback
        url: '/report/search/',         // override for form's
        timeout: 400000,
        type: 'post'        // 'get' or 'post', override
    };

    //查询按钮事件
    self.search = function () {
        //topHeader.hide();
        self.cosen["hdpagenum"].val("1");
        var ckbs = self.cosen.dpcont.children('table').find('input:checkbox')
        var dps = ""
        $.each(ckbs, function (index, ckb) {
            if ($(ckb).attr('id') !== 'sltall') {
                if ($(ckb).prop('checked') === true) {
                    dps += $(ckb).attr("value") + "$";
                }
            }
        });
        if (dps.length !== 0) {
            self.options.url = "/report/search";
            dps = dps.substr(0, dps.length - 1);
        } else {
            self.options.url = "/report/search/?dps=nodp";
            dps = 'nodp'
        }
        self.options.data = {
            sltType: self.sltcond,
            dps: dps,
            pagenum: 1,
            startDate: self.sltcond === "1" ? self.cosen.startdate.val() : self.cosen.startmonth.val(),
            endDate: self.sltcond === "1" ? self.cosen.enddate.val() : self.cosen.endmonth.val(),
            styleNo: self.cosen.styleno.val(),
            sort: 'default',
            sortT: ''
        };
        //self.options.async = false;

        $("body").data("dps", dps);
        $("body").data("startDate", self.options.data.startDate);
        $("body").data("endDate", self.options.data.endDate);
        $("body").data("styleNo", self.options.data.styleNo);

        //console.log(self.options.data);
        $('#cosenFrm').ajaxSubmit(self.options);
        
        $("body").data("opts", self.options);
        self.cosen.dpmodal.modal('hide');
        return false;
    };
    //导出excel
    self.outexcel = function () {
        var v = self.cosen.enddate.val();
        window.open('/report/outexcel?startDate=' + self.cosen.startdate.val() + "&endDate=" + v);
    }

    //切换查询条件
    self.changeSlt = function () {

        self.sltcond = $("#sltcond").val();
        if (self.sltcond === "1") {
            self.changeCond(false);
        } else {
            self.changeCond(true);
        }
    }

    //点击标题排序
    self.sort = function (data, event) {
        var target = $(event.target);
        target.find("img").remove();
        target.append("<img src='http://cdn.staticfile.org/jquery.tablesorter/2.17.3/css/images/green-desc.gif'/>");
        target.unbind("mouseover");
        target.unbind("mouseout");
    }

    //加载更多...
    self.loadMore = function () {
        if (self.cosen["finished"] !== "true") {
            $('#cosenFrm').ajaxSubmit($("body").data("opts"));
        }
    }

    self.loadData();
};


ko.applyBindings(new ReportViewModel());


//全选
function selectall(obj) {
    var r = new ReportViewModel();
    var bj = $(obj), ckbs = r.cosen.dpcont.children('table').find('input:checkbox')
    $.each(ckbs, function (index, ckb) {
        $(ckb).prop("checked", bj.prop('checked'));
    });
}

//ondblclick  编辑上市日期
function editDate(obj) {

    var input = $(obj), style_no, btn;
    if (input[0].localName === "select") {
        btn = "#btn_" + input.attr("id").split('_')[1];

    } else {
        style_no = input.attr("id");
        btn = "#btn_" + style_no
    }
    input.removeAttr('readonly');
    //$(btn).removeAttr("disabled");

}

function createSelect(v) {
    var pros = $(document.body).data("pros"), html = [];

    html.push("<option value=''>--请选择--</option>");

    $.each(pros, function (index, value) {

        if (value !== null && value.p_id === v) {
            html.push("<option selected='selected' value='" + value.p_id + "'>" + value.p_content + "</option>");
        }
        else
            html.push("<option  value='" + value.p_id + "'>" + value.p_content + "</option>");
    })
    return html.join('');
}

//保存日期
function saveDate(obj) {
    var btn = $(obj), style_no = btn.attr("id").split('_')[1], input = $("#" + style_no), cx = $("#cx_" + style_no);
    var month = parseInt(input.val()), cxinfo = cx.val();
    $.post('/report/updatedate', { styleNo: style_no, date: month < 10 ? "0" + month.toString() : month.toString(), cxinfo: cxinfo }, function (data, textStatus, jqXHR) {
        if (data === "success") {
            input.attr("readonly", "readonly");
            //btn.attr('disabled', 'disabled');
            alert("保存成功");
        } else {
            alert("保存失败,请注意您的权限");
        }
    });


}
//取消操作
function cancelSave(obj) {
    var cancel = $(obj), style_no = cancel.attr("id").split('_')[1], input = $("#" + style_no);
    var btn = $("#btn_" + style_no), cx = $("#cx_" + style_no);
    input.attr("readonly", "readonly");
    cx.attr("readonly", "readonly");
    cx.val('');
    //btn.attr('disabled', 'disabled');
}



