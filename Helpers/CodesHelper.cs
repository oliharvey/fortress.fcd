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
        public static Partner LookupPartnerByID(int userID)
        {
            using (FortressCodeContext db = new FortressCodeContext())
            {
                return db.Partners.Where(p => p.userid == userID).FirstOrDefault();
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
        public static string RandomAlphaString(Random random, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumericString(Random random, int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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
