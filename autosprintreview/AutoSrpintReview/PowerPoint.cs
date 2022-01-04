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
        private enum makeitstatus { neutral, open };

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

        private string subit(string toreplace)
        {
            return "sausage";
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
                IEnumerable<Paragraph> paragraphs = slideTitlePart.Slide.Descendants<Paragraph>();
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

            } while (index > -1);

            foreach (KeyValuePair<string,List<D.Text>> kvp in replacements)
            {
                string final = subit(kvp.Key);
                foreach(D.Text text in kvp.Value)
                {
                    text.Text = text.Text.Replace(kvp.Key, final);
                }
            }
        }



    }
}


