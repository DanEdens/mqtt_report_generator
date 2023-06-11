﻿using System;
using System.Collections.Generic;
using System.IO;

namespace mqtt_report_generator
{
    public class MqttDataProcessor
    {
        private string logFolderPath;
        private List<string> dvtValues;

        public MqttDataProcessor(string logFolderPath)
        {
            this.logFolderPath = logFolderPath;
            dvtValues = new List<string> { "dvt-2601", "dvt-3834", "dvt-3280", "dvt-2682", "dvt-2681", "dvt-2678", "dvt-2675", "dvt-2674" };
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
