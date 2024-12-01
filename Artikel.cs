using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIS
{
	public class Artikel
	{
		public int id { get; set; }
		public string naziv { get; set; }
		public decimal cena { get; set; }
		public int zaloga { get; set; }
		public int dobaviteljId { get; set; }
		public DateTime datumZadnjeNabave { get; set; }

		public Artikel(int id, string name, decimal price, int zaloga, int dobaviteljId, DateTime datumZadnjeNabave)
		{
			this.id = id;
			this.naziv = name;
			this.cena = price;
			this.zaloga = zaloga;
			this.dobaviteljId = dobaviteljId;
			this.datumZadnjeNabave = datumZadnjeNabave;
		}

		public Artikel(int id, string name, decimal price, int zaloga, DateTime datumZadnjeNabave)
		{
			this.id = id;
			this.naziv = name;
			this.cena = price;
			this.zaloga = zaloga;
			this.datumZadnjeNabave = datumZadnjeNabave;
		}

		public Artikel()
		{
		}
	}
}
