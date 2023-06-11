using System;

namespace mqtt_report_generator
{
    class Program
    {
        static string brokerAddress = Environment.GetEnvironmentVariable("BROKER_ADDRESS");
        static int brokerPort = Convert.ToInt32(Environment.GetEnvironmentVariable("BROKER_PORT"));

        static void Main(string[] args)
        {
            Console.WriteLine("MQTT Report Generator - Broker Configuration");
            Console.WriteLine("-------------------------------------------");

            while (true)
            {
                Console.WriteLine("1. Set Broker Address");
                Console.WriteLine("2. Set Broker Port");
                Console.WriteLine("3. Start Report Generation");
                Console.WriteLine("4. Exit");
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
                        if (string.IsNullOrEmpty(brokerAddress))
                        {
                            Console.WriteLine("Broker address is not set. Please set it before generating the report.");
                        }
                        else if (brokerPort == 0)
                        {
                            Console.WriteLine("Broker port is not set. Please set it before generating the report.");
                        }
                        else
                        {
                            // Call your report generation function here
                            GenerateReport();
                        }
                        break;
                    case "4":
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
