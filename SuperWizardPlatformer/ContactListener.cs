using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using System.Diagnostics;

namespace SuperWizardPlatformer
{
    static class ContactListener
    {
        public static bool OnCollision(Fixture A, Fixture B, Contact contact)
        {
            Debug.Assert(A.Body.UserData != null);

            if (A.Body.UserData != null && B.Body.UserData != null)
            {
                return ((IEntity)A.Body.UserData).OnCollision((IEntity)B.Body.UserData);
            }

            return true;
        }

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
