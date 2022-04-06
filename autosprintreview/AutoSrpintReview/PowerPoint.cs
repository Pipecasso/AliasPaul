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
        private SlidePart _burndownSlide;


        public string TeamName { get; set; }
        public string TeamDescription { get; set; }
        public string LogoPath { get; set; }
        public DateTime Date { get; set; }
        public DateTime NextDate { get; set; }
        public string BurnPath { get; set; }    

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
            Func<PowerPoint, string> SRNextDate = x => x.NextDate.ToLongDateString();
            Func<PowerPoint, string> Iteration = x => x.Iteration;
            Func<PowerPoint, string> EstimatedPoints = x => x._backlogItems.OriginalPoints().ToString();
            Func<PowerPoint, string> ActualVelocity = x => x._backlogItems.DonePoints().ToString();
            Func<PowerPoint, string> Support = x => x._backlogItems.Where(y => y.Support).Sum(z => z.Points).ToString();
            Func<PowerPoint, string> ScopeCreep = x => x._backlogItems.AdditionalItems().ToString();
            Func<PowerPoint, string> NotDone = x => x._backlogItems.NotDonePoints().ToString();
            
         
            _replacemap.Add("SR.TeamName", TeamNameF);
            _replacemap.Add("SR.TeamDescription", TeamDescriptionF);
            _replacemap.Add("SR.Date", SRDate);
            _replacemap.Add("SR.Iteration", Iteration);
            _replacemap.Add("SR.NextDate", SRNextDate);
            _replacemap.Add("SR.EP", EstimatedPoints);
            _replacemap.Add("SR.AV", ActualVelocity);
            _replacemap.Add("SR.SU", Support);
            _replacemap.Add("SR.SC", ScopeCreep);
            _replacemap.Add("SR.IP", NotDone);

            StringValue teamSlideID = _presentationPart.Presentation.SlideIdList.ChildElements.Select(x => (x as SlideId).RelationshipId).Where(y => y == "rId3").First();
            StringValue goalSlideID = _presentationPart.Presentation.SlideIdList.ChildElements.Select(x => (x as SlideId).RelationshipId).Where(y => y == "rId4").First();
            StringValue demoSlideID = _presentationPart.Presentation.SlideIdList.ChildElements.Select(x => (x as SlideId).RelationshipId).Where(y => y == "rId5").First();
            StringValue tableSideID = _presentationPart.Presentation.SlideIdList.ChildElements.Select(x => (x as SlideId).RelationshipId).Where(y => y == "rId6").First();
            StringValue burnSlideID = _presentationPart.Presentation.SlideIdList.ChildElements.Select(x => (x as SlideId).RelationshipId).Where(y => y == "rId7").First();

            _teamSlide = (SlidePart)_presentationPart.GetPartById(teamSlideID);
            _goalSlide = (SlidePart)_presentationPart.GetPartById(goalSlideID);
            _demoSplide = (SlidePart)_presentationPart.GetPartById(demoSlideID);
            _firstTableSlide = (SlidePart)_presentationPart.GetPartById(tableSideID);
            _burndownSlide = (SlidePart)_presentationPart.GetPartById(burnSlideID);
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
            string ext = System.IO.Path.GetExtension(path);
            ImagePart LogoPart = slidepart.AddImagePart(ext == ".png" ? ImagePartType.Png : ImagePartType.Jpeg );
            using (FileStream stream = File.OpenRead(path))
            {
                LogoPart.FeedData(stream);
            }

            ShapeTree shapeTree = slidepart.Slide.Descendants<DocumentFormat.OpenXml.Presentation.ShapeTree>().First();

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


        private enum ColumnType { unset, id, description, points,work_item_type }

        private Run AddColumn(BacklogItem backlogItem,ColumnType columnType,TableRow tableRow,Slide slide,TableCell benchmark)
        {
            TableCell benchclone = (TableCell)benchmark.Clone();

            Run myRun = benchclone.Descendants<D.TextBody>().First().Descendants<Paragraph>().First().Descendants<Run>().First();
            D.Text runtext = myRun.Descendants<D.Text>().First();
        
            string textval = string.Empty;
            switch (columnType)
            {
                case ColumnType.id:textval = backlogItem.ID;break;
                case ColumnType.description:textval = backlogItem.Title;break;
                case ColumnType.points:textval = backlogItem.Points.ToString();break;
                case ColumnType.work_item_type:textval = backlogItem.WorkItemType == BacklogItem.workitemtype.bug ? "Bug" : "Product Backlog Item";break;
            }

            runtext.Text = textval;

            if (columnType == ColumnType.id)
            {
                HyperlinkOnClick hyperlinkOnClick = myRun.RunProperties.Descendants<HyperlinkOnClick>().First();
                MakeHyperLink(hyperlinkOnClick, slide, textval);
            }
          
            tableRow.Append(benchclone);
            return myRun;
        }

        private void MakeHyperLink(HyperlinkOnClick hyperlinkOnClick,Slide slide,string textval)
        {
            string hyperlikid = $"rId{slide.SlidePart.HyperlinkRelationships.Count() + 1}";
            hyperlinkOnClick.Id = hyperlikid;
            string newuri = $"{_linkbase}/{textval}";
            Uri uriID = new Uri(newuri, UriKind.Absolute);
            slide.SlidePart.AddHyperlinkRelationship(uriID, true, hyperlikid);
        }

        private void RepopulateRow(Slide slide,BacklogItem backlogItem)
        {
            Table table = slide.Descendants<Table>().First();
            TableRow tableRowLast = slide.Descendants<TableRow>().Last();
            TableCell[] cells = tableRowLast.Descendants<TableCell>().ToArray();

            Run runWIT = cells[0].Descendants<D.TextBody>().First().Descendants<Paragraph>().First().Descendants<Run>().First();
            D.Text wit = runWIT.Descendants<D.Text>().First();
            wit.Text = backlogItem.WorkItemType == BacklogItem.workitemtype.bug ? "Bug" : "Product Backlog Item";

            Run runlink = cells[1].Descendants<D.TextBody>().First().Descendants<Paragraph>().First().Descendants<Run>().First();
            D.Text id = runlink.Descendants<D.Text>().First();
            id.Text = backlogItem.ID;
            HyperlinkOnClick hyperlinkOnClick = runlink.RunProperties.Descendants<HyperlinkOnClick>().First();

            string hyperlikid = $"rId{slide.SlidePart.HyperlinkRelationships.Count() + 1}";
            hyperlinkOnClick.Id = hyperlikid;
            string newuri = $"{_linkbase}/{backlogItem.ID}";
            Uri uriID = new Uri(newuri, UriKind.Absolute);
            slide.SlidePart.AddHyperlinkRelationship(uriID, true, hyperlikid);

            Run runtitle = cells[2].Descendants<D.TextBody>().First().Descendants<Paragraph>().First().Descendants<Run>().First();
            D.Text title = runtitle.Descendants<D.Text>().First();
            title.Text = backlogItem.Title;

            Run runpoints = cells[3].Descendants<D.TextBody>().First().Descendants<Paragraph>().First().Descendants<Run>().First();
            if (backlogItem.AddedDuringSprint)
            {
                AddColourToRun(runpoints, (HexBinaryValue)"DF751D");
            }
            D.Text points = runpoints.Descendants<D.Text>().First();
            points.Text = backlogItem.Points.ToString();
           
        }

        private void AddColourToRun(Run r,HexBinaryValue hexBinaryValue)
        {
            RunProperties runProperties = r.Descendants<RunProperties>().FirstOrDefault();
            if (runProperties != null)
            {
                SolidFill solidFill = runProperties.Descendants<SolidFill>().FirstOrDefault();
                RgbColorModelHex rgbColorModelHex = new RgbColorModelHex();
                rgbColorModelHex.Val = hexBinaryValue;
                solidFill.RgbColorModelHex = rgbColorModelHex;
            }
        }

        private void PopulateImageSlide(Slide slide, PowerPointBacklogItem powerPointBacklogItem)
        {
            RepopulateRow(slide, powerPointBacklogItem);

            string[] imagefilesjpg = Directory.GetFiles(powerPointBacklogItem.ImagePath, "*.jpg");
            string[] imagefilespng = Directory.GetFiles(powerPointBacklogItem.ImagePath, "*.png");
            IEnumerable<string> imagefiles = imagefilesjpg.Concat(imagefilespng);

            string ext = System.IO.Path.GetExtension(imagefiles.First());
            ImagePartType ipt = ext == ".jpg" ? ImagePartType.Jpeg : ImagePartType.Png;
            AddPicture(imagefiles.First(), slide.SlidePart, powerPointBacklogItem.ID); 
        }

        private void PopulateTableSide(IEnumerable<BacklogItem> backlogItems, Slide slide)
        {
            Table table = slide.Descendants<Table>().First();
            TableRow tableRowLast = slide.Descendants<TableRow>().Last();
            bool first = true;
            IEnumerable<TableCell> tableCells = tableRowLast.Descendants<TableCell>();
            TableCell[] tableCellBenchmarks = tableCells.ToArray();
            foreach (BacklogItem backlogItem in backlogItems)
            {
                if (first)
                {
                    RepopulateRow(slide, backlogItem);
                    first = false;
                }
                else
                {
                   TableRow tableRow = new TableRow();
                    tableRow.Height = tableRowLast.Height;
                    
                    AddColumn(backlogItem, ColumnType.work_item_type, tableRow, slide, tableCellBenchmarks[0]);
                    AddColumn(backlogItem, ColumnType.id, tableRow,slide, tableCellBenchmarks[1]);
                    AddColumn(backlogItem, ColumnType.description, tableRow,slide, tableCellBenchmarks[2]);
                    Run point_run = AddColumn(backlogItem, ColumnType.points, tableRow,slide, tableCellBenchmarks[3]);
                    if (backlogItem.AddedDuringSprint)
                    {
                        AddColourToRun(point_run, (HexBinaryValue)"DF751D");
                    }
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
                    if (!allslidessame && slidenum == totalslides)
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
            //replace
            Substitute();

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

            int pos = 4;
            foreach(IEnumerable<BacklogItem> slides in slideItems)
            {
                Slide tableSlide = TableSlideClone(pos++);
                PopulateTableSide(slides, tableSlide);
            }

            foreach(PowerPointBacklogItem powerPointBacklogItem in screenshotItems)
            {
                Slide imageSlide = TableSlideClone(pos++);
                PopulateImageSlide(imageSlide,powerPointBacklogItem);
            }

            if (inProgressItems.Any())
            {
                Slide notdoneslide = TableSlideClone(pos++);
                PopulateTableSide(inProgressItems,notdoneslide);
                AppendSlideTitle(notdoneslide," - InProgress");
            }

            AddPicture(BurnPath, _burndownSlide, "BurnDown");

            //remove table slide
            SlideId tableId = GetSlideId(_firstTableSlide.Slide);
            _presentationPart.Presentation.SlideIdList.RemoveChild(tableId);
            _presentationPart.DeletePart(_firstTableSlide);

        }

        private void AppendSlideTitle(Slide slide,string appendage)
        {
            foreach (Paragraph p in slide.Descendants<Paragraph>())
            {
                D.Text tntext = p.Descendants<D.Text>().Where(x => x.Text.Contains(TeamName)).FirstOrDefault();
                if (tntext!=null)
                {
                    string appended = $"{tntext.Text}{appendage}";
                    tntext.Text = appended;
                    break;
                }
            }
        }


        private void Substitute()
        {
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
            Dictionary<string, List<D.Text>> replacements = new Dictionary<string, List<D.Text>>();
            Dictionary<int, D.Text> strindex = new Dictionary<int, D.Text>();
            int tick = 0;
           
            foreach (D.Text text in texts)
            {
                bigpowerpointstring += text.Text;
                int tock = tick + text.Text.Length;
                for (int i = tick; i < tock; i++)
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
                if (index > -1) BracketList.Add(strindex[index]);

            } while (index > -1);

            foreach (KeyValuePair<string, List<D.Text>> kvp in replacements)
            {
                string final = subit(kvp.Key);
                foreach (D.Text text in kvp.Value)
                {
                    text.Text = text.Text.Replace(kvp.Key, final);
                }
            }

            foreach (D.Text text in BracketList)
            {
                text.Text = text.Text.Replace("[", string.Empty);
                text.Text = text.Text.Replace("]", string.Empty);
            }
        }

        private SlideId GetSlideId(Slide slide)
        {
            SlideId togo = null;
            foreach (SlideId slideId in this._presentationPart.Presentation.SlideIdList)
            {
                SlidePart candidate = (SlidePart)_presentationPart.GetPartById(slideId.RelationshipId.Value);
                if (candidate.Slide == slide)
                {
                    togo = slideId;
                    break;
                }
            }
            return togo;
        }

        public void SaveIt(string path)
        {
            _presentationDocument.SaveAs(path);
        }

       
    }
}


