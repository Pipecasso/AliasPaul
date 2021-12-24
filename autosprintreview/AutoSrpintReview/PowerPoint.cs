using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using P = DocumentFormat.OpenXml.Presentation;
using D = DocumentFormat.OpenXml.Drawing;


namespace AutoSrpintReview
{
    public class PowerPoint : IDisposable
    {
        private List<string> _Aims;
        private List<string> _Demonstrations;
        private BacklogItems _backlogItems;
        private PresentationPart _presentationPart;
        private PresentationDocument _presentationDocument;

        public string TeamName { get; set; }
        public string Descriptions { get; set; }
        public string LogoPath { get; set; }
        public DateTime Date { get; set; }
        public string ScreenshotsPath { get; set; }
        public string Iteration { get; set; }

        public PowerPoint(BacklogItems backlogItems, string templatepath)
        {
            _backlogItems = backlogItems;
            _Demonstrations = new List<string>();
            _Aims = new List<string>();
            _presentationDocument = PresentationDocument.Open(templatepath, true);
            _presentationPart = _presentationDocument.PresentationPart;
        }

        public void Dispose()
        {
            _presentationPart = null;
            _presentationDocument.Dispose();
        }

        public void MakeIt()
        {
            IEnumerable<D.Text> texts = new List<D.Text>();
            Presentation presentation = _presentationPart.Presentation;
            OpenXmlElementList slideIds = presentation.SlideIdList.ChildElements;
            foreach (OpenXmlElement openXmlElement in slideIds)
            {
                string slidePartRelationshipId = (openXmlElement as SlideId).RelationshipId;
                SlidePart slideTitlePart = (SlidePart)_presentationPart.GetPartById(slidePartRelationshipId);
                IEnumerable<Paragraph> paragraphs = slideTitlePart.Slide.Descendants<Paragraph>();
                foreach (Paragraph paragraph in paragraphs)
                { 
                    IEnumerable<D.Text> temptexts = texts.Concat(paragraph.Descendants<D.Text>());
                    texts = temptexts;
                }
            }

            D.Text teamname = texts.Where(x => x.InnerText.Contains("[TeamName]")).FirstOrDefault();
           // teamname.Text = teamname.Text.Replace("[Team Name]", TeamName);


        }


     

       

    }
}


