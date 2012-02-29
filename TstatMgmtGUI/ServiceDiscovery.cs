using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TstatMgmtGUI
{
    public partial class ServiceDiscovery : Form
    {
        public ServiceDiscovery()
        {
            InitializeComponent();
        }

        private void ServiceDiscovery_Load(object sender, EventArgs e)
        {          
            Boolean exception_thrown = false;
            
            Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            IPAddress send_to_address = IPAddress.Parse("239.255.255.250");
           
            IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 1900);


            string text_to_send = "TYPE: WM-DISCOVER\r\nVERSION: 1.0\nservices: devices.controller.tstat:1.0\r\n\r\n";
            byte[] send_buffer = Encoding.ASCII.GetBytes(text_to_send);
                
            try
            {
                sending_socket.SendTo(send_buffer, sending_end_point);
            }
            catch (Exception send_exception)
            {
                exception_thrown = true;
                richTextBox1.Text = " Exception " +  send_exception.Message;
            }
            if (exception_thrown == false)
            {
                richTextBox1.Text = "Message has been sent to the broadcast address";
            }
            else
            {
                exception_thrown = false;
                richTextBox1.Text = "The exception indicates the message was not sent.";           
                
            }

            Thread t = new Thread(listen);
            t.Start();
        }

        private void listen()
        {
            UdpClient listener = new UdpClient(1900);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 1900);
            int cnt = 0;

            while (cnt < 30)
            {
                byte[] receive_byte_array = listener.Receive(ref groupEP);
                String str = cnt.ToString() + ": " + Encoding.ASCII.GetString(receive_byte_array);
                richTextBox1.Invoke((MethodInvoker)(() => richTextBox1.Text = str));
                Thread.Sleep(1000);

                cnt++;
            }

        }
    }
}
