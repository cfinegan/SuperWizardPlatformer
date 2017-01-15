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

        // The current player-controlled game entitiy.
        public Player Player { get; private set; }

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

            var playerCheck = entity as Player;
            if (playerCheck != null)
            {
                Player = playerCheck;
            }
        }

        /// <summary>
        /// Remove all objects from the collection which are marked for removal.
        /// </summary>
        public void RemoveMarkedElements()
        {
            RemoveMarkedElements(_activeEntities);
            RemoveMarkedElements(_activeDrawables);
        }

        /// <summary>
        /// Removes all marked IRemovable objects from the specified list.
        /// </summary>
        /// <typeparam name="T">Type implementing IRemovable.</typeparam>
        /// <param name="list">The list to scan for marked objects.</param>
        /// <seealso cref="IRemovable"/>
        private static void RemoveMarkedElements<T>(List<T> list) where T : IRemovable
        {
            int count = list.Count;
            int removed = 0;
            for (int i = 0; i < count - removed; ++i)
            {
                if (list[i].IsMarkedForRemoval)
                {
                    var tmp = list[i];
                    list[i] = list[count - 1 - removed];
                    list[count - 1 - removed] = tmp;
                    ++removed;
                }
            }
            list.RemoveRange(count - removed, removed);
        }
    }
}
