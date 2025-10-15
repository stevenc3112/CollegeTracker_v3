using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SClarkC971PA.Models
{
    internal class Performance : Assessment
    {
        public Performance() : base()
        { }
        public string PAssessmentFeedback { get {return _pAssessmentFeedback;} set{ _pAssessmentFeedback = value; } }
        private string _pAssessmentFeedback;
    }
}
