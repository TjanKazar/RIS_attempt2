using RIS;
using System;
using System.Net.WebSockets;
using System.Reflection;
using System.Xml.Serialization;

static List<T> generateDummyData<T>(int count) where T : new()
{
	List<T> dummyData = new List<T>();
	Random random = new Random();

	for (int i = 1; i <= count; i++)
	{

		T instance = new T();

		foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
		{
		if (typeof(T) == typeof(Artikel))
		{
			if (prop.Name == "naziv")
				prop.SetValue(instance, $"Naziv{i}");
			else if (prop.Name == "id")
				prop.SetValue(instance, i);
		}

			if (!prop.CanWrite) continue;

			if (typeof(T) == typeof(Artikel) && prop.Name == "dobaviteljId")
			{
				List<int> list = getDobaviteljId();
				int steviloIDjev = list.Count;
				int randomIndex = random.Next(0, steviloIDjev);
				prop.SetValue(instance, list[randomIndex]);
			}
			else if (prop.PropertyType == typeof(int))
			{
				prop.SetValue(instance, random.Next(10000, 999999));
			}
			else if (prop.PropertyType == typeof(string))
			{
				prop.SetValue(instance, $"{prop.Name}{i}");
				if (prop.Name.ToLower() == "kontakt")
					prop.SetValue(instance, $"{prop.Name}{i}.nekaj@gmail.com");
			}
			else if (prop.PropertyType == typeof(double))
			{
				prop.SetValue(instance, Math.Round(random.NextDouble() * 100, 2));
			}
			else if (prop.PropertyType == typeof(decimal))
			{
				prop.SetValue(instance, Math.Round((decimal)random.NextDouble() * 100, 2));
			}
			else if (prop.PropertyType == typeof(DateTime))
			{
				prop.SetValue(instance, new DateTime(2000 + i, random.Next(1, 12), random.Next(1, 28)));
			}
			else if (prop.PropertyType == typeof(DateOnly))
			{
				prop.SetValue(instance, new DateOnly(2000 + i, random.Next(1, 12), random.Next(1, 28)));
			}
			else if (prop.PropertyType == typeof(bool))
			{
				prop.SetValue(instance, random.Next(0, 2) == 1);
			}
		}
		dummyData.Add(instance);

	}
	return dummyData;
}

static void SerializeToXml<T>(List<T> list, string RelativePath)
{
	string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\" + RelativePath));
	XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
	using (FileStream fs = new FileStream(path, FileMode.Create))
	{
		serializer.Serialize(fs, list);
	}
	Console.WriteLine($"List serialized to {RelativePath}");
}
static T DeserializeFromXml<T>(string filePath)
{
	XmlSerializer serializer = new XmlSerializer(typeof(T));
	using (FileStream fs = new FileStream(filePath, FileMode.Open))
	{
		return (T)serializer.Deserialize(fs);
	}
}
static List<int> getDobaviteljId()
{
	string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\" + "dobavitelji.xml"));
	List<int> list = new List<int>();
	List<Dobavitelj> dobavitelji = DeserializeFromXml<List<Dobavitelj>>(path);
	foreach (Dobavitelj d in dobavitelji)
		list.Add(d.id);
	return list;
}
static void cwList<T>(List<T> list)
{
	Console.WriteLine($"List of type: {typeof(T)}");
	Console.WriteLine("-----------------------------------");

	foreach (var item in list)
	{
		foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
		{
			var value = prop.GetValue(item);
			Console.WriteLine($"{prop.Name}: {value}");
		}
		Console.WriteLine("-----------------------------------");
	}
}

string dobaviteljPathRelative = "dobavitelji.xml";
string artikelPathRelative = "artikli.xml";

string basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\"));


// Main
List <Dobavitelj> dobavitelji = generateDummyData<Dobavitelj>(5);
SerializeToXml(dobavitelji, "dobavitelji.xml");
List<Artikel> artikili = generateDummyData<Artikel>(5);
SerializeToXml(artikili, "artikli.xml");
List<Dobavitelj> dobaviteljFromXml = DeserializeFromXml<List<Dobavitelj>>(Path.Combine(basePath, dobaviteljPathRelative));
List<Artikel> artikelFromXml = DeserializeFromXml<List<Artikel>>(Path.Combine(basePath, artikelPathRelative));
cwList(dobaviteljFromXml);
cwList(artikelFromXml);

string schemaPath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\XMLSchema1.xsd");

string artikliXmlPath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\artikli.xml");
string dobaviteljiXmlPath = Path.Combine(AppContext.BaseDirectory, @"..\..\..\dobavitelji.xml");

XmlValidator validationService = new XmlValidator(schemaPath);

bool artikliValid = validationService.ValidateXmlDocument(artikliXmlPath, "Artikli XML");
bool dobaviteljiValid = validationService.ValidateXmlDocument(dobaviteljiXmlPath, "Dobavitelji XML");

if (artikliValid && dobaviteljiValid)
{
	List<int> dobaviteljiId = getDobaviteljId();
	Random random = new();
	Artikel newArtikel = new Artikel(
		random.Next(10000, 999999),
		"asdf",          
		99.99m,                 
		50,                     
		dobaviteljiId[3],       
		new DateTime(2022, random.Next(1, 12), random.Next(1, 28))        
	);

	// Example new Dobavitelj
	Dobavitelj newDobavitelj = new Dobavitelj(
		100,                    
		"Nov Dobavitelj",       
		"Nov Naslov",           
		123456,                 
		"nov.kontak@gmail.com", 
		"Opis novega dobavitelja" 
	);

	bool artikelAdded = validationService.AddNewArtikel(newArtikel, artikliXmlPath);
	bool dobaviteljAdded = validationService.AddNewDobavitelj(newDobavitelj, dobaviteljiXmlPath);
}