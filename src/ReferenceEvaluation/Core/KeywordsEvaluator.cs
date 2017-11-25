using SwissAcademic.Addons.ReferenceEvaluation.Properties;
using SwissAcademic.Citavi.Shell;
using System.Linq;

namespace SwissAcademic.Addons.ReferenceEvaluation
{
    internal class KeywordsEvaluator : BaseEvaluator
    {
        #region Properties

        public override string Caption => ReferenceEvaluationResources.KeywordsEvaluator_Caption;

        #endregion

        #region Methods

        public override string Run(MainForm mainForm)
        {
            _stringBuilder.Clear();

            var keywords = mainForm.Project.Keywords.ToList();

            if (keywords.Count == 0)
            {
                _stringBuilder.AppendLine(ReferenceEvaluationResources.KeywordsEvaluator_NoKeywords);
                return _stringBuilder.ToString();
            }

            keywords.Sort((x, y) => x.References.Count.CompareTo(y.References.Count) * (-1));
            var maxLength = keywords.Max(keyword => keyword.FullName.Length);

            if (ShowHeader)
            {
                var headers = ReferenceEvaluationResources.Evaluator_Name
                              + ' '.Repeat(maxLength - ReferenceEvaluationResources.Evaluator_Name.Length + 10)
                              + ReferenceEvaluationResources.Evaluator_Count;
                _stringBuilder.AppendLine(headers);
                _stringBuilder.AppendLine();
            }

            foreach (var keyword in keywords)
            {
                _stringBuilder.AppendLine(keyword.FullName + ' '.Repeat(maxLength - keyword.FullName.Length + 10) + keyword.References.Count);
            }

            return _stringBuilder.ToString();
        }

        #endregion
    }
}