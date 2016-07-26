

function showPage(link){
    
    
    $.get(link, function(data){
        
        if(!$('.tecShowPage',document.body).length){
            exuteWindows();
        }
        $('.tecShowPage .content').empty();
        $('.tecShowPage .content').append(data);
    }).done(function(){
        $('.tecShowPage').fadeIn();
    });
    
    
}

function closePage(id){
    
    $(id).fadeOut();
}

function exuteWindows(){
    
    $(document.body).append("<div class='tecShowPage'>"+
            "<div class='contentStyle'>"+
            "<img onclick='closePage(\".tecShowPage\");' class='closebutton' src='../images/icons/close.png'/>"+
            "<div class='content'></div></div></div>");
}

function pageStyle(arrayStyle){
    
    $.each(arrayStyle,function(key, value){
        
        $('head').append('<link href="'+value+'" rel="stylesheet" type="text/css">');
    });
}