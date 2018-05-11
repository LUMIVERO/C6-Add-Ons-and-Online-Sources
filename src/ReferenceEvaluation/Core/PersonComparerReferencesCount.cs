using SwissAcademic.Citavi;
using System.Collections.Generic;

namespace SwissAcademic.Addons.ReferenceEvaluation
{
    internal class PersonComparerReferencesCount : IComparer<Person>
    {
        public int Compare(Person x, Person y)
        {
            var countCompareResult = x.References.Count.CompareTo(y.References.Count) * (-1);
            if (countCompareResult != 0) return countCompareResult;

            return x.FullName.CompareTo(y.FullName);
        }
    }


    internal class EvaluationEntityComparerByProjectCount<T> : IComparer<EvaluationEntity<T>> where T : CitaviEntity
    {
        public int Compare(EvaluationEntity<T> x, EvaluationEntity<T> y)
        {
            var countCompareResult = x.CountByProject.CompareTo(y.CountByProject) * (-1);
            if (countCompareResult != 0) return countCompareResult;

            return x.Entity.FullName.CompareTo(y.Entity.FullName);
        }
    }

    internal class EvaluationEntityComparerBySelectionCount<T> : IComparer<EvaluationEntity<T>> where T : CitaviEntity
    {
        public int Compare(EvaluationEntity<T> x, EvaluationEntity<T> y)
        {
            var countCompareResult = x.CountBySelection.CompareTo(y.CountBySelection) * (-1);
            if (countCompareResult != 0) return countCompareResult;

            return x.Entity.FullName.CompareTo(y.Entity.FullName);
        }
    }
}
