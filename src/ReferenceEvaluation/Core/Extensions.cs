using SwissAcademic.Citavi;
using System.Collections.Generic;
using System.Linq;

namespace SwissAcademic.Addons.ReferenceEvaluation
{
    internal static class Extensions
    {
        public static string Repeat(this char c, int count) => new string(c, count);

        public static IEnumerable<EntityLinkStatistic> GetStatistics(this List<Reference> references)
        {
            var statistics = new List<EntityLinkStatistic>();

            foreach (var reference in references)
            {
                var entityLinks = reference.EntityLinks.ToList();
                var fromCount = entityLinks.Count(x => (x.Source.ToString() == reference.ShortTitle) && (x.Target.ToString() != reference.ShortTitle));
                var toCount = entityLinks.Count(x => (x.Target.ToString() == reference.ShortTitle) && (x.Source.ToString() != reference.ShortTitle));
                statistics.Add(new EntityLinkStatistic { ShortTitle = reference.ShortTitle, FromCount = fromCount.ToString(), ToCount = toCount.ToString() });
            }

            return statistics;
        }
    }
}
