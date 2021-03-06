﻿using Microsoft.Kinect;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace kinect1
{
    class Program
    {

        UdpClient client;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Program p = new Program();
            p.createUdpConnection();
            p.StartKinectST();
        }


        KinectSensor kinect = null;
        Skeleton [] skeletonData = null;

        void StartKinectST()
        {
            kinect = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected); // Get first Kinect Sensor
            kinect.SkeletonStream.Enable(); // Enable skeletal tracking

            skeletonData = new Skeleton[kinect.SkeletonStream.FrameSkeletonArrayLength]; // Allocate ST data

            kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady); // Get Ready for Skeleton Ready Events
            
            kinect.Start(); // Start Kinect sensor
            while (true) { };
            
        }

        private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame()) // Open the Skeleton frame
            {
                if (skeletonFrame != null && this.skeletonData != null) // check that a frame is available
                {
                    skeletonFrame.CopySkeletonDataTo(this.skeletonData); // get the skeletal information in this frame
                }
            }
            
            foreach(var s in skeletonData)
            {
                Joint head = s.Joints[JointType.Head];
                if(s.Position.X == 0 && s.Position.Y == 0 && s.Position.Z == 0)
                {
                    continue;
                }
                String msg = "";
                var jointTypes = Enum.GetValues(typeof(JointType));
                foreach (JointType type in jointTypes)
                {
                    msg = msg + s.Joints[type].Position.X + ";" + s.Joints[type].Position.Y + ";" + (-s.Joints[type].Position.Z) + ";";
                }
                msg = msg.Replace(',', '.');
                sendToVR(msg);
            }
            //Console.WriteLine();
        }

        private void createUdpConnection()
        {
            client = new UdpClient(); //115  //38
            Console.WriteLine("Podaj ip");
            string ipAddres;
            ipAddres = Console.ReadLine();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ipAddres), 8082);
            //IPEndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.0.115"), 8082);
            client.Connect(ep);
        }

        private void sendToVR(string text)
        {
            byte[] msg = Encoding.ASCII.GetBytes(text);
            int numberOfBytes = text.Length;
            client.Send(msg, numberOfBytes);
        }

    }
}
