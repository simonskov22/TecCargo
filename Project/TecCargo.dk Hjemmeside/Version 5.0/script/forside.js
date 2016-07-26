/* Member */
$(document).ready(function(){
    $('.circleBox .hoverLineShow').hover(function(){

        var linkHover = this;
        var lineShow = $(this).attr('bind-line');
        var circleBorder = "showRedBorder";

        if($(linkHover).is('[bind-links]')){

            linkHover = $(linkHover).attr('bind-links');
        }
        if($(this).hasClass('greenBorder')){
            circleBorder = "showGreenBorder";
        }

        $(linkHover).removeClass('hideBorder');
        $(lineShow).show();
        $('.circleBox .imgBox').addClass(circleBorder);
    },function(){

        var linkHover = this;
        var lineShow = $(this).attr('bind-line');
        var circleBorder = "showRedBorder";

        if($(linkHover).is('[bind-links]')){

            linkHover = $(linkHover).attr('bind-links');
        }
        if($(this).hasClass('greenBorder')){
            circleBorder = "showGreenBorder";
        }

        $('.circleBox .imgBox').removeClass(circleBorder);
        $(lineShow).hide();
        $(linkHover).addClass('hideBorder');
    });
});