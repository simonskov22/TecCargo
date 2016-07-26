/*  jQuery ready function. Specify a function to execute when the DOM is fully loaded.  */
$(document).ready(
    /* This is the function that will get executed after the DOM is fully loaded */
    function () 
    {
        //når man skriver noget i et af felterne til at opret bruger
        $('.user-form input').keyup(function ()
        {
            //variable med text felterne
            var userformCreate = ["input[name='name']", "input[name='lastname']", "input[name='username']", "input[name='password']"];
            
            //variable til krav er rigtigt
            var createKrav = ["#check-name", "#check-lastname", "#check-username", "#check-password"];
            
            for(var i = 0;i < 4; i++)
            {
                if($(userformCreate[i]).val() !== "")
                {
                    $(createKrav[i]).attr('src', '../images/icons/001_06.ico');
                }
                else
                {
                    $(createKrav[i]).attr('src', '../images/icons/001_05.ico');
                }
            }
        });
        
        //tjekker om det er et unik brugernavn
        $('input[name="username"]').keyup(function ()
        {
            //gemmer det man har skrevet så man kan tjek om det bruger brugt
            var username = $(this).val();
            $.post('../php/admin/php/users/usernameUnikt.php' ,{bruger:username} ,function (usernameOk)
            {
                if(usernameOk === "false" || username === "")
                {
                    $('#check-uni-username').attr('src', '../images/icons/001_06.ico');
                }
                else
                {
                    $('#check-uni-username').attr('src', '../images/icons/001_05.ico');
                }
            });
        });
        
        
        
        
        
        //delete
        $('#user-table-box-save tr').click(function ()
        {
            if($(this).hasClass('selectet'))
            {
                $(this).removeClass('selectet');
            }
            else
            {
                $(this).addClass('selectet');
            }
        });
        
        
        $('#user-table-box-delete tr').click(function ()
        {
            if($(this).hasClass('selectet'))
            {
                $(this).removeClass('selectet');
            }
            else
            {
                $(this).addClass('selectet');
            }
        });
        
        
        $('#delete-button-toDelete').click(function ()
        {
            $('#user-table-box-save .selectet').each(function ()
            {
                var id = $(this).attr('id');
                $(this).appendTo('#user-table-box-delete').removeClass('selectet');
                $('#user-table-box-save input[value=' + id +']').appendTo('#user-table-box-delete');
            });
        });
        
        $('#delete-button-toSave').click(function ()
        {
            $('#user-table-box-delete .selectet').each(function ()
            {
                var id = $(this).attr('id');
                $(this).appendTo('#user-table-box-save').removeClass('selectet');
                $('#user-table-box-delete input[value=' + id +']').appendTo('#user-table-box-save');
            });
        });
        
        $('#delete-button-reset').click(function ()
        {
            $('#user-table-box-delete tr').each(function ()
            {
                var id = $(this).attr('id');
                $(this).appendTo('#user-table-box-save').removeClass('selectet');
                $('#user-table-box-delete input[value=' + id +']').appendTo('#user-table-box-save');
            });
        });
        
        
        $('#beregn').click(function ()
        {
            var postStar = $('#startPost').val();
            var postSlut = $('#slutPost').val();
            var kg = $('#kilo').val();
            
            $.post('//teccargo.dk/testside/php/admin/php/price/beregnPrisPHP.php', {startPost:postStar,slutPost:postSlut,kilo:kg},function (visPris)
            {
                $('#showPrice').html(visPris);
            });
        });
        
       
        
        $("input[id='back']").click(function ()
        {
            var site = $("input[name='site']").val();

            if(site > 1)
            {
                site--;
                $("input[name='site']").val(site);
                $("form[name='viewpic'").submit();
            }
        });
        $("input[id='next']").click(function ()
        {
            var site = $("input[name='site']").val();

            if(site >= 1 && site < $('#aSite').val())
            {
                site++;
                $("input[name='site']").val(site);
                $("form[name='viewpic'").submit();
            }
        });
        $('#uploaded-file').change(function ()
        {
            if($('#uploaded-file').val() === "")
            {
                $('#uploadName').text('Ingen fil valgt');
            }
            else
            {
                $('#uploadName').text('Fil: ' + $('#uploaded-file').val());
            }
        });
        
        $('#statusDiv button').click(function ()
        {
            $('#statusDiv').hide();
        });
        
        $('#user-table-box tr').click(function ()
        {
            if($(this).hasClass('selectet'))
            {
                $(this).removeClass('selectet');
                $('#user-form-edit input[type="text"],#user-form-edit input[type="password"]').val("");
            }
            else
            {
                var editUsername = $(this).attr('id');
                $('#user-table-box tr').each(function ()
                {
                    $(this).removeClass('selectet');
                });

                $.post('//teccargo.dk/testside/php/admin/php/users/editUserForm.php', {user:editUsername}, function (userForm)
                {
                    $('#editFieldset').html(userForm);
                });
                $(this).addClass('selectet');
            }
        });
    });

