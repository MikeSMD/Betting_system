using System;

namespace SystemSazek.Core.Sazky{
    public class Zapas{
        public int? id_zapas { get; set; }
        public DateTime datum_cas_zacatku { get; set; }
        public DateTime? datum_cas_ukonceni { get; set; }
        public int? skore_domaci { get; set; }
        public int? skore_hoste { get; set; }
        public string status { get; set; }
        public double kurz_domaci { get; set; }
        public double kurz_hoste { get; set; }
        public double kurz_shoda { get; set; }
        public Tym tym_domaci { get; set; }
        public Tym tym_hoste { get; set; }
    

        public bool ZacalZapas( )
        {
            return this.datum_cas_zacatku <= DateTime.Now;
        }
    }
}



