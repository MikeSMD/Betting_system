using System;

namespace SystemSazek.Core.Sazky{
    
    public class SazkaVP : Sazka{

        private bool _nactenePolozky = false;

        private List<Polozka> _polozky;

        public override List<Polozka> polozky
       { 
            get
            {
                if ( !_nactenePolozky )
                {
                    PolozkaDataMapper pdm = new PolozkaDataMapper("Data source=soubor.db");
                    List<Polozka> polozkyList = pdm.GetPolozkyBySazka(this);
                    _nactenePolozky = true;
                    
                    _polozky = polozkyList;
                }
                
                return _polozky;
            }
            set 
            {
                _polozky = value;
                if ( _polozky != null ) _nactenePolozky = true;
            }
        }
    }

}
