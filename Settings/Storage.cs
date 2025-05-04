using Security.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Security
{
    public class Storage
    {
        #region Private Variables
        private static readonly string DefaultFileName = "Storage.xml";
        #endregion

        #region Public Properties
        public List<Camera> Cameras { get; set; } = new List<Camera>();
        public List<DoorSensor> DoorSensors { get; set; } = new List<DoorSensor>();
        public List<WindowSensor> WindowSensors { get; set; } = new List<WindowSensor>();
        public List<Room> Rooms { get; set; } = new List<Room>();
        #endregion

        public Storage() { }

        #region Helper Functions

        private static string GetDefaultFilePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), DefaultFileName);
        }

        #endregion

        #region Save/Load Functions

        /// <summary>
        /// Saves the current state of the Storage object to an XML file in the user's Local Application Data folder.
        /// </summary>
        /// <param name="filePath">Optional full path to save the file. If null or empty, defaults to Storage.xml in %LocalAppData%\YourAppName.</param>
        /// <exception cref="Exception">Can throw exceptions related to file access or serialization.</exception>
        public void Save()
        {
            string filePath = GetDefaultFilePath();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Storage));
                using (TextWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, this);
                }
                Console.WriteLine($"Storage saved successfully to '{filePath}'."); // Optional feedback

            }
            catch (Exception ex)
            {
                // Provide more context in the error message
                string errorLocation = "the specified path";
                Console.WriteLine($"Error saving storage file to '{errorLocation}' ('{filePath}'): {ex.Message}");
                // Consider logging the full exception details: Console.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Loads Storage data from an XML file.
        /// </summary>
        /// <param name="filePath">Optional path to load the file from. If null or empty, defaults to Storage.xml in the application directory.</param>
        /// <returns>A Storage object loaded from the file, or a new empty Storage object if the file doesn't exist or an error occurs.</returns>
        public static Storage Load(string filePath = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = GetDefaultFilePath();
            }

            // If the file doesn't exist, return a new empty Storage object
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Storage file '{filePath}' not found. Returning new instance.");
                return new Storage();
            }

            try
            {
                // Create an XmlSerializer for the Storage type
                XmlSerializer serializer = new XmlSerializer(typeof(Storage));

                // Use a TextReader to read the XML file
                using (TextReader reader = new StreamReader(filePath))
                {
                    // Deserialize the XML back into a Storage object
                    Storage loadedStorage = (Storage)serializer.Deserialize(reader);
                    Console.WriteLine($"Storage loaded successfully from '{filePath}'."); // Optional feedback
                    return loadedStorage;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Error loading storage file '{filePath}': {ex.Message}. Returning new instance.");
                // Return a new empty object on error to prevent crashes
                return new Storage();
            }
        }

        #endregion
    }

    public class Room
    {
        public string Name { get; set; }
        public Room() { }
    }

    public class Camera
    {
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string WyzeUsername { get; set; }
        public string WyzePassword { get; set; }
        public bool PlaySound { get; set; } = false;
        public bool Visible { get; set; } = true;
        public Room Room { get; set; }
        [XmlIgnore] public string RTSPAddress => (!string.IsNullOrEmpty(IPAddress) && !string.IsNullOrEmpty(WyzeUsername))
                                    ? $"rtsp://{WyzeUsername}:{WyzePassword}@{IPAddress}/live"
                                    : string.Empty;

        public Camera() { }

        /// <summary>
        /// Optional parameterized constructor for creating cameras programmatically.
        /// </summary>
        public Camera(string ip, string user, string pass, bool sound, bool visible = true)
        {
            IPAddress = ip;
            WyzeUsername = user;
            WyzePassword = pass;
            PlaySound = sound;
            Visible = visible;
        }
    }
}
