$(document).ready(function(){
 
    $('.tecCustomPage .inputTitle').keyup(function(){

        var page = $(this).val();
        var link = $('.tecCustomPage #tecCustomPageLink').attr('bind-url') + page +'/';

        $('.tecCustomPage #tecCustomPageLink').text(link);
        $('.tecCustomPage #tecCustomPageLink').attr('href',link);
    });


    $('.tecDropdown .selected').click(function(){

        var ulForm = $(this).closest('ul');


        $('.option',ulForm).slideToggle();
    });

    $('.tecDropdown .option li').click(function (){

        var ulForm = $('.tecDropdown .selected .text').closest('ul');
        var text = $(this).text();
        var id = $(this).attr('bind-value');

        $('.tecDropdown .selected .text').text(text);
        $('.tecDropdown .selected .text').attr('bind-value', id);

        $('.option',ulForm).slideUp();
    });
});

function tecCustomPage_Edit(link, selector){

    var selectedVal = $(selector).parent().children('ul').children('.selected').children('.text').attr('bind-value');
    
    if(selectedVal !== '-1'){
        window.location.href = link + selectedVal+"/";
    }
}