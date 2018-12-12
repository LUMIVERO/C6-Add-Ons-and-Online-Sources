using SwissAcademic.WordProcessing;
using System.Collections.Generic;
using SwissAcademic.Resources;
using SwissAcademic.Citavi.Metadata;
using System.Linq;
using SwissAcademic.Citavi;
using SwissAcademic.Citavi.Shell;

namespace SwissAcademic.Addons.SortReferencesByParentChild
{
    public class ItemFilter : IReferenceNavigationGridDisplayItemFilter
    {
        #region Fields

        MainForm _mainForm;

        #endregion

        #region Constructors

        public ItemFilter(MainForm mainForm)
        {
            _mainForm = mainForm;
        }

        public ItemFilter()
        {
            if (Program.ProjectShells.Count == 0)
            {
                _mainForm = null;
            }
            else
            {
                _mainForm = Program.ProjectShells[0].PrimaryMainForm;
            }
        }

        #endregion

        #region Methods

        List<Reference> GetAvailableParents()
        {
            return _mainForm?
                   .GetFilteredReferences()
                   .FindAll(reference => reference?.ReferenceType?.AllowedChildren?.Count > 0) ?? new List<Reference>();
        }

        public bool Filters(ReferenceNavigationGridDisplayItemColumn column)
        {
            return true;
        }

        public string GetValue(Reference reference, ReferenceNavigationGridDisplayItemColumn column, out bool handled)
        {
            if (reference.ParentReference == null)
            {
                handled = false;
                return string.Empty;
            }


            var parentAvailable = GetAvailableParents().Any(parent => reference.ParentReference == parent);
            if (!parentAvailable)
            {
                handled = false;
                return string.Empty;
            }

            handled = true;
            var spacer = new string((char)8201, 11);
            switch (column)
            {
                #region AuthorsOrEditorsOrOrganizations

                case ReferenceNavigationGridDisplayItemColumn.AuthorsOrEditorsOrOrganizations:
                    {
                        string result;

                        if (reference.HasCoreField(ReferenceTypeCoreFieldId.Authors) && reference.Authors.Count != 0)
                        {
                            if (reference.Authors.Count <= 10) result = reference.Authors.ToString();
                            else result = ((IList<Person>)reference.Authors.Take(11)).ToString("; ");
                        }

                        else if (reference.HasCoreField(ReferenceTypeCoreFieldId.Editors) && reference.Editors.Count != 0)
                        {
                            if (reference.Editors.Count == 1) result = string.Concat(reference.Editors, " (", Strings.Ed, ")");
                            else if (reference.Editors.Count > 1 && reference.Editors.Count <= 10) result = string.Concat(reference.Editors, " (", Strings.Eds, ")");
                            else result = string.Concat(((IList<Person>)reference.Editors.Take(11)).ToString("; "), " (", Strings.Eds, ")");
                        }

                        else if (reference.HasCoreField(ReferenceTypeCoreFieldId.Organizations) && reference.Organizations.Count != 0)
                        {
                            if (reference.Organizations.Count <= 10) result = reference.Organizations.ToString();
                            else result = ((IList<Person>)reference.Organizations.Take(11)).ToString("; ");
                        }

                        else
                        {
                            result = "–";
                        }

                        return spacer + result;
                    }

                #endregion

                #region BibTeXKey

                case ReferenceNavigationGridDisplayItemColumn.BibTeXKey:
                    {
                        string result;

                        if (string.IsNullOrEmpty(reference.BibTeXKey)) result = "–";
                        else result = reference.BibTeXKey;

                        return spacer + result;
                    }


                #endregion

                #region CitationKey

                case ReferenceNavigationGridDisplayItemColumn.CitationKey:
                    {
                        string result;

                        if (string.IsNullOrEmpty(reference.CitationKey)) result = "–";
                        else result = reference.CitationKey;

                        return spacer + result;
                    }

                #endregion

                #region Title

                case ReferenceNavigationGridDisplayItemColumn.Title:
                    {
                        string result;

                        var title = reference.HasFormatsForProperty(ReferencePropertyDescriptor.Title) && reference.TitleTagged.HasTags() ?
                            reference.TitleTagged.CssStyleTagsToHtmlStyleTags() : reference.Title;

                        var subtitle = reference.HasFormatsForProperty(ReferencePropertyDescriptor.Subtitle) && reference.SubtitleTagged.HasTags() ?
                            reference.SubtitleTagged.CssStyleTagsToHtmlStyleTags() : reference.Subtitle;

                        if (string.IsNullOrEmpty(title))
                        {
                            if (string.IsNullOrEmpty(subtitle)) result = "–";
                            else result = subtitle;
                        }

                        else if (string.IsNullOrEmpty(subtitle))
                        {
                            result = title;
                        }

                        else
                        {
                            switch (reference.Title[reference.Title.Length - 1])
                            {
                                case '.':
                                case ',':
                                case '?':
                                case '!':
                                case ':':
                                case ';':
                                    result = string.Concat(title, " ", subtitle);
                                    break;

                                default:
                                    result = string.Concat(title, ". ", subtitle);
                                    break;
                            }
                        }

                        return new string(' ', 7) + result;
                    }

                #endregion

                #region YearAndReferenceType

                case ReferenceNavigationGridDisplayItemColumn.YearAndReferenceType:
                    {
                        string result;

                        if (string.IsNullOrEmpty(reference.YearResolved)) result = reference.ReferenceType.NameLocalized;
                        else result = reference.YearResolved + " – " + reference.ReferenceType.NameLocalized;

                        return spacer + result;
                    }

                #endregion

                #region default

                default:
                    {
                        handled = false;
                        return string.Empty;
                    }

                    #endregion
            }
        }

        #endregion
    }
}
