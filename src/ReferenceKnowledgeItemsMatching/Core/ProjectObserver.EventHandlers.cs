using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;
using SwissAcademic.Collections;
using System;
using System.Configuration;
using System.Linq;

namespace SwissAcademic.Addons.ReferenceKnowledgeItemsMatchingAddon
{
    partial class ProjectObserver
    {
        void ProjectShells_CollectionChanged(object sender, CollectionChangedEventArgs<ProjectShell> e)
        {
            if (!e.HasRecords) return;

            switch (e.ChangeType)
            {
                case CollectionChangeType.ItemsAdded:
                    e.Records.ForEach(record => AddProject(record.Item?.Project));
                    break;
                case CollectionChangeType.ItemsDeleted:
                    e.Records.ForEach(record => RemoveProject(record.Item?.Project));
                    break;
            }
        }

        void ProjectSettings_SettingChanging(object sender, SettingChangingEventArgs e)
        {
            if (e.SettingName.Equals(Addon.Keys.ProjectSettings, StringComparison.OrdinalIgnoreCase) && sender is ProjectSettings projectSettings)
            {
                _projects[projectSettings.Project] = AddonSettings.LoadFromJson(e.NewValue.ToString());
            }
        }

        void References_CollectionChanged(object sender, CollectionChangedEventArgs<Reference> e)
        {
            foreach (var record in e.Records)
            {
                if (record.Item is Reference reference)
                {
                    switch (e.ChangeType)
                    {
                        case CollectionChangeType.ItemsAdded:
                            ObserveReference(reference, true);
                            break;
                        case CollectionChangeType.ItemsDeleted:
                            ObserveReference(reference, false);
                            break;
                    }
                }
            }
        }

        void AllKnowledgeItems_CollectionChanged(object sender, CollectionChangedEventArgs<KnowledgeItem> e)
        {
            foreach (var record in e.Records)
            {
                if (record.Item is KnowledgeItem knowledgeItem)
                {
                    switch (e.ChangeType)
                    {
                        case CollectionChangeType.ItemsAdded:
                            ObserveKnowledgeItem(knowledgeItem, true);
                            break;
                        case CollectionChangeType.ItemsDeleted:
                            ObserveKnowledgeItem(knowledgeItem, false);
                            break;
                    }
                }
            }
        }

        void Reference_Groups_CollectionChanged(object sender, CollectionChangedEventArgs<Group> e)
        {
            if (sender is ReferenceGroupCollection collection && collection.Project is Project project)
            {
                var addonSettings = _projects[project];

                if (addonSettings == null || !addonSettings.IsMatchingActivate || !addonSettings.MatchGroupsFromReferenceToKnowledgeItems) return;

                if (e.ChangeType == CollectionChangeType.Reset && collection.Any())
                {
                    foreach (var knowledgeItem in collection.Parent.Quotations)
                    {
                        var groups = collection.ToList();
                        knowledgeItem.Groups.SuspendNotification();
                        knowledgeItem.Groups.AddRange(groups);
                        knowledgeItem.Groups.ResumeNotification(false, false);
                    }
                }
                else if (e.ChangeType == CollectionChangeType.ItemsAdded && e.HasRecords)
                {
                    foreach (var knowledgeItem in collection.Parent.Quotations)
                    {
                        var groups = e.Records.Select(record => record.Item).ToList();
                        knowledgeItem.Groups.SuspendNotification();
                        knowledgeItem.Groups.AddRange(groups);
                        knowledgeItem.Groups.ResumeNotification(false, false);
                    }
                }
            }
        }

        void Reference_Keywords_CollectionChanged(object sender, CollectionChangedEventArgs<Keyword> e)
        {
            if (sender is ReferenceKeywordCollection collection && collection.Project is Project project)
            {

                var addonSettings = _projects[project];

                if (addonSettings == null || !addonSettings.IsMatchingActivate || !addonSettings.MatchKeywordsfromReferenceToKnowledgeItems) return;

                if (e.ChangeType == CollectionChangeType.Reset && collection.Any())
                {
                    foreach (var knowledgeItem in collection.Parent.Quotations)
                    {
                        var keywords = collection.ToList();
                        knowledgeItem.Keywords.SuspendNotification();
                        knowledgeItem.Keywords.AddRange(keywords);
                        knowledgeItem.Keywords.ResumeNotification(false, false);
                    }
                }
                else if (e.ChangeType == CollectionChangeType.ItemsAdded && e.HasRecords)
                {
                    foreach (var knowledgeItem in collection.Parent.Quotations)
                    {
                        var keywords = e.Records.Select(record => record.Item).ToList();
                        knowledgeItem.Keywords.SuspendNotification();
                        knowledgeItem.Keywords.AddRange(keywords);
                        knowledgeItem.Keywords.ResumeNotification(false, false);
                    }
                }
            }
        }

        void Reference_Categories_CollectionChanged(object sender, CollectionChangedEventArgs<Category> e)
        {
            if (sender is ReferenceCategoryCollection collection && collection.Project is Project project)
            {
                var addonSettings = _projects[project];

                if (addonSettings == null || !addonSettings.IsMatchingActivate || !addonSettings.MatchCategoriesFromReferenceToKnowledgeItems) return;

                if (e.ChangeType == CollectionChangeType.Reset && collection.Any())
                {
                    foreach (var knowledgeItem in collection.Parent.Quotations)
                    {
                        var categories = collection.ToList();
                        knowledgeItem.Categories.SuspendNotification();
                        knowledgeItem.Categories.AddRange(categories);
                        knowledgeItem.Categories.ResumeNotification(false, false);
                    }
                }
                else if (e.ChangeType == CollectionChangeType.ItemsAdded && e.HasRecords)
                {
                    foreach (var knowledgeItem in collection.Parent.Quotations)
                    {
                        var categories = e.Records.Select(record => record.Item).ToList();
                        knowledgeItem.Categories.SuspendNotification();
                        knowledgeItem.Categories.AddRange(categories);
                        knowledgeItem.Categories.ResumeNotification(false, false);
                    }
                }
            }
        }

        void KnowledgeItem_Groups_CollectionChanged(object sender, CollectionChangedEventArgs<Group> e)
        {
            if (sender is KnowledgeItemGroupCollection collection && collection.Project is Project project)
            {
                var addonSettings = _projects[project];

                if (addonSettings == null || !addonSettings.IsMatchingActivate || !addonSettings.MatchGroupsFromKnowledgeItemToReference) return;

                if (e.ChangeType == CollectionChangeType.Reset && collection.Any())
                {
                    if (collection.Parent.Reference is Reference reference)
                    {
                        var groups = collection.ToList();
                        reference.Groups.SuspendNotification();
                        reference.Groups.AddRange(groups);
                        reference.Groups.ResumeNotification(false, false);
                    }
                }
                else if (e.ChangeType == CollectionChangeType.ItemsAdded && e.HasRecords)
                {
                    if (collection.Parent.Reference is Reference reference)
                    {
                        var groups = e.Records.Select(record => record.Item).ToList();
                        reference.Groups.SuspendNotification();
                        reference.Groups.AddRange(groups);
                        reference.Groups.ResumeNotification(false, false);
                    }
                }
            }
        }

        void KnowledgeItem_Keywords_CollectionChanged(object sender, CollectionChangedEventArgs<Keyword> e)
        {
            if (sender is KnowledgeItemKeywordCollection collection && collection.Project is Project project)
            {
                var addonSettings = _projects[project];

                if (addonSettings == null || !addonSettings.IsMatchingActivate || !addonSettings.MatchKeywordsFromKnowledgeItemToReference) return;

                if (e.ChangeType == CollectionChangeType.Reset && collection.Any())
                {
                    if (collection.Parent.Reference is Reference reference)
                    {
                        var keywords = collection.ToList();
                        reference.Keywords.SuspendNotification();
                        reference.Keywords.AddRange(keywords);
                        reference.Keywords.ResumeNotification(false, false);
                    }
                }
                else if (e.ChangeType == CollectionChangeType.ItemsAdded && e.HasRecords)
                {
                    if (collection.Parent.Reference is Reference reference)
                    {
                        var keywords = e.Records.Select(record => record.Item).ToList();
                        reference.Keywords.SuspendNotification();
                        reference.Keywords.AddRange(keywords);
                        reference.Keywords.ResumeNotification(false, false);
                    }
                }
            }
        }

        void KnowledgeItem_Categories_CollectionChanged(object sender, CollectionChangedEventArgs<Category> e)
        {
            if (sender is KnowledgeItemCategoryCollection collection && collection.Project is Project project)
            {
                var addonSettings = _projects[project];

                if (addonSettings == null || !addonSettings.IsMatchingActivate || !addonSettings.MatchCategoriesFromKnowledgeItemToReference) return;

                if (e.ChangeType == CollectionChangeType.Reset && collection.Any())
                {
                    if (collection.Parent.Reference is Reference reference)
                    {
                        var categories = collection.ToList();
                        reference.Categories.SuspendNotification();
                        reference.Categories.AddRange(categories);
                        reference.Categories.ResumeNotification(false, false);
                    }
                }
                else if (e.ChangeType == CollectionChangeType.ItemsAdded && e.HasRecords)
                {
                    if (collection.Parent.Reference is Reference reference)
                    {
                        var categories = e.Records.Select(record => record.Item).ToList();
                        reference.Categories.SuspendNotification();
                        reference.Categories.AddRange(categories);
                        reference.Categories.ResumeNotification(false, false);
                    }
                }
            }
        }
    }
}
