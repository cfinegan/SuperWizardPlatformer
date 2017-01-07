using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using System.Diagnostics;

namespace SuperWizardPlatformer
{
    /// <summary>
    /// Provides static callback methods which should be registered to any IEntity objects which
    /// support collision detection. This functionality requires that all IEntity objects:
    /// - Have a Body member whose UserData field is set to the IEntity which owns it.
    /// - Implement the OnCollision and OnSeparation abstract methods in IEntity.
    /// </summary>
    /// <seealso cref="IEntity"/>
    static class ContactListener
    {
        /// <summary>
        /// Invoked when an IEntity object collides with a physics body.
        /// </summary>
        /// <param name="A">Fixture which should always have a non-null Body.UserData.</param>
        /// <param name="B">Fixture which may have a null Body.UserData.</param>
        /// <param name="contact">Additional information about the collision.</param>
        /// <returns>true if the collision should continue, false otherwise.</returns>
        public static bool OnCollision(Fixture A, Fixture B, Contact contact)
        {
            Debug.Assert(A.Body.UserData != null);

            if (A.Body.UserData != null && B.Body.UserData != null)
            {
                return ((IEntity)A.Body.UserData).OnCollision((IEntity)B.Body.UserData);
            }

            return true;
        }

        /// <summary>
        /// Invoked when an IEntity object is no longer touching a physics body.
        /// </summary>
        /// <param name="A">Fixture which should always have a non-null Body.UserData.</param>
        /// <param name="B">Fixture which may have a null Body.UserData.</param>
        public static void OnSeparation(Fixture A, Fixture B)
        {
            Debug.Assert(A.Body.UserData != null);

            if (A.Body.UserData != null && B.Body.UserData != null)
            {
                ((IEntity)A.Body.UserData).OnSeparation((IEntity)B.Body.UserData);
            }
        }
    }
}
