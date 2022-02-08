using Intergraph.PersonalISOGEN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AttributeTestWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            string metaname = args[0];
            string systemXML = args[1];

            string mandir = @"C:\git\AliasPaul\AttributeTestWriter\AttributeTestWriter\bin\Debug";
            IsogenAssemblyLoader ial = new IsogenAssemblyLoader("PODGraphics.exe.manifest",mandir,mandir,true);
            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(ial))
            {
                Go(metaname, systemXML);
            }

        }
       
        static void Go(string metaname,string systemXML)
        {
            AttributeManager attribteManager = new AttributeManager();
            attribteManager.LoadSystemTemplate(systemXML);
            attribteManager.CreationMode = eCreationMode.eCMAll;

            Attributes attributes = attribteManager.CreateAttributes(metaname);

            AttributeTargetFunction attributeTargetFunction = new AttributeTargetFunction(metaname,attributes);
            string thefunction = attributeTargetFunction.TransformText();
            File.WriteAllText("TargetFunction.txt", thefunction);
        }

    }
}
