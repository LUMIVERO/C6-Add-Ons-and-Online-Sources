namespace SwissAcademic.Addons.ReferenceEvaluationAddon
{
    internal class EvaluationEntity<T>
    {
        // Constructors

        public EvaluationEntity(T entity) => Entity = entity;

        // Properties

        public T Entity { get; }

        public int CountByProject { get; set; }

        public int CountBySelection { get; set; }
    }
}
