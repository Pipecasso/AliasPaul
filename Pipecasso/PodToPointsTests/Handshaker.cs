using System;
using System.Text;
using System.Runtime.InteropServices;

namespace PodToPointsTests
{
	internal class Scrambler
	{
		static public string Scramble(string staticKey,string devId, string compId)
		{
			int i,j;
			string handshake;
			
			string strKeyLen;
			string strDevLen;
			string strCompLen;
			
			int[] ring = new int[251];
			for (i = 0;i<251;i++)
			{
				ring[i] = i;
			}

			strKeyLen = String.Format("{0:x2}", staticKey.Length);
			strDevLen = String.Format("{0:x2}", devId.Length);
			strCompLen = String.Format("{0:x2}",compId.Length);

			handshake = strKeyLen;
			handshake = handshake + strDevLen;
			handshake = handshake + strCompLen;
			handshake = handshake + staticKey + devId + compId;

			System.Random randomNumbers = new System.Random(unchecked((int)DateTime.Now.Ticks)); 
			
			while (handshake.Length < 251)
			{
				int r;
				char c;

				r = randomNumbers.Next(62);
				if (r >= 0 && r < 10)
				{
					r = r + 48;
				}
				else if ( r>=10 && r< 36)
				{
					r = r + 55;
				}
				else
				{
					r = r + 61;
				}
				
				c = (char)r;
				handshake = handshake + c;
			}

			int iMult = randomNumbers.Next(2,250);
			int iAdd = randomNumbers.Next(1,250);

			for (j = 0;j < 251;j++)
			{
				long y;
				long z;
				y = ((j + iAdd) * iMult);
				z = y % 251;
				ring[j] = (int)z;
			}

			string scramblehandshake = "";
			
			for (j=0;j < 251;j++)
			{
				char cs = handshake[ring[j]];
				scramblehandshake = scramblehandshake + cs;
			}

			string strMult = String.Format("{0:x2}",iMult);
			string strAdd = String.Format("{0:x2}",iAdd);
			scramblehandshake = scramblehandshake + strMult + strAdd;
			return scramblehandshake;
		}
	}

	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Handshaker
	{
		private static readonly string _strDevId = "AliasDev";

        //POD expires on 14/02/2037

        private static readonly string _strPodStaticKey = "V03O64AsAFBDn08DlAD932LeiAA3P31va8";

		public Handshaker()
		{
		}

		public static AliasPOD.POD CreatePOD()
		{
			AliasPOD.POD pod = new AliasPOD.PODClass();
			StringBuilder handshake = new StringBuilder(1000);
			string scrambled = Scrambler.Scramble(_strPodStaticKey, _strDevId, "POD");

			Unscramble(scrambled, handshake);
			pod.Handshake = handshake.ToString();

			return pod;
		}

		public static AliasPOD.POD LoadPOD(string strPath)
		{
			AliasPOD.POD pod = CreatePOD();
			pod.Load(strPath);
			return pod;
		}
#if X64
        [DllImport("BWFC.dll", CharSet = CharSet.Auto, EntryPoint = "Unscramble")]
#else
		[DllImport("BWFC.dll", CharSet = CharSet.Auto,EntryPoint="_Unscramble@8")]
#endif
        private static extern long Unscramble([MarshalAs(UnmanagedType.LPStr)] string scrambled,[MarshalAs(UnmanagedType.LPStr)] StringBuilder handshake);
	}

	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Utility
	{
		public static void MergePipes(AliasPOD.POD pod)
		{
			InternalMergePipes(Marshal.GetIUnknownForObject(pod));
		}

		[DllImport("PipeMerger.dll", CharSet = CharSet.Auto,EntryPoint="MergePipes")]
		private static extern void InternalMergePipes(System.IntPtr pPOD);
	}
}
