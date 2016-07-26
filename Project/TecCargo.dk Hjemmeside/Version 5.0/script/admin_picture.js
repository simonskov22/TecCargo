$(document).ready(function(){
        
    $('.tecPictureForm tr').click(function(){

        var value = $(this).attr('bind-value');
        var form = $(this).closest('form');

        if($(this).hasClass('tecFormPicturePickSelect')){

            $(this).removeClass('tecFormPicturePickSelect');

            $("input[value='"+value+"']",form).remove();
        }
        else{

            $(this).addClass('tecFormPicturePickSelect');
            $(form).append("<input class='removeDeletePic' type='hidden' name='deleteFiles[]' value='"+value+"' />")
        }

        tecDeletePicActive(form);
    });


    $(".tecPictureForm input[type='reset']").click(function(){

        var form = $(this).closest('form');


        if(tecDeletePicActive(form)){


            $('.removeDeletePic',form).remove();
            $('.tecFormPicturePickSelect', form).removeClass('tecFormPicturePickSelect');

        }

        tecDeletePicActive(form);
    });
    $(".tecPictureForm input[type='submit']").click(function(){

        var form = $(this).closest('form');

        if(tecDeletePicActive(form)){

            $(form).submit();
        }
    });
});

function tecDeletePicActive(form){

    var inputCount = 0;

    $(".tecFormPicturePickSelect",form).each(function(){
        inputCount++;
    });

    if(inputCount === 0){

        $("input[type='reset']", form).addClass('buttonInactive');
        $("input[type='submit']", form).addClass('buttonInactive');

        return false;
    }
    else{

        $("input[type='reset']", form).removeClass('buttonInactive');
        $("input[type='submit']", form).removeClass('buttonInactive');

        return true;
    }
}




$(document).ready(function(){
    
    //upload viser de billder man har valgt
    $('#pickPictureUpload').change(function ()
    {
        LoadPictureBeforeUpload(this);
    });
    
    });
    
    
    function LoadPictureBeforeUpload(input){
        
        if(window.File && window.FileList && window.FileReader) {
            
            var files = input.files ,
            filesLength = files.length,
            appendTo = ".tecFormPicturePickContent table";
            $(appendTo).empty();
            
            if(filesLength === 0){
                $("<tr><td>Ingen billeder valgt</td></tr>").appendTo(appendTo);
            }
            else{

                for (var i = 0; i < filesLength ; i++) {

                    var f = files[i];
                    var fileReader = new FileReader();

                    fileReader.onload = (function(e) {


                        var file = e.target;
                        var test = "<tr><td><img src='"+file.result+"' /></td>"
                                +"<td>" + f.name +"</td></tr>";

                        $(test).appendTo(appendTo);
                        //$("#uploadPicture_"+i).attr('src',e.target.result);

    //        ,{
    //                        class : "imageThumb",
    //                        src : e.target.result,
    //                        title : file.name
    //                                           }).insertAfter(".tecFormPicturePickContent");
                    });

                fileReader.readAsDataURL(f);
                }
            } 
        }
        else { alert("Your browser doesn't support to File API") }

    }
    
    function tecPictureBox_Show(){
    
        $('.tecFormUpload').show();
        $('#pickPictureUpload').click();
    }