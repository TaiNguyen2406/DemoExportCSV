using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyModbus;
namespace DemoExportCSV
{
    public partial class frmDocDuLieu : Form
    {
        ModbusClient modbusClient;
        public frmDocDuLieu()
        {
            InitializeComponent();
        }

        private void frmDocDuLieu_Load(object sender, EventArgs e)
        {
            txtIP.Text = GetIPAddress();

        }
        public static string GetIPAddress()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                modbusClient = new ModbusClient(txtIP.Text, 502);    //Ip-Address and Port of Modbus-TCP-Server
                modbusClient.Connect();
                lblStatus.Text = "Connected";
                timer2.Enabled = true;
            }
            catch (Exception ex)
            {
                timer2.Enabled = false;
                lblStatus.Text = ex.ToString();
                throw;
            }
        }

        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            modbusClient.Disconnect();
            lblStatus.Text = "Disconnected";
            timer2.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            bool[] readCoils = modbusClient.ReadCoils(0, 10);                        //Read 10 Coils from Server, starting with address 0
            int[] readHoldingRegisters = modbusClient.ReadHoldingRegisters(0, 10);
            txtGiaTri1.Text = readHoldingRegisters[0].ToString ();
            txtGiaTri2.Text = readHoldingRegisters[1].ToString();
            chk1.Checked = readCoils[0];
            chk2.Checked = readCoils[1];
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  modbusClient.WriteMultipleCoils(4, new bool[] { true, false, true, true, true, true, true, true, true, true });    //Write 10 Coils starting with Address 5, ghi nhiều ô coil 1 lần
         //   modbusClient.WriteSingleCoil(20, true); // ghi 1 ô coil 1 lần
         //   modbusClient.WriteMultipleRegisters(0, new int[] { 1, 2, 3, 4, 5, 6 }); //ghi nhiều ô register 1 lần
            modbusClient.WriteSingleRegister(Decimal.ToInt32(nudRegNoW.Value), Decimal.ToInt32( nudRegValue.Value));
            modbusClient.WriteSingleCoil(Decimal.ToInt32(nudCoilNoW.Value), chkCoilValue .Checked);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            bool[] readCoils = modbusClient.ReadCoils(Decimal.ToInt32(nudCoil1.Value), 1);
            bool[] readCoils2 = modbusClient.ReadCoils(Decimal.ToInt32(nudCoil2.Value), 1);
            int[] readHoldingRegisters = modbusClient.ReadHoldingRegisters(Decimal.ToInt32(nudReg1.Value), 1);
            int[] readHoldingRegisters2 = modbusClient.ReadHoldingRegisters(Decimal.ToInt32(nudReg2.Value), 1);
            int[] readInputRegisters = modbusClient.ReadInputRegisters(0, 1);// đọc analogue input
          
            txtGiaTri1.Text = readHoldingRegisters[0].ToString();
            txtGiaTri2.Text = readHoldingRegisters2[0].ToString();
            chk1.Checked = readCoils[0];
            chk2.Checked = readCoils2[0];
            txtInputReg.Text = readInputRegisters[0].ToString();
            timer2.Enabled = true;
        }
    }
}
