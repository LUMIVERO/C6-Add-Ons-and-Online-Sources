namespace SwissAcademic.Addons.ReferenceEvaluationAddon
{
    internal class EvaluationEntity<T>
    {

        #region Constructors

        public EvaluationEntity(T entity) => Entity = entity;

        #endregion

        #region Properties

        public T Entity { get; }

        public int CountByProject { get; set; }

        public int CountBySelection { get; set; }

        #endregion
    }
}
