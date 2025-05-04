using System;
using System.Collections.Generic;
using System.Diagnostics; // Added for Debug.WriteLine
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

// Place these classes in their own file (e.g., Esp32Communication.cs) or within your Security namespace

namespace Security.Sensors // Or your preferred namespace
{
    /// <summary>
    /// Manages multiple ESP32 devices, providing access to their controllers.
    /// Implements IDisposable to ensure proper cleanup of resources.
    /// </summary>
    public class Esp32Manager : IDisposable
    {
        private readonly Dictionary<string, Esp32Controller> _devices = new Dictionary<string, Esp32Controller>();
        private bool _disposed = false;

        /// <summary>
        /// Adds a new ESP32 device to the manager.
        /// </summary>
        /// <param name="deviceId">A unique identifier for the device (e.g., "Sensor_LivingRoom").</param>
        /// <param name="ipAddress">The IP address of the ESP32 device.</param>
        /// <returns>True if the device was added successfully, false if the deviceId already exists.</returns>
        public bool AddDevice(string deviceId, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(deviceId) || string.IsNullOrWhiteSpace(ipAddress))
            {
                Console.WriteLine("Error adding device: Device ID and IP address cannot be empty.");
                return false;
            }

            if (_devices.ContainsKey(deviceId))
            {
                Console.WriteLine($"Warning: Device with ID '{deviceId}' already exists. Not adding again.");
                return false; // Indicate that it wasn't newly added
            }

            try
            {
                var controller = new Esp32Controller(ipAddress);
                _devices.Add(deviceId, controller);
                Console.WriteLine($"Device '{deviceId}' added with IP {ipAddress}.");
                return true;
            }
            catch (UriFormatException ex)
            {
                Console.WriteLine($"Error adding device '{deviceId}': Invalid IP address format '{ipAddress}'. {ex.Message}");
                return false;
            }
            catch (Exception ex) // Catch other potential errors during controller creation
            {
                Console.WriteLine($"Error adding device '{deviceId}': {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves the controller for a specific device.
        /// </summary>
        /// <param name="deviceId">The unique identifier of the device.</param>
        /// <returns>The Esp32Controller instance, or null if the device ID is not found.</returns>
        public Esp32Controller GetDeviceController(string deviceId)
        {
            _devices.TryGetValue(deviceId, out Esp32Controller controller);
            if (controller == null)
            {
                Debug.WriteLine($"Device controller with ID '{deviceId}' not found.");
            }
            return controller;
        }

        /// <summary>
        /// Gets the sensor status from a specific device asynchronously.
        /// </summary>
        /// <param name="deviceId">The ID of the device to query.</param>
        /// <returns>The sensor status (e.g., 0 or 1), or -1 if an error occurred or the device was not found.</returns>
        public async Task<int> GetSensorStatusAsync(string deviceId)
        {
            Esp32Controller controller = GetDeviceController(deviceId);
            if (controller != null)
            {
                return await controller.GetSensorStatusAsync();
            }
            else
            {
                Console.WriteLine($"Cannot get sensor status: Device '{deviceId}' not found.");
                return -1; // Indicate device not found
            }
        }

        /// <summary>
        /// Sets the buzzer state for a specific device asynchronously.
        /// </summary>
        /// <param name="deviceId">The ID of the device.</param>
        /// <param name="turnOn">True to turn the buzzer on, false to turn it off.</param>
        /// <returns>True if the command was likely successful, false otherwise.</returns>
        public async Task<bool> SetBuzzerStateAsync(string deviceId, bool turnOn)
        {
            Esp32Controller controller = GetDeviceController(deviceId);
            if (controller != null)
            {
                return await controller.SetBuzzerStateAsync(turnOn);
            }
            else
            {
                Console.WriteLine($"Cannot set buzzer state: Device '{deviceId}' not found.");
                return false;
            }
        }


        /// <summary>
        /// Gets the sensor status from all registered devices concurrently.
        /// </summary>
        /// <returns>A dictionary mapping device IDs to their sensor status (-1 indicates error/timeout).</returns>
        public async Task<Dictionary<string, int>> GetAllSensorStatusesAsync()
        {
            var statusTasks = new Dictionary<string, Task<int>>();

            foreach (var kvp in _devices)
            {
                statusTasks.Add(kvp.Key, kvp.Value.GetSensorStatusAsync());
            }

            // Await all tasks concurrently
            await Task.WhenAll(statusTasks.Values);

            // Collect results
            var results = new Dictionary<string, int>();
            foreach (var kvp in statusTasks)
            {
                // The task is already completed here, accessing .Result is safe
                results.Add(kvp.Key, kvp.Value.Result);
            }

            return results;
        }

        /// <summary>
        /// Sets the buzzer state for all registered devices concurrently.
        /// </summary>
        /// <param name="turnOn">True to turn all buzzers on, false to turn them off.</param>
        /// <returns>A Task that completes when all commands have been sent (does not guarantee success for all).</returns>
        public async Task SetAllBuzzersStateAsync(bool turnOn)
        {
            var commandTasks = new List<Task>();

            foreach (var kvp in _devices)
            {
                // Add the task to the list, don't await individually here
                commandTasks.Add(kvp.Value.SetBuzzerStateAsync(turnOn));
            }

            // Await all tasks concurrently
            // You might want error handling here if individual failures are critical
            try
            {
                await Task.WhenAll(commandTasks);
                Console.WriteLine($"SetAllBuzzersStateAsync({turnOn}) command sent to all {_devices.Count} devices.");
            }
            catch (Exception ex)
            {
                // Task.WhenAll throws the first exception encountered.
                Console.WriteLine($"Error occurred during SetAllBuzzersStateAsync: {ex.Message}");
                // You might want more granular error reporting by examining task statuses after WhenAll fails
            }
        }

        // --- IDisposable Implementation ---

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Dispose managed state (managed objects).
                Console.WriteLine($"Disposing Esp32Manager ({_devices.Count} devices)...");
                if (_devices != null)
                {
                    foreach (var controller in _devices.Values)
                    {
                        controller?.Dispose(); // Dispose each controller
                    }
                    _devices.Clear();
                }
                Console.WriteLine("Esp32Manager disposed.");
            }

            // Free unmanaged resources (none in this class directly)

            _disposed = true;
        }
    }

    /// <summary>
    /// Controls a single ESP32 device via HTTP requests.
    /// Implements IDisposable to clean up the HttpClient.
    /// </summary>
    public class Esp32Controller : IDisposable
    {
        // Use IHttpClientFactory in more complex scenarios (ASP.NET Core, etc.)
        // For simple WinForms, a single HttpClient per controller is often acceptable,
        // but be mindful of potential socket exhaustion if creating/disposing many rapidly.
        private readonly HttpClient _httpClient;
        public string DeviceAddress { get; } // Base address like "http://192.168.0.101"
        private bool _disposed = false;

        /// <summary>
        /// Creates a controller for a specific ESP32 IP address.
        /// </summary>
        /// <param name="ipAddress">The IP address of the ESP32.</param>
        /// <exception cref="UriFormatException">Thrown if the IP address is not valid.</exception>
        public Esp32Controller(string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(ipAddress))
                throw new ArgumentNullException(nameof(ipAddress));

            // Basic validation - could add more robust IP checking
            if (!Uri.TryCreate($"http://{ipAddress}", UriKind.Absolute, out Uri baseUri))
            {
                throw new UriFormatException($"Invalid IP address format: {ipAddress}");
            }
            DeviceAddress = baseUri.ToString().TrimEnd('/'); // Ensure no trailing slash

            // Configure HttpClient
            _httpClient = new HttpClient
            {
                BaseAddress = baseUri,
                Timeout = TimeSpan.FromSeconds(3) // Set a reasonable timeout (e.g., 3 seconds)
            };
            Console.WriteLine($"Esp32Controller created for {DeviceAddress}");
        }

        /// <summary>
        /// Gets the sensor status from the ESP32 device asynchronously.
        /// </summary>
        /// <returns>The sensor status (e.g., 0 or 1), or -1 if an error or timeout occurred.</returns>
        public async Task<int> GetSensorStatusAsync()
        {
            if (_disposed) throw new ObjectDisposedException(nameof(Esp32Controller));

            string requestUri = "/sensor"; // Relative URI
            try
            {
                // Use GetAsync with relative URI since BaseAddress is set
                HttpResponseMessage response = await _httpClient.GetAsync(requestUri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    if (int.TryParse(content.Trim(), out int status))
                    {
                        return status;
                    }
                    else
                    {
                        Console.WriteLine($"Error getting status from {DeviceAddress}{requestUri}: Could not parse response '{content}' as integer.");
                        return -1; // Indicate parsing error
                    }
                }
                else
                {
                    Console.WriteLine($"Error getting status from {DeviceAddress}{requestUri}: HTTP {(int)response.StatusCode} ({response.ReasonPhrase})");
                    return -1; // Indicate HTTP error
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error getting status from {DeviceAddress}{requestUri}: {ex.Message}");
                return -1; // Indicate network error
            }
            catch (TaskCanceledException ex) // Catches timeouts
            {
                Console.WriteLine($"Timeout getting status from {DeviceAddress}{requestUri}: {ex.Message}");
                return -1; // Indicate timeout
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine($"Cannot get status from {DeviceAddress}{requestUri}: HttpClient has been disposed.");
                return -1;
            }
            catch (Exception ex) // Catch unexpected errors
            {
                Console.WriteLine($"Unexpected error getting status from {DeviceAddress}{requestUri}: {ex.Message}");
                return -1; // Indicate general error
            }
        }

        /// <summary>
        /// Sets the buzzer state on the ESP32 device asynchronously using POST.
        /// </summary>
        /// <param name="turnOn">True to turn the buzzer on, false to turn it off.</param>
        /// <returns>True if the command was sent successfully (HTTP 200 OK), false otherwise.</returns>
        public async Task<bool> SetBuzzerStateAsync(bool turnOn)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(Esp32Controller));

            string requestUri = "/buzzer"; // Relative URI
            string state = turnOn ? "on" : "off";
            try
            {
                // Prepare form data
                var formData = new Dictionary<string, string>
                {
                    { "state", state }
                };
                var content = new FormUrlEncodedContent(formData);

                // Use PostAsync with relative URI
                HttpResponseMessage response = await _httpClient.PostAsync(requestUri, content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine($"Successfully set buzzer state to '{state}' on {DeviceAddress}{requestUri}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Error setting buzzer state ({state}) on {DeviceAddress}{requestUri}: HTTP {(int)response.StatusCode} ({response.ReasonPhrase})");
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Network error setting buzzer state ({state}) on {DeviceAddress}{requestUri}: {ex.Message}");
                return false;
            }
            catch (TaskCanceledException ex) // Catches timeouts
            {
                Console.WriteLine($"Timeout setting buzzer state ({state}) on {DeviceAddress}{requestUri}: {ex.Message}");
                return false;
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine($"Cannot set buzzer state ({state}) on {DeviceAddress}{requestUri}: HttpClient has been disposed.");
                return false;
            }
            catch (Exception ex) // Catch unexpected errors
            {
                Console.WriteLine($"Unexpected error setting buzzer state ({state}) on {DeviceAddress}{requestUri}: {ex.Message}");
                return false;
            }
        }


        // --- IDisposable Implementation ---

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Dispose managed state (managed objects).
                if (_httpClient != null)
                {
                    Console.WriteLine($"Disposing HttpClient for {DeviceAddress}...");
                    _httpClient.Dispose();
                    Console.WriteLine($"HttpClient for {DeviceAddress} disposed.");
                }
            }

            // Free unmanaged resources (none in this class directly)

            _disposed = true;
        }
    }
}