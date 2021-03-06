﻿$(document).ready(function (e) {
    cosen = {};
    cosen["txtMaster"] = $("#txtMaster");
    cosen["txtCompose"] = $("#txtCompose");
    cosen["txtLegging1"] = $("#txtLegging1");
    cosen["txtLegging2"] = $("#txtLegging2");
    cosen["txtBottom1"] = $("#txtBottom1");
    cosen["txtBottom2"] = $("#txtBottom2");
    cosen["txtAccessory1"] = $("#txtAccessory1");
    cosen["txtAccessory2"] = $("#txtAccessory2");

    cosen["composeImg"] = $("#composeImg");
    cosen["masterImg"] = $("#masterImg");
    cosen["leggingImg1"] = $("#leggingImg1");
    cosen["leggingImg2"] = $("#leggingImg2");
    cosen["bottomImg1"] = $("#bottomImg1");
    cosen["bottomImg2"] = $("#bottomImg2");
    cosen["accessoryImg1"] = $("#accessoryImg1");
    cosen["accessoryImg2"] = $("#accessoryImg2");

    cosen["stdp"] = $("#stdp");

    cosen["msg"] = $("#msg");


    $.getJSON("/api/dpapi/GetStyles", function (arr) {
        //console.log(arr);
        //window.localStorage['data'] = data;
        //arr = $.parseJSON(data); //一开始就把页面下载到本地，然后再在本地进行操作
        autoCompose(arr.Names, cosen['txtCompose'], cosen["composeImg"]);//组合图

        autoC(arr.Styles, cosen['txtMaster'], cosen["masterImg"]);//以下都是单款图
        autoC(arr.Styles, cosen['txtLegging1'], cosen["leggingImg1"]);
        autoC(arr.Styles, cosen['txtLegging2'], cosen["leggingImg2"]);
        autoC(arr.Styles, cosen['txtBottom1'], cosen["bottomImg1"]);
        autoC(arr.Styles, cosen['txtBottom2'], cosen["bottomImg2"]);
        autoC(arr.Styles, cosen['txtAccessory1'], cosen["accessoryImg1"]);
        autoC(arr.Styles, cosen['txtAccessory2'], cosen["accessoryImg2"]);
    });


    var options = {
        //target: '#msg',   // target element(s) to be updated with server response 
        beforeSubmit: showRequest,  // pre-submit callback 
        success: showResponse,  // post-submit callback 
        //clearForm: true,        // clear all form fields after successful submit 
        resetForm: true      // reset the form after successful submit 
        //timeout: 3000
    };
    $('#frmdp').ajaxForm(options);//指示绑定设置

    $("#stdp").change(function () {
        var master = $("#txtMaster").val();
        if (master.length !== 0) {

            dp(master, $(this).val(), true, cosen["masterImg"], 'single');
            // dp(master, $(this).val());
        }
    });



});

function autoCompose(d, id, imgid) {
    id.autocomplete(d, {
        max: 20,    //列表里的条目数
        minChars: 0,    //自动完成激活之前填入的最小字符
        width: 250,     //提示的宽度，溢出隐藏
        scrollHeight: 350,   //提示的高度，溢出显示滚动条
        matchContains: true,    //包含匹配，就是data参数里的数据，是否只要包含文本框里的数据就显示
        autoFill: false,    //自动填充
        formatItem: function (row, i, max) {//这个是下拉列表的显示样式
            return "<img style='width:60px;height:60px;' src='/static/images/compose/" + row.Name + ".jpg'/> " + row.Name;
        },
        formatMatch: function (row, i, max) {//这个是过滤条件
            return row.Name;
            // return (row.StyleCode);  
        },
        formatResult: function (row) {//这个是单击选项条的时候在文本框中的显示结果

            return row.Name;
            ;
        }
    }).result(function (event, row, formatted) {
        dp(row.Name, cosen["stdp"].val(), true, imgid, 'compose');

    });
}

function autoC(d, id, imgid) {
    id.autocomplete(d, {
        max: 20,    //列表里的条目数
        minChars: 0,    //自动完成激活之前填入的最小字符
        width: 250,     //提示的宽度，溢出隐藏
        scrollHeight: 350,   //提示的高度，溢出显示滚动条
        matchContains: true,    //包含匹配，就是data参数里的数据，是否只要包含文本框里的数据就显示
        autoFill: false,    //自动填充
        formatItem: function (row, i, max) {//这个是下拉列表的显示样式
            return "<img style='width:60px;height:60px;' src='/static/images/single/" + row.StyleCode + row.CmCode + ".jpg'/> " + row.StyleCode + row.CmCode;
            //return row.StyleCode + row.CmCode + "[" + row.StyleName + "]";
        },
        formatMatch: function (row, i, max) {//这个是过滤条件
            return row.StyleCode + row.CmCode;
            // return (row.StyleCode);  
        },
        formatResult: function (row) {//这个是单击选项条的时候在文本框中的显示结果

            return (row.StyleCode + row.CmCode);
            ;
        }
    }).result(function (event, row, formatted) {
        var id = event.target.getAttribute('id');
        if (id === "txtMaster") {

            dp(row.StyleCode + row.CmCode, cosen["stdp"].val(), true, imgid, 'single');

        } else {
            dp(row.StyleCode + row.CmCode, cosen["stdp"].val(), false, imgid, 'single');
        }
    });
}
//根据主打款和搭配类型
function dp(style, stdp, ismaster, imgid, gtype) {
    if (ismaster) {
        $.post("/api/DpApi/GetDpByMaster/?style=" + style + "&dpType=" + stdp + "&gType=" + gtype, function (data) {
            //console.log(data);
            if (data.length !== 0 && data !== "None") {//如果存在搭配类型那么就显示出来
                //var json = $.parseJSON(data);
                //console.log(json);
                data = data[0];
                if (data.cbpicture !== "" && data.cbpicture !== "None" && data.cbpicture !== null) {
                    cosen["txtCompose"].val(data.cbpicture);
                    cosen["composeImg"].attr('src', '/static/images/compose/' + data.cbpicture + ".jpg");
                }
                else {
                    cosen["txtCompose"].val("");
                    cosen["composeImg"].attr('src', '/static/images/myimg/noimg.jpg')
                }
                if (data.masterstyle !== "" && data.masterstyle !== "None" && data.masterstyle !== null) {
                    cosen["txtMaster"].val(data.masterstyle);
                    cosen["masterImg"].attr('src', '/static/images/single/' + data.masterstyle + ".jpg");
                }
                else {
                    cosen["txtMaster"].val("");
                    cosen["masterImg"].attr('src', '/static/images/myimg/noimg.jpg')
                }
                if (data.type !== "" && data.type !== "None" && data.type !== null) {
                    cosen["stdp"].val(data.type);
                }
                if (data.legging1 !== "" && data.legging1 !== "None" && data.legging1 !== null) {
                    cosen["txtLegging1"].val(data.legging1);
                    //console.log(cosen["txtLegging1"].val());
                    cosen["leggingImg1"].attr('src', '/static/images/single/' + data.legging1 + ".jpg");
                }
                else {
                    cosen["txtLegging1"].val("");
                    cosen["leggingImg1"].attr('src', '/static/images/myimg/noimg.jpg')
                }
                if (data.legging2 !== "" && data.legging2 != "None" && data.legging2 != null) {
                    cosen["txtLegging2"].val(data.legging2);
                    cosen["leggingImg2"].attr('src', '/static/images/single/' + data.legging2 + ".jpg");
                }
                else {
                    cosen["txtLegging2"].val("");
                    cosen["leggingImg2"].attr('src', '/static/images/myimg/noimg.jpg')
                }
                if (data.bottom1 !== "" && data.bottom1 !== "None" && data.bottom1 !== null) {
                    cosen["txtBottom1"].val(data.bottom1);
                    cosen["bottomImg1"].attr('src', '/static/images/single/' + data.bottom1 + ".jpg");
                }
                else {
                    cosen["txtBottom1"].val("");
                    cosen["bottomImg1"].attr('src', '/static/images/myimg/noimg.jpg')
                }
                if (data.bottom2 !== "" && data.bottom2 !== "None" && data.bottom2 !== null) {
                    cosen["txtBottom2"].val(data.bottom2);
                    cosen["bottomImg2"].attr('src', '/static/images/single/' + data.bottom2 + ".jpg");
                }
                else {
                    cosen["txtBottom2"].val("");
                    cosen["bottomImg2"].attr('src', '/static/images/myimg/noimg.jpg')
                }
                if (data.accessory1 !== "" && data.accessory1 != "None" && data.accessory1 != null) {
                    cosen["txtAccessory1"].val(data.accessory1);
                    cosen["accessoryImg1"].attr('src', '/static/images/single/' + data.accessory1 + ".jpg");
                }
                else {
                    cosen["txtAccessory1"].val("");
                    cosen["accessoryImg1"].attr('src', '/static/images/myimg/noimg.jpg')
                }
                if (data.accessory2 !== "" && data.accessory2 !== "None" && data.accessory2 !== null) {
                    cosen["txtAccessory2"].val(data.accessory2);
                    cosen["accessoryImg2"].attr('src', '/static/images/single/' + data.accessory2 + ".jpg");
                }
                else {
                    cosen["txtAccessory2"].val("");
                    cosen["accessoryImg2"].attr('src', '/static/images/myimg/noimg.jpg')
                }
            } else {//这个是不存在的情况,清空前面的查询记录
                imgid.attr('src', '/static/images/' + gtype + "/" + style + ".jpg");
                if (ismaster === true) {
                    if (gtype === "compose") {

                        if (cosen["txtMaster"].val() === "") {
                            cosen["masterImg"].attr('src', '/static/images/myimg/noimg.jpg');
                        }

                        //cosen["stdp"].val("1");
                    }
                    if (gtype === "single") {
                        if (cosen["txtCompose"].val() === "") {

                            cosen["composeImg"].attr('src', '/static/images/myimg/noimg.jpg');
                        }
                    }
                    cosen["txtLegging1"].val("");
                    cosen["leggingImg1"].attr('src', '/static/images/myimg/noimg.jpg');
                    cosen["txtLegging2"].val("");
                    cosen["leggingImg2"].attr('src', '/static/images/myimg/noimg.jpg');

                    cosen["txtBottom1"].val("");
                    cosen["bottomImg1"].attr('src', '/static/images/myimg/noimg.jpg');
                    cosen["txtBottom2"].val("");
                    cosen["bottomImg2"].attr('src', '/static/images/myimg/noimg.jpg');

                    cosen["txtAccessory1"].val("");
                    cosen["accessoryImg1"].attr('src', '/static/images/myimg/noimg.jpg');
                    cosen["txtAccessory2"].val("");
                    cosen["accessoryImg2"].attr('src', '/static/images/myimg/noimg.jpg');
                    //cosen["stdp"].val("1");
                }


            }
        });
    } else {
        imgid.attr('src', '/static/images/' + gtype + "/" + style + ".jpg");
    }


}

function chgslt() {
    //alert("hello");
    dp(cosen["txtMaster"], cosen["stdp"].val(), true, cosen["masterImg"], 'single');
}

function showRequest(formData, jqForm, options) {
    if (confirm("您确定保存该搭配吗？那么以前的搭配会被覆盖")) {
        if (document.getElementById("txtMaster").value.length === 0) {
            cosen["msg"].fadeIn().text("主打款不能为空").fadeOut(2000);
            return false;
        }
        cosen["msg"].fadeIn().text("正在保存搭配信息...");
        return true;
    }
    return false;
}

function showResponse(responseText, statusText, xhr, $form) {

    cosen["msg"].text(responseText).fadeOut(2000);
    $("#frmdp img").each(function (index, el) {
        $(el).attr('src', '/static/images/myimg/noimg.jpg');
    });

}
