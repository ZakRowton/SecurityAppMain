using Security.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Security.Blueprints
{
    public class BlueprintDesign
    {
        // Default path uses CurrentDirectory. Consider Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) for user-specific data.
        public string BackgroundImage { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "images", "blueprint.png");
        public List<CameraControl> CameraControls { get; set; } = new List<CameraControl>();
        public List<SensorControl> SensorControls { get; set; } = new List<SensorControl>();
        public BlueprintDesign() { }

        #region Load/Save Functions

        /// <summary>
        /// Saves the current state of the BlueprintDesign object to BlueprintDesign.xml in the application's current directory.
        /// </summary>
        /// <exception cref="IOException">Throws IOException if saving fails, wrapping the original exception.</exception>
        public void Save()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "BlueprintDesign.xml");
            try
            {
                // Ensure directory exists (optional but good practice)
                string directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Use XmlSerializerNamespaces to avoid xmlns attributes if desired
                // XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                // ns.Add("", ""); // Adds an empty namespace

                XmlSerializer serializer = new XmlSerializer(typeof(BlueprintDesign));
                using (TextWriter writer = new StreamWriter(filePath)) // Overwrites existing file
                {
                    serializer.Serialize(writer, this /*, ns*/); // Pass 'this' object to save
                }
                Console.WriteLine($"BlueprintDesign saved successfully to '{filePath}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving BlueprintDesign file to '{filePath}': {ex.ToString()}"); // Log full exception details
                // Rethrow a more specific exception type if preferred, wrapping the original
                throw new IOException($"Failed to save BlueprintDesign to {filePath}", ex);
            }
        }

        /// <summary>
        /// Loads BlueprintDesign data from BlueprintDesign.xml in the application directory.
        /// </summary>
        /// <returns>A BlueprintDesign object loaded from the file. Returns a new empty BlueprintDesign if the file doesn't exist or a fatal error occurs during loading.</returns>
        public static BlueprintDesign Load()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "BlueprintDesign.xml");

            // FIX 1: Removed the redundant and incorrect 'if (string.IsNullOrWhiteSpace(filePath))' block.

            // If the file doesn't exist, return a new empty object cleanly.
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"BlueprintDesign file '{filePath}' not found. Returning new instance.");
                return new BlueprintDesign();
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(BlueprintDesign));

                // Use FileStream for better control and error handling potential
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (TextReader reader = new StreamReader(fs))
                {
                    BlueprintDesign loadedDesign = (BlueprintDesign)serializer.Deserialize(reader);
                    Console.WriteLine($"BlueprintDesign loaded successfully from '{filePath}'.");

                    // FIX 2: Removed unnecessary loadedDesign.Save();

                    // IMPORTANT: Post-load processing (like re-linking Camera objects)
                    // should happen *after* this method returns, in the code that
                    // has access to both the 'loadedDesign' and the master 'Storage.Cameras' list.
                    // See example below the class.

                    return loadedDesign; // Return the loaded object
                }
            }
            catch (Exception ex)
            {
                // Log detailed error for debugging
                Console.WriteLine($"CRITICAL ERROR loading BlueprintDesign file '{filePath}': {ex.ToString()}. Returning new instance to prevent crash.");
                // Consider more robust error handling like attempting a backup load, user notification etc.

                // FIX 3: Removed 'newDesign.Save();'. DO NOT overwrite the file on a load error.
                // Return a new empty object to allow the application to potentially continue,
                // but the original file (even if corrupted) remains for inspection/recovery.
                return new BlueprintDesign();
            }
        }
        #endregion
    }

    public class CameraControl
    {
        // FIX 4: Store an identifier (e.g., Camera Name) instead of the whole Camera object
        public string CameraIdentifier { get; set; }

        // Position remains the same
        public Point Position { get; set; }

        // Name for this specific control instance (optional)
        public string Name { get; set; }

        public Camera Camera { get; set; }

        // Parameterless constructor required by XmlSerializer
        public CameraControl() { }
    }

    public class SensorControl
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string RoomID { get; set; }

        // Apply same pattern if Sensor object is complex
        public string SensorIdentifier { get; set; }

        public DoorSensor DoorSensor { get; set; }
        public WindowSensor WindowSensor { get; set; }

        public Point Position { get; set; }
        public SensorControl() { }
    }
}
