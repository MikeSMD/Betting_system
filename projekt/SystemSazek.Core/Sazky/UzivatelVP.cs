using System;

namespace SystemSazek.Core.Sazky{
    
    public class UzivatelVP : Uzivatel{

        private bool _nacteneSazky = false;

        private List<Sazka> _sazky = new List<Sazka>();

        public override List<Sazka> sazky
       { 
            get
            {
                if ( !_nacteneSazky )
                {
                    SazkaDataMapper sdm = new SazkaDataMapper("Data source=soubor.db");

                    List<Sazka> sazkyList = sdm.GetSazkyByUzivatel(this);
                    _nacteneSazky = true;
                    
                    _sazky = sazkyList;
                }
                
                return _sazky;
            }
            set {}
        }
    }

}
