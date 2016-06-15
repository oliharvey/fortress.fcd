using FortressCodesDomain.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FortressCodesDomain.Repository
{
    public interface IRepository
    {
        Task<Voucher> GetCodeAsync(string code);
        Task<int> AddTransactionAsync(Transaction entity);
        Task<List<TransactionType>> GetAllTransactionTypesAsync();
        Task<TransactionType> GetTransactionTypeIdAsync(string transactionType);
        Task<int> UpdateVoucherAsync(Voucher entity);
        Task<int> GetCodeAttemptsInTimeLimitAsync(int code, int timeLimit, int validatedTransactionTypeId);
        Task<int> GetCodeUsageCountAsync(int codeId, int activatedTransactionTypeId);
        Task<DeviceLevel> GetDeviceLevelByFormattedDeviceNameAsync(String formattedDeviceName, String userDeviceCountryIso, PricingModel pricingModel);
        Task<PricingModel> GetPricingModelByVoucherCodeAsync(String voucherCode);
        Task<PricingModel> GetPricingModelByDeviceIdAsync(Int32 deviceID);

        Task<Boolean> AddAsync<T>(T entity) where T : class;
        Task<Boolean> DeleteAsync<T>(T entity) where T : class;
        Task<Boolean> UpdateAsync<T>(T entity) where T : class;
        IQueryable<T> GetAll<T>() where T : class;
        T GetSingleOrDefault<T>(Expression<Func<T, Boolean>> predicate) where T : class;
        IQueryable<T> FindBy<T>(Expression<Func<T, Boolean>> predicate) where T : class;
    }
}
