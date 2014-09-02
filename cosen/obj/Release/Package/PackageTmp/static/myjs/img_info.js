

jQuery(document).ready(function($) {


	 // $(window).scroll( function() {
		//   $("img").each(function(i){
		//     if($(this).offset().top<($(window).height()+document.documentElement.scrollTop-200)){
		//     $(this).attr("src",$(this).attr("value"));
		//    }
		//  });
	 // });
	$("img").lazyload({
      // placeholder : "默认等待图片",
      effect      : "fadeIn"
      });

	$('#cosenfixed img').toggle(function() {
		// console.log($(this).attr('alt'))
		var img=$(this),styleno=img.attr('alt').split('.')[0];
		get_img_info(styleno,img);
		
	}, function() {
		$(this).grumble('hide');
	});
	
});	

function get_img_info(styleno,img){
	$.get('/lfl/get_img_info/'+styleno+'/', function(data) {
		// console.log(data);
		img.grumble(
			{
				text: data,
				angle: 85,
				distance: 50,
				// showAfter: 4000,
				// hideAfter: 2000,
				type:'alt-'
			}
		);
	});
}