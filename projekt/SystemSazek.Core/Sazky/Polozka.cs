using System;

namespace SystemSazek.Core.Sazky{
    public class Polozka{
        
        public int? id_sazka_zapas { get; set; }
        public int vsazeno_na { get; set; }
        public virtual Zapas zapas { get; set; }
        public Sazka sazka { get; set; }
    
        public bool validace_polozky()
        {
            if ( vsazeno_na < -1 || vsazeno_na > 1 || vsazeno_na == null || zapas == null )
            {
                return false;
            }

            return true;
        }

    }

}
