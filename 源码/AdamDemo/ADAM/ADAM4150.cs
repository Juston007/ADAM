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
    public class ADAM4150 : ADAM
    {
        private bool[] dI = new bool[7];
        private bool[] dO = new bool[8];

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model">串口设置模型</param>
        public ADAM4150(ComSettingModel model)
            : base(model)
        {

        }

        /// <summary>
        /// 建立起与数字量设备连接
        /// </summary>
        /// <returns>连接状态</returns>
        public bool Connect()
        {
            if (ADAM.arrPort.ContainsKey(model.DigitalCOM))
            {
                Port = (SerialPort)arrPort[model.DigitalCOM.ToString()];
                base.isOpen = true;
                return true;
            }
            else
            {
                bool result = base.Connect(model.DigitalCOM);
                if (result)
                    arrPort.Add(model.DigitalCOM.ToString(), Port);
                return result;
            }
        }

        public override bool Close()
        {
            if (arrPort.ContainsKey(model.DigitalCOM.ToString()))
                arrPort.Remove(model.DigitalCOM.ToString());
            return base.Close();
        }

        /// <summary>
        /// 获取输入输出数据
        /// </summary>
        public void setData()
        {
            int responselength = 6;
            int direadlength = dI.Length;
            int doreadlength = dO.Length;

            byte[] cmd = new byte[] { 0x01, 0x01, 0x00, 0x00, 0x00, (byte)direadlength, 0x00, 0x00 };
            byte[] temp = new byte[6] { cmd[0], cmd[1], cmd[2], cmd[3], cmd[4], cmd[5] };
            //计算校验位
            byte[] crc = Util.CRC16_C(temp);
            //低位在前 高位在后
            cmd[6] = crc[1];
            cmd[7] = crc[0];
            //发送获取数据命令
            base.SendCommand(cmd);
            //接受数据
            byte[] rdi = base.ReceiveData(responselength);

            cmd[6] = (byte)doreadlength;
            cmd[3] = 0x10;
            temp = new byte[6] { cmd[0], cmd[1], cmd[2], cmd[3], cmd[4], cmd[5] };
            crc = Util.CRC16_C(temp);
            cmd[6] = crc[1];
            cmd[7] = crc[0];
            base.SendCommand(cmd);
            byte[] rdo = base.ReceiveData(responselength);

            int templength = 0;
            String distr = Convert.ToString(rdi[3], 2);
            String dostr = Convert.ToString(rdo[3], 2);

            //补0
            while ((templength = direadlength - distr.Length) != 0)
            {
                distr = '0' + distr;
            }

            while ((templength = doreadlength - dostr.Length) != 0)
            {
                dostr = '0' + dostr;
            }

            char[] arrdi = distr.ToArray();
            char[] arrdo = dostr.ToArray();

            //将结果赋值给属性
            for (int i = 0; i <= 7; i++)
            {
                if (i < 7)
                {
                    dI[i] = arrdi[direadlength - 1 - i] == '1';
                }
                dO[i] = arrdo[doreadlength - 1 - i] == '1';
            }
        }

        /// <summary>
        /// 控制输出
        /// </summary>
        /// <param name="index">序号</param>
        /// <param name="status">状态</param>
        /// <returns>执行结果</returns>
        public bool Switchs(int index, bool status)
        {
            byte[] cmd = new byte[] { 0x01, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            if (index <= 7 && index >= 0)
            {
                //内存位置
                cmd[3] = (byte)(index + 0x10);
                //数据 FF开 00关
                cmd[4] = (byte)(status ? 0xFF : 0x00);
                //计算校验位
                byte[] crc = Util.CRC16_C(new byte[] { cmd[0], cmd[1], cmd[2], cmd[3], cmd[4], cmd[5] });
                cmd[6] = crc[1];
                cmd[7] = crc[0];
                //发送命令
                base.SendCommand(cmd);
                //接受响应 并判断是不是执行成功
                byte[] buf = base.ReceiveData(cmd.Length);
                for (int i = 0; i < cmd.Length - 1; i++)
                {
                    if (cmd[i] != buf[i])
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 获取输入状态集合
        /// </summary>
        /// <returns>输入状态集合</returns>
        public bool[] getDI()
        {
            return dI.ToArray();
        }

        /// <summary>
        /// 获取输出状态集合
        /// </summary>
        /// <returns>输出状态集合</returns>
        public bool[] getDO()
        {
            return dO.ToArray();
        }

        #region 输入输出属性

        public bool DO7
        {
            get { return dO[7]; }
        }

        public bool DO6
        {
            get { return dO[6]; }
        }

        public bool DO5
        {
            get { return dO[5]; }
        }

        public bool DO4
        {
            get { return dO[4]; }
        }

        public bool DO3
        {
            get { return dO[3]; }
        }

        public bool DO2
        {
            get { return dO[2]; }
        }

        public bool DO1
        {
            get { return dO[1]; }
        }

        public bool DO0
        {
            get { return dO[0]; }
        }

        public bool DI6
        {
            get { return dI[6]; }
        }

        public bool DI5
        {
            get { return dI[5]; }
        }

        public bool DI3
        {
            get { return dI[3]; }
        }

        public bool DI4
        {
            get { return dI[4]; }
        }

        public bool DI2
        {
            get { return dI[2]; }
        }

        public bool DI1
        {
            get { return dI[1]; }
        }

        public bool DI0
        {
            get { return dI[0]; }
        }

        #endregion
    }
}
