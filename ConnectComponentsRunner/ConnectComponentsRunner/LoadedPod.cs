using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AliasPOD;
using Intergraph.PersonalISOGEN;
using System.IO;
using PodHandshake;

namespace ManagedPodLoader
{
    public class LoadedPod
    {
        private POD _pod;
        private IsogenAssemblyLoader _ial;

        public LoadedPod(string manifest, string pod_path)
        {
            string manifestroot = Path.GetDirectoryName(manifest);
            _ial = new IsogenAssemblyLoader(manifest, manifestroot, manifestroot,true);
         
            using (IsogenAssemblyLoaderCookie cookieMonster = new IsogenAssemblyLoaderCookie(_ial))
            {
                MakePOD(pod_path, manifestroot); 
            }
        }

        private void MakePOD(string pod_path,string bwfc)
        {
            _pod = new POD();
            _pod.Handshake = PodHandshake.HandshakeTools.GetPODHandshake(bwfc);
            _pod.Load(pod_path);
        }
    
        public POD pod { get => _pod; }
        public IsogenAssemblyLoader isogenAssemblyLoader { get => _ial; }

    }
}
