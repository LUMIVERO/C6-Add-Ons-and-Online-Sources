using SwissAcademic.Addons.ReferenceEvaluation.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using System.Collections.Generic;
using System.Linq;

namespace SwissAcademic.Addons.ReferenceEvaluation
{
    internal class PersonAndSexEvaluator : BaseEvaluator
    {
        #region Properties

        public override string Caption => ReferenceEvaluationResources.PersonAndSexEvaluator_Caption;

        #endregion

        #region Methods

        public override string Run(MainForm mainForm)
        {
            var referencesBySelection = mainForm.GetFilteredReferences();
            var isFiltered = !mainForm.ReferenceEditorFilterSet.IsEmpty;

            _stringBuilder.Clear();

            var entities = mainForm.Project.Persons.Select(person => new EvaluationEntity<Person>(person)).ToList();

            if (entities.Count == 0)
            {
                _stringBuilder.AppendLine(ReferenceEvaluationResources.PersonEvaluator_NoPersons);
                return _stringBuilder.ToString();
            }

            foreach (var entity in entities)
            {
                entity.CountByProject = entity.Entity.References.Count;

                if (!mainForm.ReferenceEditorFilterSet.IsEmpty)
                {
                    entity.CountBySelection = entity.Entity.References.Where(reference => referencesBySelection.Contains(reference)).Count();
                }
            }

            if (isFiltered)
            {
                entities.Sort(new EvaluationEntityComparerBySelectionCount<Person>());
            }
            else
            {
                entities.Sort(new EvaluationEntityComparerByProjectCount<Person>());
            }

            var columnsWidth = CreateColumnsWidth(entities, isFiltered);

            if (ShowHeader)
            {
                var headers = CreateHeader(entities, isFiltered, columnsWidth);
                _stringBuilder.AppendLine(headers);
                _stringBuilder.AppendLine();
            }

            foreach (var entity in entities)
            {
                var person = entity.Entity;

                if (!isFiltered)
                {
                    _stringBuilder.AppendLine(person.FullName + ' '.Repeat(columnsWidth[0] - person.FullName.Length + 10) + getShortSexForm(person.Sex) + ' '.Repeat(ReferenceEvaluationResources.Evaluator_Sex.Length + 9) + entity.CountByProject);
                }
                else
                {
                    _stringBuilder.AppendLine(
                        person.FullName
                        + ' '.Repeat(columnsWidth[0] - person.FullName.Length + 10)
                        + getShortSexForm(person.Sex)
                        + ' '.Repeat(columnsWidth[1] + 9)
                        + entity.CountBySelection
                        + ' '.Repeat(columnsWidth[2] - entity.CountBySelection.ToString().Length + 10)
                        + entity.CountByProject);
                }
            }
            return _stringBuilder.ToString();
        }

        string getShortSexForm(Sex sex)
        {
            return sex.ToString().FirstOrDefault().ToString();
        }

        string CreateHeader(List<EvaluationEntity<Person>> entities, bool isFiltered, List<int> columnsWidth)
        {
            return ReferenceEvaluationResources.Evaluator_Name
                           + ' '.Repeat(columnsWidth[0] - ReferenceEvaluationResources.Evaluator_Name.Length + 10)
                           + ReferenceEvaluationResources.Evaluator_Sex
                           + ' '.Repeat(columnsWidth[1] - ReferenceEvaluationResources.Evaluator_Sex.Length + 10)
                           + (isFiltered
                                 ? ReferenceEvaluationResources.Evaluation_CountBySelection
                                    + ' '.Repeat(columnsWidth[2] - ReferenceEvaluationResources.Evaluation_CountBySelection.Length + 10)
                                    + ReferenceEvaluationResources.Evaluation_CountByProject
                                  : ReferenceEvaluationResources.Evaluator_Count);

        }

        List<int> CreateColumnsWidth(List<EvaluationEntity<Person>> entities, bool isFiltered)
        {
            return new List<int>
            {
                entities.Max(entity => entity.Entity.FullName.Length),
                ReferenceEvaluationResources.Evaluator_Sex.Length,
                isFiltered
                 ? System.Math.Max(entities.Max(entity => entity.CountBySelection.ToString().Length), ReferenceEvaluationResources.Evaluation_CountBySelection.Length)
                 : System.Math.Max(entities.Max(entity => entity.CountByProject.ToString().Length), ReferenceEvaluationResources.Evaluator_Count.Length)
            };
        }

        #endregion
    }
}
