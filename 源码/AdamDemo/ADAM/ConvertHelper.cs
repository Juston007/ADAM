using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADAM
{
    /// <summary>
    /// Written By Juston
    /// </summary>
    public class ConvertHelper
    {
        /// <summary>
        /// 温度
        /// </summary>
        /// <param name="value">Vin数值</param>
        /// <returns>温度值</returns>
        public static double Temperature(int value)
        {
            int max = 60, min = -10;
            return getValue(max,min,value);
        }

        /// <summary>
        /// 光照
        /// </summary>
        /// <param name="value">Vin数值</param>
        /// <returns>光照值</returns>
        public static double Ligth(int value)
        {
            int max = 20000, min = 0;
            return getValue(max, min, value);
        }

        /// <summary>
        /// 湿度
        /// </summary>
        /// <param name="value">Vin数值</param>
        /// <returns>湿度值</returns>
        public static double Humidity(int value)
        {
            int max = 100, min = 50;
            return getValue(max, min, value);
        }

        /// <summary>
        /// 风速
        /// </summary>
        /// <param name="value">Vin数值</param>
        /// <returns>风速值</returns>
        public static double WindSpeed(int value)
        {
            int max = 70, min = 0;
            return getValue(max, min, value);
        }

        /// <summary>
        /// 大气压力
        /// </summary>
        /// <param name="value">Vin数值</param>
        /// <returns>大气压力值</returns>
        public static double Barometic(int value)
        {
            int max = 110, min = 0;
            return getValue(max, min, value);
        }

        /// <summary>
        /// Co2
        /// </summary>
        /// <param name="value">Vin数值</param>
        /// <returns>Co2值</returns>
        public static double Co2(int value)
        {
            int max = 5000, min = 0;
            return getValue(max, min, value);
        }

        /// <summary>
        /// 空气质量
        /// </summary>
        /// <param name="value">Vin数值</param>
        /// <returns>空气质量值</returns>
        public static double AirQuality(int value)
        {
            int max = 100, min = 0;
            return getValue(max, min, value);
        }

        /// <summary>
        /// 自定义量程来获取传感值
        /// </summary>
        /// <param name="max">量程最大值</param>
        /// <param name="min">量程最小值</param>
        /// <param name="value">Vin值</param>
        /// <returns>传感数据</returns>
        public static double getValue(int max, int min, int value)
        {
            if (value == (int)0xFFFF)
                return max;
            if (value == (int)0x00)
                return min;
            double temp = max - min;
            temp = temp / (int)0xFFFF;
            return min + (temp * value);
        }
    }
}
