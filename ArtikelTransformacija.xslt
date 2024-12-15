<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

	<!-- Predstavitev XML kot HTML -->
	<xsl:output method="html" encoding="UTF-8" />

	<xsl:template match="/">
		<html>
			<head>
				<title>Seznam Artiklov</title>
				<style>
					table {
					width: 100%;
					border-collapse: collapse;
					}
					th, td {
					border: 1px solid black;
					padding: 8px;
					text-align: left;
					}
					th {
					background-color: #f2f2f2;
					}
				</style>
			</head>
			<body>
				<h1>Seznam Artiklov</h1>
				<table>
					<tr>
						<th>ID</th>
						<th>Naziv</th>
						<th>Cena</th>
						<th>Zaloga</th>
						<th>Dobavitelj ID</th>
						<th>Datum Zadnje Nabave</th>
					</tr>
					<!-- Iteracija skozi vse elemente Artikel -->
					<xsl:for-each select="ArrayOfArtikel/Artikel">
						<tr>
							<td>
								<xsl:value-of select="id" />
							</td>
							<td>
								<xsl:value-of select="naziv" />
							</td>
							<td>
								<xsl:value-of select="cena" />
							</td>
							<td>
								<xsl:value-of select="zaloga" />
							</td>
							<td>
								<xsl:value-of select="dobaviteljId" />
							</td>
							<td>
								<xsl:value-of select="datumZadnjeNabave" />
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>

</xsl:stylesheet>
