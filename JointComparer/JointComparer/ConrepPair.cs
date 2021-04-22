using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JointComparer
{
    public class ConrepPair
    {
        private Dictionary<Tuple<JointRun, JointRun>, JointRunComparison> _Matchups;
        string Name { get; }

        public ConrepPair(string name,List<JointRun> referencejoints,List<JointRun> currentjoints)
        {
            Name = name;
            _Matchups = new Dictionary<Tuple<JointRun, JointRun>, JointRunComparison>();
            int tick = 0;
            foreach (JointRun jrref in referencejoints)
            {
                JointRun jrrcur = currentjoints[tick];
                JointRunComparison joint_compare = new JointRunComparison(jrref,jrrcur);
                _Matchups.Add(new Tuple<JointRun, JointRun>(jrref, jrrcur), joint_compare);
                tick++;
            }   
        }

        void Report()
        {
            Dictionary<string, int> _JointDiffs = new Dictionary<string, int>();

            foreach(KeyValuePair<Tuple<JointRun, JointRun>, JointRunComparison> kvp in _Matchups)
            {
                string name = kvp.Key.Item1.Name;
                _JointDiffs.Add(name, kvp.Value.Count);

                JointRunComparison jrc = kvp.Value;

            }



        }
    
    
    }
}
