using System;
using Newtonsoft.Json;
namespace SystemSazek.Core.Sazky{
    public class Sazka{
        
        public int? id_sazka { get; set; }
        public DateTime datum_cas_vytvoreni { get; set; }
        public DateTime? datum_cas_uzavreni { get; set; }
        public double castka { get; set; }
        public string status { get; set; }

        [JsonIgnore] 
        public Uzivatel uzivatel { get; set; }
        public virtual List<Polozka> polozky { get; set; }
        public ( bool, string ) lze_zrusit()
        {
            for ( int i = 0; i < polozky.Count(); ++i )
            {
                if ( this.status == "Z" )
                {
                    return ( false, "nelze zrusit, sazka je jiz zrusena" );
                }
                if ( polozky[ i ].zapas.ZacalZapas() )
                {
                    return ( false, "nelze zrusit, zapasy na ktere bylo vsazeno jiz zacaly" );
                }
            }

            return (true, "OK" );
        }

        public ( bool, string ) validace_sazky()
        {
            Console.WriteLine( "running" );
            if ( !this.uzivatel.aktivni )
            {
                return ( false, "vas ucet je deaktivovany, nemuzete podavat sazky" );                                           // uzivatel nemuze podavat sazky
            }   

            if ( this.castka < 200 || castka > 2000000 )
            {
                return ( false, "sazka musi byt v intervalu <200, 2000000>" );                                           // castka nesedi
            }
            if ( polozky.Count() == 0 )
            {
                return ( false, "musite vsadit alespon na jeden zapas" ) ;                                          // nevsazi na ani jeden ze zapasu
            }
            for ( int i = 0; i < polozky.Count(); ++i )
            {
                if ( polozky[ i ].zapas == null )
                {
                    return ( false, "zapas, na ktery chcete vsadit, neexistuje" );                                     // zapas jiz neexistuje
                }
            }

            for ( int i = 0; i < polozky.Count(); ++i )
            {
                if ( polozky[ i ].zapas.ZacalZapas() )
                {
                    return ( false, "zapas, na ktery chcete vsadit, jiz zacal." );                                     // zapas jiz neexistuje
                }
            }

            for ( int i = 0; i < polozky.Count(); ++i )
            {
                if ( !uzivatel.lze_vsadit( polozky[ i ].zapas ) ) 
                { 
                    return ( false, "na konkretni zapas nemuzete vsadit" );                                       // uzivatel nemuze vsadit na konkretni zapas
                }
            }

            for ( int i = 0; i < polozky.Count(); ++i )
            {

                for ( int j = i + 1; j < polozky.Count(); ++j )
                {
                    if ( polozky[ i ].zapas.id_zapas == polozky[ j ].zapas.id_zapas )
                    {
                        return ( false, "nelze vsadit na stejny zapas 2x" );                                   // vsazi 2x na stejny zapas
                    }
                }

            }
            for ( int i = 0; i < polozky.Count(); ++i )
            {
                if ( !polozky[ i ].validace_polozky() )
                {
                    return ( false, "musite zadat validni tip" );                               // spatne vsazeno na nejaky ze sapasu
                }
            }
            

            return ( true, "OK" );
        }
        
        public (bool, string hlaska, List<Polozka> nove, List<Polozka> aktualizovane, List<Polozka> odstranene) validace_aktualizace(double upravenaCastka, List<Polozka> novePolozky)
        {

            for ( int i = 0; i < polozky.Count(); ++i )
            {
                if ( polozky[ i ].zapas.ZacalZapas() )
                {
                    return ( false, "nelze upravit sazku, jejiz zapasy jiz zacaly", null,null,null);                                     // zapas jiz neexistuje
                }
            }

            if ( this.status != "O" )
            {
                return ( false, "Sazka neni ve stavu otevrena / podana", null, null, null );
            }

            // kontrola castky
            if ( upravenaCastka < 200 || upravenaCastka > 2000000 )
            {
                return ( false,"zadana nespravna castka", null, null, null );                                          // castka nesedi
            }


            // odstranene polozky musi byt spojeny se zapasem, ktery jeste nezacal

            List<Polozka> odstranene = this.polozky.Where(puvodni => !novePolozky.Any(nova => (nova.id_sazka_zapas ?? -1) == puvodni.id_sazka_zapas)).ToList() ?? new List<Polozka>();
            for ( int i = 0; i < odstranene.Count; ++i )
            {
                if ( odstranene[ i ].zapas.ZacalZapas() )
                {
                    return ( false, "zapas nelze odstranit, jelikoz jiz zacal", null, null, null );
                }
            }

            // nove polozky, musi byt spojeny se zapasem, ktery nezacal a uzivatel na nej muze vsadit, musi take existovat
            
            var nove = novePolozky.Where(nova => !nova.id_sazka_zapas.HasValue || nova.id_sazka_zapas.Value == -1 ).ToList() ?? new List<Polozka>();

            for ( int i = 0; i < nove.Count; ++i )
            {
                if ( nove[ i ].zapas == null ) return ( false, "Zapas, na ktery vsazite neexistuje", null, null, null );

                if ( nove[ i ].zapas.ZacalZapas() || !this.uzivatel.lze_vsadit( nove[ i ].zapas ) || !nove[ i ].validace_polozky() )
                {
                    return ( false, "nelze vsadit na zapas v jeho prubehu / dokonceni", null, null, null );
                }
            }

            // u aktualizovanych polozek, zapas nesmi byt ve stavu ze zacal..


            List<Polozka> aktualizovane = novePolozky.Where(nova => this.polozky.Any(p =>(p.id_sazka_zapas.GetValueOrDefault(-2)) == (nova.id_sazka_zapas.GetValueOrDefault(-1)) && (p.vsazeno_na != nova.vsazeno_na || p.zapas.id_zapas.GetValueOrDefault(-2) != nova.zapas.id_zapas.GetValueOrDefault(-2) ) ) ).ToList() ?? new List<Polozka>();

            for ( int i = 0; i < aktualizovane.Count; ++i )
            {
                if ( aktualizovane[ i ].zapas.ZacalZapas() || !aktualizovane[ i ].validace_polozky() )
                {
                    return ( false, "Zapas jiz zacal / zkoncil", null, null, null );
                }
            }

            return ( true, "OK", nove, aktualizovane, odstranene );
        }

        public double MoznaVyhra()
        {
            return this.castka * this.CelkovyKurz();
        }

        public double CelkovyKurz()
        {
            double kurz = 1.0;
            for ( int i = 0; i < polozky.Count; ++i )
            {
                    if ( polozky[ i ].vsazeno_na == 1 )
                        kurz *= polozky[ i ].zapas.kurz_domaci;
                    if ( polozky[ i ].vsazeno_na == -1 )
                        kurz *= polozky[ i ].zapas.kurz_hoste;
                    if ( polozky[ i ].vsazeno_na == 0 )
                        kurz *= polozky[ i ].zapas.kurz_shoda;
            }
            return kurz;
        }

        public bool jeVyhrana()
        {
            if ( this.status != "D" )
            {
                return false;
            }

            for ( int i = 0; i < polozky.Count; ++i )
            {
                int vysledek = this.polozky[ i ].zapas.skore_domaci > this.polozky[ i ].zapas.skore_hoste ? 1 : this.polozky[ i ].zapas.skore_domaci == this.polozky[ i ].zapas.skore_hoste ? 0 : -1;
                if ( this.polozky[ i ].vsazeno_na != vysledek ) return false;
            }
            return true;
        }
    }
}

