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
}
