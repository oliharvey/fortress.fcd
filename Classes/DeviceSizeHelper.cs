using System;
using System.Text.RegularExpressions;

namespace FortressCodesDomain
{
    public static class DeviceSizeHelper
    {
        /// <summary>
        /// Returns the closest size of for the device size from the raw
        /// </summary>
        /// <param name="deviceLevelRequest">The device level request.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Extract the decimal part of the device capacity raw
        /// </summary>
        /// <param name="deviceCapacityRaw">The device capacity raw e.g "12.56 GB"</param>
        /// <returns></returns>
        private static Double ExtractDoubleFromDeviceCapacityRaw(String deviceCapacityRaw)
        {
            Regex digitsRegex = new Regex(@"^\D*?((-?(\d+(\.\d+)?))|(-?\.\d+)).*");
            Match mx = digitsRegex.Match(deviceCapacityRaw);
            return mx.Success ? Convert.ToDouble(mx.Groups[1].Value) : 0;
        }

    }
}
