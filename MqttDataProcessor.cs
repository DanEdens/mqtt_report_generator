using MQTTnet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace mqtt_report_generator
{
    public class MqttDataProcessor
    {
        private string logFolderPath;
        private List<string> dvtValues;
        private MqttClient mqttClient;

        public MqttDataProcessor(string logFolderPath, string brokerAddress, int brokerPort)
        {
            this.logFolderPath = logFolderPath;
            // Retrieve the dvt-#### values from the MQTT topic
            var dvtTopic = "AppTestKit/log/testlist";

            // Instantiate an MQTT client object
            mqttClient = new MqttClient(brokerAddress, brokerPort);
            MqttClient.Connect().GetAwaiter().GetResult();

            // Retrieve the dvtMessage using the MQTT client library
            MqttApplicationMessage dvtMessage = mqttClient.RetrieveMessage(dvtTopic).GetAwaiter().GetResult();

            // Extract the dvt-#### values from the message
            string[] dvtValueArray = dvtMessage.Payload.ToString().Split(',');

            // Convert the array to a list
            dvtValues = new List<string>(dvtValueArray);
        }

        public void ProcessData()
        {
            // Get the log files from the specified folder
            string[] logFiles = Directory.GetFiles(logFolderPath, "*.log");

            // Prepare the CSV file path
            string csvFilePath = Path.Combine(logFolderPath, "report.csv");

            // Create or append to the CSV file
            using (var writer = new StreamWriter(csvFilePath, true))
            {
                // Write the header row if the file is empty
                if (writer.BaseStream.Position == 0)
                {
                    writer.WriteLine("Device,Version,MAC," + string.Join(",", dvtValues));
                }

                // Process each log file
                foreach (string logFile in logFiles)
                {
                    // Read the contents of the log file
                    string[] lines = File.ReadAllLines(logFile);

                    // Process each line in the log file
                    foreach (string line in lines)
                    {
                        // Extract the required data from the line
                        string device = GetValue(line, "DeNa");
                        string version = GetValue(line, "version");
                        string mac = GetValue(line, "mac");

                        // Prepare the list of dvt values
                        List<string> dvtData = new List<string>();
                        foreach (string dvtValue in dvtValues)
                        {
                            string value = GetValue(line, dvtValue);
                            dvtData.Add(value);
                        }

                        // Prepare the row for the CSV file
                        string csvRow = $"{device},{version},{mac},{string.Join(",", dvtData)}";

                        // Append the row to the CSV file
                        writer.WriteLine(csvRow);
                    }
                }
            }

            Console.WriteLine("Data processing completed.");
        }

        private string GetValue(string line, string key)
        {
            // Extract the value after the key
            int startIndex = line.IndexOf(key, StringComparison.OrdinalIgnoreCase) + key.Length + 1;
            int endIndex = line.IndexOf("=", startIndex);
            return line.Substring(startIndex, endIndex - startIndex);
        }
    }
}
