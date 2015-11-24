using System;
using System.Collections.Generic;

namespace PttLib.Helpers
{
    public class Dictionnaries
    {
        public static Dictionary<int, string> MealTypes
        {
            get
            {
                var result = new Dictionary<int, string>
                                 {
                                     {0, "All"},
                                     {1, "RO"},
                                     {2, "BB"},
                                     {3, "HB"},
                                     {4, "FB"},
                                     {5, "AI"},
                                     {6, "UAI"},
                                     {7, "Other"}
                                 };
                return result;
            }
        }
        public static Dictionary<int, string> HotelCategories
        {
            get
            {
                var result = new Dictionary<int, string>
                                 {
                                     {0, "All"},
                                     {1, "2*"},
                                     {2, "2*+"},
                                     {3, "3*"},
                                     {4, "3*+"},
                                     {5, "4*"},
                                     {6, "4*+"},
                                     {7, "5*"},
                                     {8,"HV1"},
                                     {9,"HV2"},
                                     {10,"SC"}
                                 };
                return result;
            }
        }

        public static Dictionary<int, string> Regions
        {
            get
            {
                var result = new Dictionary<int, string>
                                 {
                                     {0, "CHELYABINSK"},//Челябинск
                                     {1, "EKATERINBURG"},//Екатеринбург
                                     {2, "IRKUTSK"},//Иркутск
                                     {3, "KAZAN"},//Казань
                                     {4, "KEMEROVO"},//Кемерово
                                     {5, "KHABAROVSK"},//Хабаровск
                                     {6, "KRASNODAR"},//Краснодар
                                     {7, "KRASNOYARSK"},//Красноярск
                                     {8, "MOSCOW"},//Москва
                                     {9, "NOVOSIBIRSK"},//Новосибирск
                                     {10, "OMSK"},//Омск
                                     {11, "PERM"},//Пермь
                                     {12, "ROSTOV NA DONU"},//Ростов-на-Дону
                                     {13, "SAMARA"},//Самара
                                     {14, "SURGUT"},//Сургут
                                     {15, "TYUMEN"},//Тюмень
                                     {16, "UFA"},//Уфа
                                     {17, "VALADIVOSTOK"},//Владивосто́к
                                     {18, "SANKT PETERSBURG"},//Санкт-Петербург
                                     {19, "YUZHNO SAKHALINSK"},
                                     {20, "VOLGOGRAD"},//Волгоград
                                     {21,"BLAGOVESCHENSK"}, //Благовещенск
                                     {22, "ARHANGELSK"},//Архангельск
                                     {23, "NIZHNI NOVGOROD"},//Нижний Новгород
                                     {24, "MINERALNIE VODY"},//Минеральные Воды
                                     {25, "ORENBURG"},//Оренбу́рг
                                     {26, "BELGOROD"},//Бе́лгород
                                     {27, "BARNAUL"},//Барнаул
                                     {28, "NOVOKUZNETSK"},//Новокузнецк
                                     {29, "TOMSK"},//Томск
                                     {30, "KALININGRAD"}


                                     //Arhangelsk, Nizhni Novgorod, Minarelnie vody , Orenburg , Belgorod.
                                 };

                return result;
            }
        }

        //item1: hatirlamiyorum (default olabilir?)
        //item2: ulke adi
        //item3: desteklenen origin sehirler
        //item4: desteklenen operatorler
        //item5: desteklenen destinasyon sehirler
        public static List<Tuple<int, string, IList<int>, IList<string>, IList<string>, string>> Domains
        {
            get
            {
                return new List<Tuple<int, string, IList<int>, IList<string>, IList<string>, string>>()
                    {
                        new Tuple<int, string, IList<int>, IList<string>,IList<string>,string>(0,"Turkiye", 
                            new List<int>(){0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,23,25}, 
                            new List<string>(){"TEZ","BIBLO","CORAL","ANEX_","SUNMAR"},
                            new List<string>(){"Antalya","Dalaman","Bodrum","Izmir"},"USD"),
                        new Tuple<int, string, IList<int>, IList<string>,IList<string>,string>(1,"Thailand", 
                            new List<int>(){0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,27,28,29}, 
                            new List<string>(){"TEZ","CORAL","ANEX","BIBLO","POLAR","TROYKA","SUNMAR"},
                            new List<string>(){"Phuket","Pattaya"},"USD"),
                        new Tuple<int, string, IList<int>, IList<string>,IList<string>,string>(0,"BAE", 
                            new List<int>(){0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26}, 
                            new List<string>(){"TEZ","BIBLO","CORAL","NATALIE"},
                            new List<string>(){"Dubai"},"USD"),
                        new Tuple<int, string, IList<int>, IList<string>,IList<string>,string>(0,"VIETNAM", 
                            new List<int>(){0,1,2,3,4,5,7,8,9,10,12,13,14,15,16,17,18,23,24,27,28,29,30}, 
                            new List<string>(){"ANEX","CORAL"},//,"SPACE","VERSA"
                            new List<string>(){"Vietnam"},"USD"),
                       new Tuple<int, string, IList<int>, IList<string>,IList<string>,string>(0,"YUNANISTAN", 
                            new List<int>(){0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26}, 
                            new List<string>(){"TEZ","BIBLO","CORAL","ANEX_","NATALIE","LABIRINTH"},
                            new List<string>(){"Rhodes","Heraklion"},"EUR"),
                            
                       new Tuple<int, string, IList<int>, IList<string>,IList<string>,string>(0,"ISPANYA", 
                            new List<int>(){0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26}, 
                            new List<string>(){"TEZ","BIBLO","CORAL","ANEX","NATALIE","TUI","LABIRINTH","SUNMAR"},
                            new List<string>(){"Canaria","Tenerife","Barcelona","Mallorca"},"EUR"),
                            /*
                       new Tuple<int, string, IList<int>, IList<string>,IList<string>,string>(0,"ANDORRA", 
                            new List<int>(){0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26}, 
                            new List<string>(){"TEZ","CORAL","ANEX_","NATALIE"},//"TUI"
                            new List<string>(){"Andorra"},"EUR"),*/
                        new Tuple<int, string, IList<int>, IList<string>,IList<string>,string>(0,"Italy", 
                            new List<int>(){0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,23,25}, 
                            new List<string>(){"TEZ","TUI","NATALIE"},
                            new List<string>(){"Olbia","Cagliari","Calabria","Rimini"},"EUR"),

                        new Tuple<int, string, IList<int>, IList<string>,IList<string>,string>(0,"Bulgaria", 
                            new List<int>(){0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,23,25}, 
                            new List<string>(){"TEZ","CORAL","TUI","BIBLO"},
                            new List<string>(){"Burgas","Varna"},"EUR"),
                            
                        new Tuple<int, string, IList<int>, IList<string>,IList<string>,string>(0,"Tunisia", 
                            new List<int>(){0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,23,25}, 
                             new List<string>(){"INTOURIST","CORAL","BIBLO"},//"TUI"
                            new List<string>(){"Monastir","Hammamet","Sousse"},"USD"),



                    };
            }
        }
    }
}
