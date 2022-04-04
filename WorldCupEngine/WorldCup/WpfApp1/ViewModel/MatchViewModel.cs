using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorldCupEngine;

namespace WpfApp1.ViewModel
{
    public class MatchViewModel
    {
        private Match _match;

        public MatchViewModel(Match match)
        {
            _match = match;
        }

        public string Contestent1 { get => _match==null ? string.Empty : _match.Item1.Name; }
        public string Contestent2 { get => _match==null ? string.Empty : _match.Item2.Name; }

        public bool NotPlayed { get => _match.result == Match.Result.notplayed; }

    }
}
