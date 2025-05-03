using System;
using System.Transactions;

namespace SystemSazek.Core.Sazky{


    public class Zadana_polozka
    {
        public int? id_sazka_zapas { get; set; }
        public int idZapasu;
        public int vysledek;
    }


    public class SazkaService{

        public static ServiceReturn upravSazku(Sazka puvodni, double castka, List<Zadana_polozka> zadane_polozky)
        {
            List<Polozka> polozky = new List<Polozka>();
            for ( int i = 0; i < zadane_polozky.Count(); ++i )
            {
                PolozkaVP t = new PolozkaVP{
                    id_sazka_zapas = zadane_polozky[ i ].id_sazka_zapas,
                    vsazeno_na = zadane_polozky[ i ].vysledek,
                    sazka = puvodni
                };
                t.set_id_zapas(zadane_polozky[ i ].idZapasu);

                polozky.Add(t);
            }
            

            var ( uspesne, hlaska, nove, aktualizovane, odstranene ) = puvodni.validace_aktualizace( castka, polozky );
            
            if ( uspesne )
            {  
                PolozkaDataMapper pdm = new PolozkaDataMapper( "Data source=soubor.db" );
                using (var transaction = new TransactionScope())
                {

                    SazkaDataMapper sdm = new SazkaDataMapper("Data source=soubor.db");
                    puvodni.castka = castka;
                    sdm.Update( puvodni );

                    for ( int i = 0; i < odstranene.Count; ++i )
                    {
                        pdm.Delete( odstranene[ i ] );
                    }

                    for ( int i = 0; i < aktualizovane.Count; ++i )
                    {
                        pdm.Update( aktualizovane[ i ] );
                    }
                    
                    for ( int i = 0; i < nove.Count; ++i )
                    {
                        pdm.Save( nove[ i ] );
                    }

                    transaction.Complete();
                }
                puvodni.polozky = polozky;
                
                return new ServiceReturn { Uspech = true, ChybovaHlaska="OK" };
            }
            
            return new ServiceReturn { Uspech = false, ChybovaHlaska=hlaska };
        }

        public static ServiceReturn podejSazku(Uzivatel uzivatel, double castka, List<Zadana_polozka> zadane_polozky)
        {
            Sazka sazka = new SazkaVP {
                id_sazka = null,
                datum_cas_vytvoreni = DateTime.Now,
                datum_cas_uzavreni = null,
                status = "O",
                castka = castka,
                uzivatel = uzivatel
            };
            List<Polozka> polozky = new List<Polozka>();
            for ( int i = 0; i < zadane_polozky.Count(); ++i )
            {
                PolozkaVP t = new PolozkaVP{
                    id_sazka_zapas = null,
                    vsazeno_na = zadane_polozky[ i ].vysledek,
                    sazka = sazka
                };
                t.set_id_zapas(zadane_polozky[ i ].idZapasu);

                polozky.Add(t);
            }
            sazka.polozky = polozky;

            var ( uspech, hlaska ) = sazka.validace_sazky();
            if ( !uspech )
            {
                return new ServiceReturn { Uspech = false, ChybovaHlaska=hlaska };
            }
            
            SazkaDataMapper sdm = new SazkaDataMapper("Data source=soubor.db");
            int database_id = sdm.Save(sazka);

            sazka.id_sazka = database_id;

            PolozkaDataMapper pdm = new PolozkaDataMapper("Data source=soubor.db");

            for ( int i = 0; i < sazka.polozky.Count; ++i )
            {
                pdm.Save( sazka.polozky[ i ] );
            }   
                uzivatel.sazky.Add(sazka);
                return new ServiceReturn { Uspech = true, ChybovaHlaska=hlaska };
        } 


        public static ServiceReturn ZrusSazku(Uzivatel uzivatel, int id_sazka)
        {
            Sazka sazka = null;
            for ( int i = 0; i < uzivatel.sazky.Count; ++i )
            {
                if ( uzivatel.sazky[ i ].id_sazka == id_sazka )
                {
                    sazka = uzivatel.sazky[ i ];
                }
            }
            if ( sazka == null )  return new ServiceReturn { Uspech = false, ChybovaHlaska="neznama sazka" };
            var ( uspech, hlaska ) = sazka.lze_zrusit();
            if ( !uspech )
            {
                return new ServiceReturn { Uspech = false, ChybovaHlaska=hlaska };
            }

            sazka.status = "Z";
            SazkaDataMapper sdm = new SazkaDataMapper("Data source=soubor.db");
            sdm.Update(sazka);          

                return new ServiceReturn { Uspech = true, ChybovaHlaska="OK" };
        }
            
    }
}
