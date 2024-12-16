<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:fo="http://www.w3.org/1999/XSL/Format"
                version="1.0">
	<!-- Match the root element -->
	<xsl:template match="/">
		<fo:fo xmlns:fo="http://www.w3.org/1999/XSL/Format">
			<fo:layout-master-set>
				<fo:simple-page-master master-name="A4-Page"
                                       page-height="29.7cm" page-width="21cm"
                                       margin-top="1cm" margin-bottom="1cm" margin-left="1.5cm" margin-right="1.5cm">
					<fo:region-body margin-top="2cm"/>
				</fo:simple-page-master>
			</fo:layout-master-set>

			<fo:page-sequence master-reference="A4-Page">
				<fo:flow flow-name="xsl-region-body">
					<!-- Title of the document -->
					<fo:block font-size="18pt" font-weight="bold" text-align="center" space-after="1cm">
						Seznam Artiklov
					</fo:block>

					<!-- Table of articles -->
					<fo:table border="1px solid black" width="100%">
						<fo:table-column column-width="25%"/>
						<fo:table-column column-width="25%"/>
						<fo:table-column column-width="25%"/>
						<fo:table-column column-width="25%"/>

						<fo:table-header>
							<fo:table-row>
								<fo:table-cell background-color="#e6e6e6">
									<fo:block font-weight="bold">ID</fo:block>
								</fo:table-cell>
								<fo:table-cell background-color="#e6e6e6">
									<fo:block font-weight="bold">Naziv</fo:block>
								</fo:table-cell>
								<fo:table-cell background-color="#e6e6e6">
									<fo:block font-weight="bold">Cena</fo:block>
								</fo:table-cell>
								<fo:table-cell background-color="#e6e6e6">
									<fo:block font-weight="bold">Zaloga</fo:block>
								</fo:table-cell>
							</fo:table-row>
						</fo:table-header>

						<!-- Body of the table with article data -->
						<fo:table-body>
							<xsl:for-each select="/ArrayOfArtikel/Artikel[zaloga &gt; 100000]">
								<fo:table-row>
									<fo:table-cell>
										<fo:block>
											<xsl:value-of select="id"/>
										</fo:block>
									</fo:table-cell>
									<fo:table-cell>
										<fo:block>
											<xsl:value-of select="naziv"/>
										</fo:block>
									</fo:table-cell>
									<fo:table-cell>
										<fo:block>
											<xsl:value-of select="cena"/>
										</fo:block>
									</fo:table-cell>
									<fo:table-cell>
										<fo:block>
											<xsl:value-of select="zaloga"/>
										</fo:block>
									</fo:table-cell>
								</fo:table-row>
							</xsl:for-each>
						</fo:table-body>
					</fo:table>
				</fo:flow>
			</fo:page-sequence>
		</fo:fo>
	</xsl:template>
</xsl:stylesheet>
