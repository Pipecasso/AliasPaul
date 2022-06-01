using GeoFilter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoTests
{
    [TestClass]
    public class ChunkTests
    {
        [TestMethod]
        public void TestConstructor()
        {
            ChunkyIntList chint = new ChunkyIntList(0, 100, 7);
            Dictionary<ChunkyIntList.Chunk, int> chunks = chint.Chunks;
            Assert.AreEqual(7, chunks.Count);
            ChunkyIntList.Chunk chunk1 = new ChunkyIntList.Chunk(0, 15);
            ChunkyIntList.Chunk chunk2 = new ChunkyIntList.Chunk(15, 30);
            ChunkyIntList.Chunk chunk3 = new ChunkyIntList.Chunk(30, 44);
            ChunkyIntList.Chunk chunk4 = new ChunkyIntList.Chunk(44, 58);
            ChunkyIntList.Chunk chunk5 = new ChunkyIntList.Chunk(58, 72);
            ChunkyIntList.Chunk chunk6 = new ChunkyIntList.Chunk(72, 86);
            ChunkyIntList.Chunk chunk7 = new ChunkyIntList.Chunk(86, 101);
            Assert.IsTrue(chunks.ContainsKey(chunk1));
            Assert.IsTrue(chunks.ContainsKey(chunk2));
            Assert.IsTrue(chunks.ContainsKey(chunk3));
            Assert.IsTrue(chunks.ContainsKey(chunk4));
            Assert.IsTrue(chunks.ContainsKey(chunk5));
            Assert.IsTrue(chunks.ContainsKey(chunk6));
            Assert.IsTrue(chunks.ContainsKey(chunk7));
           
        }

        [TestMethod]
        public void AddTest()
        {
            ChunkyIntList chint = new ChunkyIntList(0, 100, 7);
            Dictionary<ChunkyIntList.Chunk, int> chunks = chint.Chunks;
            chint.Add(60);
            chint.Add(15);
            chint.Add(17);
            chint.Add(18);
            chint.Add(0);
            ChunkyIntList.Chunk chunk1 = new ChunkyIntList.Chunk(0, 15);
            ChunkyIntList.Chunk chunk2 = new ChunkyIntList.Chunk(15, 30);
            ChunkyIntList.Chunk chunk5 = new ChunkyIntList.Chunk(58, 72);
            Dictionary<ChunkyIntList.Chunk, int> targets = new Dictionary<ChunkyIntList.Chunk, int>();
            targets.Add(chunk1, 1);
            targets.Add(chunk2, 3);
            targets.Add(chunk5, 1);
            foreach (ChunkyIntList.Chunk chunk in chunks.Keys)
            {
                int target;
                if (chunks.ContainsKey(chunk))
                {
                    target = chunks[chunk];
                }
                else { target = 0; }
                Assert.AreEqual(target, chunks[chunk]);
            }
        }

    
    }
}
