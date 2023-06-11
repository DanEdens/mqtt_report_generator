using System;
using System.Collections.Generic;
using System.IO;

namespace mqtt_report_generator
{
    class Program
    {
        static int brokerPort = Convert.ToInt32(Environment.GetEnvironmentVariable("BROKER_PORT"));

        public static string BrokerAddress { get; set; } = Environment.GetEnvironmentVariable("BROKER_ADDRESS");
        public static string Device { get; set; } = Environment.GetEnvironmentVariable("DUT_DEVICE");
        public static string Version { get; set; } = Environment.GetEnvironmentVariable("DUT_VERSION");
        public static string Mac { get; set; } = Environment.GetEnvironmentVariable("DUT_MAC_ADDRESS");

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
                Console.WriteLine("7. Print Variables");
                Console.WriteLine("8. Save Variables");
                Console.WriteLine("9. Exit");
                Console.WriteLine();

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        BrokerAddress = GetInput("Enter broker address: ");
                        Console.WriteLine($"Broker address set to: {BrokerAddress}");
                        break;
                    case "2":
                        int.TryParse(GetInput("Enter broker port: "), out brokerPort);
                        Console.WriteLine($"Broker port set to: {brokerPort}");
                        break;
                    case "3":
                        Device = GetInput("Enter device: ");
                        Console.WriteLine($"Device set to: {Device}");
                        break;
                    case "4":
                        Version = GetInput("Enter version: ");
                        Console.WriteLine($"Version set to: {Version}");
                        break;
                    case "5":
                        Mac = GetInput("Enter MAC address: ");
                        Console.WriteLine($"MAC address set to: {Mac}");
                        break;
                    case "6":
                        if (string.IsNullOrEmpty(BrokerAddress))
                        {
                            Console.WriteLine("Broker address is not set. Please set it before generating the report.");
                        }
                        else if (brokerPort == 0)
                        {
                            Console.WriteLine("Broker port is not set. Please set it before generating the report.");
                        }
                        else if (string.IsNullOrEmpty(Device))
                        {
                            Console.WriteLine("Device is not set. Please set it before generating the report.");
                        }
                        else if (string.IsNullOrEmpty(Version))
                        {
                            Console.WriteLine("Version is not set. Please set it before generating the report.");
                        }
                        else if (string.IsNullOrEmpty(Mac))
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
                        PrintVariables();
                        break;
                    case "8":
                        SaveVariables();
                        break;
                    case "9":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        static string GetInput(string message)
        {
            Console.Write(message);
            return Console.ReadLine();
        }

        static void GenerateReport()
        {
            try
            {
                // Create an instance of MqttClient with the broker address and port
                var mqttClient = new MqttClient(BrokerAddress, brokerPort);

                // Connect to the MQTT broker
                mqttClient.Connect();

                // Subscribe to the desired topics
                mqttClient.Subscribe("AppTestKit/log/#");

                // Create an instance of MqttDataProcessor with the log folder path
                var dataProcessor = new MqttDataProcessor(LogFolderPath);

                // Process the data and generate the report
                dataProcessor.ProcessData();

                // Disconnect from the MQTT broker
                mqttClient.Disconnect();

                Console.WriteLine("Report generation completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during report generation: {ex.Message}");
            }
        }



        static void PrintVariables()
        {
            Console.WriteLine("Current Variables:");
            Console.WriteLine("------------------");
            Console.WriteLine($"Broker Address: {BrokerAddress}");
            Console.WriteLine($"Broker Port: {brokerPort}");
            Console.WriteLine($"Device: {Device}");
            Console.WriteLine($"Version: {Version}");
            Console.WriteLine($"MAC Address: {Mac}");
        }

        static void SaveVariables()
        {
            try
            {
                // Create a dictionary to store the variables and their values
                var variables = new Dictionary<string, string>
                {
                    { "BROKER_ADDRESS", BrokerAddress },
                    { "BROKER_PORT", brokerPort.ToString() },
                    { "DUT_DEVICE", Device },
                    { "DUT_VERSION", Version },
                    { "DUT_MAC_ADDRESS", Mac }
                };

                // Create or overwrite the configuration file
                using (var writer = new StreamWriter("config.txt"))
                {
                    foreach (var variable in variables)
                    {
                        // Write each variable and its value in the format "KEY=VALUE"
                        writer.WriteLine($"{variable.Key}={variable.Value}");
                    }
                }

                Console.WriteLine("Variables saved to the configuration file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving the variables: {ex.Message}");
            }
        }
    }
}
