using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JointComparer
{
    public class ConrepPair
    {
        private List<string> _UnbalancedJointRuns; //differing numbers of joints
        private Dictionary<Tuple<JointRun, JointRun>, JointRunComparison> _Matchups;  //each joint run paired to the differences  between them.
        private Dictionary<string, JointRunDifferenceTotals> _DiffTotals; //a each joint run matched to a summary of the joint diffs
        Dictionary<string, int> _JointDiffs; //the total number of differing joints per run
        public string Name { get; }

        public ConrepPair(string name,List<JointRun> referencejoints,List<JointRun> currentjoints)
        {
            Name = name;
            _Matchups = new Dictionary<Tuple<JointRun, JointRun>, JointRunComparison>();
            _DiffTotals = new Dictionary<string, JointRunDifferenceTotals>();
            _JointDiffs = new Dictionary<string, int>();
            _UnbalancedJointRuns = new List<string>();
            int tick = 0;
            foreach (JointRun jrref in referencejoints)
            {
                JointRun jrrcur = currentjoints[tick];
                JointRunComparison joint_compare = new JointRunComparison(jrref,jrrcur);
                _Matchups.Add(new Tuple<JointRun, JointRun>(jrref, jrrcur), joint_compare);
                tick++;
            }   
        }

        public void Report()
        {
            foreach(KeyValuePair<Tuple<JointRun, JointRun>, JointRunComparison> kvp in _Matchups)
            {
                string run_name = kvp.Key.Item1.Name;
                _JointDiffs.Add(run_name, kvp.Value.Count);

                JointRunComparison jrc = kvp.Value;
                if (jrc.SameSize==false)
                {
                    _UnbalancedJointRuns.Add(run_name);
                    continue;
                }


                JointRunDifferenceTotals runDifferenceTotals = new JointRunDifferenceTotals();
                foreach (KeyValuePair<JointPair,JointComparison> kvp2 in jrc)
                {
                    JointComparison jointComparison = kvp2.Value;
                 
                    if (jointComparison.KeypointA == false) runDifferenceTotals.KeypointAInc();
                    if (jointComparison.KeypointB == false) runDifferenceTotals.KeypointBInc();
                    if (jointComparison.Type == false)
                    {
                        runDifferenceTotals.TypeInc();
                        if (kvp2.Key.Item1.Type == AliasPOD.eConnection.eCUnknown)
                        {
                            runDifferenceTotals.TypeImprovementInc();
                        }
                    }
                    if (jointComparison.ConnectorCount == false) runDifferenceTotals.ConnectorCountInc();
                }
                  _DiffTotals.Add(run_name,runDifferenceTotals);
            }
        }

        public void WriteReport(TextWriter stream)
        {
            int total = _JointDiffs.Values.Sum();
            JointRunDifferenceTotals summary = Summary();
            stream.WriteLine($"\t{total} differing joints\n");
            stream.WriteLine($"\t\t{summary.KeypointA} are Keypoint B");
            stream.WriteLine($"\t\t{summary.KeypointB} are Keypoint B");
            stream.WriteLine($"\t\t{summary.Type}      are Keypoint Type");
            stream.WriteLine($"\t\t{summary.TypeImprovement} are Keypoint Type+");
            stream.WriteLine($"\t\t{summary.ConnectorCount} are Keypoint Connectors");

            stream.WriteLine($"\t{_UnbalancedJointRuns.Count} unbalanced joint runs\n");
           

            foreach (KeyValuePair<string, JointRunDifferenceTotals> kvp in _DiffTotals)
            {
                string joint_runname = kvp.Key;
                JointRunDifferenceTotals jointdiffs = kvp.Value;
                if (jointdiffs.Identical == false)
                {
                    stream.WriteLine($"\t\t{joint_runname} ");
                    stream.WriteLine($"\t\t\t KeypointA  {jointdiffs.KeypointA}");
                    stream.WriteLine($"\t\t\t KeypointB  {jointdiffs.KeypointB}");
                    stream.WriteLine($"\t\t\t Type       {jointdiffs.Type}");
                    stream.WriteLine($"\t\t\t Type+      {jointdiffs.TypeImprovement}");
                    stream.WriteLine($"\t\t\t Connectors {jointdiffs.ConnectorCount}\n");
                }
            }

            stream.WriteLine("\t Unbalanced Joints");
            foreach (string s in _UnbalancedJointRuns)
            {
                stream.WriteLine($"\t {s}");
            }

            stream.WriteLine("\n");
        }

        public JointRunDifferenceTotals Summary()
        {
            JointRunDifferenceTotals jrdt_summary = new JointRunDifferenceTotals();
            foreach(JointRunDifferenceTotals jrdt in _DiffTotals.Values)
            {
                jrdt_summary += jrdt;
            }
            return jrdt_summary;
        }

        public int UnbalancedJoints { get => _UnbalancedJointRuns.Count; }



    }
}
