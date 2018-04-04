using FortressCodesDomain.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FortressCodesDomain.Repository
{
    public interface IRepository
    {
        Task<Voucher> GetCodeAsync(string code);
        Voucher GetCode(string code);
        Task<int> AddTransactionAsync(Transaction entity);
        Task<List<TransactionType>> GetAllTransactionTypesAsync();
        Task<TransactionType> GetTransactionTypeIdAsync(string transactionType);
        Task<int> UpdateVoucherAsync(Voucher entity);
        Task<int> GetCodeAttemptsInTimeLimitAsync(int code, int timeLimit, int validatedTransactionTypeId);
        Task<int> GetCodeUsageCountAsync(int codeId, int activatedTransactionTypeId);
        Task<DeviceLevel> GetDeviceLevelByFormattedDeviceNameAsync(String formattedDeviceName, String userDeviceCountryIso, PricingModel pricingModel);
        Task<PricingModel> GetPricingModelByVoucherCodeAsync(String voucherCode);
        Task<PricingModel> GetPricingModelByDeviceIdAsync(Int32 deviceID);
        Task<PricingModel> GetPricingModelByIdAsync(Int32 id);
        Task<IEnumerable<PricingModel>> GetPricingModelsByFamilyIdAsync(Int32 familyId, Int32 deviceLevelID);
        Task<IEnumerable<PricingModel>> GetPricingModelsByFamilyIdTierAsync(Int32 familyId, Int32 deviceLevelID, Tier tierValue);
        Task<Device> GetDeviceByFormattedDeviceNameAsync(String formattedDeviceName);
        Task<Device> GetDeviceByMakeModelCapacityAsync(String make, String model, String capacity);
        Task<Tuple<FortressCodesDomain.DbModels.Device, Boolean>> GetDBDeviceOrUnknownDeviceAsync(String make,
                                                                                                  String capacity,
                                                                                                  String model,
                                                                                                  String countryIso);
        Task<Tuple<bool, tbl_VoucherRegistration>> CheckIfVoucherDeviceMatchesVoucher(string voucherCode, string countryISO, string deviceMake, string deviceModel, string imei, string deviceCapacity);

        Task<tbl_PreloadedDevice> GetPreloadedDeviceByImei(string imei, string countryISO);
        Task<Tuple<Boolean, String>> GetDeviceLevelAsync(String make, String model, String capacity, String voucherCode, String countryIso);
        Task<PricingModel> GetPricingModelByDevicePartnerFamilyAsync(string deviceLevel, Int32 tierId, Int32 familyId);
        Task<PricingModel> GetActivePricingModelByDevicePartnerFamilyAsync(String deviceLevel, Int32 tierId, Int32 familyId);

        Task<IEnumerable<PricingModel>> GetActivePricingModelByFamilyAsync(Int32 familyId);
        Task<string> GetPlanNameFromPricingModel(int pricingModelID, int billingCycle);

        Task<Boolean> AddAsync<T>(T entity) where T : class;
        Task<Boolean> DeleteAsync<T>(T entity) where T : class;
        Task<Boolean> UpdateAsync<T>(T entity) where T : class;
        IQueryable<T> GetAll<T>() where T : class;
        T GetSingleOrDefault<T>(Expression<Func<T, Boolean>> predicate) where T : class;
        IQueryable<T> FindBy<T>(Expression<Func<T, Boolean>> predicate) where T : class;
    }
}
