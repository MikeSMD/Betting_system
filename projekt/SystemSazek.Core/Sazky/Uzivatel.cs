using System;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;
using System.IO;
namespace SystemSazek.Core.Sazky{
    public class Uzivatel{

        public int? id_uzivatele { get; set; }
        public string jmeno { get; set; }
        public string? prostredni_jmeno { get; set; }
        public string prijmeni { get; set; }
        public string email { get; set; }
        public string? telefon { get; set; }
        public DateTime datum_narozeni { get; set; }
        public string heslo { get; set; }
        public string sul { get; set; }
        public string stat { get; set; }
        public string mesto { get; set; }
        public string ulice { get; set; }
        public string psc { get; set; }
        public bool aktivni { get; set; }

        public virtual List<Sazka> sazky { get; set; } = new List<Sazka>();

        const int minimalni_delka_hesla = 8;

        public bool validuj_heslo()
        {
            return ( this.heslo.Length >= minimalni_delka_hesla );
        }

        public void zahashuj_heslo()
        {
            var (hashedPassword, salt) = PasswordHasher.ZahashujHeslo(this.heslo);
            this.heslo = hashedPassword;
            this.sul = salt;
        }

        public bool validuj_email(){

            string regex_email_validator = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            return ( Regex.IsMatch(this.email, regex_email_validator, RegexOptions.IgnoreCase) );

        }

        public bool validuj_datum_narozeni()
        {

            return ( datum_narozeni < DateTime.Now );

        }

        public bool validuj_prihlaseni(string email, string heslo)
        {
            if ( !this.aktivni )
            {
                return false;
            }

            if ( this.email != email )
            {
                return false;
            }

            return PasswordHasher.OvereniHesla(heslo, this.sul, this.heslo);
        }

        public bool lze_vsadit( Zapas zapas )
        {

            int id_zapas;

            if ( zapas.id_zapas.HasValue ){
                id_zapas = zapas.id_zapas.Value;
            }
            else return false;

            for ( int i = 0; i < sazky.Count(); ++i )
            {
                if ( sazky[ i ].status == "Z" || sazky[ i ].status == "V" || sazky[ i ].status == "P" ) continue;

                for ( int j = 0; j < sazky[ i ].polozky.Count(); ++j )
                {
                    if ( sazky[ i ].polozky[ j ].zapas.id_zapas == id_zapas )
                    {
                        return false;
                    }
                }
            
            }

            return true;
        }

        public bool lze_deaktivovat(){
            if ( ! this.aktivni )
            {
                return false;
            }
            for ( int i = 0; i < sazky.Count(); ++i )
            {
                if ( sazky[ i ].status != "D" && sazky[ i ].status!="Z" )
                {
                    var ( uspech, hlaska ) = sazky[ i ].lze_zrusit();
                    if ( !uspech )
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public double CelkovaVsazenaCastkaUzivatele()
        {
            double celkem = 0.0;
            for ( int i = 0; i < sazky.Count; ++i )
            {
                if ( sazky[ i ].status == "Z" ) continue;
                celkem += sazky[ i ].castka;
            }

            return celkem;
        }

        public double CelkovaVyhranaCastkaUzivatele()
        {
            double celkem = 0.0;
            for ( int i = 0; i < sazky.Count; ++i )
            {
                if ( sazky[ i ].jeVyhrana() )
                {
                    celkem += sazky[ i ].MoznaVyhra();
                }
            }

            return celkem;

        }

        public double ProcentualniUspesnost()
        {
            int vyhry = 0;
            int prohry = 0;

            for ( int i = 0; i < sazky.Count; ++i )
            {

                if ( sazky[ i ].status == "D" )
                {
                    if ( sazky[ i ].jeVyhrana() ) vyhry += 1;
                    else prohry += 1;
                }
            }
            if ( vyhry + prohry == 0 ) return 0;
            return ( 100 * vyhry ) / ( vyhry + prohry );
        }

        public bool SerializujSazkyUzivatele()
        {
            string filePath = "sazky.json";
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                                          Formatting = Formatting.Indented
                };

                string jsonString = JsonConvert.SerializeObject(sazky, settings);

                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(jsonString);
                }
            }
            return true;
        }


    }

}
