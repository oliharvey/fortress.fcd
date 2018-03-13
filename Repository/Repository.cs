using FortressCodesDomain.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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
        public Voucher GetCode(string code)
        {
            return db.Vouchers.Include("TransactionType").FirstOrDefault(v => v.vouchercode == code);
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
        public int AddTransaction(Transaction entity)
        {
            db.Transactions.Add(entity);
            return db.SaveChanges();
        }
        public int AddVoucherRegistration(tbl_VoucherRegistration entity)
        {
            db.tbl_VoucherRegistrations.Add(entity);
            return db.SaveChanges();
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
            DateTime dateTime15MinsAgo = DateTime.UtcNow.AddMinutes(-timeLimit);
            return await db.Transactions.CountAsync(t => t.CodeId == codeId &&
                                                         t.Date > dateTime15MinsAgo &&
                                                         t.TransactionTypeId == validatedTransactionTypeId);
        }
        public async Task<Tuple<bool, tbl_VoucherRegistration>> CheckIfVoucherDeviceMatchesVoucher(string voucherCode, string countryISO, string deviceMake, string deviceModel, string imei, string deviceCapacity)
        {
            Tuple<bool, tbl_VoucherRegistration> returnValFail = new Tuple<bool, tbl_VoucherRegistration>(false, null);
            var voucher = await GetCodeAsync(voucherCode);
            var calculatedStorage = CalculateDeviceTotalSizeFromRaw(deviceCapacity) + "gb";
            if (voucher == null)
            {

                return returnValFail;
            }


            var vouchReg = db.tbl_VoucherRegistrations.Where(vouch => vouch.VoucherID == voucher.Id && vouch.CountryISO == countryISO && vouch.DeviceMake == deviceMake && vouch.DeviceModel == deviceModel && vouch.DeviceCapacity == deviceCapacity).SingleOrDefault();
            if (vouchReg == null)
            {
                return returnValFail;
            }
            else
            {
                Tuple<bool, tbl_VoucherRegistration> returnValSuccess = new Tuple<bool, tbl_VoucherRegistration>(true, vouchReg);
                return returnValSuccess;
            }


        }
        public async Task<string> GetPlanNameFromPricingModel(int pricingModelID, int billingCycle)
        {
            var dbContext = new FortressCodeContext();
            PricingModel pm = await db.PricingModels.Where(pms => pms.Id == pricingModelID).SingleOrDefaultAsync();
            Tier tier = db.Tiers.Where(tie => tie.Id == pm.TeirId).SingleOrDefault();
            Partner partner = db.Partners.Where(part => part.userid == pm.PartnerId).SingleOrDefault();
            string planName = "";
            planName = string.Format("{0} - {1} - {2}", partner != null ? partner.partnername : "Fortress", tier != null ? tier.Name : "Basic", (billingCycle == 0 ? "(Monthly)" : "(Annual)"));

            return planName;
        }

        public async Task<string> GenerateVoucherCode(bool NumericVoucherOnly, int voucherlength)
        {
            var allWords = db.tbl_Profanitys.Select(pr => pr.Profanity).Distinct();
            var maxCanGenerate = Math.Pow(10d, voucherlength);

            List<String> AllBadWords = new List<string>();
            if (allWords.Any())
            {
                AllBadWords = new List<String>();
                AllBadWords.AddRange(allWords);
            }

            int maxattempts = 100;
            int counter = 0;
            bool foundVoucher = false;
            String sVoucherCode = String.Empty;
            while (!foundVoucher && counter < maxattempts)
            {
                //define a voucher code


                Boolean bExists = false;
                Boolean bIsBadWord = false;
                Boolean bIsDupe = false;

                Random rndRandom = new Random();
                if (NumericVoucherOnly)
                {
                    //choose random characters                    
                    sVoucherCode = Helpers.CodesHelper.RandomNumericString(rndRandom, voucherlength);
                }
                else
                {
                    //choose random characters
                    sVoucherCode = Helpers.CodesHelper.RandomAlphaString(rndRandom, voucherlength);
                    if (AllBadWords.Any(sVoucherCode.Contains))
                    {
                        bIsBadWord = true;
                    }
                }

                bIsDupe = db.Vouchers.Where(v => v.vouchercode == sVoucherCode).Any();
                if (bExists || bIsBadWord || bIsDupe)
                {
                    sVoucherCode = String.Empty;
                }
                else
                {
                    foundVoucher = true;
                }
                counter++;
            }
            if (foundVoucher)
                return sVoucherCode;
            else
                return "";

        }


        public string GetPlanNameFromPricingModelNonAsync(int pricingModelID, int billingCycle)
        {


            var dbContext = new FortressCodeContext();
            PricingModel pm = db.PricingModels.Where(pms => pms.Id == pricingModelID).SingleOrDefault();
            Tier tier = db.Tiers.Where(tie => tie.Id == pm.TeirId).SingleOrDefault();
            Partner partner = db.Partners.Where(part => part.userid == pm.PartnerId).SingleOrDefault();
            string planName = "";
            planName = string.Format("{0} - {1} - {2}", partner != null ? partner.partnername : "Fortress", tier != null ? tier.Name : "Basic", (billingCycle == 0 ? "(Monthly)" : "(Annual)"));


            return planName;
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
        public async Task<DeviceLevel> GetDeviceLevelByFormattedDeviceNameAsync(String formattedDeviceName,
                                                                                String userDeviceCountryIso,
                                                                                PricingModel pricingModel)
        {
            DeviceLevel ret = null;
            //TODO: leaving the country check in for the time being to test voucher validation
            var device = await db.Devices.SingleOrDefaultAsync(d => d.name.ToLower() == formattedDeviceName.ToLower());
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

        public async Task<IEnumerable<PricingModel>> GetPricingModelsByFamilyIdAsync(Int32 familyId, Int32 deviceLevelID)
        {
            return await db.PricingModels.Where(pm => pm.FamilyId == familyId && pm.LevelId == deviceLevelID).ToListAsync();
        }
        public async Task<IEnumerable<PricingModel>> GetPricingModelsByFamilyIdTierAsync(Int32 familyId, Int32 deviceLevelID, Tier tier)
        {
            //return await db.PricingModels.Where(pm => pm.FamilyId == familyId && pm.LevelId == deviceLevelID).ToListAsync();

            //Code reverted 06/06/2017 to return pricingmodels equal and greater to current coverages tier

            //Code re-reverted 02/11/2017 to return pricingmodels greater than current tier except when ultimate then return other pricingmodels which are of tier ultimate
            var pms = await (from pm in db.PricingModels
                             join t in db.Tiers on pm.Tier.Id equals t.Id
                             //where pm.FamilyId == familyId && pm.LevelId == deviceLevelID && (t.TierLevel > tier.TierLevel || (tier.Name.ToLower()=="ultimate" && (t.TierLevel >= tier.TierLevel || t.Name.ToLower() == "ultimate")))
                             where pm.FamilyId == familyId && pm.LevelId == deviceLevelID && (t.TierLevel >= tier.TierLevel)
                             select pm)
              .ToListAsync();
            return pms;
        }
        public async Task<IEnumerable<PricingModel>> GetPricingModelsByFamilyIdTierSubUpgradeAsync(Int32 familyId, Int32 deviceLevelID, Tier tier)
        {
            //new method to facilitate providing an upgrade to recurring subscription customers. Return only 
            var pms = await (from pm in db.PricingModels
                             join t in db.Tiers on pm.Tier.Id equals t.Id
                             where pm.FamilyId == familyId && pm.LevelId == deviceLevelID && (t.TierLevel > tier.TierLevel)
                             select pm)
              .ToListAsync();
            return pms;
        }
        public async Task<PricingModel> GetPricingModelByIdAsync(Int32 id)
        {
            return await db.PricingModels.SingleOrDefaultAsync(pm => pm.Id == id);
        }
        public async Task<Tier> GetTierByPartnerTierAsync(Int32 partnerID, String tierName)
        {
            return await db.Tiers.SingleOrDefaultAsync(pm => pm.PartnerId == partnerID && pm.Name == tierName);
        }
        public async Task<Level> GetLevelByNameAsync(String levelName)
        {
            return await db.Levels.SingleOrDefaultAsync(pm => pm.Name == levelName);
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
        private static Double ExtractDoubleFromDeviceCapacityRaw(String deviceCapacityRaw)
        {
            Regex digitsRegex = new Regex(@"^\D*?((-?(\d+(\.\d+)?))|(-?\.\d+)).*");
            Match mx = digitsRegex.Match(deviceCapacityRaw);
            return mx.Success ? Convert.ToDouble(mx.Groups[1].Value) : 0;
        }
        public async Task<string> GetDeviceMarketingName(string DeviceModel, string rawCapacity)
        {
            string strMarketingName = "";

            int calculateCapacity = CalculateDeviceTotalSizeFromRaw(rawCapacity);
            string newCapacityString = calculateCapacity.ToString() + "GB";

            var deviceListing = await db.Devices.Where(p => p.model == DeviceModel && p.capacity == newCapacityString).FirstOrDefaultAsync();

            if (deviceListing != null)
            {
                strMarketingName = deviceListing.name;
            }
            else
            {
                strMarketingName = DeviceModel;
            }

            return strMarketingName;

        }
        public static Int32 CalculateDeviceTotalSizeFromRaw(String capacityRaw)
        {
            Int32 ret = 0;
            Double capacity = ExtractDoubleFromDeviceCapacityRaw(capacityRaw);
            if (capacity < 1.00d)
            {
                ret = 1;
            }
            if (capacity >= 1.00d && capacity < 2.00d)
            {
                ret = 2;
            }
            if (capacity >= 2.00d && capacity < 4.00d)
            {
                ret = 4;
            }
            if (capacity >= 4.00d && capacity < 8.00d)
            {
                ret = 8;
            }
            if (capacity >= 8.00d && capacity < 16.00d)
            {
                ret = 16;
            }
            if (capacity >= 16.00d && capacity < 32.00d)
            {
                ret = 32;
            }
            if (capacity >= 32.00d && capacity < 64.00d)
            {
                ret = 64;
            }
            if (capacity >= 64.00d && capacity < 128.00d)
            {
                ret = 128;
            }
            if (capacity >= 128.00d && capacity < 256.00d)
            {
                ret = 256;
            }
            if (capacity >= 256.00d && capacity < 512.00d)
            {
                ret = 512;
            }

            return ret;
        }
        public async Task<Device> GetDeviceByFormattedDeviceNameAsync(String formattedDeviceName)
        {
            return await db.Devices.SingleOrDefaultAsync(d => d.name == formattedDeviceName.ToLower());
        }
        public Device GetDeviceByFormattedDeviceName(String formattedDeviceName)
        {
            return db.Devices.SingleOrDefault(d => d.name == formattedDeviceName.ToLower());
        }
        public async Task<Device> GetDeviceByMakeModelCapacityAsync(String make, String model, String capacity)
        {
            return await db.Devices.SingleOrDefaultAsync(d => d.make.ToLower() == make.ToLower() &&
                                                              d.model.ToLower() == model.ToLower() &&
                                                              d.capacity.ToLower() == capacity.ToLower() + "gb");
        }
        public Device GetDeviceByMakeModelCapacity(String make, String model, String capacity)
        {
            return db.Devices.SingleOrDefault(d => d.make.ToLower() == make.ToLower() &&
                                                             d.model.ToLower() == model.ToLower() &&
                                                             d.capacity.ToLower() == capacity.ToLower() + "gb");
        }

        public async Task<Tuple<FortressCodesDomain.DbModels.Device, Boolean>> GetDBDeviceOrUnknownDeviceAsync(String make,
                                                                                                               String capacity,
                                                                                                               String model,
                                                                                                               String countryIso)
        {
            FortressCodesDomain.DbModels.Device fcdDevice = null;
            Boolean bIsDeviceMissing = false;

            var calcStorage = DeviceSizeHelper.CalculateDeviceTotalSizeFromRaw(capacity);

            var masterDevice = await GetDeviceByMakeModelCapacityAsync(make, model, calcStorage.ToString());
            if (masterDevice == null)
            {
                var unknownDevice = await db.Devices.SingleOrDefaultAsync(d => d.name.ToLower().Contains("unknown"));
                if (unknownDevice != null)
                {
                    //if returning the master unknown device, the users device is missing
                    fcdDevice = unknownDevice;
                    bIsDeviceMissing = true;
                }
            }
            else
            {
                fcdDevice = masterDevice;
            }

            return new Tuple<Device, bool>(fcdDevice, bIsDeviceMissing);
        }
        public Tuple<FortressCodesDomain.DbModels.Device, Boolean> GetDBDeviceOrUnknownDevice(String make,
                                                                                                               String capacity,
                                                                                                               String model,
                                                                                                               String countryIso)
        {
            FortressCodesDomain.DbModels.Device fcdDevice = null;
            Boolean bIsDeviceMissing = false;

            var calcStorage = DeviceSizeHelper.CalculateDeviceTotalSizeFromRaw(capacity);

            var masterDevice = GetDeviceByMakeModelCapacity(make, model, calcStorage.ToString());
            if (masterDevice == null)
            {
                var unknownDevice = db.Devices.SingleOrDefault(d => d.name.ToLower().Contains("unknown"));
                if (unknownDevice != null)
                {
                    //if returning the master unknown device, the users device is missing
                    fcdDevice = unknownDevice;
                    bIsDeviceMissing = true;
                }
            }
            else
            {
                fcdDevice = masterDevice;
            }

            return new Tuple<Device, bool>(fcdDevice, bIsDeviceMissing);
        }
        public async Task<tbl_PreloadedDevice> GetPreloadedDeviceByImei(string imei, string countryISO)
        {
            tbl_PreloadedDevice dev;
            bool deviceFound = false;
            dev = await db.tbl_PreloadedDevices.SingleOrDefaultAsync(d => d.Imei == imei);
            if (dev != null)
            {
                var voucher = await db.Vouchers.SingleOrDefaultAsync(d => d.vouchercode == dev.VoucherID);
                if (voucher != null)
                {
                    VoucherMetadata vmd = voucher.VoucherMetadatas.SingleOrDefault();
                    if (vmd != null)
                    {
                        PricingModel pm = vmd.PricingModel;
                        if (pm.Country != null)
                        {
                            if (pm.Country.ISO == countryISO)
                                deviceFound = true;
                        }
                    }
                }
            }
            if (!deviceFound)
                dev = null;

            return dev;
        }
        public async Task<Tuple<Boolean, String>> GetDeviceLevelAsync(String make, String model, String capacity,
                                                                      String voucherCode, String countryIso)
        {
            Boolean bIsUnknownDevice = false;
            String sLevelName = null;

            String deviceCapacity = DeviceSizeHelper.CalculateDeviceTotalSizeFromRaw(capacity).ToString();

            var voucher = db.Vouchers.SingleOrDefault(v => v.vouchercode == voucherCode);

            //Int32? iPartnerID = null;
            PricingModel pricingModel = null;
            if (voucher != null)
            {
                var metadata = voucher.VoucherMetadatas.FirstOrDefault();
                if (metadata != null)
                {
                    if (metadata.PricingModel != null)
                    {
                        pricingModel = metadata.PricingModel;
                        //iPartnerID = metadata.PricingModel.PartnerId;
                    }
                }
            }

            //match on the formatted name, and the partner id
            //TODO: include country lookup, but is it country of device or voucher

            //Check if the device the user has registered with is known to the system, if not return the unknown device
            //var device = await GetDeviceByMakeModelCapacityAsync(make, model, capacity);
            if (pricingModel != null)
            {
                var device = await GetDeviceByMakeModelCapacityAsync(make, model, deviceCapacity);
                if (device == null)
                {
                    var unknownDevice = await GetDeviceByFormattedDeviceNameAsync("Unknown Device");
                    if (unknownDevice != null)
                    {
                        bIsUnknownDevice = true;
                        //find the unknown device level that matches the voucher level
                        var unknownDeviceLevel = unknownDevice.DeviceLevels.SingleOrDefault(dl => dl.LevelId == pricingModel.LevelId);
                        if (unknownDeviceLevel != null)
                        {
                            sLevelName = unknownDeviceLevel.Level.Name;
                        }
                    }
                }
                else
                {
                    var deviceLevel = await GetDeviceLevelByDeviceDetailsAsync(make,
                                                                               model,
                                                                               deviceCapacity,
                                                                               countryIso,
                                                                               pricingModel);
                    if (deviceLevel != null)
                    {
                        sLevelName = deviceLevel.Level.Name;
                    }
                }
            }
            return new Tuple<Boolean, String>(bIsUnknownDevice, sLevelName);
        }
        public Tuple<Boolean, String> GetDeviceLevel(String make, String model, String capacity,
                                                                      String voucherCode, String countryIso)
        {
            Boolean bIsUnknownDevice = false;
            String sLevelName = null;

            String deviceCapacity = DeviceSizeHelper.CalculateDeviceTotalSizeFromRaw(capacity).ToString();

            var voucher = db.Vouchers.SingleOrDefault(v => v.vouchercode == voucherCode);

            //Int32? iPartnerID = null;
            PricingModel pricingModel = null;
            if (voucher != null)
            {
                var metadata = voucher.VoucherMetadatas.FirstOrDefault();
                if (metadata != null)
                {
                    if (metadata.PricingModel != null)
                    {
                        pricingModel = metadata.PricingModel;
                        //iPartnerID = metadata.PricingModel.PartnerId;
                    }
                }
            }

            //match on the formatted name, and the partner id
            //TODO: include country lookup, but is it country of device or voucher

            //Check if the device the user has registered with is known to the system, if not return the unknown device
            //var device = await GetDeviceByMakeModelCapacityAsync(make, model, capacity);
            if (pricingModel != null)
            {
                var device = GetDeviceByMakeModelCapacity(make, model, deviceCapacity);
                if (device == null)
                {
                    var unknownDevice = GetDeviceByFormattedDeviceName("Unknown Device");
                    if (unknownDevice != null)
                    {
                        bIsUnknownDevice = true;
                        //find the unknown device level that matches the voucher level
                        var unknownDeviceLevel = unknownDevice.DeviceLevels.SingleOrDefault(dl => dl.LevelId == pricingModel.LevelId);
                        if (unknownDeviceLevel != null)
                        {
                            sLevelName = unknownDeviceLevel.Level.Name;
                        }
                    }
                }
                else
                {
                    var deviceLevel = GetDeviceLevelByDeviceDetails(make,
                                                                               model,
                                                                               deviceCapacity,
                                                                               countryIso,
                                                                               pricingModel);
                    if (deviceLevel != null)
                    {
                        sLevelName = deviceLevel.Level.Name;
                    }
                }
            }
            return new Tuple<Boolean, String>(bIsUnknownDevice, sLevelName);
        }

        public async Task<DeviceLevel> GetDeviceLevelByDeviceDetailsAsync(String deviceMake, String deviceModel,
                                                                          String deviveCapactiy, String userDeviceCountryIso,
                                                                          PricingModel pricingModel)
        {
            DeviceLevel ret = null;
            var device = await db.Devices.SingleOrDefaultAsync(d =>
                d.make.ToLower() == deviceMake.ToLower() &&
                d.model.ToLower() == deviceModel.ToLower() &&
                d.capacity.ToLower() == deviveCapactiy.ToLower() + "gb");

            if (device != null)
            {
                if (pricingModel.Country.ISO == userDeviceCountryIso)
                {
                    ret = device.DeviceLevels.SingleOrDefault(dl => dl.PartnerId == pricingModel.PartnerId);
                }
            }
            return ret;
        }
        public DeviceLevel GetDeviceLevelByDeviceDetails(String deviceMake, String deviceModel,
                                                                          String deviveCapactiy, String userDeviceCountryIso,
                                                                          PricingModel pricingModel)
        {
            DeviceLevel ret = null;
            var device = db.Devices.SingleOrDefault(d =>
                d.make.ToLower() == deviceMake.ToLower() &&
                d.model.ToLower() == deviceModel.ToLower() &&
                d.capacity.ToLower() == deviveCapactiy.ToLower() + "gb");

            if (device != null)
            {
                if (pricingModel.Country.ISO == userDeviceCountryIso)
                {
                    ret = device.DeviceLevels.SingleOrDefault(dl => dl.PartnerId == pricingModel.PartnerId);
                }
            }
            return ret;
        }
        public async Task<PricingModel> GetPricingModelByDevicePartnerTierLevelAsync(Int32 partnerID, String deviceLevel, String tierName)
        {
            PricingModel ret = null;

            ret = await db.PricingModels.SingleOrDefaultAsync(pm => pm.PartnerId == partnerID &&
            pm.Level.Name == deviceLevel &&
                                                                    pm.Tier.Name == tierName);

            return ret;
        }

        public async Task<PricingModel> GetPricingModelByDevicePartnerFamilyAsync(String deviceLevel, Int32 tierId, Int32 familyId)
        {
            PricingModel ret = null;

            ret = await db.PricingModels.SingleOrDefaultAsync(pm => pm.FamilyId == familyId &&
                                                                    pm.Level.Name == deviceLevel &&
                                                                    pm.TeirId == tierId);

            return ret;
        }
        public PricingModel GetPricingModelByDevicePartnerFamily(String deviceLevel, Int32 tierId, Int32 familyId)
        {
            PricingModel ret = null;

            ret = db.PricingModels.SingleOrDefault(pm => pm.FamilyId == familyId &&
                                                                    pm.Level.Name == deviceLevel &&
                                                                    pm.TeirId == tierId);

            return ret;
        }
        public async Task<IEnumerable<PricingModel>> GetActivePricingModelByFamilyAsync(int familyId)
        {
            IEnumerable<PricingModel> ret = null;

            ret = await db.PricingModels.Where(pm => pm.FamilyId == familyId &&
                                                     pm.Active == true).ToListAsync();

            return ret;
        }

        public async Task<PricingModel> GetActivePricingModelByDevicePartnerFamilyAsync(String deviceLevel, Int32 tierId, Int32 familyId)
        {
            PricingModel ret = null;

            ret = await db.PricingModels.SingleOrDefaultAsync(pm => pm.FamilyId == familyId &&
                                                                    pm.Level.Name == deviceLevel &&
                                                                    pm.TeirId == tierId &&
                                                                    pm.Active == true);

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
