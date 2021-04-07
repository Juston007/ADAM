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
    public class ComSettingModel
    {
        /// <summary>
        /// 模拟、数字串口
        /// </summary>
        private String analogCOM = "COM1", digitalCOM = "COM1";

        public String DigitalCOM
        {
            get { return digitalCOM; }
            set { digitalCOM = value; }
        }

        public String AnalogCOM
        {
            get { return analogCOM; }
            set { analogCOM = value; }
        }
    }
}
