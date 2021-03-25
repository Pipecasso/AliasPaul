using PodHandshake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PodHandshake
{
    class PODHandshaker
    {
        [DllImport("BWFC.dll", CharSet = CharSet.Auto, EntryPoint = "_Unscramble@8")]
        private static extern long Unscramble([MarshalAs(UnmanagedType.LPStr)] string scrambled, [MarshalAs(UnmanagedType.LPStr)] StringBuilder handshake);

        internal StringBuilder GetHandshake(string staticKey, string devId, string componentId)
        {
            StringBuilder handshake = new StringBuilder(255);

            string scrambled = Scrambler.Scramble(staticKey, devId, componentId);
            Unscramble(scrambled, handshake);

            return handshake;
        }
    }
}
