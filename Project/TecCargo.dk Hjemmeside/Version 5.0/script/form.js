//billeder og navn til billed vægler
var picktureList = null;

$(document).ready(function(){
    
    $('.formCheckLabel').each(function(){

        formCheckbutton(this);
    });
    
    
    //dropdown menu for bruger kan redigere
    $("#nowDropdownEdit ,#dropdownuserId, #dropdownuserId span").click(function () {
        $("#dropdownUserUl").addClass("showDropdownUsers");
    });
    
    
    //tjekker alle form med class tecForm for se om reset eller submit
    //skal være aktiv med det samme
    $('.tecForm').each(function(){
        
        tecFormCheckSubmit_Activate(this);
        tecFormCheckReset_Activate(this);
    });
    
    //opdatere alle input i tecForm for se om de skal have *
    //og tilføjer en knap hvis som kan åben billed vælger
    $('.tecForm input').each(function(){
        
        if($(this).attr('bind-required')){
            
            $(this).after("<span class='tecFormRequired tecFormRequiredRed'>*</span>");
        }
        
        if($(this).attr('type') === 'tecFormPicturePick'){
            
            var buttonText = $(this).attr('bind-buttonText');
            var lastSelect = $(this).attr('value');
            var hiddenName = $(this).attr('name');

            var hiddeVal = "<input type='hidden' name='"+hiddenName+"' value='"+lastSelect+"'/>";
            var buttonNew = "<input type='button' id='"+hiddenName+"'"+
                " bind-lastselect='"+lastSelect+"' bind-saveto='"+hiddenName+"'"+
                " onclick='tecFormPicturePick_Open(this);' class='tecFormPicturePickButtonOpen' value='"+buttonText+"'/>";

            $(this).replaceWith(hiddeVal + buttonNew);
        }
        
    });
    
    //når man skriver i et input bliver reset submit tjekket om de skal være aktiv
    //og om der skal være et hak ved vigtige felter
    $('.tecForm input').keyup(function(){
        
        var form = $(this).closest('form');
        
        tecFormCheckRequired(form);
        tecFormCheckSubmit_Activate(form);
        tecFormCheckReset_Activate(form);
    });
    
    //gør det lettere at hvad input man skriver i når man gå ind i den
    $('.tecForm input').focus(function(){
        var focusId = $(this).attr("bind-focusId");
        
        $(focusId).addClass('userCrowSelect');
        
    });
    
    //fjener focus fra det input man var i
    $('.tecForm input').blur(function(){
        
        var focusId = $(this).attr("bind-focusId");
        
        $(focusId).removeClass('userCrowSelect');
    });
    
    //sætter værdier tilbage til standar som står i bind-resetVal
    $(".tecForm input[type='reset']").click(function(){
        
        var form = $(this).closest('form');
        
        $("input[bind-resetVal]", form).each(function(){
            
            var defaultVal = $(this).attr("bind-resetVal");
            $(this).val(defaultVal);
        });
    });
    
    //sætter værdier tilbage til standar som står i bind-resetVal
    $(".tecForm input[type='submit']").click(function(){
        
        var form = $(this).closest('form');
        var allowSubmit = tecFormCheckSubmit_Activate(form);
        
        if(allowSubmit){
            form.submit();
        }
        else{
            
            alert('false');
        }
    });
    
    //gør det muligt at se hvilken billed man har valgt selv om html er blivet opdateret
    $(document.body).on('click', '.tecFormPicturePickBox tr', function (){
        
        $('.tecFormPicturePickBox tr').removeClass('tecFormPicturePickSelect');
        $(this).addClass('tecFormPicturePickSelect');
    });
    
    //når man skriver i søge feltet skal kun hvis de billeder der starter med
    //det man har skrevet
    $('.tecFormFloatBox .tecFormPicturePickHeader input').keyup(function(){
        
        var searchVal = $(this).val();
        
        LoadPicture(searchVal);
    });
    
    
    $('.tecFormPicker .tecFormClickOn').click(function(){
            
        var selectClass = "tecFormPicturePickSelect";
        var multiSelect = $(this).attr('bind-multiSelect');


            var form = $(this).closest('form');
            var inputName = $(this).attr("bind-inputname");
            var inputVal = $(this).attr("bind-inputval");

            if($(this).hasClass(selectClass)){

                $("input[name='"+inputName+"'][value='"+inputVal+"']",form).remove();

                $(this).removeClass(selectClass); 
            }
            else{

                $(form).append("<input class='tecFormPickerHidden' type='hidden' name='"+inputName+"' value='"+inputVal+"' />");

                $(this).addClass(selectClass);
            }

    });

    $(".tecFormPicker input[type='reset']").click(function (){

        var form = $(this).closest('form');

        $('.tecFormClickOn', form).removeClass('tecFormPicturePickSelect');
        $(".tecFormPickerHidden", form).remove();
    });
    
    
    
    
    var zindex = 25;
    
    $('.tecSelect .selected').each(function(){
        
        var divForm = $(this).closest('div');
        
        $(divForm).css('height', $(this).outerHeight());
        $('.tecSelect', divForm).css('z-index', zindex);
        
        zindex--;
    });
    
    $('.tecSelect .selected').click(function(){
        
        var ulForm = $(this).closest('ul');
        
        $('.option ul',ulForm).slideToggle();
    });
    
    $('.tecSelect .option ul li').click(function(){
        
        var ulForm = $(this).closest('ul').parent().closest('ul');
        var selectedText = $(this).text();
        var selectedVal = $(this).attr('data-value');
        var divForm = $(ulForm).parent();
        
        $('.option ul li',ulForm).removeClass('userCrowSelect');
        $(this).addClass('userCrowSelect');
        $('.selected .text',ulForm).text(selectedText);
        $("input",divForm).val(selectedVal).change();
        $('.option ul',ulForm).slideToggle();
    });
    
});
//når man klikker et sted på siden
$(document).click(function (e) { 

    //lukker bruger dropdown menu når man klikker på noget andet
    if($("#dropdownUserUl li").css("display") == "block" && !$("#nowDropdownEdit, #dropdownuserId, #dropdownuserId span").is(e.target)) {
        $("#dropdownUserUl").removeClass("showDropdownUsers");
    }
});


//tjekker om felterne overholder kræv og tilføjer et hak ellers *
function tecFormCheckRequired(form){
    
    $("input[bind-required]",form).each(function(){
        
        var valLent = $(this).val().length;
        var minLent = $(this).attr("bind-minlengt");
           
        if(!$(this).is('[bind-minlengt]') && (valLent >= 1) || 
            ($(this).is('[bind-minlengt]') && valLent >= minLent)){

            $("span",$(this).parent()).html("&#10003;");
            $("span",$(this).parent()).removeClass('tecFormRequiredRed');
            $("span",$(this).parent()).addClass('tecFormRequiredGreen');
        }
        else{

            $("span",$(this).parent()).html("*");
            $("span",$(this).parent()).removeClass('tecFormRequiredGreen');
            $("span",$(this).parent()).addClass('tecFormRequiredRed');
        }
        
        if($(this).is('[bind-compare]')){
            
            var name = $(this).attr('bind-compare');
            var compareVal = $('input[name="'+name+'"]',form).val();
            
            if(compareVal !== $(this,form).val()){
                
                $("span",$(this).parent()).html("*");
                $("span",$(this).parent()).removeClass('tecFormRequiredGreen');
                $("span",$(this).parent()).addClass('tecFormRequiredRed');
            }
        }
    });
    
}

//om submit skal kunne bruges
function tecFormCheckSubmit_Activate(form){
    
    var activateSubmit = true;
    
    $('.tecForm input[bind-required]').each(function(){
        
        var valLent = $(this).val().length;
        
        if($(this).is('[bind-minlengt]')){

            var minLent = $(this).attr("bind-minlengt");


            if(minLent > valLent){

                activateSubmit = false;

                return false;
            }
        }
        else{

            if(valLent <= 0){

                activateSubmit = false;

                return false;   
            }
        }
        
        if($(this).is('[bind-compare]')){
            
            var name = $(this).attr('bind-compare');
            var compareVal = $('input[name="'+name+'"]',form).val();
            
            if(compareVal !== $(this,form).val()){
                
                activateSubmit = false;
                return false;
            }
        }
        
    });
    
    if(activateSubmit){
        
        $("input[type='submit']",form).removeClass('buttonInactive');
    }
    else{
        
        $("input[type='submit']",form).addClass('buttonInactive');
    }
    
    return activateSubmit;
}

//om reset skal kunne bruges
function tecFormCheckReset_Activate(form){
    
    var activateReset = false;
    
    $('input[bind-resetVal]',form).each(function(){
        var resetval = $(this).attr('bind-resetVal');
        var valNow = $(this).val();
        
        if(resetval !== valNow){
            activateReset = true;
            
            return false;
        }
    });
    
    if(activateReset){
        
        $("input[type='reset']",form).removeClass('buttonInactive');
    }
    else{

        $("input[type='reset']",form).addClass('buttonInactive');
    }
}

//bliver brugt på edit user henter en person info og sætter dem i felter
//bind-resetVal bliver også opdateret
function SetUserValues(link,formId){
    
    $.getJSON(link, function(data){
    
        $.each(data,function(key, value){
            
            $("input[name='"+key+"']",formId).val(value);
            $("input[name='"+key+"']",formId).attr("bind-resetVal",value);
        });
    }).done(function(){
        
        tecFormCheckRequired(formId);
        tecFormCheckSubmit_Activate(formId);
        tecFormCheckReset_Activate(formId);
    });
    
}

//tilføjer javascript array som kan blive lavet med php til variable picktureList
function addPictureList(picture){
    picktureList = picture;
    
    LoadPicture('');
}

//opdater html i table hvor der bliver vis billed og navn
function LoadPicture(contain){
    
    if(picktureList === null){
        return false;
    }
    
    var containLower = contain.toLowerCase();
    var picturesHTML = "";
    
    for(var i = 0; i < picktureList.length; i++){
        
        if(picktureList[i][1].startsWith(containLower)){
            
            picturesHTML += "<tr>";
            picturesHTML += "<td style='width: 110px;'><img src='"+picktureList[i][0]+"' /></td>";
            picturesHTML += "<td>"+picktureList[i][1]+"</td>";
            picturesHTML += "</tr>";
        }
    }
    
    $(".tecFormFloatBox .tecFormPicturePickContent table").html(picturesHTML);
}

//Gemmer valgt billed link
function tecFormPicturePick_Save(trigger){

    var buttonName = $(trigger).closest('.tecFormFloatBox').attr('bind-buttonName');
    var saveName = $(trigger).closest('.tecFormFloatBox').attr('bind-saveName');
    var value = $('.tecFormPicturePickSelect img').attr('src');
    
    $(buttonName).attr('bind-lastselect',value);
    $("input[name='"+saveName+"']").val(value);
    
    tecFormPicturePick_Close('.tecFormFloatBox');
}

//åbener for vælg billed og giver den man har valgt sidst class tecFormPicturePickSelect
//så man kan se den lettere
function tecFormPicturePick_Open(trigger){
    
    LoadPicture("");
    var lastSelectImg = $(trigger).attr('bind-lastselect');
    
    $('.tecFormPicturePickContent img').each(function (){
        
        var imgSrc = $(this).attr('src');
        
        if(lastSelectImg === imgSrc){
            
            $(this).closest('tr').addClass('tecFormPicturePickSelect');
            
            return false;
        }
    });
    
    var buttonName = "#"+$(trigger).attr('id');
    var saveTo = $(trigger).attr('bind-saveto');
    
    $('.tecFormFloatBox').attr('bind-buttonName',buttonName);
    $('.tecFormFloatBox').attr('bind-saveName',saveTo);
    $('.tecFormFloatBox').show();
}

//lukker for et vindue
function tecFormPicturePick_Close(closeId){
    
    $(closeId).hide();
}
function tecCloseBox(closeId){

    $(closeId).fadeOut();
}

function formCheckbutton(selector){

    var checkBox = "#" + $(selector).attr('for');
    var onText = $(selector).attr('bind-on');
    var offText = $(selector).attr('bind-off');



    if($(checkBox).is(':checked')){

        $(selector).text(onText);
        $(selector).removeClass('formCheckBoxOff').addClass('formCheckBoxOn');
    }
    else{

        $(selector).text(offText);
        $(selector).addClass('formCheckBoxOff').removeClass('formCheckBoxOn');
    }
}
