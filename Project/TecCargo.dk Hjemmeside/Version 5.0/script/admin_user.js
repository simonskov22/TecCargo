$(document).ready(function(){
                
    $(".adminDropDown .dropdownHeader").click(function(){

        $('.adminDropDown .dropdownOption ul li').slideToggle();
    });
});

function GetData(link){
    $.getJSON(link,function(data){

       $("input[name='userId']").val(data.userId).attr('bind-resetval',data.userId);

       $("input[name='user']").val(data.user).attr('bind-resetval',data.user);
       $("input[name='name']").val(data.name).attr('bind-resetval',data.name);
       $("input[name='lastname']").val(data.lastname).attr('bind-resetval',data.lastname);
       $("input[name='email']").val(data.email).attr('bind-resetval',data.email);
    }).done(function(){
        $('.adminDropDown .dropdownOption ul li').slideUp();
    });

}