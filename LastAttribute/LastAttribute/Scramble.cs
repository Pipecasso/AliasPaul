using System;

namespace PodHandshake
{
	/// <summary>
	/// Summary description for Scramble.
	/// </summary>
	public class Scrambler
	{
		static public string Scramble(string StaticKey,string DevId, string CompId)
		{
			int i,j;
			string Handshake;
			
			string strKeyLen;
			string strDevLen;
			string strCompLen;
			
			int[] Ring = new int[251];
			for (i = 0;i<251;i++)
			{
				Ring[i] = i;
			}

			strKeyLen = String.Format("{0:x2}", StaticKey.Length);
			strDevLen = String.Format("{0:x2}", DevId.Length);
			strCompLen = String.Format("{0:x2}",CompId.Length);

			Handshake = strKeyLen;
			Handshake = Handshake + strDevLen;
			Handshake = Handshake + strCompLen;
			Handshake = Handshake + StaticKey + DevId + CompId;

			System.Random RandomNumbers = new System.Random(unchecked((int)DateTime.Now.Ticks)); 
			
			while (Handshake.Length < 251)
			{
				int r;
				char c;

				r = RandomNumbers.Next(62);
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
				Handshake = Handshake + c;
			}

			int iMult = RandomNumbers.Next(2,250);
			int iAdd = RandomNumbers.Next(1,250);

			for (j = 0;j < 251;j++)
			{
				long y;
				long z;
				y = ((j + iAdd) * iMult);
				z = y % 251;
				Ring[j] = (int)z;
			}

			string scramblehandshake = "";
			
			for (j=0;j < 251;j++)
			{
				char cs = Handshake[Ring[j]];
				scramblehandshake = scramblehandshake + cs;
			}

			string strMult = String.Format("{0:x2}",iMult);
			string strAdd = String.Format("{0:x2}",iAdd);
			scramblehandshake = scramblehandshake + strMult + strAdd;
			return scramblehandshake;
		}
	}
}
