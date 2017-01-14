using System;
using System.Collections.Generic;

namespace SuperWizardPlatformer
{
    /// <summary>
    /// Maintains a collection of IEntity objects and sorts them by implementation into 
    /// indexable groups.
    /// </summary>
    class EntityContainer
    {
        // Initial number of IEntity and IDrawable references allocated. Intentionally high to
        // avoid resizing.
        private const int INIT_CAPACITY = 250;

        // List of active IEntity objects.
        private List<IEntity> _activeEntities = new List<IEntity>(INIT_CAPACITY);

        // List of active IDrawable objects.
        private List<IDrawable> _activeDrawables = new List<IDrawable>(INIT_CAPACITY);

        /// <summary>
        /// Constructs an EntityContainer with zero elements.
        /// </summary>
        public EntityContainer() { }

        /// <summary>
        /// Constructs an EntityContainer from a list of IEntity objects. Uses the AddEntity
        /// method to insert each entity.
        /// </summary>
        /// <param name="entities">Entities to add to the collection of active entities.</param>
        public EntityContainer(List<IEntity> entities)
        {
            if (entities == null) { throw new ArgumentNullException(nameof(entities)); }

            foreach (var e in entities)
            {
                AddEntity(e);
            }
        }

        // Readonly interface to the list of active IEntity objects.
        // Note: using foreach on this interface will generate garbage. Use explicit indexing.
        public IReadOnlyList<IEntity> ActiveEntities => _activeEntities;

        // Readonly interface to the list of active IDrawable objects.
        // Note: using foreach on this interface will generate garbage. Use explicit indexing.
        public IReadOnlyList<IDrawable> ActiveDrawables => _activeDrawables;

        /// <summary>
        /// Adds a new entity to the collection. Also adds it as an IDrawable, depending on 
        /// its type.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        public void AddEntity(IEntity entity)
        {
            if (_activeEntities.Count == _activeEntities.Capacity)
            {
                Console.WriteLine("[WARNING] {0} resizing at count: {1}", 
                    nameof(_activeEntities), _activeEntities.Count);
            }
            _activeEntities.Add(entity);

            var drawableCheck = entity as IDrawable;
            if (drawableCheck != null)
            {
                if (_activeDrawables.Count == _activeDrawables.Capacity)
                {
                    Console.WriteLine("[WARNING] {0} resizng at count: {1}",
                        nameof(_activeDrawables), _activeDrawables.Count);
                }
                _activeDrawables.Add(drawableCheck);
            }
        }

        /// <summary>
        /// Remove all objects from the collection which are marked for removal.
        /// </summary>
        public void RemoveMarkedElements()
        {
            RemoveMarkedEntities();
            RemoveMarkedDrawables();
        }

        /// <summary>
        /// Removes all IEntity objects from the collection which are marked for removal.
        /// </summary>
        private void RemoveMarkedEntities()
        {
            int count = _activeEntities.Count;
            int removed = 0;
            for (int i = 0; i < count; ++i)
            {
                if (_activeEntities[i].IsMarkedForRemoval)
                {
                    var tmp = _activeEntities[i];
                    _activeEntities[i] = _activeEntities[count - 1 - removed];
                    _activeEntities[count - 1 - removed] = tmp;
                    ++removed;
                }
            }
            _activeEntities.RemoveRange(count - removed, removed);
        }

        /// <summary>
        /// Removes all IDrawable objects from the collection which are marked for removal.
        /// </summary>
        private void RemoveMarkedDrawables()
        {
            int count = _activeDrawables.Count;
            int removed = 0;
            for (int i = 0; i < count; ++i)
            {
                if (_activeDrawables[i].IsMarkedForRemoval)
                {
                    var tmp = _activeDrawables[i];
                    _activeDrawables[i] = _activeDrawables[count - 1 - removed];
                    _activeDrawables[count - 1 - removed] = tmp;
                    ++removed;
                }
            }
            _activeDrawables.RemoveRange(count - removed, removed);
        }
    }
}
