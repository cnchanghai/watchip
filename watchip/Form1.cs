﻿using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Collections.Specialized;
using System.Text;
using System.Diagnostics;
namespace watchip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.label3.Text = null;
            this.label1.Text = ShowIP();

        }
        private string ShowIP()
        {
            string ips = null;
            //ipv4地址也可能不止一个
            foreach (string ip in GetLocalIpv4())
            {
                ips += ip.ToString() + '\n';
            }
            return ips;
        }
        private string[] GetLocalIpv4()
        {
            //事先不知道ip的个数，数组长度未知，因此用StringCollection储存
            IPAddress[] localIPs;
            localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            StringCollection IpCollection = new StringCollection();
            foreach (IPAddress ip in localIPs)
            {
                //根据AddressFamily判断是否为ipv4,如果是InterNetWorkV6则为ipv6
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    IpCollection.Add(ip.ToString());
            }
            string[] IpArray = new string[IpCollection.Count];
            IpCollection.CopyTo(IpArray, 0);
            return IpArray;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.label1.Text = ShowIP();
            bool stat = false;
            stat = multiping();
            if (stat)
            {
                System.Diagnostics.Process.Start("http://10.65.202.79:8001/Login.aspx");
            } else
            {
                MessageBox.Show("计算机网络不通，请拨打63333");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.label1.Text = ShowIP();
            bool stat = false;
            stat = multiping();
            if (stat == true)
            {
                this.label3.Text = "网络正常";
            }
            else
            {
                this.label3.Text = "网络不通";
                MessageBox.Show("计算机网络不通，请拨打63333");
            }
        }
    
    private bool multiping()
    {
        bool st1 = false;
        bool st2 = false;
        st1 = Ping("10.65.1.53");
        st2 = Ping("10.65.1.51");
        if (st1 == true || st2 == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool Ping(string ip)
    {

        System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
        System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();
        options.DontFragment = true;
        string data = "Test Data!";
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        int timeout = 1000; // Timeout 时间，单位：毫秒
        try
        {
            System.Net.NetworkInformation.PingReply reply = p.Send(ip, timeout, buffer, options);
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                return true;
            else
                return false;
        }
        catch
        {
        }
        return false;


    }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            //要执行的程序名称，cmd  
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            //可能接受来自调用程序的输入信息  
            p.StartInfo.RedirectStandardInput = true;
            //由调用程序获取输出信息  
            p.StartInfo.RedirectStandardOutput = true;
            //不显示程序窗口  
            p.StartInfo.CreateNoWindow = true;
            p.Start();//启动程序  
            //向CMD窗口发送输入信息：  
            p.StandardInput.WriteLine("net use s: /delete");
            //不过不要忘记加上Exit，不然程序会异常退出  
            p.StandardInput.WriteLine("exit");
            p.Close();
            p.Start();//启动程序  
            p.StandardInput.WriteLine("net use s: \\\\10.65.1.68\\asenfileshare");
            p.StandardInput.WriteLine("exit");
            p.Close();
        }
    }
}
