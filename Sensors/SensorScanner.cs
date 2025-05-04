using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Security.Sensors
{
    public class SensorScanner
    {
        public SensorScanner() { }

        /// <summary>
        /// Scans a range of IP addresses on the network using Ping to find reachable devices.
        /// </summary>
        /// <param name="startIpBase">The first three octets of the IP range (e.g., "192.168.0").</param>
        /// <param name="startOctet">The starting number for the last octet (e.g., 200).</param>
        /// <param name="endOctet">The ending number for the last octet (e.g., 250).</param>
        /// <param name="timeout">The timeout in milliseconds to wait for a ping reply.</param>
        /// <returns>A list of IPAddress objects representing the devices that responded successfully.</returns>
        public async Task<List<IPAddress>> FindDevicesAsync(string startIpBase, int startOctet, int endOctet, int timeout = 500)
        {
            var reachableAddresses = new List<IPAddress>();
            // Store tasks that will return our custom result data object
            var pingTasks = new List<Task<PingResultData>>();

            Console.WriteLine($"Scanning range {startIpBase}.{startOctet} to {startIpBase}.{endOctet}...");

            for (int i = startOctet; i <= endOctet; i++)
            {
                string ipString = $"{startIpBase}.{i}";
                if (IPAddress.TryParse(ipString, out IPAddress? ipAddress))
                {
                    if (ipAddress != null) // Ensure ipAddress is not null after TryParse
                    {
                        // Add the task to the list. Each task will execute the lambda.
                        pingTasks.Add(Task.Run(async () => {
                            // Using statement ensures Ping object is disposed
                            using var pingSender = new Ping();
                            try
                            {
                                // Perform the ping
                                PingReply reply = await pingSender.SendPingAsync(ipAddress, timeout);
                                // Return success or standard failure (e.g., timeout)
                                return new PingResultData(ipAddress, reply);
                            }
                            catch (Exception ex) when (ex is PingException || ex is InvalidOperationException || ex is PlatformNotSupportedException)
                            {
                                // Catch potential exceptions from SendPingAsync itself
                                // Return an error result containing the exception
                                return new PingResultData(ipAddress, ex);
                            }
                            // Catching general Exception can mask other issues, be specific if possible
                            catch (Exception ex)
                            {
                                Console.WriteLine($"!! Unexpected Error Pinging {ipAddress}: {ex.GetType().Name} - {ex.Message}");
                                return new PingResultData(ipAddress, ex);
                            }
                        }));
                    }
                }
                else
                {
                    Console.WriteLine($"Warning: Could not parse IP address string: {ipString}");
                }
            }

            // Wait for all the launched ping tasks to complete
            PingResultData[] results = await Task.WhenAll(pingTasks);

            Console.WriteLine("Processing results...");

            // Process the results collected from all tasks
            foreach (PingResultData result in results)
            {
                if (result.Error != null)
                {
                    // Log if an exception occurred during the ping attempt for this IP
                    Console.WriteLine($"  Error pinging {result.IpAddress}: {result.Error.GetType().Name} - {result.Error.Message}");
                }
                else if (result.Reply != null && result.Reply.Status == IPStatus.Success)
                {
                    // If Reply is not null AND status is Success, the device responded
                    reachableAddresses.Add(result.IpAddress);
                    Console.WriteLine($"---> Device Found: {result.IpAddress} (Status: {result.Reply.Status}, Time: {result.Reply.RoundtripTime}ms)");
                }
                // Optional: Log other non-success statuses for debugging (e.g., TimedOut)
                // else if (result.Reply != null)
                // {
                //     Console.WriteLine($"     Device Not Found/Timed Out: {result.IpAddress} (Status: {result.Reply.Status})");
                // }
                else
                {
                    // Should not happen with current logic, but good for robustness
                    Console.WriteLine($"     No reply data or error for {result.IpAddress}.");
                }
            }

            return reachableAddresses;
        }

        public class PingResultData
        {
            public IPAddress IpAddress { get; }
            public PingReply? Reply { get; } // Nullable if ping failed or exception occurred
            public Exception? Error { get; } // Nullable if no exception

            // Constructor for success or standard failure (like timeout)
            public PingResultData(IPAddress ip, PingReply reply)
            {
                IpAddress = ip;
                Reply = reply;
                Error = null;
            }

            // Constructor for when an exception occurs during the ping process
            public PingResultData(IPAddress ip, Exception error)
            {
                IpAddress = ip;
                Reply = null; // No valid reply if an exception was thrown
                Error = error;
            }
        }
    }
}
