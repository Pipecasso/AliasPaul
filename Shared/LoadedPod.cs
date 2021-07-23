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
    internal class LoadedPod
    {
        private POD _pod;
        private IsogenAssemblyLoader _ial;

        internal LoadedPod(string manifest, string pod_path)
        {
            string manifestroot = Path.GetDirectoryName(manifest);
            _ial = new IsogenAssemblyLoader(manifest, manifestroot);
            string core_components = Path.Combine(manifestroot, "Core Components");
            _ial.AddStringPath(core_components);

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
    
        internal POD pod { get => _pod; }
        internal IsogenAssemblyLoader isogenAssemblyLoader { get => _ial; }

    }
}
