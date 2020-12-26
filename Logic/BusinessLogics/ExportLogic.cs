using Logic.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using Logic.HelperModels;

namespace Logic.Logics
{
    public class ExportLogic
    {
        static string exportDirectory = Directory.GetCurrentDirectory() + "\\wwwroot\\Export\\";
        Database context;

        public ExportLogic(Database _context)
        {
            context = _context;
        }


        public static void CreateDocAnalysis(WordInfo info, List<(Sale, IEnumerable<IGrouping<string, Product>>)> productList)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(exportDirectory + info.FileName, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body docBody = mainPart.Document.AppendChild(new Body());

                docBody.AppendChild(CreateParagraph(new WordParagraph
                {
                    Texts = new List<string> { info.Title },
                    TextProperties = new WordParagraphProperties
                    {
                        Bold = true,
                        Size = "28",
                        JustificationValues = JustificationValues.Center
                    }
                }));

                if (productList != null)
                {
                    foreach (var list in productList)
                    {
                        docBody.AppendChild(CreateParagraph(new WordParagraph
                        {
                            Texts = new List<string> { "Покупка: " + list.Item1.Date.ToString("yyyy-MM-dd"), "" },
                            TextProperties = new WordParagraphProperties
                            {
                                Bold = true,
                                Size = "24",
                                JustificationValues = JustificationValues.Both
                            }
                        }));
                        foreach (var byGroup in list.Item2)
                        {
                            docBody.AppendChild(CreateParagraph(new WordParagraph
                            {
                                Texts = new List<string> { "Группа: " + byGroup.Key, "" },
                                TextProperties = new WordParagraphProperties
                                {
                                    Bold = true,
                                    Size = "24",
                                    JustificationValues = JustificationValues.Both
                                }
                            }));
                            foreach (var product in byGroup)
                            {
                                docBody.AppendChild(CreateParagraph(new WordParagraph
                                {
                                    Texts = new List<string> { "Наименование: " + product.Name, "Цена: " + product.Price, "" },
                                    TextProperties = new WordParagraphProperties
                                    {
                                        Bold = false,
                                        Size = "24",
                                        JustificationValues = JustificationValues.Both
                                    }
                                }));
                            }
                        }
                    }
                }
                docBody.AppendChild(CreateSectionProperties());
                wordDocument.MainDocumentPart.Document.Save();
            }
        }

        public static void CreateDocCross(WordInfo info, List<List<string>> result)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(exportDirectory + info.FileName, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body docBody = mainPart.Document.AppendChild(new Body());

                docBody.AppendChild(CreateParagraph(new WordParagraph
                {
                    Texts = new List<string> { info.Title },
                    TextProperties = new WordParagraphProperties
                    {
                        Bold = true,
                        Size = "28",
                        JustificationValues = JustificationValues.Center
                    }
                }));

                Table table = new Table();
                TableProperties tblProp = new TableProperties(
                    new TableBorders(
                        new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
                        new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
                        new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
                        new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
                        new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 },
                        new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 8 }
                    )
                );
                table.AppendChild(tblProp);

                if (result != null)
                {
                    foreach (var row in result)
                    {
                        var tableRow = new TableRow();
                        foreach (var cell in row)
                        {
                            tableRow.Append(new TableCell(new Paragraph(new Run(new Text(cell)))));
                        }
                        table.Append(tableRow);
                    }
                }
                docBody.Append(table);
                docBody.AppendChild(CreateSectionProperties());
                wordDocument.MainDocumentPart.Document.Save();
            }
        }

        private static SectionProperties CreateSectionProperties()
        {
            SectionProperties properties = new SectionProperties();
            PageSize pageSize = new PageSize { Orient = PageOrientationValues.Portrait };

            properties.AppendChild(pageSize);

            return properties;
        }

        private static Paragraph CreateParagraph(WordParagraph paragraph)
        {
            if (paragraph != null)
            {
                Paragraph docParagraph = new Paragraph();

                docParagraph.AppendChild(CreateParagraphProperties(paragraph.TextProperties));

                foreach (var run in paragraph.Texts)
                {
                    Run docRun = new Run();
                    RunProperties properties = new RunProperties();
                    properties.AppendChild(new FontSize { Val = paragraph.TextProperties.Size });

                    if (!run.StartsWith(" - ") && paragraph.TextProperties.Bold)
                    {
                        properties.AppendChild(new Bold());
                    }

                    docRun.AppendChild(properties);
                    docRun.AppendChild(new Text { Text = run, Space = SpaceProcessingModeValues.Preserve });
                    docParagraph.AppendChild(docRun);
                }

                return docParagraph;
            }

            return null;
        }

        private static ParagraphProperties CreateParagraphProperties(WordParagraphProperties paragraphProperties)
        {
            if (paragraphProperties != null)
            {
                ParagraphProperties properties = new ParagraphProperties();

                properties.AppendChild(new Justification() { Val = paragraphProperties.JustificationValues });
                properties.AppendChild(new SpacingBetweenLines { LineRule = LineSpacingRuleValues.Auto });
                properties.AppendChild(new Indentation());

                ParagraphMarkRunProperties paragraphMarkRunProperties = new ParagraphMarkRunProperties();

                if (!string.IsNullOrEmpty(paragraphProperties.Size))
                {
                    paragraphMarkRunProperties.AppendChild(new FontSize { Val = paragraphProperties.Size });
                }

                if (paragraphProperties.Bold)
                {
                    paragraphMarkRunProperties.AppendChild(new Bold());
                }

                properties.AppendChild(paragraphMarkRunProperties);

                return properties;
            }

            return null;
        }
    }
}
