$(document).ready(function(){
        
    $('.tecGodsPrice .mark').click(function(){


        var trForm = $(this).closest('tr');

        if($(trForm).hasClass('selecedPrice')){

            $('.selecedPrice').removeClass('selecedPrice');
        }
        else{
            $('.selecedPrice').removeClass('selecedPrice');

            $(trForm).addClass('selecedPrice');
        }



    });
});