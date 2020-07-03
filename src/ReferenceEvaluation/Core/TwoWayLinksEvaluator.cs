using SwissAcademic.Addons.ReferenceEvaluationAddon.Properties;
using SwissAcademic.Citavi.Shell;
using System;
using System.Linq;

namespace SwissAcademic.Addons.ReferenceEvaluationAddon
{
    internal class TwoWayLinksEvaluator : BaseEvaluator
    {
        // Properties

        public override string Caption => Resources.TwoLinksEvaluator_Caption;

        // Methods

        public override string Run(MainForm mainForm)
        {
            _stringBuilder.Clear();

            var references = mainForm.GetFilteredReferences();
            if (references.Count == 0)
            {
                _stringBuilder.AppendLine(Resources.TwoLinksEvaluator_NoReferences);
            }
            else
            {
                var titleHeader = Resources.TwoLinksEvaluator_ShortTitle;
                var fromHeader = Resources.TwoLinksEvaluator_From;
                var toHeader = Resources.TwoLinksEvaluator_To;

                var statistics = references.GetStatistics();

                var maxTitleHeaderLength = Math.Max(references.Max(reference => reference.ShortTitle.Length), titleHeader.Length);
                var maxFromHeaderLength = Math.Max(statistics.Max(statistic => statistic.FromCount.ToString().Length), fromHeader.Length);


                if (ShowHeader)
                {
                    var headers = titleHeader
                                  + ' '.Repeat(maxTitleHeaderLength - titleHeader.Length + 5)
                                  + fromHeader
                                   + ' '.Repeat(maxFromHeaderLength - fromHeader.Length + 5)
                                  + toHeader;
                    _stringBuilder.AppendLine(headers);
                    _stringBuilder.AppendLine();
                }

                foreach (var statistic in statistics)
                {
                    _stringBuilder.AppendLine(
                        statistic.ShortTitle
                        + ' '.Repeat(maxTitleHeaderLength - statistic.ShortTitle.Length + 5)
                        + statistic.FromCount.ToString()
                        + ' '.Repeat(maxFromHeaderLength - statistic.FromCount.Length + 5)
                        + statistic.ToCount);
                }
            }

            return _stringBuilder.ToString();
        }
    }
}