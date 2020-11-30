using Newtonsoft.Json;
using SwissAcademic.Citavi;
using System;

namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    public class AddonSettings
    {
        internal AddonSettings()
        {

        }

        public bool IsMatchingActivate { get; set; }

        public bool MatchCategoriesFromReferenceToKnowledgeItems { get; set; }

        public bool MatchGroupsFromReferenceToKnowledgeItems { get; set; }

        public bool MatchKeywordsfromReferenceToKnowledgeItems { get; set; }

        public bool MatchCategoriesFromKnowledgeItemToReference { get; set; }

        public bool MatchGroupsFromKnowledgeItemToReference { get; set; }

        public bool MatchKeywordsFromKnowledgeItemToReference { get; set; }

        [JsonIgnore]
        public static AddonSettings Default => new AddonSettings();

        public static AddonSettings LoadfromProjectSettings(ProjectSettings projectSettings)
        {
            try
            {
                var json = projectSettings[Addon.Keys.ProjectSettings]?.ToString();
                return LoadFromJson(json);
            }
            catch (Exception)
            {
                // TODO
                return AddonSettings.Default;
            }
        }

        public static void SaveToProjectSettings(ProjectSettings projectSettings, AddonSettings settings)
        {
            try
            {
                var json = JsonConvert.SerializeObject(settings);
                projectSettings[Addon.Keys.ProjectSettings] = json;
            }
            catch (Exception)
            {
                // TODO
            }
        }

        internal static AddonSettings LoadFromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<AddonSettings>(json);
            }
            catch (Exception)
            {
                // TODO
                return AddonSettings.Default;
            }
        }

    }
}
