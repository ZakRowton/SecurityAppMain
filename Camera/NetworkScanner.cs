using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices; // Required for P/Invoke (DllImport)
using System.Threading.Tasks;
using System.Linq;
using System.Text; // Required for StringBuilder
using System.ComponentModel; // Required for Win32Exception

/// <summary>
/// Scans the local network by attempting ARP resolution for a specific subnet
/// to retrieve MAC addresses. Logs devices and their MAC addresses, including ARP error codes on failure.
/// Does NOT perform OUI lookup to find the manufacturer name.
/// </summary>
public class NetworkScanner
{
    // Hardcoded base IP address for scanning
    private const string TargetIpBase = "192.168.0";

    // P/Invoke declaration for SendARP
    [DllImport("iphlpapi.dll", ExactSpelling = true)]
    private static extern int SendARP(int DestIP, int SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

    /// <summary>
    /// Scans the hardcoded network subnet (192.168.0.*) asynchronously by attempting
    /// ARP resolution and logs discovered device IPs and MAC addresses to the console.
    /// </summary>
    public async Task ScanAndLogDeviceMacAsync()
    {
        Console.WriteLine($"Starting network scan on subnet {TargetIpBase}.* (Attempting ARP resolution)...");

        List<Task> arpTasks = new List<Task>();
        // Scan the typical range for a Class C network (1-254)
        for (int i = 1; i < 255; i++)
        {
            string ipAddressString = $"{TargetIpBase}.{i}";
            // Add the task to attempt ARP resolution directly
            arpTasks.Add(GetMacAndLogAsync(ipAddressString));
        }

        // Wait for all ARP tasks to complete
        await Task.WhenAll(arpTasks);

        Console.WriteLine("\nNetwork scan finished.");
    }

    /// <summary>
    /// Attempts to resolve the MAC address for a specific IP address using SendARP.
    /// Logs the result to the console if successful, or logs an error code on failure.
    /// </summary>
    /// <param name="ipAddressString">The IP address string to resolve.</param>
    private async Task GetMacAndLogAsync(string ipAddressString)
    {
        try
        {
            IPAddress? ipAddress = IPAddress.TryParse(ipAddressString, out var addr) ? addr : null;
            if (ipAddress == null || ipAddress.AddressFamily != AddressFamily.InterNetwork)
            {
                return; // Skip non-IPv4 addresses
            }

            // Convert IP address to integer format needed by SendARP
#pragma warning disable CS0618 // Type or member is obsolete
            int destIpInt = BitConverter.ToInt32(ipAddress.GetAddressBytes(), 0);
#pragma warning restore CS0618 // Type or member is obsolete

            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;

            int arpResult = await Task.Run(() => SendARP(destIpInt, 0, macAddr, ref macAddrLen));

            if (arpResult == 0) // Success
            {
                StringBuilder macString = new StringBuilder();
                for (int j = 0; j < macAddrLen; j++)
                {
                    macString.Append(macAddr[j].ToString("X2"));
                    if (j < macAddrLen - 1) macString.Append("-");
                }

                string oui = macString.ToString().Substring(0, Math.Min(8, macString.Length));

                Console.WriteLine($"  [ARP SUCCESS] {ipAddressString} - MAC: {macString} (OUI: {oui})");
                // TODO: Add OUI lookup here
            }
            else // ARP Failed
            {
                // --- Added Error Logging ---
                // Use Win32Exception to get a meaningful error message from the code
                string errorMessage = new Win32Exception(arpResult).Message;
                Console.WriteLine($"  [ARP FAIL] {ipAddressString} - Error Code: {arpResult} ({errorMessage})");
                // --- End Added Error Logging ---
            }
        }
        catch (Exception ex) // Catch other potential errors
        {
            Console.WriteLine($"  [ERROR] Processing {ipAddressString}: {ex.Message}");
        }
    }

    // --- Example Usage ---
    // public static async Task Main(string[] args)
    // {
    //     NetworkScanner scanner = new NetworkScanner();
    //     await scanner.ScanAndLogDeviceMacAsync(); // Call the new method
    //
    //     Console.WriteLine("\nPress any key to exit.");
    //     Console.ReadKey();
    // }
}
