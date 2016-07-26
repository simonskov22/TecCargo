$(document).ready(function(){
    
    $(window).load(function(){
                    
        $('.TecNavigation .navbar:has(div)').each(function(){

            //alert($(this).outerHeight());

            var height = $(this).outerHeight(true);

            $(this).children('div').css('top',height);
        });
        
    });

   $(".TecNavigation .navbar").hoverIntent( {    
        sensitivity: 3, // number = sensitivity threshold (must be 1 or higher)    
        interval: 200, // number = milliseconds for onMouseOver polling interval    
        over: function(){ 
            $(".showOnHover",this).slideDown();
        }, // function = onMouseOver callback (REQUIRED)    
        timeout: 500, // number = milliseconds delay before onMouseOut    
        out: function(){
            $(".showOnHover",this).slideUp();
        } // function = onMouseOut callback (REQUIRED)
   });


});