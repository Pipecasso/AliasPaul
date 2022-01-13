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
        private List<string> _Aims;
        private List<string> _Demonstrations;
        private BacklogItems _backlogItems;
        private string _iteration;
        private PresentationPart _presentationPart;
        private PresentationDocument _presentationDocument;
        private enum makeitstatus { neutral, open };
        private SlidePart _teamSlide;

        public string TeamName { get; set; }
        public string TeamDescription { get; set; }
        public string LogoPath { get; set; }
        public DateTime Date { get; set; }
        public string ScreenshotsPath { get; set; }
        public string Iteration { get => _iteration; }
        public Dictionary<string, Func<PowerPoint,string>> _replacemap;
 

        public PowerPoint(BacklogItems backlogItems, string templatepath,string outputdir,string itearion)
        {
       
            _backlogItems = backlogItems;
            _Demonstrations = new List<string>();
            _Aims = new List<string>();

            string year = DateTime.Now.Year.ToString();
            _iteration = $"{year}_{itearion}";
            string outputfilename = $"{TeamName} Sprint Review {_iteration}.pptx";
            string outputpath = System.IO.Path.Combine(outputdir,year,_iteration, outputfilename);
            string dir = System.IO.Path.GetDirectoryName(outputpath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            if (File.Exists(outputpath)) File.Delete(outputpath);
            File.Copy(templatepath, outputpath);

            _presentationDocument = PresentationDocument.Open(outputpath, true);
            _presentationPart = _presentationDocument.PresentationPart;

            _replacemap = new Dictionary<string, Func<PowerPoint,string>>();
            Func<PowerPoint, string> TeamNameF = x => x.TeamName;
            Func<PowerPoint, string> TeamDescriptionF = x => x.TeamDescription;
            Func<PowerPoint, string> SRDate = x => x.Date.ToLongDateString();
            Func<PowerPoint, string> Iteration = x => x.Iteration;
            _replacemap.Add("SR.TeamName", TeamNameF);
            _replacemap.Add("SR.TeamDescription", TeamDescriptionF);
            _replacemap.Add("SR.Date", SRDate);
            _replacemap.Add("SR.Iteration", Iteration);

            StringValue teamSlideID = _presentationPart.Presentation.SlideIdList.ChildElements.Select(x => (x as SlideId).RelationshipId).Where(y => y == "rId3").First();
            _teamSlide = (SlidePart)_presentationPart.GetPartById(teamSlideID);


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

                IEnumerable<IdPartPair> parts = slideTitlePart.Parts;
                IEnumerable<OpenXmlPart> imageparts = parts.Select(x => x.OpenXmlPart).Where(y => y.GetType() == typeof(ImagePart));
                foreach (OpenXmlPart openXmlPart1 in imageparts)
                {
                    ImagePart imagePart = openXmlPart1 as ImagePart;
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








        }
    }
}


