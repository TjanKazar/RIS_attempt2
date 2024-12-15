using RIS;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml;

public class XmlValidator
{
	private XmlSchemaSet schemaSet;
	private List<string> validationErrors;

	public XmlValidator(string schemaPath)
	{
		schemaSet = new XmlSchemaSet();
		schemaSet.Add(null, schemaPath);
		validationErrors = new List<string>();
	}

	public bool ValidateXmlDocument(string xmlFilePath, string documentType)
	{
		validationErrors.Clear();

		try
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.ValidationType = ValidationType.Schema;
			settings.Schemas = schemaSet;
			settings.ValidationEventHandler += ValidationEventHandler;

			using (XmlReader reader = XmlReader.Create(xmlFilePath, settings))
			{
				while (reader.Read()) { } 
			}

			if (validationErrors.Count > 0)
			{
				Console.WriteLine($"Validation errors in {documentType}:");
				foreach (var error in validationErrors)
				{
					Console.WriteLine(error);
				}
				return false;
			}

			return true;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error validating {documentType}: {ex.Message}");
			return false;
		}
	}

	private void ValidationEventHandler(object sender, ValidationEventArgs e)
	{
		if (e.Severity == XmlSeverityType.Error)
		{
			validationErrors.Add($"Error: {e.Message}");
		}
		else if (e.Severity == XmlSeverityType.Warning)
		{
			validationErrors.Add($"Warning: {e.Message}");
		}
	}

	public bool AddNewArtikel(Artikel newArtikel, string artikliXmlPath)
	{
		if (!ValidateXmlDocument(artikliXmlPath, "Artikli XML"))
		{
			Console.WriteLine("Cannot add new artikel due to existing XML validation errors.");
			return false;
		}

		try
		{
			XDocument doc = XDocument.Load(artikliXmlPath);

			XElement newArtikelElement = new XElement("Artikel",
				new XElement("id", newArtikel.id),
				new XElement("naziv", newArtikel.naziv),
				new XElement("cena", newArtikel.cena),
				new XElement("zaloga", newArtikel.zaloga),
				new XElement("dobaviteljId", newArtikel.dobaviteljId),
				new XElement("datumZadnjeNabave", newArtikel.datumZadnjeNabave)
			);

			doc.Root.Add(newArtikelElement);

			string tempXmlPath = Path.GetTempFileName();
			doc.Save(tempXmlPath);

			if (ValidateXmlDocument(tempXmlPath, "Updated Artikli XML"))
			{
				doc.Save(artikliXmlPath);
				File.Delete(tempXmlPath);
				Console.WriteLine("New artikel added successfully.");
				return true;
			}
			else
			{
				File.Delete(tempXmlPath);
				Console.WriteLine("New artikel failed validation.");
				return false;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error adding new artikel: {ex.Message}");
			return false;
		}
	}

	public bool AddNewDobavitelj(Dobavitelj newDobavitelj, string dobaviteljiXmlPath)
	{
		if (!ValidateXmlDocument(dobaviteljiXmlPath, "Dobavitelji XML"))
		{
			Console.WriteLine("Cannot add new dobavitelj due to existing XML validation errors.");
			return false;
		}

		try
		{
			XDocument doc = XDocument.Load(dobaviteljiXmlPath);

			XElement newDobaviteljElement = new ("Dobavitelj",
				new XElement("id", newDobavitelj.id),
				new XElement("naziv", newDobavitelj.naziv),
				new XElement("naslov", newDobavitelj.naslov),
				new XElement("davcnaSt", newDobavitelj.davcnaSt),
				new XElement("kontakt", newDobavitelj.kontakt),
				new XElement("opis", newDobavitelj.opis ?? "")
			);

			doc.Root.Add(newDobaviteljElement);

			string tempXmlPath = Path.GetTempFileName();
			doc.Save(tempXmlPath);

			if (ValidateXmlDocument(tempXmlPath, "Updated Dobavitelji XML"))
			{
				doc.Save(dobaviteljiXmlPath);
				File.Delete(tempXmlPath);
				Console.WriteLine("New dobavitelj added successfully.");
				return true;
			}
			else
			{
				File.Delete(tempXmlPath);
				Console.WriteLine("New dobavitelj failed validation.");
				return false;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error adding new dobavitelj: {ex.Message}");
			return false;
		}
	}
}