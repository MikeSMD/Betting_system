using System;

namespace SystemSazek.Core.Sazky{

        public class UzivatelService{
        
            public static ServiceReturn RegistraceUzivatele(string jmeno, string? prostredni_jmeno, string prijmeni,string?telefon, string email, string heslo, DateTime datum_narozeni, string stat, string mesto, string ulice, string psc){
                
                if (string.IsNullOrEmpty(jmeno) || string.IsNullOrEmpty(prijmeni) || string.IsNullOrEmpty(stat) || string.IsNullOrEmpty(mesto) || string.IsNullOrEmpty(ulice) || string.IsNullOrEmpty(psc) )
                {
                    return new ServiceReturn{ Uspech = false, ChybovaHlaska = "Musite vyplnit vsechny povinne polozky" };
                } 


                Uzivatel uzivatel = new UzivatelVP
                {
                    id_uzivatele = null,
                    jmeno = jmeno,
                    prostredni_jmeno = prostredni_jmeno,
                    prijmeni = prijmeni,
                    telefon = telefon,
                    email = email,
                    datum_narozeni = datum_narozeni,
                    heslo = heslo,
                    sul = "",
                    mesto = mesto,
                    ulice = ulice,
                    psc = psc,
                    stat = stat,
                    aktivni = true
                };
                
                if ( !uzivatel.validuj_email() )
                {
                    return new ServiceReturn{ Uspech = false, ChybovaHlaska = "Zadan nevalidni email" };
                }

                if ( !uzivatel.validuj_heslo() )
                {
                    return new ServiceReturn{ Uspech = false, ChybovaHlaska = "Prilis kratke heslo"};
                }
                
                if ( !uzivatel.validuj_datum_narozeni() )
                {
                    return new ServiceReturn{ Uspech = false, ChybovaHlaska = "Zadano spatne datum narozeni" };
                    
                }


                UzivatelDataMapper udm = new UzivatelDataMapper("Data source=soubor.db");

                if ( udm.existuje_email(uzivatel.email) )
                {
                    return new ServiceReturn{ Uspech = false, ChybovaHlaska = "Email je jiz pouzit jinym uctem" };
                }

                uzivatel.zahashuj_heslo();
                
                udm.Save(uzivatel);

                return new ServiceReturn{ Uspech = true, ChybovaHlaska = "OK" };
                
        }

            public static Uzivatel PrihlaseniUzivatele(string zadany_email, string zadane_heslo)
            {
                UzivatelDataMapper udm = new UzivatelDataMapper("Data source=soubor.db");

                Uzivatel uzivatel = udm.GetUzivatelByEmail(zadany_email);

                if ( uzivatel == null )
                {
                    return null;
                }

                if ( !uzivatel.validuj_prihlaseni(zadany_email, zadane_heslo) )
                {
                    return null;
                }

                return uzivatel;

            }

            public static ServiceReturn DeaktivaceUzivatele(Uzivatel uzivatel){
                if ( !uzivatel.lze_deaktivovat() )
                {
                    return new ServiceReturn{ Uspech = false, ChybovaHlaska = "ucet nelze deaktivovat, kontaktujte spravce" };
                }

                uzivatel.aktivni = false;
                // jeste musim zrusit vsechny sazky
                UzivatelDataMapper udm = new UzivatelDataMapper("Data source=soubor.db");

               udm.Update(uzivatel); 
                return new ServiceReturn{ Uspech = true, ChybovaHlaska = "OK" };
            }

            public static bool SerializujSazky( Uzivatel uzivatel )
            {
                uzivatel.SerializujSazkyUzivatele();
                return true;
            }

    }
}
