	
//获取某一店铺的最近10个搭配日期信息
function showcols(obj){
	var id=$(obj).attr("href"),dpid=id.substring(1,id.length),panelbody=$(id+" div.panel-body");
	panelbody.html("加载数据...");
	$.post('/api/DpApi/LookAllTjDp/?dpId='+dpid, function (data) {
	    var html = "";//json=$.parseJSON(data),
		html="<div class='panel-group' id='accordion2'>"+
		     "</div>";
		$.each(data,function(index,value){
			html+="<div class='panel panel-info'>"+
		      		"<div class='panel-heading'>"+
		            		"<h4 class='panel-title'>"+
			            	"<a onclick='showdpdetails(this);'  data-toggle='collapse' data-parent='#accordion2' href='#"+value.tjdate+dpid+"' id='"+dpid+"'>"+value.tjdate+
				         "</a>"+
					 "</h4>"+
				"</div>"+
				"<div id='"+value.tjdate+dpid+"' class='panel-collapse collapse'>"+
		     			"<div class='panel-body'>"+
		     			"</div>"+
				"</div>"+
			"</div>";
		});
		panelbody.html("").append(html);
	});
}
//具体某一店铺某一天的搭配
function showdpdetails(obj){
	var href=$(obj).attr("href"),dpid=$(obj).attr("id"),tjdate=href.substr(1,10),
	panelbody=$(href+" div.panel-body");
	console.log(panelbody);
	if(panelbody.html()!==""){
		return true;
	}
	panelbody.html("加载数据...");
	$.post('/api/DpApi/LookUpTjDp?dpId=' + dpid + '&tjdate=' + tjdate, function (data) {
		//var json=$.parseJSON(data);
		if(data.length===0){
			
			panelbody.html("没有查询到推荐信息");
		}else{
			var html=[]
			html.push("<table class='table table-strip table-hover'>")
			$.each(data,function(index,value){
				html.push("<tr><td>"+(index+1)+"</td><td><img alt='"+value.cbpicture+"' onmouseover='showbigimg(event);' onmouseout='hidebigimg(event);' onmousemove='mousemoveonimg(event);'  class='img-circle' src='/static/images/compose/"+value.cbpicture+".jpg'/></td>")	
				

				if(value.masterstyle!=="None"&&value.masterstyle!==""){
					html.push("<td><img title='"+value.masterstyle+"' src='/static/images/single/"+value.masterstyle+".jpg' class='img-circle'/></br>"+value.mqu+"</td>");}
				else{
					html.push("<td></td>")
				}

				if(value.legging1!=="None"&&value.legging1!==""){
					html.push("<td><img class='img-circle' title='"+value.legging1+"' src='/static/images/single/"+value.legging1+".jpg'/></br>"+value.lqu1+"</td>");}
				else{
					html.push("<td></td>")
				}
				if(value.legging2!=="None"&&value.legging2!==""){
					html.push("<td><img class='img-circle' title='"+value.legging2+"' src='/static/images/single/"+value.legging2+".jpg'/></br>"+value.lqu2+"</td>");}
				else{
					html.push("<td></td>")
				}
				if(value.bottom1!=="None"&&value.bottom1!==""){
					html.push("<td><img class='img-circle' title='"+value.bottom1+"' src='/static/images/single/"+value.bottom1+".jpg'/></br>"+value.bqu1+"</td>");}
				else{
					html.push("<td></td>")
				}
				if(value.bottom2!=="None"&&value.bottom2!==""){
					html.push("<td><img class='img-circle' title='"+value.bottom2+"' src='/static/images/single/"+value.bottom2+".jpg'/></br>"+value.bqu2+"</td>");}
				else{
					html.push("<td></td>")
				}
				if(value.accessory1!=="None"&&value.accessory1!==""){
					html.push("<td><img class='img-circle' title='"+value.accessory1+"' src='/static/images/single/"+value.accessory1+".jpg'/></br>"+value.aqu1+"</td>");}
				else{
					html.push("<td></td>")
				}
				if(value.accessory2!=="None"&&value.accessory2!==""){
					html.push("<td><img class='img-circle' title='"+value.accessory2+"' src='/static/images/single/"+value.accessory2+".jpg'/><br/>"+value.aqu2+"</td>");}
				else{
					html.push("<td></td>")
				}
				html.push("</tr>")
			});
			html.push("</table>")
			panelbody.html("").append(html.join(""));
		}
	});
}
//显示组图的大图

function showbigimg(e){
	var evt=window.event?window.event:e;
	var imgid=evt.target.getAttribute("alt"),url="/static/images/composebig/"+imgid+"_bg.jpg";
	var x=10,y=20,tooltip="<div id='tooltip'><img src='"+url+"' class='img-circle'/></div>";
	$("body").append(tooltip);
	$("#tooltip").css({"position":"absolute","z-index":10011,"top":(evt.pageY+y)+"px","left":(evt.pageX+x)+"px"}).show("fast");

	
}
//隐藏组图的大图显示
function hidebigimg(obj){

	$("#tooltip").remove();
}
//鼠标在图片上移动
function mousemoveonimg(e){
	var evt=window.event?window.event:e,x=10,y=10;
	$("#tooltip").css({"position":"absolute","z-index":10011,"top":(evt.pageY+y)+"px","left":(evt.pageX+x)+"px"}).show("fast");
}

// knockout js

function AllTjViewModel() {
    var self = this;
    self.allTjs = ko.observableArray();

    self.loadAllTjs = function () {
        $.post('/api/DpApi/LookAllTjDp', function (data) {
            self.allTjs(data);
           // console.log(data);
        });
    }

    self.loadAllTjs();//调用 加载所有搭配过的店铺
}

ko.applyBindings(new AllTjViewModel());