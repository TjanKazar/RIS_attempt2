using RIS;
using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net.WebSockets;
using System.Reflection;
using System.Security.Principal;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Xceed.Document.NET;
using Xceed.Words.NET;

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
		9,
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


	// naloga 6


	Console.WriteLine("-----------------------------------");
	Console.WriteLine("poizvedbe za nalogo 6 ");
	Console.WriteLine("-----------------------------------");

	XPathNavigator Anav;
	XPathNavigator Dnav;
	XPathDocument ArtikelNav;
	XPathDocument DobaviteljNav;
	XPathNodeIterator NodeIter;

	ArtikelNav = new XPathDocument(artikliXmlPath);
	DobaviteljNav = new XPathDocument(dobaviteljiXmlPath);

	Dnav = DobaviteljNav.CreateNavigator();
	Anav = ArtikelNav.CreateNavigator();



	// poveprečna cena artikla

	string artikelPriceExpression = "sum(/ArrayOfArtikel/Artikel/cena) div count(/ArrayOfArtikel/Artikel)";


	//vsi artikli
	string allArtikelExpresssion = "/ArrayOfArtikel/Artikel/naziv";

	NodeIter = Anav.Select(allArtikelExpresssion);
	Console.WriteLine("vsi nazivi artiklov: ");
	while (NodeIter.MoveNext())
		Console.WriteLine("naziv: " + NodeIter.Current.Value);

	//vsi Dobavitelji
	string allDobaviteljExpresssion = "/ArrayOfDobavitelj/Dobavitelj/naziv";

	NodeIter = Dnav.Select(allDobaviteljExpresssion);
	Console.WriteLine("vsi nazivi Dobaviteljev: ");
	while (NodeIter.MoveNext())
		Console.WriteLine("naziv: " + NodeIter.Current.Value);

	// cena na 50.00
	string above50Expression = "/ArrayOfArtikel/Artikel/naziv[../cena>50.00]";
	Console.WriteLine("povprečna cena artikla je {0}", Anav.Evaluate(artikelPriceExpression));

	NodeIter = Anav.Select(above50Expression);
	Console.WriteLine("vsi artikili z ceno višjo od 50 EUR so : ");

	while (NodeIter.MoveNext())
		Console.WriteLine("Naziv artikla: {0}", NodeIter.Current.Value);


	//izpis artikla z id = 1

	string id1Expression = "/ArrayOfArtikel/Artikel/naziv[../id=1]";

	NodeIter = Anav.Select(id1Expression);
	Console.WriteLine("artikel na id = 1: ");
	while (NodeIter.MoveNext())
		Console.WriteLine("Naziv Artikla : {0}", NodeIter.Current.Value);

	//izpis artikla z id = 1

	string id1ExpressionDobavitelj = "/ArrayOfDobavitelj/Dobavitelj/naziv[../id=1]";

	NodeIter = Dnav.Select(id1ExpressionDobavitelj);
	Console.WriteLine("dobavitelj na id = 1: ");
	while (NodeIter.MoveNext())
		Console.WriteLine("Naziv Artikla : {0}", NodeIter.Current.Value);


	// preverjanje, če je zaloga prazna

	string zeroStockExpression = "boolean(/ArrayOfArtikel/Artikel[zaloga=0])";
	bool hasZeroStock = (bool)Anav.Evaluate(zeroStockExpression);
	Console.WriteLine(hasZeroStock
		? "Obstajajo artikli brez zaloge."
		: "Vsi artikli imajo zalogo.");

	string totalPriceExpression = "sum(/ArrayOfArtikel/Artikel/cena)";
	Console.WriteLine("Cena vseh artiklov skupaj : " + Anav.Evaluate(totalPriceExpression));

	// dobavitelj z davčno = 123456

	string davcna123456Expression = "/ArrayOfDobavitelj/Dobavitelj/naziv[../davcnaSt=123456]";
	NodeIter = Dnav.Select(davcna123456Expression);
	Console.WriteLine("Dobavitelj z davčno številko enako 123456: ");
	while (NodeIter.MoveNext())
		Console.WriteLine("Dobavitelj z nazivom : " + NodeIter.Current.Value);

	// nizka zaloga : 

	string lowStockIdsExpression = "/ArrayOfArtikel/Artikel/id[../zaloga < 10]";
	NodeIter = Anav.Select(lowStockIdsExpression);
	Console.WriteLine("Artikli z zalogo manj kot 10:");
	while (NodeIter.MoveNext())
		Console.WriteLine("ID artikla: " + NodeIter.Current.Value);

	//stevilo artiklov : 

	string totalArtikelCountExpression = "count(/ArrayOfArtikel/Artikel)";
	Console.WriteLine("Skupno število artiklov: " + Anav.Evaluate(totalArtikelCountExpression));



	// naloga 7

	XslCompiledTransform myXslTrans = new XslCompiledTransform();
	XmlTextWriter writer = new XmlTextWriter(@"Artikli.html", null);

	myXslTrans.Load(@"../../../ArtikelTransformacija.xslt");
	myXslTrans.Transform(ArtikelNav, null, writer);


	ProcessStartInfo startInfo = new ProcessStartInfo
	{
		FileName = "Artikli.html",
		UseShellExecute = true
	};
	Process process = Process.Start(startInfo);
	process.WaitForExit();


	string baseDirectory = AppContext.BaseDirectory;
	DirectoryInfo directory = new DirectoryInfo(baseDirectory);
	DirectoryInfo targetDirectory = directory.Parent?.Parent?.Parent;


	XmlDocument xmlDoc = new XmlDocument();
	xmlDoc.Load(artikliXmlPath);



	// 9 naloga


	string fileLocation = Path.GetFullPath(AppContext.BaseDirectory);
	DirectoryInfo targetDocx = new DirectoryInfo(fileLocation).Parent.Parent.Parent;
	string docxDirectory = Path.Combine(targetDocx.ToString(), "artikli.docx");


	var doc = DocX.Create(docxDirectory);
	doc.InsertParagraph("Seznam artiklov");



	 List <Artikel> artikelList = DeserializeFromXml<List<Artikel>>(artikliXmlPath);

	Table t = doc.AddTable(artikelList.Count + 1, 6);
	t.Alignment = Alignment.center;
	t.Design = TableDesign.MediumGrid1Accent2;

	t.Rows[0].Cells[0].Paragraphs.First().Append("id");
	t.Rows[0].Cells[1].Paragraphs.First().Append("naziv");
	t.Rows[0].Cells[2].Paragraphs.First().Append("cena");
	t.Rows[0].Cells[3].Paragraphs.First().Append("zaloga");
	t.Rows[0].Cells[4].Paragraphs.First().Append("dobaviteljId");
	t.Rows[0].Cells[5].Paragraphs.First().Append("datum zadnje nabave");

	for (int i = 0; i < artikelList.Count; i++)
	{
		var artikel = artikelList[i];
        Console.WriteLine("something");
		t.Rows[i + 1].Cells[0].Paragraphs.First().Append(artikel.id.ToString());  
		t.Rows[i + 1].Cells[1].Paragraphs.First().Append(artikel.naziv);  
		t.Rows[i + 1].Cells[2].Paragraphs.First().Append(artikel.cena.ToString("F2")); 
		t.Rows[i + 1].Cells[3].Paragraphs.First().Append(artikel.zaloga.ToString()); 
		t.Rows[i + 1].Cells[4].Paragraphs.First().Append(artikel.dobaviteljId.ToString()); 
		t.Rows[i + 1].Cells[5].Paragraphs.First().Append(artikel.datumZadnjeNabave.ToString("yyyy-MM-dd"));  
	}
	doc.InsertTable(t);
	doc.Save();

	DirectoryInfo start = new DirectoryInfo(baseDirectory).Parent.Parent.Parent;

	Process.Start(docxDirectory);

	DocX doc2 = DocX.Load(docxDirectory);

    Console.WriteLine(doc2.Xml);


	File.WriteAllText(docxDirectory, doc2.Xml.ToString());
}


	
	
