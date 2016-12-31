using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

namespace SuperWizardPlatformer
{
    static class ContactListener
    {
        public static bool OnCollision(Fixture A, Fixture B, Contact contact)
        {
            IEntity userDataA = A.Body.UserData != null ? A.Body.UserData as IEntity : null;
            IEntity userDataB = B.Body.UserData != null ? B.Body.UserData as IEntity : null;

            if (userDataA != null && userDataB != null)
            {
                return userDataA.OnCollision(userDataB) && userDataB.OnCollision(userDataA);
            }

            return true;
        }
    }
}
