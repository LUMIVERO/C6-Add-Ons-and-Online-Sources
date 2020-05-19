using SwissAcademic.Addons.ReferenceEvaluationAddon.Properties;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using System.Collections.Generic;
using System.Linq;

namespace SwissAcademic.Addons.ReferenceEvaluationAddon
{
    internal class PersonEvaluator : BaseEvaluator
    {
        #region Properties

        public override string Caption => Resources.PersonEvaluator_Caption;

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
                _stringBuilder.AppendLine(Resources.PersonEvaluator_NoPersons);
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

                var headers = CreateHeader(isFiltered, columnsWidth);

                _stringBuilder.AppendLine(headers);
                _stringBuilder.AppendLine();
            }

            foreach (var entity in entities)
            {
                var person = entity.Entity;

                if (!isFiltered)
                {
                    _stringBuilder.AppendLine(person.FullName + ' '.Repeat(columnsWidth[0] - person.FullName.Length + 10) + entity.CountByProject);
                }
                else
                {
                    _stringBuilder.AppendLine(
                        person.FullName
                        + ' '.Repeat(columnsWidth[0] - person.FullName.Length + 10)
                        + entity.CountBySelection
                        + ' '.Repeat(columnsWidth[1] - entity.CountBySelection.ToString().Length + 10)
                        + entity.CountByProject);
                }
            }

            return _stringBuilder.ToString();
        }

        string CreateHeader(bool isFiltered, List<int> columnsWidth)
        {
            return Resources.Evaluator_Name
                           + ' '.Repeat(columnsWidth[0] - Resources.Evaluator_Name.Length + 10)
                           + (isFiltered
                                 ? Resources.Evaluation_CountBySelection
                                    + ' '.Repeat(columnsWidth[1] - Resources.Evaluation_CountBySelection.Length + 10)
                                    + Resources.Evaluation_CountByProject
                                  : Resources.Evaluator_Count);

        }

        List<int> CreateColumnsWidth(List<EvaluationEntity<Person>> entities, bool isFiltered)
        {
            return new List<int>
            {
                entities.Max(entity => entity.Entity.FullName.Length),
                isFiltered
                 ? System.Math.Max(entities.Max(entity => entity.CountBySelection.ToString().Length), Resources.Evaluation_CountBySelection.Length)
                 : System.Math.Max(entities.Max(entity => entity.CountByProject.ToString().Length), Resources.Evaluator_Count.Length)
            };
        }

        #endregion
    }
}