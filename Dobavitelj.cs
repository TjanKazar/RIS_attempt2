using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RIS
{
	public class Dobavitelj
	{

		//		Pripravite podrobnejše podatke dobaviteljev(dobavitelji.xml). Vključite: id dobavitelja, naziv dobavitelja, naslov, davčna številka, kontakt, opis, ...

		//Oba XML dokumenta medsebojno povežite: vsak artikel naj beleži id dobavitelja.Dobaviteljev naj bo vsaj 5.

		public int id { get; set; }
		public string naziv {  get; set; }
		public string naslov { get; set; }
		public int davcnaSt{ get; set; }
		public string kontakt { get; set; }
		public string opis { get; set; }

		public Dobavitelj(int id, string naziv, string naslov, int davcnaSt, string kontakt, string opis)
		{
			this.id = id;
			this.naziv = naziv;
			this.naslov = naslov;
			this.davcnaSt = davcnaSt;
			this.kontakt = kontakt;
			this.opis = opis;
		}

		public Dobavitelj()
		{
		}
	}
}
