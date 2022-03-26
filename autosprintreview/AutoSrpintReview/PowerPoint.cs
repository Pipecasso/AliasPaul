using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using P = DocumentFormat.OpenXml.Presentation;
using D = DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.Drawing;


namespace AutoSrpintReview
{
    public class PowerPoint : IDisposable
    {

        private const string _linkbase = "https://dev.azure.com/hexagonPPMCOL/PPM/_workitems/edit/";
        private PowerpointBacklogItems _backlogItems;
        private string _iteration;
        private PresentationPart _presentationPart;
        private PresentationDocument _presentationDocument;
        private enum makeitstatus { neutral, open };
        private SlidePart _teamSlide;
        private SlidePart _goalSlide;
        private SlidePart _demoSplide;
        private SlidePart _firstTableSlide;

        public string TeamName { get; set; }
        public string TeamDescription { get; set; }
        public string LogoPath { get; set; }
        public DateTime Date { get; set; }
     
        public string Iteration { get => _iteration; }
        public Dictionary<string, Func<PowerPoint, string>> _replacemap;


        public enum BulletCat
        {
            SprintGoal,
            Demo,
            QuarterGoal
        }

        private List<Tuple<BulletCat, string>> _BulletText;

        public PowerPoint(PowerpointBacklogItems backlogItems, string templatepath, string outputdir, string itearion)
        {

            _backlogItems = backlogItems;
            _BulletText = new List<Tuple<BulletCat, string>>();


            string year = DateTime.Now.Year.ToString();
            _iteration = $"{year}_{itearion}";
            string outputfilename = $"{TeamName} Sprint Review {_iteration}.pptx";
            string outputpath = System.IO.Path.Combine(outputdir, year, _iteration, outputfilename);
            string dir = System.IO.Path.GetDirectoryName(outputpath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            if (File.Exists(outputpath)) File.Delete(outputpath);
            File.Copy(templatepath, outputpath);

            _presentationDocument = PresentationDocument.Open(outputpath, true);
            _presentationPart = _presentationDocument.PresentationPart;

            _replacemap = new Dictionary<string, Func<PowerPoint, string>>();
            Func<PowerPoint, string> TeamNameF = x => x.TeamName;
            Func<PowerPoint, string> TeamDescriptionF = x => x.TeamDescription;
            Func<PowerPoint, string> SRDate = x => x.Date.ToLongDateString();
            Func<PowerPoint, string> Iteration = x => x.Iteration;
            _replacemap.Add("SR.TeamName", TeamNameF);
            _replacemap.Add("SR.TeamDescription", TeamDescriptionF);
            _replacemap.Add("SR.Date", SRDate);
            _replacemap.Add("SR.Iteration", Iteration);

            StringValue teamSlideID = _presentationPart.Presentation.SlideIdList.ChildElements.Select(x => (x as SlideId).RelationshipId).Where(y => y == "rId3").First();
            StringValue goalSlideID = _presentationPart.Presentation.SlideIdList.ChildElements.Select(x => (x as SlideId).RelationshipId).Where(y => y == "rId4").First();
            StringValue demoSlideID = _presentationPart.Presentation.SlideIdList.ChildElements.Select(x => (x as SlideId).RelationshipId).Where(y => y == "rId5").First();
            StringValue tableSideID = _presentationPart.Presentation.SlideIdList.ChildElements.Select(x => (x as SlideId).RelationshipId).Where(y => y == "rId6").First();

            _teamSlide = (SlidePart)_presentationPart.GetPartById(teamSlideID);
            _goalSlide = (SlidePart)_presentationPart.GetPartById(goalSlideID);
            _demoSplide = (SlidePart)_presentationPart.GetPartById(demoSlideID);
            _firstTableSlide = (SlidePart)_presentationPart.GetPartById(tableSideID);

        }

        public void AddBulletText(BulletCat bulletCat, string text)
        {
            Tuple<BulletCat, string> Bullet = new Tuple<BulletCat, string>(bulletCat, text);
            _BulletText.Add(Bullet);
        }

        public void AddBullets(P.Shape shape,IEnumerable<string> bullets)
        {
            D.Paragraph sampleparagraph = shape.TextBody.Descendants<Paragraph>().First();
            D.Run corn = sampleparagraph.Descendants<D.Run>().First();
            bool firstgo = true;
            foreach (string bullet in bullets)
            {
                if (firstgo)
                {
                    firstgo = false;
                    corn.Text = new D.Text(bullet);
                }
                else
                {
                    D.Paragraph newPara = new D.Paragraph();
                    if (sampleparagraph.ParagraphProperties != null)
                    {
                        newPara.ParagraphProperties = new D.ParagraphProperties(sampleparagraph.ParagraphProperties.OuterXml);
                    }
                    D.Run newRun = new D.Run();
                    newRun.RunProperties = new RunProperties(corn.RunProperties.OuterXml);
                    newRun.Text = new D.Text(bullet);
                    newPara.Append(newRun);
                    shape.TextBody.Append(newPara);
                }
            }
        }

        public void Dispose()
        {
            _presentationPart = null;
            _presentationDocument.Dispose();
        }

        private string subit(string toreplace)
        {
            string togo = string.Empty;
            if (_replacemap.ContainsKey(toreplace))
            {
                togo = _replacemap[toreplace](this);
            }
            else
            {
                togo = "sasauges";
            }
            return togo;
        }

        private P.Shape ShapeFinder(string id,SlidePart slidePart)
        {
            ShapeTree shapeTree = slidePart.Slide.Descendants<ShapeTree>().FirstOrDefault();
            return shapeTree.Descendants<P.Shape>().Where(x => x.NonVisualShapeProperties.Descendants<P.NonVisualDrawingProperties>().Where(y => y.Id == id).Any()).FirstOrDefault();
        }

        private void AddPicture(string path,SlidePart slidepart,string name)
        {
            ImagePart LogoPart = slidepart.AddImagePart(ImagePartType.Png);
            using (FileStream stream = File.OpenRead(LogoPath))
            {
                LogoPart.FeedData(stream);
            }

            ShapeTree shapeTree = _teamSlide.Slide.Descendants<DocumentFormat.OpenXml.Presentation.ShapeTree>().First();

            P.Picture picture = new P.Picture();
            P.NonVisualPictureProperties nonVisualPictureProperties = new P.NonVisualPictureProperties();
            picture.NonVisualPictureProperties = nonVisualPictureProperties;
            P.NonVisualDrawingProperties nonVisualDrawingProperties = new P.NonVisualDrawingProperties()
            {
                Name = name,
                Id = (UInt32)shapeTree.ChildElements.Count - 1
            };

            nonVisualPictureProperties.Append(nonVisualDrawingProperties);

            P.NonVisualPictureDrawingProperties nonVisualPictureDrawingProperties = new P.NonVisualPictureDrawingProperties();
            D.PictureLocks pictureLocks = new D.PictureLocks()
            { NoChangeAspect = true};
            nonVisualPictureDrawingProperties.Append(pictureLocks);

            picture.NonVisualPictureProperties.Append(nonVisualPictureDrawingProperties);
            P.ApplicationNonVisualDrawingProperties applicationNonVisualDrawingProperties = new ApplicationNonVisualDrawingProperties();
            picture.NonVisualPictureProperties.Append(applicationNonVisualDrawingProperties);

            P.BlipFill blipFill = new P.BlipFill();
            D.Blip blip = new D.Blip()
            {
                Embed = slidepart.GetIdOfPart(LogoPart)
            };

            D.BlipExtensionList blipExtensionList1 = new D.BlipExtensionList();
            D.BlipExtension blipExtension1 = new D.BlipExtension();
            Guid g = Guid.NewGuid();
            blipExtension1.Uri = g.ToString();

            
            UseLocalDpi useLocalDpi1 = new UseLocalDpi()
            {
                Val = false
            };
            useLocalDpi1.AddNamespaceDeclaration("a14", "http://schemas.microsoft.com/office/drawing/2010/main");
            blipExtension1.Append(useLocalDpi1);
            blipExtensionList1.Append(blipExtension1);
            blip.Append(blipExtensionList1);

            D.Stretch stretch = new D.Stretch();
            D.FillRectangle fillRectangle = new FillRectangle();
            stretch.Append(fillRectangle);
            blipFill.Append(blip);
            blipFill.Append(stretch);
            picture.Append(blipFill);

            P.ShapeProperties shapeProperties = new P.ShapeProperties();
            picture.ShapeProperties = shapeProperties;
            D.Transform2D transform2D = new D.Transform2D();
            picture.ShapeProperties.Transform2D = transform2D;
            D.Offset offset = new D.Offset()
            { 
                X= 7737927,
                Y= 145856 
            };
            transform2D.Append(offset);
            D.Extents extents = new D.Extents()
            {
                Cx = 4269928,
                Cy = 3913580
            };
            transform2D.Append(extents);

            D.PresetGeometry presetGeometry = new PresetGeometry()
            {
                Preset = D.ShapeTypeValues.Rectangle
            };
            shapeProperties.Append(presetGeometry);
            shapeTree.Append(picture);
        }


        private Slide TableSlideClone(int position)
        {
            //clone the slide
            Slide newSlide = (Slide)_firstTableSlide.Slide.CloneNode(true);


            SlideIdList slideIdList = _presentationPart.Presentation.SlideIdList;
            IEnumerable<SlideId> slideIdCollection = slideIdList.ChildElements.Cast<SlideId>();

            SlideId thatsourman = null;
            foreach (SlideId slideId in slideIdCollection)
            {
                position--;
                if (position == 0)
                {
                    thatsourman = slideId;
                    break;
                }
            }

            uint newSlideIdVal = slideIdCollection.Max(x => x.Id.Value) + 1;

            SlidePart newSlidePart = _presentationPart.AddNewPart<SlidePart>();
            newSlidePart.Slide = newSlide;
           // newSlide.Save(newSlidePart);

            SlideId newSlideId = slideIdList.InsertAfter(new SlideId(), thatsourman);
            newSlideId.Id = newSlideIdVal;
            newSlideId.RelationshipId = _presentationPart.GetIdOfPart(newSlidePart);

            newSlidePart.AddPart(_firstTableSlide.SlideLayoutPart);

            return newSlide;

        }


        private enum ColumnType { unset, id, description, points }

        private void AddColumn(BacklogItem backlogItem,ColumnType columnType,TableRow tableRow)
        {
            BodyProperties commonBodyProperties = new BodyProperties();
            ListStyle commonListStyle = new ListStyle();
            EndParagraphRunProperties commonEndRunProperties = new EndParagraphRunProperties() { Language = "en-GB", Dirty = false };
            TableCellProperties commonTableProperties = new TableCellProperties();
            RunProperties commonRunProperties = new RunProperties() { Language = "en-US", Dirty = false, SmartTagClean = false };

            TableCell tableCell = new TableCell();
            D.TextBody textBody = new D.TextBody();
            Paragraph paragraph = new Paragraph();

            string textval = string.Empty;
            switch (columnType)
            {
                case ColumnType.id:textval = backlogItem.ID;break;
                case ColumnType.description:textval = backlogItem.Title;break;
                case ColumnType.points:textval = backlogItem.Points.ToString();break;

            }

            Run run = new Run();
            D.Text text = new D.Text();
            text.Text = textval;


            run.Append(commonRunProperties);
            run.Append(text);

            paragraph.Append(run);
            paragraph.Append(commonEndRunProperties);
            textBody.Append(commonBodyProperties);
            textBody.Append(commonListStyle);
            textBody.Append(paragraph);

            tableCell.Append(textBody);
            tableCell.Append(commonTableProperties);

            tableRow.Append(tableCell);


        }

        private void PopulateTableSide(IEnumerable<BacklogItem> backlogItems, Slide slide)
        {
            Table table = slide.Descendants<Table>().First();
            TableRow tableRowLast = slide.Descendants<TableRow>().Last();
            int colCount = tableRowLast.Descendants<TableCell>().Count();

            bool first = true;
            foreach (BacklogItem backlogItem in backlogItems)
            {
                if (first)
                {
                    TableCell[] cells = tableRowLast.Descendants<TableCell>().ToArray();

                    Run runlink = cells[0].Descendants<D.TextBody>().First().Descendants<Paragraph>().First().Descendants<Run>().First();
                    D.Text id = runlink.Descendants<D.Text>().First();
                    id.Text = backlogItem.ID;
                    HyperlinkOnClick hyperlinkOnClick = runlink.RunProperties.Descendants<HyperlinkOnClick>().First();
                    string hyperlikid = hyperlinkOnClick.Id;
                    HyperlinkRelationship hlr = slide.SlidePart.HyperlinkRelationships.Where(x => x.Id == hyperlinkOnClick.Id).FirstOrDefault();
                    if (hlr != null)
                    {
                        string newuri = $"{_linkbase}/{backlogItem.ID}";
                        Uri uriID = new Uri(newuri, UriKind.Absolute);
                        slide.SlidePart.DeleteReferenceRelationship(hlr);
                        slide.SlidePart.AddHyperlinkRelationship(uriID, true, hyperlikid);
                    }


                    Run runtitle = cells[1].Descendants<D.TextBody>().First().Descendants<Paragraph>().First().Descendants<Run>().First();
                    D.Text title = runtitle.Descendants<D.Text>().First();
                    title.Text = backlogItem.Title;


                    Run runpoints = cells[2].Descendants<D.TextBody>().First().Descendants<Paragraph>().First().Descendants<Run>().First();
                    D.Text points = runpoints.Descendants<D.Text>().First();
                    points.Text = backlogItem.Points.ToString();
                    first = false;
                }
                else
                {
                    TableRow tableRow = new TableRow();
                    tableRow.Height = tableRowLast.Height;

                    AddColumn(backlogItem, ColumnType.id, tableRow);
                    AddColumn(backlogItem, ColumnType.description, tableRow);
                    AddColumn(backlogItem, ColumnType.points, tableRow);
                    table.Append(tableRow);
                }
            }
        }

  
        private IEnumerable<IEnumerable<BacklogItem>> DivvyUp(IEnumerable<BacklogItem> backlogItems,int itemcount)
        {
            int totalslides;
            int lastslide;
            bool allslidessame = backlogItems.Count() % itemcount == 0;
            BacklogItem[] itemArray = backlogItems.ToArray();
            totalslides = Math.DivRem(backlogItems.Count(), itemcount, out lastslide);
            BacklogItem[][] itemstogo = new BacklogItem[allslidessame ? totalslides : totalslides+1][];
            for (int i=0;i<backlogItems.Count();i++)
            {
                int slidenum;
                int itemnum;
                slidenum = Math.DivRem(i, itemcount, out itemnum);
                if (itemnum == 0)
                {
                    if (!allslidessame && slidenum == backlogItems.Count() - 1)
                    {
                        itemstogo[slidenum] = new BacklogItem[lastslide];
                    }
                    else
                    {
                        itemstogo[slidenum] = new BacklogItem[itemcount];
                    }
                }
                itemstogo[slidenum][itemnum] = itemArray[i];
            }
            return itemstogo;
        }
    
        public void MakeIt()
        {
            List<string> Expressions = new List<string>();
            IEnumerable<D.Text> texts = new List<D.Text>();
            Presentation presentation = _presentationPart.Presentation;
            OpenXmlElementList slideIds = presentation.SlideIdList.ChildElements;
            foreach (OpenXmlElement openXmlElement in slideIds)
            {
                string slidePartRelationshipId = (openXmlElement as SlideId).RelationshipId;
                SlidePart slideTitlePart = (SlidePart)_presentationPart.GetPartById(slidePartRelationshipId);
                Slide slide = slideTitlePart.Slide;
                IEnumerable<Paragraph> paragraphs = slide.Descendants<Paragraph>();
                foreach (Paragraph paragraph in paragraphs)
                { 
                    IEnumerable<D.Text> temptexts = texts.Concat(paragraph.Descendants<D.Text>());
                    texts = temptexts;
                }  
            }
            makeitstatus status = makeitstatus.neutral;

            string bigpowerpointstring = string.Empty;
            Dictionary<int, D.Text> strindex = new Dictionary<int, D.Text>();
            int tick = 0;
            Dictionary<string, List<D.Text> > replacements = new Dictionary<string, List<D.Text>>();
            foreach (D.Text text in texts)
            {
                bigpowerpointstring += text.Text;
                int tock = tick + text.Text.Length;
                for (int i=tick;i<tock;i++)
                {
                    strindex.Add(i, text);
                }
                tick = tock;
            }

            int index = 0;
            int index2 = 0;
            string replacement = string.Empty;
            List<D.Text> BracketList = new List<D.Text>();
            do
            {
                switch (status)
                {
                    case makeitstatus.neutral:
                        index = bigpowerpointstring.IndexOf('[', index);
                        status = makeitstatus.open;
                        break;
                    case makeitstatus.open:
                        index2 = bigpowerpointstring.IndexOf(']', index);
                        replacement = bigpowerpointstring.Substring(index + 1, index2 - index - 1);
                        if (replacements.ContainsKey(replacement))
                        {
                            replacements[replacement].Add(strindex[index + 1]);
                        }
                        else
                        {
                            List<D.Text> newlist = new List<D.Text>() { strindex[index + 1] };
                            replacements.Add(replacement, newlist);
                        }
                      
                        index = index2;
                        status = makeitstatus.neutral;
                        break;
                }
                if (index>-1) BracketList.Add(strindex[index]);

            } while (index > -1);

            foreach (KeyValuePair<string,List<D.Text>> kvp in replacements)
            {
                string final = subit(kvp.Key);
                foreach(D.Text text in kvp.Value)
                {
                    text.Text = text.Text.Replace(kvp.Key, final);
                }
            }

            foreach (D.Text text in BracketList)
            {
                text.Text = text.Text.Replace("[",string.Empty);
                text.Text = text.Text.Replace("]", string.Empty);
            }

            //logo
            if (File.Exists(LogoPath))
            {
                AddPicture(LogoPath, _teamSlide,"TeamLogo");
            }
   
            IEnumerable<string> goals = _BulletText.Where(x => x.Item1 == BulletCat.SprintGoal).Select(y => y.Item2);
            IEnumerable<string> demos = _BulletText.Where(x => x.Item1 == BulletCat.Demo).Select(y => y.Item2);
            ShapeTree shapeTree = _goalSlide.Slide.Descendants<ShapeTree>().FirstOrDefault();
            P.Shape goalshape = ShapeFinder("5", _goalSlide);
            AddBullets(goalshape, goals);
            if (demos.Any())
            {
                P.Shape demoshape = ShapeFinder("5", _demoSplide);
                AddBullets(demoshape, demos);
            }

            IEnumerable<PowerPointBacklogItem> inProgressItems = _backlogItems.Where(x => !x.Done);
            IEnumerable<PowerPointBacklogItem> screenshotItems = _backlogItems.Where(x => x.solo);
            IEnumerable<PowerPointBacklogItem> tableitems = _backlogItems.Where(x => (x.Done && !x.solo));
            IEnumerable<IEnumerable<BacklogItem>> slideItems = DivvyUp(tableitems, 3);
            
            foreach (BacklogItem backlogItem in  screenshotItems)
            {

            }

            int pos = screenshotItems.Count() + 4;
            foreach(IEnumerable<BacklogItem> slides in slideItems)
            {
                Slide tableSlide = TableSlideClone(pos);
                PopulateTableSide(slides, tableSlide);
                pos++;
            }


            foreach (BacklogItem backlogItem in inProgressItems)
            {

            }
        }

        public void SaveIt(string path)
        {
            _presentationDocument.SaveAs(path);
        }
    }
}


