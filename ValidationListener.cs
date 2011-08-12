using System.IO;
using NHibernate.Event;
using NHibernateCourse.QuickStart.Model;

namespace NHibernateCourse.QuickStart
{
    public class ValidationListener : IPreUpdateEventListener, IPreInsertEventListener
    {
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var wantValidationOldStyleWay = @event.Entity as IWantValidationOldStyleWay;
            if (wantValidationOldStyleWay != null && wantValidationOldStyleWay.IsValid == false)
                throw new InvalidDataException("DUDE, invalid data");

            return false;
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            var wantValidationOldStyleWay = @event.Entity as IWantValidationOldStyleWay;
            if (wantValidationOldStyleWay != null && wantValidationOldStyleWay.IsValid == false)
                throw new InvalidDataException("DUDE, invalid data");

            return false;
        }
    }
}