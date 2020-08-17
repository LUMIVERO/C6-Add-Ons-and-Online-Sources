using SwissAcademic.Citavi;
using System.Collections.Generic;

namespace SwissAcademic.Addons.SortReferencesByParentChildAddon
{
    public class ReferenceComparerByParentChild : IComparer<Reference>
    {
        // Constructors

        ReferenceComparerByParentChild()
        {

        }

        // Properties

        static ReferenceComparerByParentChild _default;

        public static ReferenceComparerByParentChild Default => _default ?? (_default = new ReferenceComparerByParentChild());

        // Methods

        public int Compare(Reference x, Reference y)
        {
            if (x == null)
            {
                return y == null ? 0 : -1;
            }

            if (y == null)
            {
                return 1;
            }

            if (x.ParentReference == null)
            {
                if (y.ParentReference == null)
                {
                    return ReferenceComparer.ShortTitleAscending.Compare(x, y);
                }

                if (x == y.ParentReference)
                {
                    return -1;
                }

                return ReferenceComparer.ShortTitleAscending.Compare(x, y.ParentReference);
            }

            if (y.ParentReference == null)
            {
                if (x.ParentReference == y)
                {
                    return 1;
                }

                return ReferenceComparer.ShortTitleAscending.Compare(x.ParentReference, y);
            }

            if (x.ParentReference == y.ParentReference)
            {
                return ReferenceComparer.ShortTitleAscending.Compare(x, y);
            }

            return ReferenceComparer.ShortTitleAscending.Compare(x.ParentReference, y.ParentReference);
        }
    }
}
