using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliasPOD;
using Intergraph.PersonalISOGEN;


namespace KeypointRunner
{
    class KeypointRunner
    {
        private POD _Pod;
        private KeypointRunItems _KeypointRunItems;
        private IsogenAssemblyLoader _ial;

        public KeypointRunner(POD p,IsogenAssemblyLoader ial)
        {
            _Pod = p;
            _ial = ial;
        }

        public bool Initialise(string iuci,string anchor)
        {
            using (IsogenAssemblyLoaderCookie monster = new IsogenAssemblyLoaderCookie(_ial))
            {
                _KeypointRunItems = null;
               Component component = _Pod.Pipelines.Item(0).Components.Item(iuci);
                ComponentKeypoint anchor_key = component != null ? component.Keypoints.Item(anchor) : null;
                KeypointRunItem anchorkri = new KeypointRunItem(anchor_key);
                if (component == null || anchor_key == null)
                {
                    return false;
                }
                else
                {
                    _KeypointRunItems = new KeypointRunItems(anchorkri);
                    for (int i=0;i<component.Keypoints.Count;i++)
                    {
                        ComponentKeypoint ck = component.Keypoints.Item(i);
                        string sub = ck.Name.Substring(0, 3);
                        if (sub == "End" || sub == "Mid") continue;

                        KeypointRunItem kri = new KeypointRunItem(ck);
                        _KeypointRunItems.Add(kri);
                    }

                    for (int i=0;i<component.ChildCount;i++)
                    {
                        Component child = component.GetChild(i);
                        if (child.Material.ComponentType == "Support")
                        {
                            KeypointRunItem kri = new KeypointRunItem(child.Keypoints.Item(0));
                            _KeypointRunItems.Add(kri);
                        }
                    }
                    return true;
                }
            }
        }

        public KeypointRunItems keypointRunItems { get => _KeypointRunItems; }

       
    }
}
