$(document).ready(function(){
    calculator_status();

    $('#calculator_input').keydown(function(e){

        var code = e.which; // recommended to use e.which, it's normalized across browsers

        switch(code){
            case 13:
                e.preventDefault();
                calculator_submit();
                break;

            case 27:
                e.preventDefault();
                calculator_clear();
                break;
        }

    });  

    $('#calculator_input').keyup(function(e){

        calculator_status();
    });
});

function calculator_addvalue(value){

    var textNow = $('#calculator_input').text();
    $('#calculator_input').text(textNow + value);
    calculator_status();
}
function calculator_back(){

    var textNow = $('#calculator_input').text();
    $('#calculator_input').text(textNow.substring(0,textNow.length -1));
    calculator_status();
}
function calculator_clear(){

    $('#calculator_input').text('');
    calculator_status();
}
function calculator_status(){

    $('.helpText span').removeClass('calculaterStatusSeleted');
    var textNow = $('#calculator_input').text();
    var plusFound = 0;

    for(var i = 0; i < textNow.length; i++){

        if(textNow.substring(i,i + 1) === '+'){
            plusFound++;
        }
    }

    if(plusFound === 0){
        $('#calculaterStatus_1').addClass('calculaterStatusSeleted');
    }
    else if(plusFound === 1){
        $('#calculaterStatus_2').addClass('calculaterStatusSeleted');
    }
    else{
        $('#calculaterStatus_3').addClass('calculaterStatusSeleted');
    }

    return plusFound;
}
function calculator_submit(){

    var textNow = $('#calculator_input').text();
    var value = ["","",""];
    var select = 0;

    for(var i = 0; i < textNow.length; i++){

        var letter = textNow.substring(i,i + 1);

        if(letter === '+'){
            select++;
        }
        else{
            value[select] += letter;
        }
    }

    $.post("admin_calculator/",{action: "calculator",data: value},function(){

        $('#calculator_input').text('Loader . . .');

    },"json").done(function(data){

        if(data.status){
            $('#calculator_input').html("<span class='inputResult'>"+data.result+"</span>");
        }
        else{

            $('#calculator_input').text("Fejl! prøv igen.");
        }
    });
}