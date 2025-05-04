using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web; // For HttpUtility used in query string building

namespace Security.SmartThings // Your application's namespace
{
    /// <summary>
    /// Interacts with the SmartThings Cloud API.
    /// Requires a valid Personal Access Token (PAT) for authentication.
    /// </summary>
    public class SmartThingsApiClient : IDisposable
    {
        private const string BaseApiUrl = "https://api.smartthings.com/v1/";
        private readonly HttpClient _httpClient;
        private readonly string _accessToken; // Your SmartThings Personal Access Token (PAT)

        /// <summary>
        /// Initializes a new instance of the SmartThingsApiClient.
        /// </summary>
        /// <param name="personalAccessToken">Your SmartThings Personal Access Token (PAT). DO NOT HARDCODE.</param>
        public SmartThingsApiClient(string personalAccessToken)
        {
            if (string.IsNullOrWhiteSpace(personalAccessToken))
            {
                throw new ArgumentNullException(nameof(personalAccessToken), "SmartThings Personal Access Token cannot be empty.");
            }

            _accessToken = personalAccessToken;

            _httpClient = new HttpClient { BaseAddress = new Uri(BaseApiUrl) };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Set authorization header for all requests using this client instance
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        }

        /// <summary>
        /// Gets a list of devices accessible by the configured access token, with optional filtering.
        /// Requires 'l:devices' and 'r:devices:*' scopes on the token.
        /// </summary>
        /// <param name="locationIds">Optional list of Location IDs to filter by.</param>
        /// <param name="capabilityIds">Optional list of Capability IDs to filter by (acts as AND filter by default).</param>
        /// <param name="deviceIds">Optional list of Device IDs to filter by.</param>
        /// <param name="capabilitiesMode">Treat capability filters as "or" or "and" (default: "and").</param>
        /// <param name="includeRestricted">Set to true to include restricted devices (default: false).</param>
        /// <returns>A DeviceListResponse containing the list of devices, or null if an error occurs.</returns>
        public async Task<DeviceListResponse?> GetAllDevicesAsync(
            List<string>? locationIds = null,
            List<string>? capabilityIds = null,
            List<string>? deviceIds = null,
            string capabilitiesMode = "and", // "and" or "or"
            bool includeRestricted = false)
        {
            string requestUri = BuildDeviceListUri("devices", locationIds, capabilityIds, deviceIds, capabilitiesMode, includeRestricted);

            try
            {
                Console.WriteLine($"DEBUG: Requesting URI: {requestUri}"); // Log the request URI

                HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

                Console.WriteLine($"DEBUG: Response Status Code: {response.StatusCode}"); // Log status

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"DEBUG: Raw JSON Response:\n{jsonResponse.Substring(0, Math.Min(jsonResponse.Length, 1000))}..."); // Log part of the response

                    // Configure JsonSerializer options if needed (e.g., property naming policy)
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // Handles potential case mismatches
                    };

                    DeviceListResponse? deviceList = JsonSerializer.Deserialize<DeviceListResponse>(jsonResponse, options);
                    return deviceList;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error fetching devices: {response.StatusCode} - {response.ReasonPhrase}");
                    Console.WriteLine($"Error details: {errorContent}");
                    // Consider throwing a custom exception here
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error fetching devices: {ex.Message}");
                // Consider logging ex.ToString() for full details
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Deserialization Error fetching devices: {ex.Message}");
                return null;
            }
            catch (Exception ex) // Catch other potential errors
            {
                Console.WriteLine($"Generic Error fetching devices: {ex.ToString()}");
                return null;
            }
        }

        // Helper method to build the query string URI
        private string BuildDeviceListUri(
            string path,
            List<string>? locationIds,
            List<string>? capabilityIds,
            List<string>? deviceIds,
            string capabilitiesMode,
            bool includeRestricted)
        {
            var query = HttpUtility.ParseQueryString(string.Empty); // Requires System.Web

            if (locationIds != null && locationIds.Count > 0)
            {
                foreach (var id in locationIds) query.Add("locationId", id);
            }
            if (capabilityIds != null && capabilityIds.Count > 0)
            {
                foreach (var id in capabilityIds) query.Add("capability", id); // API uses 'capability'
                if (!string.IsNullOrEmpty(capabilitiesMode) && capabilitiesMode.Equals("or", StringComparison.OrdinalIgnoreCase))
                {
                    query.Add("capabilitiesMode", "or");
                }
                // "and" is default, no need to add explicitly unless required by API for clarity
            }
            if (deviceIds != null && deviceIds.Count > 0)
            {
                foreach (var id in deviceIds) query.Add("deviceId", id);
            }
            if (includeRestricted)
            {
                query.Add("includeRestricted", "true");
            }

            // Add other optional params like accessLevel, includeAllowedActions if needed
            // query.Add("includeAllowedActions", "true");

            string queryString = query.ToString();
            return string.IsNullOrEmpty(queryString) ? path : $"{path}?{queryString}";
        }

        // Implement IDisposable to release HttpClient resources properly
        public void Dispose()
        {
            _httpClient?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the list of commands available for a specific device component and capability.
        /// Requires appropriate scopes (likely 'r:devices:*' or potentially 'x:devices:*').
        /// </summary>
        /// <param name="deviceId">The unique identifier of the device.</param>
        /// <returns>A DeviceCommandsResponse containing the list of commands, or null if an error occurs or the device is not found.</returns>
        /// <exception cref="ArgumentException">Thrown if deviceId is null or empty.</exception>
        public async Task<DeviceCommandsResponse?> GetDeviceCommandsAsync(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                throw new ArgumentException("Device ID cannot be null or empty.", nameof(deviceId));
            }

            // Construct the specific request path
            string requestUri = $"devices/{deviceId}/commands";

            try
            {
                Console.WriteLine($"DEBUG: Requesting Device Commands URI: {requestUri}");

                HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

                Console.WriteLine($"DEBUG: GetDeviceCommands Response Status Code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"DEBUG: Raw GetDeviceCommands JSON Response:\n{jsonResponse.Substring(0, Math.Min(jsonResponse.Length, 1000))}...");

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    // Deserialize into the wrapper class
                    DeviceCommandsResponse? commandList = JsonSerializer.Deserialize<DeviceCommandsResponse>(jsonResponse, options);

                    // If the API returns the list directly (no "commands" wrapper), adjust deserialization:
                    // List<DeviceCommandInfo>? commandListDirect = JsonSerializer.Deserialize<List<DeviceCommandInfo>>(jsonResponse, options);
                    // return new DeviceCommandsResponse { Commands = commandListDirect ?? new List<DeviceCommandInfo>() }; // Wrap it manually

                    return commandList;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error fetching device commands for {deviceId}: {response.StatusCode} - {response.ReasonPhrase}");
                    Console.WriteLine($"Error details: {errorContent}");
                    // Handle specific errors like 404 Not Found if needed
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error fetching device commands for {deviceId}: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Deserialization Error fetching device commands for {deviceId}: {ex.Message}");
                return null;
            }
            catch (Exception ex) // Catch other potential errors
            {
                Console.WriteLine($"Generic Error fetching device commands for {deviceId}: {ex.ToString()}");
                return null;
            }
        }

        /// <summary>
        /// Gets the current health state of a specific device (e.g., ONLINE, OFFLINE).
        /// Requires appropriate scopes (likely 'r:devices:*').
        /// </summary>
        /// <param name="deviceId">The unique identifier of the device.</param>
        /// <returns>A DeviceHealthResponse containing the health status, or null if an error occurs.</returns>
        /// <exception cref="ArgumentException">Thrown if deviceId is null or empty.</exception>
        public async Task<DeviceHealthResponse?> GetDeviceHealthAsync(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                throw new ArgumentException("Device ID cannot be null or empty.", nameof(deviceId));
            }
            string requestUri = $"devices/{deviceId}/health";
            return await SendRequestAsync<DeviceHealthResponse>(HttpMethod.Get, requestUri);
        }

        /// <summary>
        /// Gets the full status of all components and capabilities for a specific device.
        /// Requires appropriate scopes (likely 'r:devices:*').
        /// </summary>
        /// <param name="deviceId">The unique identifier of the device.</param>
        /// <returns>A DeviceStatusResponse containing the component statuses, or null if an error occurs.</returns>
        /// <exception cref="ArgumentException">Thrown if deviceId is null or empty.</exception>
        public async Task<DeviceStatusResponse?> GetDeviceStatusAsync(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                throw new ArgumentException("Device ID cannot be null or empty.", nameof(deviceId));
            }
            string requestUri = $"devices/{deviceId}/status";
            return await SendRequestAsync<DeviceStatusResponse>(HttpMethod.Get, requestUri);
        }

        /// <summary>
        /// Gets the status of all capabilities within a specific component of a device.
        /// Response format is a Dictionary where key is capabilityId, value describes capability state.
        /// Requires appropriate scopes (likely 'r:devices:*').
        /// </summary>
        /// <param name="deviceId">The unique identifier of the device.</param>
        /// <param name="componentId">The component identifier (e.g., "main").</param>
        /// <returns>A Dictionary mapping capability IDs to their status (represented by JsonElement), or null if an error occurs.</returns>
        /// <exception cref="ArgumentException">Thrown if deviceId or componentId is null or empty.</exception>
        public async Task<Dictionary<string, JsonElement>?> GetDeviceComponentStatusAsync(string deviceId, string componentId)
        {
            if (string.IsNullOrWhiteSpace(deviceId)) throw new ArgumentException("Device ID cannot be null or empty.", nameof(deviceId));
            if (string.IsNullOrWhiteSpace(componentId)) throw new ArgumentException("Component ID cannot be null or empty.", nameof(componentId));

            string requestUri = $"devices/{deviceId}/components/{componentId}/status";
            // Deserialize directly into the expected Dictionary structure
            return await SendRequestAsync<Dictionary<string, JsonElement>>(HttpMethod.Get, requestUri);
        }

        /// <summary>
        /// Gets the status of a specific capability within a specific component of a device.
        /// Response format is a Dictionary where key is attribute name, value describes attribute state.
        /// Requires appropriate scopes (likely 'r:devices:*').
        /// </summary>
        /// <param name="deviceId">The unique identifier of the device.</param>
        /// <param name="componentId">The component identifier (e.g., "main").</param>
        /// <param name="capabilityId">The capability identifier (e.g., "switchLevel").</param>
        /// <returns>A Dictionary mapping attribute names to their status details (CapabilityAttributeStatus), or null if an error occurs.</returns>
        /// <exception cref="ArgumentException">Thrown if deviceId, componentId, or capabilityId is null or empty.</exception>
        public async Task<Dictionary<string, CapabilityAttributeStatus>?> GetDeviceCapabilityStatusAsync(string deviceId, string componentId, string capabilityId)
        {
            if (string.IsNullOrWhiteSpace(deviceId)) throw new ArgumentException("Device ID cannot be null or empty.", nameof(deviceId));
            if (string.IsNullOrWhiteSpace(componentId)) throw new ArgumentException("Component ID cannot be null or empty.", nameof(componentId));
            if (string.IsNullOrWhiteSpace(capabilityId)) throw new ArgumentException("Capability ID cannot be null or empty.", nameof(capabilityId));

            string requestUri = $"devices/{deviceId}/components/{componentId}/capabilities/{capabilityId}/status";
            // Deserialize directly into the expected Dictionary structure
            return await SendRequestAsync<Dictionary<string, CapabilityAttributeStatus>>(HttpMethod.Get, requestUri);
        }


        // In SmartThingsApiClient.cs

        // Helper WITHIN SmartThingsApiClient to parse DateTimeOffset from timestamp property
        private DateTimeOffset? GetTimestampFromJson(JsonElement attributeStatusElement)
        {
            if (attributeStatusElement.TryGetProperty("timestamp", out var timestampElement) &&
                timestampElement.ValueKind == JsonValueKind.String &&
                DateTimeOffset.TryParse(timestampElement.GetString(), out var dto))
            {
                return dto;
            }
            return null;
        }

        public async Task<RefrigeratorStatus?> GetRefrigeratorStatusAsync(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                Console.WriteLine("Error: Device ID cannot be empty.");
                return null;
            }

            DeviceStatusResponse? fullStatus = await GetDeviceStatusAsync(deviceId);

            if (fullStatus?.Components == null)
            {
                Console.WriteLine($"Error: Could not retrieve status or components for device {deviceId}.");
                return null;
            }

            var status = new RefrigeratorStatus();
            Console.WriteLine($"[GetFridgeStatus] Parsing status received at: {DateTimeOffset.UtcNow}");

            try
            {
                // --- Cooler (Fridge) ---
                if (fullStatus.Components.TryGetValue("cooler", out var coolerComp) && coolerComp.Capabilities != null)
                {
                    Console.WriteLine("[GetFridgeStatus] Processing 'cooler' component.");
                    // Measured Temp
                    if (coolerComp.Capabilities.TryGetValue("temperatureMeasurement", out var fridgeTempCap) &&
                        fridgeTempCap.TryGetProperty("temperature", out var fridgeTempAttr))
                    {
                        if (TryDeserializeAttribute<CapabilityAttributeStatus>(fridgeTempAttr, out var fridgeTempStatus) && fridgeTempStatus != null)
                        {
                            status.FridgeTemperatureF = fridgeTempStatus.GetDoubleValue();
                            status.FridgeTemperatureTimestamp = GetTimestampFromJson(fridgeTempAttr); // Get Timestamp
                            Console.WriteLine($"[GetFridgeStatus] Parsed FridgeTemperatureF: {status.FridgeTemperatureF} (Timestamp: {status.FridgeTemperatureTimestamp})");
                        }
                    }
                    // Setpoint
                    if (coolerComp.Capabilities.TryGetValue("thermostatCoolingSetpoint", out var fridgeSetpointCap) &&
                        fridgeSetpointCap.TryGetProperty("coolingSetpoint", out var fridgeSetpointAttr))
                    {
                        if (TryDeserializeAttribute<CapabilityAttributeStatus>(fridgeSetpointAttr, out var fridgeSetpointStatus) && fridgeSetpointStatus != null)
                        {
                            status.FridgeSetpointF = fridgeSetpointStatus.GetDoubleValue();
                            status.FridgeSetpointTimestamp = GetTimestampFromJson(fridgeSetpointAttr); // Get Timestamp
                            Console.WriteLine($"[GetFridgeStatus] Parsed FridgeSetpointF: {status.FridgeSetpointF} (Timestamp: {status.FridgeSetpointTimestamp})");
                        }
                    }
                    // Contact Sensor
                    if (coolerComp.Capabilities.TryGetValue("contactSensor", out var coolerContactCap) &&
                        coolerContactCap.TryGetProperty("contact", out var coolerContactAttr))
                    {
                        if (TryDeserializeAttribute<CapabilityAttributeStatus>(coolerContactAttr, out var coolerContactStatus) && coolerContactStatus != null)
                        {
                            status.IsCoolerDoorClosed = coolerContactStatus.GetStringValue()?.Equals("closed", StringComparison.OrdinalIgnoreCase) ?? false;
                            status.CoolerDoorTimestamp = GetTimestampFromJson(coolerContactAttr); // Get Timestamp
                            Console.WriteLine($"[GetFridgeStatus] Parsed IsCoolerDoorClosed: {status.IsCoolerDoorClosed} (Timestamp: {status.CoolerDoorTimestamp})");
                        }
                    }
                }
                else { /* log warning */ }

                // --- Freezer ---
                if (fullStatus.Components.TryGetValue("freezer", out var freezerComp) && freezerComp.Capabilities != null)
                {
                    Console.WriteLine("[GetFridgeStatus] Processing 'freezer' component.");
                    // Measured Temp
                    if (freezerComp.Capabilities.TryGetValue("temperatureMeasurement", out var freezerTempCap) &&
                        freezerTempCap.TryGetProperty("temperature", out var freezerTempAttr))
                    {
                        if (TryDeserializeAttribute<CapabilityAttributeStatus>(freezerTempAttr, out var freezerTempStatus) && freezerTempStatus != null)
                        {
                            status.FreezerTemperatureF = freezerTempStatus.GetDoubleValue();
                            status.FreezerTemperatureTimestamp = GetTimestampFromJson(freezerTempAttr); // Get Timestamp
                            Console.WriteLine($"[GetFridgeStatus] Parsed FreezerTemperatureF: {status.FreezerTemperatureF} (Timestamp: {status.FreezerTemperatureTimestamp})");
                        }
                    }
                    // Setpoint
                    if (freezerComp.Capabilities.TryGetValue("thermostatCoolingSetpoint", out var freezerSetpointCap) &&
                        freezerSetpointCap.TryGetProperty("coolingSetpoint", out var freezerSetpointAttr))
                    {
                        if (TryDeserializeAttribute<CapabilityAttributeStatus>(freezerSetpointAttr, out var freezerSetpointStatus) && freezerSetpointStatus != null)
                        {
                            status.FreezerSetpointF = freezerSetpointStatus.GetDoubleValue();
                            status.FreezerSetpointTimestamp = GetTimestampFromJson(freezerSetpointAttr); // Get Timestamp
                            Console.WriteLine($"[GetFridgeStatus] Parsed FreezerSetpointF: {status.FreezerSetpointF} (Timestamp: {status.FreezerSetpointTimestamp})");
                        }
                    }
                    // Contact Sensor
                    if (freezerComp.Capabilities.TryGetValue("contactSensor", out var freezerContactCap) &&
                        freezerContactCap.TryGetProperty("contact", out var freezerContactAttr))
                    {
                        if (TryDeserializeAttribute<CapabilityAttributeStatus>(freezerContactAttr, out var freezerContactStatus) && freezerContactStatus != null)
                        {
                            status.IsFreezerDoorClosed = freezerContactStatus.GetStringValue()?.Equals("closed", StringComparison.OrdinalIgnoreCase) ?? false;
                            status.FreezerDoorTimestamp = GetTimestampFromJson(freezerContactAttr); // Get Timestamp
                            Console.WriteLine($"[GetFridgeStatus] Parsed IsFreezerDoorClosed: {status.IsFreezerDoorClosed} (Timestamp: {status.FreezerDoorTimestamp})");
                        }
                    }
                }
                else { /* log warning */ }

                // --- Ice Maker ---
                if (fullStatus.Components.TryGetValue("icemaker", out var icemakerComp) && icemakerComp.Capabilities != null)
                {
                    Console.WriteLine("[GetFridgeStatus] Processing 'icemaker' component.");
                    if (icemakerComp.Capabilities.TryGetValue("switch", out var icemakerSwitchCap) &&
                        icemakerSwitchCap.TryGetProperty("switch", out var icemakerSwitchAttr))
                    {
                        if (TryDeserializeAttribute<CapabilityAttributeStatus>(icemakerSwitchAttr, out var icemakerSwitchStatus) && icemakerSwitchStatus != null)
                        {
                            status.IsIceMakerOn = icemakerSwitchStatus.GetStringValue()?.Equals("on", StringComparison.OrdinalIgnoreCase) ?? false;
                            status.IceMakerTimestamp = GetTimestampFromJson(icemakerSwitchAttr); // Get Timestamp
                            Console.WriteLine($"[GetFridgeStatus] Parsed IsIceMakerOn: {status.IsIceMakerOn} (Timestamp: {status.IceMakerTimestamp})");
                        }
                    }
                }
                else { /* log warning */ }

                // --- Water Filter (from main/execute or main/custom.waterFilter) ---
                if (fullStatus.Components.TryGetValue("main", out var mainComp) && mainComp.Capabilities != null)
                {
                    Console.WriteLine("[GetFridgeStatus] Processing 'main' component for Water Filter.");
                    // Check custom capability first
                    if (mainComp.Capabilities.TryGetValue("custom.waterFilter", out var waterFilterCap) &&
                        waterFilterCap.TryGetProperty("waterFilterStatus", out var waterFilterStatusAttr))
                    {
                        if (TryDeserializeAttribute<CapabilityAttributeStatus>(waterFilterStatusAttr, out var waterFilterStatus) && waterFilterStatus != null)
                        {
                            status.WaterFilterStatus = waterFilterStatus.GetStringValue() ?? "N/A";
                            status.WaterFilterTimestamp = GetTimestampFromJson(waterFilterStatusAttr); // Get Timestamp
                            Console.WriteLine($"[GetFridgeStatus] Parsed WaterFilterStatus (custom): {status.WaterFilterStatus} (Timestamp: {status.WaterFilterTimestamp})");
                        }
                    }
                    // Fallback/Alternative: Check execute block (more complex parsing needed if temps were here)
                    else if (mainComp.Capabilities.TryGetValue("execute", out var executeCap) &&
                             executeCap.TryGetProperty("data", out var executeData) &&
                             executeData.TryGetProperty("value", out var executeValue) && executeValue.ValueKind == JsonValueKind.Object &&
                             executeValue.TryGetProperty("payload", out var payloadData) && payloadData.ValueKind == JsonValueKind.Object &&
                             payloadData.TryGetProperty("x.com.samsung.da.filterStatus", out var filterStatusElement))
                    {
                        status.WaterFilterStatus = filterStatusElement.GetString() ?? "N/A";
                        // Timestamp for execute block is harder to get directly associated with the filter status
                        // We might use the timestamp of the 'execute.data' itself if needed
                        if (executeCap.TryGetProperty("data", out var dataElement))
                        { // Re-get 'data' element to access its timestamp
                            status.WaterFilterTimestamp = GetTimestampFromJson(dataElement);
                        }
                        Console.WriteLine($"[GetFridgeStatus] Parsed WaterFilterStatus (execute): {status.WaterFilterStatus} (Timestamp: {status.WaterFilterTimestamp})");
                    }
                    else { Console.WriteLine("[GetFridgeStatus] Water filter status not found in main component."); }
                }
                else { /* log warning */ }

                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetFridgeStatus] Error parsing full status: {ex}");
                return null; // Return null or partial status on error
            }
        }

        // Helper to deserialize nested capability attributes safely
        private bool TryDeserializeAttribute<T>(JsonElement element, out T? result) where T : class
        {
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                result = element.Deserialize<T>(options);
                return result != null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"DEBUG: JSON deserialization error for attribute: {ex.Message}");
                result = null;
                return false;
            }
        }


        // --- NEW: Generic Function to Execute Device Commands ---
        /// <summary>
        /// Executes one or more commands on a device.
        /// Requires 'x:devices:*' scope.
        /// </summary>
        /// <param name="deviceId">The ID of the device.</param>
        /// <param name="commands">A list of commands to execute.</param>
        /// <returns>True if the command POST request was successful (HTTP 200 OK), false otherwise.</returns>
        public async Task<bool> ExecuteDeviceCommandsAsync(string deviceId, List<DeviceCommand> commands)
        {
            if (string.IsNullOrWhiteSpace(deviceId)) throw new ArgumentException("Device ID cannot be null or empty.", nameof(deviceId));
            if (commands == null || !commands.Any()) throw new ArgumentException("Commands list cannot be null or empty.", nameof(commands));

            string requestUri = $"devices/{deviceId}/commands";
            var payload = new DeviceCommandPayload { Commands = commands };

            // Use the generic SendRequestAsync logic, but we only care about success/failure here.
            // We expect a simple success status, not a specific T response body for command execution.
            HttpContent? content = null;
            try
            {
                string jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
                content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                Console.WriteLine($"DEBUG: POST URI: {requestUri}");
                Console.WriteLine($"DEBUG: POST Payload: {jsonPayload}");

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = content };
                HttpResponseMessage response = await _httpClient.SendAsync(request);

                Console.WriteLine($"DEBUG: Command Response Status Code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    // Optionally read and log the response body if needed for debugging
                    // string responseBody = await response.Content.ReadAsStringAsync();
                    // Console.WriteLine($"DEBUG: Command Response Body: {responseBody}");
                    return true;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error executing commands on {deviceId}: {response.StatusCode} - {response.ReasonPhrase}");
                    Console.WriteLine($"Error details: {errorContent}");
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error executing commands on {deviceId}: {ex.Message}");
                return false;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Processing Error executing commands on {deviceId}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Generic Error executing commands on {deviceId}: {ex.ToString()}");
                return false;
            }
            finally
            {
                content?.Dispose(); // Dispose StringContent
            }
        }

        // --- NEW: Specific Command Functions ---

        /// <summary>
        /// Sets the target cooling setpoint for the freezer.
        /// Requires 'x:devices:*' scope.
        /// </summary>
        /// <param name="deviceId">The ID of the refrigerator device.</param>
        /// <param name="temperatureF">The desired temperature in Fahrenheit.</param>
        /// <returns>True if the command was sent successfully, false otherwise.</returns>
        public async Task<bool> SetFreezerTemperatureAsync(string deviceId, double temperatureF)
        {
            var command = new DeviceCommand
            {
                Component = "freezer",
                Capability = "thermostatCoolingSetpoint",
                Command = "setCoolingSetpoint",
                Arguments = new List<object> { temperatureF }
            };
            return await ExecuteDeviceCommandsAsync(deviceId, new List<DeviceCommand> { command });
        }

        /// <summary>
        /// Sets the target cooling setpoint for the fridge (cooler) compartment.
        /// Requires 'x:devices:*' scope.
        /// </summary>
        /// <param name="deviceId">The ID of the refrigerator device.</param>
        /// <param name="temperatureF">The desired temperature in Fahrenheit.</param>
        /// <returns>True if the command was sent successfully, false otherwise.</returns>
        public async Task<bool> SetFridgeTemperatureAsync(string deviceId, double temperatureF)
        {
            var command = new DeviceCommand
            {
                Component = "cooler", // Based on your JSON, the fridge compartment is 'cooler'
                Capability = "thermostatCoolingSetpoint",
                Command = "setCoolingSetpoint",
                Arguments = new List<object> { temperatureF }
            };
            return await ExecuteDeviceCommandsAsync(deviceId, new List<DeviceCommand> { command });
        }

        /// <summary>
        /// Turns the icemaker on or off.
        /// Requires 'x:devices:*' scope.
        /// </summary>
        /// <param name="deviceId">The ID of the refrigerator device.</param>
        /// <param name="turnOn">True to turn the icemaker on, false to turn it off.</param>
        /// <returns>True if the command was sent successfully, false otherwise.</returns>
        public async Task<bool> SetIceMakerStateAsync(string deviceId, bool turnOn)
        {
            var command = new DeviceCommand
            {
                Component = "icemaker",
                Capability = "switch",
                Command = turnOn ? "on" : "off",
                Arguments = null // Switch on/off commands usually don't need arguments
            };
            return await ExecuteDeviceCommandsAsync(deviceId, new List<DeviceCommand> { command });
        }


        // --- Helper Method for Sending Requests (Refactored for Generics) ---
        private async Task<T?> SendRequestAsync<T>(HttpMethod method, string requestUri, object? requestBody = null) where T : class
        {
            HttpContent? content = null;
            if (requestBody != null)
            {
                // Use DefaultIgnoreCondition to avoid sending null arguments for commands if not needed
                string jsonPayload = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
                content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                Console.WriteLine($"DEBUG: {method} URI: {requestUri}");
                Console.WriteLine($"DEBUG: {method} Payload: {jsonPayload}");
            }
            else
            {
                Console.WriteLine($"DEBUG: {method} URI: {requestUri}");
            }

            try
            {
                HttpRequestMessage request = new HttpRequestMessage(method, requestUri);
                if (content != null) request.Content = content;

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                Console.WriteLine($"DEBUG: Response Status Code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    // Only log beginning of potentially large status responses
                    Console.WriteLine($"DEBUG: Raw JSON Response:\n{jsonResponse.Substring(0, Math.Min(jsonResponse.Length, 1000))}...");

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    T? result = JsonSerializer.Deserialize<T>(jsonResponse, options);
                    return result;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error {method} {requestUri}: {response.StatusCode} - {response.ReasonPhrase}");
                    Console.WriteLine($"Error details: {errorContent}");
                    return null; // Or throw specific exception based on status code
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error {method} {requestUri}: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Processing Error {method} {requestUri}: {ex.Message}");
                // Log the problematic JSON if possible and legal
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Generic Error {method} {requestUri}: {ex.ToString()}");
                return null;
            }
            finally
            {
                content?.Dispose(); // Dispose StringContent if created
            }
        }

        // --- DTO Classes for Deserialization (Keep existing ones) ---
        // ... (DeviceListResponse, Device, DeviceComponent, etc.) ...


        // --- NEW: DTO for Refrigerator Status ---
        /// <summary>
        /// Holds the specific status values extracted for a refrigerator.
        /// Properties are nullable to handle cases where data might be missing.
        /// </summary>
        // In SmartThingsApiClient.cs

        public class RefrigeratorStatus
        {
            // Existing properties...
            public double FridgeTemperatureF { get; set; } = 0;
            public double FreezerTemperatureF { get; set; } = 0;
            public bool IsCoolerDoorClosed { get; set; } = false;
            public bool IsFreezerDoorClosed { get; set; } = false;
            public bool IsIceMakerOn { get; set; } = false;
            public string WaterFilterStatus { get; set; } = "";

            // --- NEW: Add properties for setpoints and ALL timestamps ---
            public double FridgeSetpointF { get; set; } = 0;
            public double FreezerSetpointF { get; set; } = 0;

            public DateTimeOffset? FridgeTemperatureTimestamp { get; set; }
            public DateTimeOffset? FridgeSetpointTimestamp { get; set; }
            public DateTimeOffset? FreezerTemperatureTimestamp { get; set; }
            public DateTimeOffset? FreezerSetpointTimestamp { get; set; }
            public DateTimeOffset? CoolerDoorTimestamp { get; set; }
            public DateTimeOffset? FreezerDoorTimestamp { get; set; }
            public DateTimeOffset? IceMakerTimestamp { get; set; }
            public DateTimeOffset? WaterFilterTimestamp { get; set; } // If available
        }

        // --- NEW: DTOs for Command Execution Payload ---
        /// <summary>
        /// Payload structure for sending commands via the API.
        /// </summary>
        public class DeviceCommandPayload
        {
            [JsonPropertyName("commands")]
            public List<DeviceCommand> Commands { get; set; } = new List<DeviceCommand>();
        }

        /// <summary>
        /// Represents a single command to be sent to a device component/capability.
        /// </summary>
        public class DeviceCommand
        {
            [JsonPropertyName("component")]
            public string Component { get; set; } = string.Empty;

            [JsonPropertyName("capability")]
            public string Capability { get; set; } = string.Empty;

            [JsonPropertyName("command")]
            public string Command { get; set; } = string.Empty;

            // Arguments can be of different types (numbers, strings, etc.)
            [JsonPropertyName("arguments")]
            [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // Don't serialize if null
            public List<object>? Arguments { get; set; }
        }

        // --- DTO Classes for Deserialization ---
        // IMPORTANT: These classes are basic placeholders based on typical API structures.
        // You MUST inspect the ACTUAL JSON response from the SmartThings API
        // and adjust these classes (properties, names, types) accordingly.

        public class DeviceListResponse
        {
            [JsonPropertyName("items")]
            public List<Device> Items { get; set; } = new List<Device>();

            // Check API response for pagination links (_links, etc.)
            // [JsonPropertyName("_links")]
            // public Links Links { get; set; }
        }

        public class Device
        {
            [JsonPropertyName("deviceId")]
            public string? DeviceId { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("label")]
            public string? Label { get; set; }

            [JsonPropertyName("manufacturerName")]
            public string? ManufacturerName { get; set; }

            [JsonPropertyName("presentationId")]
            public string? PresentationId { get; set; }

            [JsonPropertyName("locationId")]
            public string? LocationId { get; set; }

            [JsonPropertyName("roomId")]
            public string? RoomId { get; set; }

            // Add other properties as needed based on API response (type, deviceTypeId, etc.)

            [JsonPropertyName("components")]
            public List<DeviceComponent> Components { get; set; } = new List<DeviceComponent>();

            // Placeholder for device health state, adjust based on actual response
            [JsonPropertyName("healthState")]
            public DeviceHealthState? HealthState { get; set; }

            // Add other fields like type, profileId, app, etc. as needed
        }

        public class DeviceComponent
        {
            [JsonPropertyName("id")]
            public string? Id { get; set; } // e.g., "main"

            [JsonPropertyName("label")]
            public string? Label { get; set; }

            [JsonPropertyName("capabilities")]
            public List<DeviceCapabilityReference> Capabilities { get; set; } = new List<DeviceCapabilityReference>();

            // Add categories if present in response
        }

        public class DeviceCapabilityReference
        {
            [JsonPropertyName("id")]
            public string? Id { get; set; } // e.g., "switch", "temperatureMeasurement"
            [JsonPropertyName("version")]
            public int Version { get; set; }
        }

        public class DeviceHealthState
        {
            [JsonPropertyName("state")]
            public string? State { get; set; } // e.g., "ONLINE", "OFFLINE"
                                               // Add lastUpdatedDate if available
        }

        // --- DTOs for GetDeviceCommands Response ---
        // IMPORTANT: Verify against actual API JSON response and adjust properties/types as needed.

        public class DeviceCommandsResponse // Assuming the API returns an object containing a list
        {
            // The API might return the list directly, in which case you'd deserialize to List<DeviceCommandInfo>
            // Using a wrapper class is often safer for handling empty results.
            [JsonPropertyName("commands")] // Check if the list is nested under a key like "commands"
            public List<DeviceCommandInfo> Commands { get; set; } = new List<DeviceCommandInfo>();
        }

        public class DeviceCommandInfo
        {
            [JsonPropertyName("component")]
            public string? Component { get; set; } // e.g., "main"

            [JsonPropertyName("capability")]
            public string? Capability { get; set; } // e.g., "switch", "switchLevel"

            [JsonPropertyName("command")]
            public string? Command { get; set; } // e.g., "on", "off", "setLevel"

            // Optional: Include argument details if the API provides them
            [JsonPropertyName("arguments")]
            public List<CommandArgumentInfo>? Arguments { get; set; }
        }

        public class CommandArgumentInfo
        {
            [JsonPropertyName("name")]
            public string? Name { get; set; } // e.g., "level", "color", "temperature"

            // Schema can be complex. This is a simplified version.
            // You might need a more detailed class or use JsonElement if structure varies wildly.
            [JsonPropertyName("schema")]
            public CommandArgumentSchema? Schema { get; set; }

            [JsonPropertyName("optional")] // Or maybe "required"? Check API response
            public bool Optional { get; set; } = false; // Default based on common patterns
        }

        public class CommandArgumentSchema
        {
            [JsonPropertyName("type")]
            public string? Type { get; set; } // e.g., "integer", "number", "string", "object", "array", "enum"

            // Add other schema properties based on API response, like:
            [JsonPropertyName("minimum")]
            public double? Minimum { get; set; } // Use double? to handle potential absence/non-numeric types

            [JsonPropertyName("maximum")]
            public double? Maximum { get; set; }

            [JsonPropertyName("enumCommands")] // If it's an enum type
            public List<string>? EnumCommands { get; set; } // Or EnumValues? Check API response
        }

        // --- DTO for GetDeviceHealth Response ---
        public class DeviceHealthResponse
        {
            [JsonPropertyName("deviceId")]
            public string? DeviceId { get; set; }

            [JsonPropertyName("state")]
            public string? State { get; set; } // e.g., "ONLINE", "OFFLINE", "UNKNOWN"

            [JsonPropertyName("lastUpdatedDate")]
            public DateTimeOffset? LastUpdatedDate { get; set; } // DateTimeOffset handles timezone correctly
        }

        // --- DTOs for GetDeviceStatus Response ---
        // This structure can be complex as it contains status for all capabilities/attributes.
        // Using Dictionaries with JsonElement offers flexibility for varying attribute structures.
        public class DeviceStatusResponse
        {
            [JsonPropertyName("components")]
            public Dictionary<string, ComponentStatusContainer>? Components { get; set; }
        }

        public class ComponentStatusContainer // Represents the status of one component (e.g., "main")
        {
            // Key is Capability ID (e.g., "switch", "temperatureMeasurement")
            // Value contains the attributes and their status for that capability
            [JsonExtensionData] // Captures all properties not explicitly mapped
            public Dictionary<string, JsonElement>? Capabilities { get; set; }
        }

        // --- DTO for GetDeviceCapabilityStatus Response ---
        // Represents the status of attributes within a single capability
        // Key is Attribute Name (e.g., "switch", "temperature", "level")
        // Value holds the attribute's details (value, unit, timestamp etc.)
        // Using Dictionary<string, CapabilityAttributeStatus>
        public class CapabilityAttributeStatus
        {
            // Using JsonElement for Value allows flexibility (string, number, boolean, object)
            [JsonPropertyName("value")]
            public JsonElement Value { get; set; } // Might be null if value isn't set

            [JsonPropertyName("unit")]
            public string? Unit { get; set; } // e.g., "C", "F", "%"

            [JsonPropertyName("timestamp")]
            public DateTimeOffset? Timestamp { get; set; }

            // Capture any other potential fields in the attribute status
            [JsonExtensionData]
            public Dictionary<string, JsonElement>? ExtraData { get; set; }

            // Helper methods to try and get typed values (add more as needed)
            public string GetStringValue() => Value.ValueKind == JsonValueKind.String ? Value.GetString() : null;
            public double GetDoubleValue() => Value.ValueKind == JsonValueKind.Number ? Value.GetDouble() : -1;
            public int GetIntValue() => Value.ValueKind == JsonValueKind.Number ? Value.GetInt32() : -1;
            public bool GetBooleanValue() => Value.ValueKind == JsonValueKind.True ? true : Value.ValueKind == JsonValueKind.False ? false : false;

        }

        // Placeholder for potential _links structure if pagination is used
        // public class Links { ... }

    }
}