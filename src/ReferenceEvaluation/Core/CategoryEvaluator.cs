using SwissAcademic.Addons.ReferenceEvaluation.Properties;
using SwissAcademic.Citavi.Shell;
using System.Linq;

namespace SwissAcademic.Addons.ReferenceEvaluation
{
    internal class CategoryEvaluator : BaseEvaluator
    {
        #region Properties

        public override string Caption => ReferenceEvaluationResources.CategoryEvaluator_Caption;

        #endregion

        #region Methods

        public override string Run(MainForm mainForm)
        {
            _stringBuilder.Clear();

            var categories = mainForm.Project.AllCategories.ToList();

            if (categories.Count == 0)
            {
                _stringBuilder.AppendLine(ReferenceEvaluationResources.CategoryEvaluator_NoCategories);
                return _stringBuilder.ToString();
            }

            categories.Sort((x, y) => x.References.Count.CompareTo(y.References.Count) * (-1));

            var maxLength = categories.Max(category => category.FullName.Length);

            if (ShowHeader)
            {
                var headers = ReferenceEvaluationResources.Evaluator_Name
                              + ' '.Repeat(maxLength - ReferenceEvaluationResources.Evaluator_Name.Length + 10)
                              + ReferenceEvaluationResources.Evaluator_Count;
                _stringBuilder.AppendLine(headers);
                _stringBuilder.AppendLine();
            }

            foreach (var category in categories)
            {
                _stringBuilder.AppendLine(category.FullName + ' '.Repeat(maxLength - category.FullName.Length + 10) + category.References.Count);
            }

            return _stringBuilder.ToString();
        }

        #endregion
    }
}
