using SwissAcademic.Citavi.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwissAcademic.Addons.ReferenceEvaluationAddon
{
    internal abstract class BaseEvaluator
    {
        #region Fields

        protected StringBuilder _stringBuilder;

        #endregion

        #region Constructors

        protected BaseEvaluator() => _stringBuilder = new StringBuilder();

        #endregion

        #region Properties

        public abstract string Caption { get; }

        public bool ShowHeader { get; set; }

        #endregion

        #region Methods

        public abstract string Run(MainForm mainForm);

        internal static IEnumerable<BaseEvaluator> GetAvailableEvaluators()
        {
            return typeof(BaseEvaluator)
                   .Assembly
                   .GetTypes()
                   .Where(type => type.BaseType.Equals(typeof(BaseEvaluator)))
                   .Select(type => Activator.CreateInstance(type) as BaseEvaluator)
                   .ToList();
        }

        #endregion
    }
}
