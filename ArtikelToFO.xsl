<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

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

				<xsl:variable name="maxCena">
					<xsl:for-each select="ArrayOfArtikel/Artikel">
						<xsl:if test="not(preceding-sibling::Artikel/cena &gt; cena)">
							<xsl:if test="not(following-sibling::Artikel/cena &gt; cena)">
								<xsl:value-of select="cena" />
							</xsl:if>
						</xsl:if>
					</xsl:for-each>
				</xsl:variable>

				<xsl:variable name="minCena">
					<xsl:for-each select="ArrayOfArtikel/Artikel">
						<xsl:if test="not(preceding-sibling::Artikel/cena &lt; cena)">
							<xsl:if test="not(following-sibling::Artikel/cena &lt; cena)">
								<xsl:value-of select="cena" />
							</xsl:if>
						</xsl:if>
					</xsl:for-each>
				</xsl:variable>

				<table>
					<tr>
						<th>ID</th>
						<th>Naziv</th>
						<th>Cena</th>
						<th>Zaloga</th>
						<th>Dobavitelj ID</th>
						<th>Datum Zadnje Nabave</th>
					</tr>

					<xsl:for-each select="ArrayOfArtikel/Artikel">
						<tr>
							<td>
								<xsl:value-of select="id" />
							</td>
							<td>
								<xsl:value-of select="naziv" />
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="cena = $maxCena">
										<td style="background-color: red; color: white;">
											<xsl:value-of select="cena" />
										</td>
									</xsl:when>
									<xsl:when test="cena = $minCena">
										<td style="background-color: green; color: white;">
											<xsl:value-of select="cena" />
										</td>
									</xsl:when>
									<xsl:otherwise>
										<td>
											<xsl:value-of select="cena" />
										</td>
									</xsl:otherwise>
								</xsl:choose>
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
