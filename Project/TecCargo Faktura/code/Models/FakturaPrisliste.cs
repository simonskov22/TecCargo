using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecCargo_Faktura.Models
{
    public class FakturaPrisliste
    {
        public static class kurerPriser
        {
            //0-3 gorush 
            //4-7 goflex
            public static double[][] minimun = { new double[] { 245, 270, 300, 335 }, new double[] { 165, 190, 215, 240 }},
                startgebyr = { new double[] { 175, 195, 220, 250 }, new double[] {  95, 115, 135, 155 }},
                kilometer = { new double[] { 7.05, 7.45, 7.85, 8.25 }, new double[] {  7.05, 7.45, 7.85, 8.25 }},
                ekstraTidforbrug = { new double[] { 6.5, 7, 7.5, 8 }, new double[] {  6.5, 7, 7.5, 8 }},
                medhjaelper = { new double[] { 200, 200, 200, 200 }, new double[] {  200, 200, 200, 200 }},
                flytte = { new double[] { 150, 150, 150, 150 }, new double[] {  150, 150, 150, 150 }},
                adr = { new double[] { 145, 165, 185, 205, 145 }, new double[] { 165, 185, 205 }},
                aftenOgNat = { new double[] { 120, 130, 140, 150 }, new double[] {  120, 130, 140, 150 }},
                weekend = { new double[] { 85, 105, 125, 145 }, new double[] {  85, 105, 125, 145 }},
                yderzone = { new double[] { 10, 10, 10, 10 }, new double[] {  10, 10, 10, 10 }},
                byttePalle = { new double[] { 0, 0, 10, 10 }, new double[] {  0, 0, 10, 10 }},
                smsService = { new double[] { 8, 8, 8, 8 }, new double[] {  8, 8, 8, 8 }},
                addresseKirrektion = { new double[] { 60, 60, 60, 60 }, new double[] {  60, 60, 60, 60 }},
                braendstof = { new double[] { 13, 13, 13, 13 }, new double[] {  13, 13, 13, 13 }},
                miljoegebyr = { new double[] { 4, 4, 4, 4 }, new double[] {  4, 4, 4, 4 }},
                Adminnistrationsgebyr = { new double[] { 35, 35, 35, 35 }, new double[] {  35, 35, 35, 35 }};

        }



        public static class pakkePriser
        {
            //0 goplus 
            //1 gogreen
            public static double[] xsPakke = { 60, 40 },
                sPakke = { 72, 48 },
                mPakke = { 84, 56 },
                lPakke = { 104, 72 },
                xlPakke = { 132, 88 },
                xxlPakke = { 156, 104 },
                xxxlPakke = { 192, 128 },
                pakkeGebyr = { 8, 4 };


            public static double[][] Prices = {
                new double[] { 60, 40 },
                new double[] { 72, 48 },
                new double[] { 84, 56 },
                new double[] { 104, 72 },
                new double[] { 132, 88 },
                new double[] { 156, 104 },
                new double[] { 192, 128 },
                new double[] { 8, 4 },
            };

        }

        public static class godsPriser
        {
            public static int[] Kilos = { 
                30,40,50,75,100,
                125,150,175,200,250,
                300,350,400,450,500,
                600,700,800,900,1000,1100,1200,1300,1400,1500,1600,1700,
                1800,1900,2000,2100,2200,2300,2400,2500,2600,2700,2800,
                2900,300
            };
            public static double[] mainPrice = {
                                     118,126,140,167.50,170,
                                     172.50,197.50,220,240,285,
                                     307,325,350,370,385,
                                     400,451,490,532,570,
                                     604,646,686,724,760,
                                     778,809,838,865,890,
                                     892,912,930,946,960,
                                     985,1009,1032,1068.50,
                                     1105
                                     };

            public static double[][] PriceProcent = { 
                new double[] {0,20,50,70,90,110,130,150,170,200},
                new double[] {0,40,60,90,110,130,150,170,190,250},
                new double[] {0,55,90,190,210,230,250,275,300,340},
                new double[] {0,60,95,210,230,250,270,290,310,350},
                new double[] {0,47,85,205,220,240,260,280,300,340},
                new double[] {0,49,84,210,230,260,280,300,320,360},
                new double[] {0,55,90,225,240,260,280,300,350,390},
                new double[] {0,60,100,247,270,290,320,340,380,420},
            };
        }


        public static class EkstraGebyr
        {
            public static bool chauffoer = false,
                flytteTilaeg = false,
                adrTilaeg = false,
                aftenNat = false,
                weekend = false,
                yederzone = false,
                byttePalle = false,
                smsService = false,
                adresseKorrektion = false,
                broAfgrif = false,
                vejAfgrif = false,
                faergeAfgrif = false,
                bemaerkning = false;

            public static int medHelper = 0,
                flyttePrEnhed = 0,
                byttePallePrPalle = 0,
                smsAdvisering = 0;

            public static double broAfgrifD = 0,
                vejAfgrifD = 0,
                faergeAfgrifD = 0;
        }
    }
}
