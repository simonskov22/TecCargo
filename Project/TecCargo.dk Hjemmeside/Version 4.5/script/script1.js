/*  jQuery ready function. Specify a function to execute when the DOM is fully loaded.  */
$(document).ready(
  /* This is the function that will get executed after the DOM is fully loaded */
  function () {
      
        //menu gør så dropdown menu bliver skjul når man klikker på et link
	$('.mega-menu a').click(function ()
        {
            var sidelink = '.' + $(this).attr('id');
            
            if(sidelink.indexOf('.page') !== -1)
            {
                //skjuler alle siderne
                $('.side-link').each(function ()
                {
                    $(this).hide();
                });
                
                //viser siden
                $(sidelink).show();
                $('.white .hide').hide();
            }
        });
        
        
        //forside logo
        //når man har musen linket f.eks GoRush så vil der kommer en kasse rundt om
        $('.logoLink p').mouseover(function ()
        {
            var id = $(this).attr('id');
            if(id === 'logoLink-5')
            {
                $(this).addClass('greenCargoBorder');
            }
            else
            {
                $(this).addClass('redCargoBorder');
            }
            $('.' + id).show();
        });
        $('.logoLink p').mouseout(function ()
        {
            var id = $(this).attr('id');
            if(id === 'logoLink-5')
            {
                $(this).removeClass('greenCargoBorder');
            }
            else
            {
                $(this).removeClass('redCargoBorder');
            }
            $('.' + id).hide();
        });
        $('.logoLink a').click(function ()
        {
            var side = $(this).attr('id');

            $('.side-link').each(function ()
            {
                $(this).hide();
            });
            $('.' + side).show();
        });
        
        //pris listen på gopart og gofull
        //gør så linjen man har klikket på bliver makeret
        $('.prislist table tr').click(function ()
        {
            $('.prislist table tr').each(function ()
            {
                $(this).removeClass('prisSelect');
            });
            $(this).addClass('prisSelect');
        });
        //gopart prisliste
        //gør så man kan skifte side
        $('.gopartButton').click(function ()
        {
            $('.gopartButton').removeClass('gopartButtonSelect');
            
            $('.gopart-link').each(function ()
            {
                $(this).hide();
            });
            
            $(this).addClass('gopartButtonSelect');
            
            var sidenavn = '.' + $(this).attr('id');
            
            switch (sidenavn)
            {
                case '.gopart1':
                    $('#pricesInfoGods').html('Takst Zoner');
                    $('#pricesInfoGods').removeClass('pricesInfoGods2').removeClass('pricesInfoGods3').addClass('pricesInfoGods1');
                    break;
                case '.gopart2':
                    $('#pricesInfoGods').html('Takstzoner prisliste<br>Danmark excl moms');
                    $('#pricesInfoGods').removeClass('pricesInfoGods1').removeClass('pricesInfoGods3').addClass('pricesInfoGods2');
                    break;
                case '.gopart3':
                    $('#pricesInfoGods').html('Godstransport');
                    $('#pricesInfoGods').removeClass('pricesInfoGods1').removeClass('pricesInfoGods2').addClass('pricesInfoGods3');
                    break;
            }
            
            $(sidenavn).slideDown();
        });
        
        /*
        $('.showprices a').click(function ()
        {
            var sideShowPrices = '.' + $(this).attr('id');
            
            $(sideShowPrices).fadeIn();
        });
        $('.lukWindow').click(function ()
        {
            $('.centerWindow').each(function ()
            {
                $(this).fadeOut();
            });
            
            $('#showflexprices_head').show();
            $('#showflexprices_view').hide();
            
            $('#showrushprices_head').show();
            $('#showrushprices_view').hide();
            
            $('.backWindow').hide();
            $('.lukWindow').width(500);
        });
       
       
        
        
        
        //prisliste for kurertransport
        
        //rush
        $('.showrushprices .kurerBil').click(function ()
        {
            //hvad prisliste der skal vise
            var kurerview = $(this).attr('id');
            kurerview = kurerview.substring(15);
            
            $.post('../php/sider/pris/kurertransport/kurerPostTransport.php',{mode:kurerview,type:"'rush'"}, function (rushPrice)
            {
                $('#showrushprices_head').hide();
                $('#showrushprices_view').html(rushPrice);
                $('#showrushprices_view').show();
                $('.lukWindow').width(250);
                $('.backWindow').show();
            });
        });
        
        //tilbage
        $('.showrushprices .backWindow').click(function ()
        {
            $('#showrushprices_view').hide();
            $('#showrushprices_head').show();
            
            $('.backWindow').hide();
            $('.lukWindow').width(500);
        });
        
        //flex
        $('.showflexprices .kurerBil').click(function ()
        {
            //hvad prisliste der skal vise
            var kurerview = $(this).attr('id');
            kurerview = kurerview.substring(15);
            if(kurerview === "1")
            {
                kurerview = "5";
            }
            else if(kurerview === "2")
            {
                kurerview = "6";
            }
            else if(kurerview === "3")
            {
                kurerview = "7";
            }
            else 
            {
                kurerview = "8";
            }
            $.post('../php/sider/pris/kurertransport/kurerPostTransport.php',{mode:kurerview,type:"'flex'"}, function (rushPrice)
            {
                $('#showflexprices_head').hide();
                $('#showflexprices_view').html(rushPrice);
                $('#showflexprices_view').show();
                $('.lukWindow').width(250);
                $('.backWindow').show();
            });
        });
        
        //tilbage
        $('.showflexprices .backWindow').click(function ()
        {
            $('#showflexprices_view').hide();
            $('#showflexprices_head').show();
            
            $('.backWindow').hide();
            $('.lukWindow').width(500);
        });
        */
  }
);