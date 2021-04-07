using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Collections;

namespace ADAM
{
    /// <summary>
    /// Written By Juston
    /// </summary>
    public class ADAM
    {
        /// <summary>
        /// 串口设置模型
        /// </summary>
        protected ComSettingModel model = null;
        /// <summary>
        /// 当前使用的串口
        /// </summary>
        private SerialPort port = null;

        public bool isOpen = false;


        /// <summary>
        /// 读取锁
        /// </summary>
        private static object readLock = new object();
        /// <summary>
        /// 写入锁
        /// </summary>
        private static object writeLock = new object();

        protected static Hashtable arrPort = new Hashtable();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="model">串口设置模型</param>
        public ADAM(ComSettingModel model)
        {
            this.model = model;
        }


        /// <summary>
        /// 连接
        /// </summary>
        /// <returns>连接状态</returns>
        protected virtual bool Connect(String comstr)
        {
            port = new SerialPort(comstr, 9600);
            try
            {
                port.Open();
                isOpen = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isOpen = false;
                return false;
            }
        }

        /// <summary>
        /// 释放连接
        /// </summary>
        /// <returns>释放状态</returns>
        public virtual bool Close()
        {
            bool result = false;
            try
            {
                port.Close();
                isOpen = false;
                result = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                isOpen = false;
                result = false;
            }
            return result;
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="data">命令</param>
        public void SendCommand(byte[] data)
        {
            if (isOpen && port != null && data != null)
            {
                lock (writeLock)
                {
                    port.Write(data, 0, data.Length);
                }
            }
        }

        /// <summary>
        /// 接受数据
        /// </summary>
        /// <param name="offset">从何处开始写</param>
        /// <param name="readlength">读取长度</param>
        /// <returns>数据</returns>
        public byte[] ReceiveData(int readlength)
        {
            if (isOpen && port != null && readlength > 0)
            {
                byte[] data = new byte[readlength];
                int i = 0;
                lock (readLock)
                {
                    //读取数据
                    while ((i += port.Read(data, i, readlength - i)) != readlength) ;
                }
                return data;
            }
            else
                return null;
        }


        public SerialPort Port
        {
            get { return port; }
            set { port = value; }
        }
    }
}
