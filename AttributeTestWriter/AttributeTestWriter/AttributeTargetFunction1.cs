using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPI= Intergraph.PersonalISOGEN;

namespace AttributeTestWriter
{
    public partial class AttributeTargetFunction
    {
        private string _name;
        private List<string> _attributeNames;

        public AttributeTargetFunction(string name,IPI.Attributes attributes)
        {
            _name = name;
            _attributeNames = new List<string>();
            foreach (IPI.Attribute attribute in attributes)
            {
                _attributeNames.Add(attribute.Name);
            }
        }
           


      
    
    
    }
}
