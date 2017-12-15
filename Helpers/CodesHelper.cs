using FortressCodesDomain.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortressCodesDomain.Helpers
{
    public static class CodesHelper
    {
        public static Partner LookupPartnerByName(string partnername)
        {
            using (FortressCodeContext db = new FortressCodeContext())
            {
                return db.Partners.Where(p => p.partnername == partnername).FirstOrDefault();
            }
        }

        public static int LookupFamilyIdByName(string familyName)
        {
            using (FortressCodeContext db = new FortressCodeContext())
            {
                return db.Families.Where(f => f.Name == familyName)
                    .Select(f => f.Id).FirstOrDefault();
            }
        }

        public static int LookupTierIdByName(string tier, int partnerId)
        {
            using (FortressCodeContext db = new FortressCodeContext())
            {
                return db.Tiers.Where(t => t.Name == tier
                   && t.PartnerId == partnerId).Select(t=> t.Id).FirstOrDefault();
            }
        }

    }
}
