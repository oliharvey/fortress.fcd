using System;
using System.ComponentModel;
using System.Reflection;

namespace FortressCodesDomain
{
    public class Enumerations
    {
        public enum VoucherType
        {
            Voucher = 1,
            Promo = 2,
            Gateway = 3,
            Deductible = 4,
            Billing = 5,
            Subscription = 7,
            NonPhone = 8
        }

        public enum MethodName
        {
            [Description("Validate")]
            Validate,
            [Description("Activate")]
            Activate,
            [Description("ChangeCoverage")]
            ChangeCoverage
        }

        public enum TransactionType
        {
            [Description("Validation Request")]
            ValidationRequest,
            [Description("Validated")]
            Validated,
            [Description("In Use")]
            InUse,
            [Description("Locked")]
            Locked,
            [Description("Maximum Use")]
            MaximumUse,
            [Description("Time Limit")]
            TimeLimit,
            [Description("Error")]
            Error,
            [Description("Activation Request")]
            ActivationRequest,
            [Description("Activated")]
            Activated,
            [Description("Cancellation Approved")]
            CancellationApproved,
            [Description("Cancellation Declined")]
            CancellationDeclined,
            [Description("Cancelled")]
            Cancelled,
            [Description("Cancellation Request")]
            CancellationRequest,
            [Description("Returned")]
            Returned,
            [Description("Category Mismatch")]
            CategoryMismatch,
            [Description("Territory Mismatch")]
            TerritoryMismatch,
            [Description("Invalid voucher code")]
            InvalidVoucherCode,
            [Description("Device Level Request")]
            DeviceLevelRequest,
            [Description("No Device Level")]
            NoDeviceLevel,
            [Description("Gateway Voucher Alteration")]
            GatewayVoucherAlteration,
            [Description("Created")]
            Created,
            [Description("Missing Family Level")]
            MissingFamilyLevel,
            [Description("Change Coverage")]
            ChangeCoverage,
            [Description("Missing Device")]
            MissingDevice,
            [Description("Missing Device Activation")]
            MissingDeviceActivation,
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            T val = ((T[])Enum.GetValues(typeof(T)))[0];

            foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
            {
                if (Convert.ToInt32(enumValue).Equals(intValue))
                {
                    val = enumValue;
                    break;
                }
            }
            return val;
        }
    }
}