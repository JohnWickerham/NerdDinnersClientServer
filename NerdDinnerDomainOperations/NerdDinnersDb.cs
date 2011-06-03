using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NerdDinnerDomain
{
    public class NerdDinnersDb : DbContext
    {
        public NerdDinnersDb()
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public void ChangeObjectState<T>(T entity, EntityState entityState)
        {
            ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.ChangeObjectState(entity, entityState);
        }

        public DbSet<Dinner> Dinners { get; set; }
        public DbSet<Rsvp> RSVPs { get; set; }
    }
}
