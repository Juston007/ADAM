using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace ADAM
{
    /// <summary>
    /// Written By Juston
    /// </summary>
    public class ADAM4017 : ADAM
    {
        private int[] vin = new int[8];


        public ADAM4017(ComSettingModel model)
            : base(model)
        {

        }

        public bool Connect()
        {
            if (ADAM.arrPort.ContainsKey(model.AnalogCOM))
            {
                Port = (SerialPort)arrPort[model.AnalogCOM.ToString()];
                base.isOpen = true;
                return true;
            }
            else
            {
                bool result = base.Connect(model.AnalogCOM);
                if (result)
                    arrPort.Add(model.AnalogCOM.ToString(), Port);
                return result;
            }
        }

        public override bool Close()
        {
            if (arrPort.ContainsKey(model.AnalogCOM.ToString()))
                arrPort.Remove(model.AnalogCOM.ToString());
            return base.Close();
        }

        public void setData()
        {
            byte[] cmd = new byte[] { 0x02, 0x04, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00 };
            byte[] crc = Util.CRC16_C(new byte[] { cmd[0], cmd[1], cmd[2], cmd[3], cmd[4], cmd[5] });
            cmd[6] = crc[1];
            cmd[7] = crc[0];
            base.SendCommand(cmd);
            //读取长度
            //地址 功能 字节 数据*8*2 CRC 共21位
            byte[] data = base.ReceiveData(21);
            if (data == null)
                return;
            int temp = 0;
            int count = 0;
            //将值设置到属性当中
            for (int i = 3; i < 19; i += 2)
            {
                temp = data[i];
                temp = temp << 8;
                temp += data[i + 1];
                vin[count] = temp;
                count++;
            }
        }

        public int[] Vin
        {
            get { return vin.ToArray(); }
        }

        #region Vin0~7

        public int Vin0
        {
            get { return vin[0]; }
            set { vin[0] = value; }
        }

        public int Vin1
        {
            get { return vin[1]; }
            set { vin[1] = value; }
        }

        public int Vin2
        {
            get { return vin[2]; }
            set { vin[2] = value; }
        }

        public int Vin3
        {
            get { return vin[3]; }
            set { vin[3] = value; }
        }

        public int Vin4
        {
            get { return vin[4]; }
            set { vin[4] = value; }
        }

        public int Vin5
        {
            get { return vin[5]; }
            set { vin[5] = value; }
        }

        public int Vin6
        {
            get { return vin[6]; }
            set { vin[6] = value; }
        }

        public int Vin7
        {
            get { return vin[7]; }
            set { vin[7] = value; }
        }
        #endregion
    }
}
