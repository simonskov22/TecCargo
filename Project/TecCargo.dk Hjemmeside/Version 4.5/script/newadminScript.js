$(document).ready(function () {
    //var tecCargoLink = "http://teccargo.dk/testside/";
    var tecCargoLink = "http://teccargo.dk/";
    
    //Opret Bruger
    //
    //bliver brugt til opret bruger og viser hvad man ændre
    $(".userCrowRemove input").focus(function () {
        var row = $(this).attr("id");
        
        $(".userCrowRemove").removeClass("userCrowSelect");
        $("." + row).addClass("userCrowSelect");
    });
    
    
    //
    //
    //slet bruger
    $(".deleteUrow").click(function (e) {
        var id = $(this).attr('id');

        //om det er hak ved brugern
        if(!$(".adminNdeleteU input[type='checkbox'][value='" +id.substr(9) + "']").is(":checked") && !$(e.target).is("input[type='checkbox']")) {
            $(".adminNdeleteU input[type='checkbox'][value='" +id.substr(9) + "']").prop("checked", true);
        }
        else if(!$(e.target).is("input[type='checkbox']")){
            $(".adminNdeleteU input[type='checkbox'][value='" +id.substr(9) + "']").prop("checked", false);
        }

        $(".adminNdeleteU input[type='checkbox']").each(function () {
            $(".adminNdeleteU input[type='reset']").addClass("buttonInactive");
            $(".adminNdeleteU input[type='submit']").addClass("buttonInactive").removeClass("adminFdelete");
            if($(this).is(":checked")) {
                $(".adminNdeleteU input[type='reset']").removeClass("buttonInactive").attr("onclick","return true;");
                $(".adminNdeleteU input[type='submit']").removeClass("buttonInactive").addClass("adminFdelete")
                return false;
            }

        });
    });
    
    //deaktiver reset og sumit når man klikker på reset
    $(".adminNdeleteU input[type='reset']").click(function () {
        $(".removeInactive").addClass("buttonInactive").removeClass("adminFdelete").attr("onclick","return true;");
    });
    
    //
    //billede Indstillinger
    //
    //gør så slev om man ikke klikker på checkboxen så vil der stadig komme hak
    $(".adminpictureT tr").click(function (e) {
        var id = $(this).attr('id');
        
        //om der er hak ved billedet
        if(!$(".adminpictureT input[type='checkbox'][value='" +id.substr(4) + "']").is(":checked") &&
                !$(e.target).is("input[type='checkbox']") &&
                !$(e.target).is("button.pictureSettings")) {
            $(".adminpictureT input[type='checkbox'][value='" +id.substr(4) + "']").prop("checked", true);
        }
        else if(!$(e.target).is("input[type='checkbox']") &&
                !$(e.target).is("button.pictureSettings")){
            $(".adminpictureT input[type='checkbox'][value='" +id.substr(4) + "']").prop("checked", false);
        }

        $(".adminpictureT input[type='checkbox']").each(function () {
            $(".adminpictureT input[type='reset']").addClass("buttonInactive");
            $(".adminpictureT input[type='submit']").addClass("buttonInactive").removeClass("adminFdelete");
            if($(this).is(":checked")) {
                $(".adminpictureT input[type='reset']").removeClass("buttonInactive").attr("onclick","return true;");
                $(".adminpictureT input[type='submit']").removeClass("buttonInactive").addClass("adminFdelete")
                return false;
            }

        });
    });
    
    //deaktiver reset og sumit når man klikker på reset
    $(".adminNdeleteU input[type='reset'], .adminpictureT input[type='reset']").click(function () {
        $(".removeInactive").addClass("buttonInactive").removeClass("adminFdelete").attr("onclick","return true;");
    });
    
    //gør så slev om man ikke klikker på radioboxen så vil der stadig komme hak
    $(".adminChangePic fieldset div").click(function (e) {
        var id = $(this).attr('id');
        
        //om der er hak
        if(!$(".adminChangePic input#c" +id.substr(1)).is(":checked") &&
                !$(e.target).is("input[type='radio']")) {
            $(".adminChangePic input#c" +id.substr(1)).prop("checked", true);
        }
        else if(!$(e.target).is("input[type='radio']")){
            $(".adminChangePic input#c" +id.substr(1)).prop("checked", false);
        }
    });
    
    
    //får tilføj billede fægle efter vis man scoller
    $("#followScoll").sticky({topSpacing:0, className:"tablepicture"});
    
    //
    //      ###### CreateUser ######
    //

    //når der skrives i et input flet så
    //reset og submit knappen bliver aktiveret
    $(".adminNcreateU input").on('input', function () {

        //input der ikke kan skrives i 
        var inputs = ["input[type='radio']","input[type='reset']","input[type='submit']"];

        //aktivere reset
        $(".adminNcreateU input").each(function () {
            var value = $(this).val();

            //vis der er skrevet noget i value så tillad reset
            if(value && !$(this).is(inputs[0]) && !$(this).is(inputs[1]) && !$(this).is(inputs[2])) {
                $(inputs[1]).removeClass("buttonInactive").attr('onclick','return true');
                return false;
            }
            else {
                $(inputs[1]).addClass("buttonInactive").attr('onclick','return false');
            }
        });

        //tillad submit hvis
        // username, password, passwordC har en værdi og
        // password skal være ens med passwordC og
        // password skal være på 6 tegn

        var usernameT = $(".adminNcreateU input[name='username']").val();
        var passwordT = $(".adminNcreateU input[name='password']").val();
        var passwordCT = $(".adminNcreateU input[name='passwordC']").val();

        //om der skal være flueben eller stjerne ved det flet som skal udfyldes
        if(usernameT.length >= 3) {
            $('#usernameOK').html("&#10003;").removeClass("adminUserNeed").addClass("adminUserNeedOk");
        }
        else{
            $('#usernameOK').html("&#42;").removeClass("adminUserNeedOk").addClass("adminUserNeed");                
        }
        if(passwordT.length >= 6){
            $('#passwordOK').html("&#10003;").removeClass("adminUserNeed").addClass("adminUserNeedOk");
        }
        else{
            $('#passwordOK').html("&#42;").removeClass("adminUserNeedOk").addClass("adminUserNeed");
        }
        if(passwordT.length >= 6 && passwordT === passwordCT){
            $('#passwordCK').html("&#10003;").removeClass("adminUserNeed").addClass("adminUserNeedOk");
        }
        else{
            $('#passwordCK').html("&#42;").removeClass("adminUserNeedOk").addClass("adminUserNeed");    
        }

        //aktivere submit
        if(usernameT.length >= 3 && passwordT === passwordCT && passwordT.length >= 6) {
            $(inputs[2]).removeClass("buttonInactive");
        }
        else {
            $(inputs[2]).addClass("buttonInactive");
        }
    });

    //reset button
    $(".adminForm input[type='reset']").click(function () {
        if(!$(this).hasClass("buttonInactive")){
            
            resetAllInput();
        }
    });

    //submit
    $(".adminNcreateU input[type='submit']").click(function () {
        //submit må ikke have class buttonInactive
        if(!$(this).hasClass("buttonInactive")) {
            //åbner succes window og starter loading gif
            $("#adminSucces").toggle();
            $("#loader").show();

            //henter info til ny bruger
            var username = $("input[name='username']").val(),
            password = $("input[name='password']").val(),
            passwordC = $("input[name='passwordC']").val(),
            name = $("input[name='name']").val(),
            lastname = $("input[name='lastname']").val(),
            email = $("input[name='email']").val(),
            rank = $("input[name='rank']:checked").val();

            //prøver at op ret brugeren
            $.post(tecCargoLink + "php/admin/php/PHPcreateuser.php", {
            username:username,password:password,passwordC:passwordC,name:name,lastname:lastname,email:email,rank:rank
            }, function (data) {
                var status = data.substr(7);
                var msg = "";
                var img = "sad.png";

                //fejl meddelelse
                switch(status) {
                    case "1":
                        msg = "Variabler ikke udflydt. Tjek log";
                        break;
                    case "2":
                        msg = "Kodeordene skal være ens.";
                        break;
                    case "3":
                        msg = "Kodeordet skal minimum være på 6 tegn.";
                        break;
                    case "4":
                        msg = "Brugernavnet bruges af en anden.";
                        break;
                    case "5":
                        msg = "Email brugrs af en anden.";
                        break;
                    case "6":
                        msg = "Kunne ikke opret bruger. Tjek log";
                        resetAllInput();
                        break;
                    default:
                        msg = "Brugeren er nu oprettet.";
                        img = "happy.png";
                        resetAllInput();
                        break;
                }

                //viser meddelelse og skjuler loading gif
                $("#succuesTextStatus").html(msg);
                $("#succuesImgStatus").attr("src" ,"../../../images/admin/" + img);
                $("#loader").hide();
            });
        }
    });
    
    //
    //      ###### EditUser ######
    //
    
    //dropdown menu for bruger kan redigere
    $("#nowDropdownEdit ,#dropdownuserId, #dropdownuserId span").click(function () {
        $("#dropdownUserUl").addClass("showDropdownUsers");
    });

    //når man klikker på en bruger vil den hente hans oplysninger
    $(".edituserS").click(function () {
        var id = $(this).attr("id");
        var name = $(this).html();
        
        //skifter <Vælg bruger> ud med navnet på den man har valgt
        $("#nowDropdownEdit").html(name);
        
        //tillad reset og submit
        $(".removeInactive").removeClass("buttonInactive");

        //henter bruger oplysninger
        $.getJSON(tecCargoLink + "php/admin/php/PHPedituserGet.php?id=" + id.substr(11), function (json){
            if(json.status === "OK"){
                $(".adminNeditU input[name='userId']").val(json.id);
                $(".adminNeditU input[name='username']").val(json.username);
                $(".adminNeditU input[name='name']").val(json.firstname);
                $(".adminNeditU input[name='lastname']").val(json.lastname);
                $(".adminNeditU input[name='email']").val(json.email);

                if(json.rank === "Admin"){
                    $("#radioAdmin").prop('checked',true);
                }
                else{
                    $(".adminNeditU input[value='User']").prop("checked", true);
                }
            }
            else{
                alert(json.status);
            }
        });

    });
    
    //gemmer de nye ændringer
    //hvis submit ikke har class <buttonInactive>
    $(".adminNeditU input[type='submit']").click(function () {

        if(!$(".adminNeditU input[type='submit']").hasClass('buttonInactive')){

            //åbner result vindue og viser loading gif
            $("#adminSucces").show();
            $("#loader").show();

            //henter de nye bruger informationer
            var id = $(".adminNeditU input[name='userId']").val(),
                username = $(".adminNeditU input[name='username']").val(),
                password = $(".adminNeditU input[name='password']").val(),
                passwordC = $(".adminNeditU input[name='passwordC']").val(),
                firstname = $(".adminNeditU input[name='name']").val(),
                lastname = $(".adminNeditU input[name='lastname']").val(),
                email = $(".adminNeditU input[name='email']").val(),
                rank = $(".adminNeditU input[name='rank']:checked").val();

            //oploader dem til databasen
            $.post(tecCargoLink + "php/admin/php/PHPedituserSubmit.php",{id:id,username:username,
                password:password,passwordC:passwordC,firstname:firstname,
                lastname:lastname,email:email,rank:rank},function (data) {
                
                var status = data.substr(7); //giver et nummer hvis der skete en fejl
                var text;
                var img = "sad.png";
                
                
                //status beskeder
                switch(status){
                    case '1':
                        text = "Vigtige variabler ikke udflydt. Tjek log";
                        break;
                    case '2':
                        text = "Du kan ikke ændre brugerniveau på den sidste admin.";
                        break;
                    case '3':
                        text = "Brugernavnet bliver brugt af en anden.";
                        break;
                    case '4':
                        text = "Kodeordet skal være på 6 eller flere tegn.";
                        break;
                    case '5':
                        text = "Kodeordne skal være ens.";
                        break;
                    case '6':
                        text = "Kunne ikke opdatere brugen. Tjek log";
                        break;
                    default :
                        text = "Brugeren er nu opdateret.";
                        img = "happy.png";
                        
                        //ændre brugernavnet i dropdown menuen
                        $("#nowDropdownEdit").html("Vælg bruger");
                        $("#editUserId_" + id).html(username);
                        resetAllInput();
                        break;
                }

                $("#succuesTextStatus").html(text);
                $("#succuesImgStatus").attr("src" ,"../../../images/admin/" + img);

                $("#loader").hide();

            });
        }
    });
    
    
    //
    //      ###### Delete User ######
    //
    
    
    //sletter valgte bruger
    //hvis submit ikke har class <buttonInactive>
     $(".adminNdeleteU input[type='submit']").click(function (){
        
        if(!$(".adminNdeleteU input[type='submit']").hasClass('buttonInactive')){

            //åbner result vindue og viser loading gif
            $("#adminSucces").show();
            $("#loader").show();
            
            //array med brugernes database id
            var id = [];
            
            //henter valgte brugere ids og ligger dem i variablen <id>
            $("input[type='checkbox']:checked").each(function () {
                id.push($(this).val());
            });

            //prøver at slete brugerne
            $.post(tecCargoLink + "php/admin/php/PHPdeleteuser.php",{id:JSON.stringify(id)}, function (data){
               var status = data.substr(7); //giver et nummer hvis der er fejl
               var text = "";
               var img = "sad.png";
               
                //status beskeder
                switch(status){
                   case '1':
                        text = "Vigtige variabler ikke udflydt. Tjek log.";
                       break;
                   case '2':
                        text = "Der skal være minimum 1 admin tilbage.";
                       break;
                   case '3':
                        text = "Kunne ikke slette brugerne. Tjek log";
                       break;
                   default :
                        text = "Brugerne er nu slettet.";
                        img = "happy.png";
                        var countId = id.length;

                        //sletter brugerne fra listen
                        for (i = 0; i < countId; i++) {
                            $("#selectId_" +id[i]).remove();
                        }
                        break;
                }

                $("#succuesTextStatus").html(text);
                $("#succuesImgStatus").attr("src" ,"../../../images/admin/" + img);
                $("#loader").hide();
            });
        }
    });
    
    
    
    //når man klikker et sted på siden
    $(document).click(function (e) {        
        //lukker loading/status vinduet når man klikker 
        if($(e.target).is(".centerWindow, .closePictureImg, .closeSuccesButton")) {
            $(".centerWindow").hide();
        }
        
        //lukker bruger dropdown menu når man klikker på noget andet
        if($("#dropdownUserUl li").css("display") == "block" && !$("#nowDropdownEdit, #dropdownuserId, #dropdownuserId span").is(e.target)) {
            $("#dropdownUserUl").removeClass("showDropdownUsers");
        }
    });
    
    
    
    //////////////////////////////////
    //#                            #//
    //#     GoPart GodsTransport   #//
    //#                            #//
    //////////////////////////////////
    
    $(".removeGodsSelect input[type='text']").focus(function () {
        var select = $(this).attr('id');
        
        $(".removeGodsSelect").removeClass("userCrowSelect");
        $("." + select).addClass("userCrowSelect");
    });
    
    
    
    //uploader priserne til databsen
    //hvis submit ikke har class <buttonInactive>
     $(".adminNgodsPris input[type='submit']").click(function (){
        
        if(!$(".adminNgodsPris input[type='submit']").hasClass('buttonInactive')){

            //åbner result vindue og viser loading gif
            $("#adminSucces").show();
            $("#loader").show();
            
            var prices = [];
            var takst2 = [];
            var takst3 = [];
            var takst4 = [];
            var takst5 = [];
            var takst6 = [];
            var takst7 = [];
            var takst8 = [];
            var takst9 = [];
            var takst10 = [];

            //henter de nye priser
            for (var i = 0, max = 40; i < max; i++) {
                $("input[name='godsName_P_" + i +"']").each(function (){
                    prices.push($(this).val());
                });
            }

            //henter de nye takst procenter
            for (var i = 0, max = 9; i < max; i++) {
                $("input[name='godsName_T_" + i +"']").each(function (){
                    switch (i) {
                        case 0:
                            takst2.push($(this).val());
                            break;
                        case 1:
                            takst3.push($(this).val());
                            break;
                        case 2:
                            takst4.push($(this).val());
                            break;
                        case 3:
                            takst5.push($(this).val());
                            break;
                        case 4:
                            takst6.push($(this).val());
                            break;
                        case 5:
                            takst7.push($(this).val());
                            break;
                        case 6:
                            takst8.push($(this).val());
                            break;
                        case 7:
                            takst9.push($(this).val());
                            break;
                        case 8:
                            takst10.push($(this).val());
                            break;
                    }
                });
            }
            
            //prøver at opdaterer gopart priserne
            $.post(tecCargoLink + "php/admin/php/PHPgodspart.php",{
                prices:JSON.stringify(prices), takst2:JSON.stringify(takst2), takst3:JSON.stringify(takst3),
                takst4:JSON.stringify(takst4), takst5:JSON.stringify(takst5), takst6:JSON.stringify(takst6),
                takst7:JSON.stringify(takst7), takst8:JSON.stringify(takst8), takst9:JSON.stringify(takst9),
                takst10:JSON.stringify(takst10)
            }, function (data){
               var status = data.substr(7); //giver et nummer hvis der er fejl
               var text = "";
               var img = "sad.png";

                //status beskeder
                switch(status){
                   case '1':
                        text = "Vigtige variabler ikke udflydt. Tjek log.";
                       break;
                   case '2':
                        text = "Priserne kunne ikke opdateres. Tjek log";
                       break;
                   default :
                        text = "Priserne er nu opdateret.";
                        img = "happy.png";
                        break;
                }
                $("#test1234").html(data);
                $("#succuesTextStatus").html(text);
                $("#succuesImgStatus").attr("src" ,"../../../images/admin/" + img);
                $("#loader").hide();
            });
        }
    });
    
    $(".inputChangeAllow input[type='text']").keyup(function (){
       $(".removeInactive").removeClass("buttonInactive"); 
    });
    
    
    //#
    //#     Billed.php
    //#
    
    //
    //  Skift billede på menuen og goplus/gogreen priserne  
    //
    $(".pictureSettings").click(function () {
        //viser loading billede
        $("#loader2").show();
        $("#changeLoadeSize").css("height", "104px");
        $("#changePicture").show();

        //find ud af om det her billedet bliver brugt i databasen
        var billedeNavn = $(this).attr('id');

        //sætter value for hidden input så man kan skifte billedet
        $("input[name='changePicture_NewName']").val(billedeNavn);


        $.getJSON(tecCargoLink + "php/admin/php/PHPbilledeCheck.php",{billedeName:billedeNavn}, function (dataJSON) {
            //alert("Menu " +dataJSON.menu +" Pakke: " +dataJSON.pakke);

            //navne fra databasen
            var dbMenuName = ["Kurertransport", "Pakketransport", "Godstransport", "Logistics", "Montage"];
            var dbPakkeName = ["XS", "S", "M", "L", "XL", "2XL", "3XL", "Servicegebyr"];

            //tjekker om det findes i database hvis der er sæt et hak og stop loopet
            for (var i = 0, max = 5; i < max; i++) {
                if (dataJSON.menu == dbMenuName[i]) {
                    $("#cpic_" + dbMenuName[i]).prop("checked",true).attr("checked",true);
                    break;
                }
                else{
                    $(".radioMenu input[type='radio']").prop("checked",false).attr("checked",false);
                }
            }

            //tjekker om det findes i database hvis der er sæt et hak og stop loopet
            for (var i = 0, max = 8; i < max; i++) {
                if (dataJSON.pakke == dbPakkeName[i]) {
                    $("#cpic_" + dbPakkeName[i]).prop("checked",true).attr("checked",true);
                    break;
                }
                else{
                    $(".radioPakke input[type='radio']").prop("checked",false).attr("checked",false);
                }
            }


            //fjern loading billedet
            $('#loader2').hide();
            $("#changeLoadeSize").css("height", "inherit");
        });
    });


    //uploader et nyt billede til menu eller pakke priserne
    $("#changePicture input[type='submit']").click(function (){
        if (!$(this).hasClass("buttonInactive")) {

            //viser loading billede
            $("#loader2").show();
            $("#changeLoadeSize").css("height", "104px");

            //henter navnet på det nye billede
            //og hvor det skal bruges
            var billedeNavn = $("input[name='changePicture_NewName']").val();
            var menuNavn = $(".radioMenu input[type='radio']:checked").attr('id');
            var pakkeNavn = $(".radioPakke input[type='radio']:checked").attr('id');

            $.post(tecCargoLink + "php/admin/php/PHPbilledeChange.php",{billedName:billedeNavn, menuName:menuNavn, pakkeName:pakkeNavn},function(data) {
                alert(data);


                //fjern loading billedet
                $('#loader2').hide();
                $("#changeLoadeSize").css("height", "inherit");
            });
        }
    });

    //så man kan fjerne de valgte igen
    $(".removeRadioDot input[type='reset']").click(function (){
        if (!$(this).hasClass("buttonInactive")) {
            $(".adminChangePic").trigger('reset');
        }
    });


    //upload viser de billder man har valgt
    $('#pickPictureUpload').change(function ()
    {
        if($('#pickPictureUpload').val() === "")
        {
            $('#nowPictureUpload').empty();
            $('#nowPictureUpload').append("<li>Ingen billeder valgt</li>"); 

            $(".adminNbilledeUp .removeInactive").addClass('buttonInactive');
        }
        else
        {
            $('#nowPictureUpload').empty();

            var filesnames = $("#pickPictureUpload")[0].files;

            for (var i = 0, max = filesnames.length; i < max; i++) {
                $('#nowPictureUpload').append("<li>Billed: " + filesnames[i].name +"</li>");
            }

            //tillad reset og upload
            $(".adminNbilledeUp .removeInactive").removeClass('buttonInactive');
        }
    });

    //resetter upload
    $(".adminNbilledeUp input:reset").click(function (){
        if (!$(this).hasClass("buttonInactive")) {
            $(".adminNbilledeUp").trigger("reset");

            $('#nowPictureUpload').empty();
            $('#nowPictureUpload').append("<li>Ingen billeder valgt</li>");   

            $(".adminNbilledeUp .removeInactive").addClass('buttonInactive');
        }
    });


    //resetter upload
    $(".adminNbilledeUp input:submit").click(function (){
        if (!$(this).hasClass("buttonInactive")) {

            //viser loading billede
            $("#adminSucces").show();
            $("#loader1").show();

            var files = $("#pickPictureUpload")[0].files;

            var data = new FormData();
            $.each(files, function(key, value)
            {
                data.append(key, value);
            });

           $.ajax({
            url: tecCargoLink + 'php/admin/php/PHPbilledeUpload.php',
            type: 'POST',
            data: data,
            cache: false,
            dataType:'json',
            processData: false, // Don't process the files
            contentType: false, // Set content type to false as jQuery will tell the server its a query string request
            success: function(data){
                var img = "happy.png";
                //hvis der er fejl
                if (data.Error) {
                  img = "sad.png";
                }

                $("#succuesTextStatus").html(data.Text);
                $("#succuesImgStatus").attr("src" ,"../../../images/admin/" + img);

                $("#loader1").hide();
                }
            });


        }
    });


    //sletter valgte billeder
    $(".adminpictureT input:submit").click(function (){
        var billedeName = [];

        $(".adminpictureT input:checkbox:checked").each(function (){
            billedeName.push($(this).val());
        });

        $.post(tecCargoLink + "php/admin/php/PHPbilledeDelete.php",{billedeName:JSON.stringify(billedeName)}, function (data){
            //var json = JSON.parse(data);

            alert(data.Text);
        }, 'json');

    });
    
    
    //#
    //#     Beregn takst zone
    //#
    $("#beregnKiloReset").click(function (){
                        
        $("#beregn_Info").html("Vælg start post nummer");
        $("#post_1").html('');
        $("#post_2").html('');
        $("#post_3").html('');
        $("#post_kilo").val('');
        $(".beregnButton").show();
        $(".beregnKilo").hide();

    });

   $(".beregnButton button").click(function (){
        //hent post nummeret
        var postN = $(this).html();

        //hvis man ikke har valgt et start post nummer
        if ($("#post_1").html() == "") {
            $("#post_1").html(postN);
            $("#beregn_Info").html("Vælg slut post nummer");
        }
        else {
            $("#post_2").html(postN);
            $("#beregn_Info").html("Indtast antal kilo");

            //skjuler post nummerene og hvis Instast kilo
            $(".beregnButton").hide();
            $(".beregnKilo").show();
        }
   });

   $("#post_kiloSubmit").click(function (){
        //hvis at den loader
        $("#beregn_Info").html("Loader. . .");
        $(".beregnKilo").hide();

        //info til at finde resultatet
        var kilo = $("#post_kilo").val();
        var post_1 = $("#post_1").html();
        var post_2 = $("#post_2").html();

        $("#post_3").html(kilo);


       $.post(tecCargoLink + "php/admin/php/PHPberegnResult.php",{post1:post_1, post2:post_2, kilo:kilo},function (data){

            $("#beregn_Info").html("Resultat: " + data);
       });
   });
});

//bliver brugt så fleterne bliver resat igen og 
// der kommer stjerne ved fleter der skal udflydes
function resetAllInput(){
    $(".adminForm").trigger('reset');
    $(".resetUserNeed ").html("&#42;").removeClass("adminUserNeedOk").addClass("adminUserNeed");
    $("input[type='reset']").addClass("buttonInactive").attr('onclick','return false');
    $("input[type='submit']").addClass("buttonInactive").attr('onclick','return false');
}
function previousPage(){
        parent.history.back();
        return false;
}
function reloadPage(){
    if(!$(this).hasClass('buttonInactive')){
        location.reload();
    }
}