using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mac65
{
    public class LabelStore
    {
        private readonly Dictionary<string, Label> labels = new Dictionary<string, Label>();
        private readonly Dictionary<string, ISet<int>> referencedLabels = new Dictionary<string, ISet<int>>();

        public void AddLabel(Label label)
        {
            labels[label.Name] = label;
        }

        public bool TryGetLabel(string name, out Label label)
        {
            return labels.TryGetValue(name, out label);
        }

        public void AddReferencedLabel(string labelName, int line)
        {
            ISet<int> referencedOnLines;
            if (!referencedLabels.TryGetValue(labelName, out referencedOnLines))
            {
                referencedOnLines = new HashSet<int>();
                referencedLabels[labelName] = referencedOnLines;
            }

            referencedOnLines.Add(line);
        }

        public bool TryGetReferencingLines(string labelName, out ISet<int> referencingLines)
        {
            return referencedLabels.TryGetValue(labelName, out referencingLines);
        }

        public Label[] ToArray()
        {
            return labels.Values.ToArray();
        }

        public void ClearReferencedLabels()
        {
            referencedLabels.Clear();
        }
    }
}
