using SwissAcademic.Citavi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwissAcademic.Addons.ReferenceEvaluation
{
    internal static class Extensions
    {
        public static string Repeat(this char c, int count)
        {
            return new String(c, count);
        }

        public static IEnumerable<EntityLinkStatistic> GetStatistics(this List<Reference> references)
        {
            var statistics = new List<EntityLinkStatistic>();

            foreach (var reference in references)
            {
                var entityLinks = reference.EntityLinks.ToList();
                var fromCount = entityLinks.Where(x => (x.Source.ToString() == reference.ShortTitle) && (x.Target.ToString() != reference.ShortTitle)).Count();
                var toCount = entityLinks.Where(x => (x.Target.ToString() == reference.ShortTitle) && (x.Source.ToString() != reference.ShortTitle)).Count();
                statistics.Add(new EntityLinkStatistic { ShortTitle = reference.ShortTitle, FromCount = fromCount.ToString(), ToCount = toCount.ToString() });
            }

            return statistics;
        }
    }
}
