using System;

namespace SystemSazek.Core.Sazky{
    
    public class PolozkaVP : Polozka{

        private bool _nactenyZapas = false;

        private int id_zapas = -1;
        private Zapas _zapas;

        public void set_id_zapas(int id_zapas){
            this.id_zapas = id_zapas;
        }

        public override Zapas zapas
       { 
            get
            {
                if ( !_nactenyZapas )
                {
                    ZapasDataMapper zdm = new ZapasDataMapper("Data source=soubor.db");
                    this._zapas = zdm.GetZapasById(this.id_zapas);
                    _nactenyZapas = true;
                }
                
                return _zapas;
            }
            set {}
        }
    }

}
