using FortressCodesDomain.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FortressCodesDomain.Repository
{
    public class Respository : IRepository
    {
        private FortressCodeContext db;

        private Exception ThrowDbEntityValidationException(DbEntityValidationException dbEx)
        {
            Exception raise = dbEx;
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    string message = string.Format("{0}:{1}",
                        validationErrors.Entry.Entity.ToString(),
                        validationError.ErrorMessage);
                    // raise a new exception nesting the current instance as InnerException
                    raise = new InvalidOperationException(message, raise);
                }
            }
            return raise;
        }

        public Respository(FortressCodeContext db)
        {
            this.db = db;
        }

        public async Task<Voucher> GetCodeAsync(string code)
        {
            return await db.Vouchers.Include("TransactionType").FirstOrDefaultAsync(v => v.vouchercode == code);
        }

        public async Task<Voucher> GetCodeByIdAsync(int codeid)
        {
            return await db.Vouchers.Include("TransactionType").FirstOrDefaultAsync(v => v.Id == codeid);
        }

        public async Task<int> AddTransactionAsync(Transaction entity)
        {
            db.Transactions.Add(entity);
            return await db.SaveChangesAsync();
        }

        public async Task<List<TransactionType>> GetAllTransactionTypesAsync()
        {
            return await db.TransactionTypes.ToListAsync();
        }

        public async Task<TransactionType> GetTransactionTypeIdAsync(string transactionType)
        {
            return await db.TransactionTypes.SingleOrDefaultAsync(t => t.Name == transactionType);
        }

        public async Task<int> UpdateVoucherAsync(Voucher entity)
        {
            try
            {
                db.Vouchers.AddOrUpdate(entity);
                var ret = await db.SaveChangesAsync();
                return ret;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating voucher", ex);
            }
        }

        public async Task<int> GetCodeAttemptsInTimeLimitAsync(int codeId, int timeLimit, int validatedTransactionTypeId)
        {
            DateTime dateTime15MinsAgo = DateTime.Now.AddMinutes(-timeLimit);
            return await db.Transactions.CountAsync(t => t.CodeId == codeId && t.Date > dateTime15MinsAgo && t.TransactionTypeId == validatedTransactionTypeId);
        }

        public async Task<int> GetCodeUsageCountAsync(int codeId, int activatedTransactionTypeId)
        {
            return await db.Transactions.CountAsync(t => t.CodeId == codeId && t.TransactionTypeId == activatedTransactionTypeId);
        }

        /// <summary>
        /// TODO://  need to possibly look at returning a more meaningful error when the countries do not match, 
        /// currently it will be a device level does not exist
        /// </summary>
        /// <param name="formattedDeviceName">Name of the formatted device.</param>
        /// <param name="userDeviceCountryIso">The user device country iso.</param>
        /// <param name="pricingModel">The pricing model.</param>
        /// <returns></returns>
        public async Task<DeviceLevel> GetDeviceLevelByFormattedDeviceNameAsync(String formattedDeviceName, string userDeviceCountryIso, PricingModel pricingModel)
        {
            DeviceLevel ret = null;
            //TODO: leaving the country check in for the time being to test voucher validation
            var device = await db.Devices.SingleOrDefaultAsync(d => d.name_raw.ToLower() == formattedDeviceName.ToLower() && d.Country.ISO == userDeviceCountryIso);
            if (device != null)
            {
                if (pricingModel.Country.ISO == userDeviceCountryIso)
                {
                    ret = device.DeviceLevels.SingleOrDefault();
                }
            }
            return ret;
        }


        public async Task<PricingModel> GetPricingModelByVoucherCodeAsync(String voucherCode)
        {
            PricingModel ret = null;
            var voucher = await db.Vouchers.SingleOrDefaultAsync(v => v.vouchercode == voucherCode);
            if (voucher != null)
            {
                var metadata = voucher.VoucherMetadatas.FirstOrDefault();
                if (metadata != null)
                {
                    if (metadata.PricingModel != null)
                    {
                        ret = metadata.PricingModel;
                    }
                }
            }
            return ret;
        }

        public async Task<PricingModel> GetPricingModelByDeviceIdAsync(Int32 deviceID)
        {
            PricingModel ret = null;
            var device = await db.Devices.SingleOrDefaultAsync(d => d.id == deviceID);
            if (device != null)
            {
                var deviceLevel = device.DeviceLevels.SingleOrDefault();
                if (deviceLevel != null)
                {
                    var level = deviceLevel.Level;
                    if (level != null)
                    {
                        ret = level.PricingModels.SingleOrDefault();
                    }
                }
            }
            return ret;
        }


        public Boolean IsValid<T>(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("The entity can not be null.");
            }
            return true;
        }


        public async Task<Boolean> AddAsync<T>(T entity) where T : class
        {
            if (!IsValid(entity))
            {
                return false;
            }
            try
            {
                db.Set(typeof(T)).Add(entity);
                await db.SaveChangesAsync();
                return db.Entry(entity).GetValidationResult().IsValid;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                throw ThrowDbEntityValidationException(dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error updating {0}", entity.GetType().Name), ex);
            }
        }


        public async Task<Boolean> UpdateAsync<T>(T entity) where T : class
        {
            if (!IsValid(entity))
            {
                return false;
            }
            try
            {
                db.Set(typeof(T)).Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return db.Entry(entity).GetValidationResult().IsValid;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                throw ThrowDbEntityValidationException(dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error updating {0}", entity.GetType().Name), ex);
            }
        }


        public async Task<Boolean> DeleteAsync<T>(T entity) where T : class
        {
            if (!IsValid(entity))
            {
                return false;
            }
            try
            {
                db.Set(typeof(T)).Remove(entity);
                await db.SaveChangesAsync();
                return db.Entry(entity).GetValidationResult().IsValid;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                throw ThrowDbEntityValidationException(dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error updating {0}", entity.GetType().Name), ex);
            }
        }


        public IQueryable<T> GetAll<T>() where T : class
        {
            IQueryable<T> query = db.Set<T>();
            return query;
        }


        public IQueryable<T> FindBy<T>(Expression<Func<T, Boolean>> predicate) where T : class
        {

            IQueryable<T> query = db.Set<T>().Where(predicate);
            return query;
        }


        public T GetSingleOrDefault<T>(Expression<Func<T, Boolean>> predicate) where T : class
        {

            T query = db.Set<T>().SingleOrDefault(predicate);
            return query;
        }
    }
}
