﻿using System;

namespace mqtt_report_generator
{
    class Program
    {
        static string brokerAddress = Environment.GetEnvironmentVariable("BROKER_ADDRESS");
        static int brokerPort = Convert.ToInt32(Environment.GetEnvironmentVariable("BROKER_PORT"));

        static string device;
        static string version;
        static string mac;

        static void Main(string[] args)
        {
            Console.WriteLine("MQTT Report Generator - Broker Configuration");
            Console.WriteLine("-------------------------------------------");

            while (true)
            {
                Console.WriteLine("1. Set Broker Address");
                Console.WriteLine("2. Set Broker Port");
                Console.WriteLine("3. Set Device");
                Console.WriteLine("4. Set Version");
                Console.WriteLine("5. Set MAC Address");
                Console.WriteLine("6. Start Report Generation");
                Console.WriteLine("7. Exit");
                Console.WriteLine();

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter broker address: ");
                        brokerAddress = Console.ReadLine();
                        Console.WriteLine("Broker address set to: " + brokerAddress);
                        break;
                    case "2":
                        Console.Write("Enter broker port: ");
                        int.TryParse(Console.ReadLine(), out brokerPort);
                        Console.WriteLine("Broker port set to: " + brokerPort);
                        break;
                    case "3":
                        Console.Write("Enter device: ");
                        device = Console.ReadLine();
                        Console.WriteLine("Device set to: " + device);
                        break;
                    case "4":
                        Console.Write("Enter version: ");
                        version = Console.ReadLine();
                        Console.WriteLine("Version set to: " + version);
                        break;
                    case "5":
                        Console.Write("Enter MAC address: ");
                        mac = Console.ReadLine();
                        Console.WriteLine("MAC address set to: " + mac);
                        break;
                    case "6":
                        if (string.IsNullOrEmpty(brokerAddress))
                        {
                            Console.WriteLine("Broker address is not set. Please set it before generating the report.");
                        }
                        else if (brokerPort == 0)
                        {
                            Console.WriteLine("Broker port is not set. Please set it before generating the report.");
                        }
                        else if (string.IsNullOrEmpty(device))
                        {
                            Console.WriteLine("Device is not set. Please set it before generating the report.");
                        }
                        else if (string.IsNullOrEmpty(version))
                        {
                            Console.WriteLine("Version is not set. Please set it before generating the report.");
                        }
                        else if (string.IsNullOrEmpty(mac))
                        {
                            Console.WriteLine("MAC address is not set. Please set it before generating the report.");
                        }
                        else
                        {
                            // Call your report generation function here
                            GenerateReport();
                        }
                        break;
                    case "7":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static void GenerateReport()
        {
            // TODO: Implement your report generation logic here
            Console.WriteLine("Generating report...");
            // Perform MQTT connection and report generation tasks
            // You can use a library like MQTTnet to handle the MQTT communication

            Console.WriteLine("Report generation completed.");
        }
    }
}
