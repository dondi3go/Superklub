using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Superklub
{
    public enum SupersynkClientStatus 
    { 
        NOT_SET, 
        CONNECTED, 
        CONNECTION_ERROR,
        RESPONSE_ERROR 
    }

    /// <summary>
    /// A client for the supersynk server
    /// Rely on a
    /// Can perform :
    /// - [GET] HTTP request to get the clients of a supersynk channel, cf GetAsync
    /// - [POST] HTTP request to update a client of supersynk channel, cf PostAsync
    /// </summary>
    public class SupersynkClient
    {
        // Dotnet implementation dependant HTTP Client
        private IHttpClient httpClient;

        // Current client status relative to networking
        public SupersynkClientStatus Status { get; private set; } = SupersynkClientStatus.NOT_SET;

        // clientId should unique among all clients connected to a channel
        public string clientId = String.Empty;

        // ClientId should unique among all clients connected to a channel
        public string ClientId
        {
            get { return clientId; }
        }

        // LATE REQUESTS / LATE RESPONSES MANAGEMENT
        private bool HandleLateMessages = false;

        // LATE REQUESTS / LATE RESPONSES MANAGEMENT
        // Will be used to compute "Client-Time" header value
        private DateTime clientStartTime = DateTime.Now;

        // LATE REQUESTS / LATE RESPONSES MANAGEMENT
        // Store time sent by server in message header 'Server-Time'
        private double latestServerTime = 0;

        // LATE REQUESTS / LATE RESPONSES MANAGEMENT
        // Request Headers
        private static readonly string CLIENT_ID_HEADER = "Client-Id";
        private static readonly string CLIENT_TIME_HEADER = "Client-Time";
        // Response Headers
        private static readonly string SERVER_TIME_HEADER = "Server-Time";

        /// <summary>
        /// 
        /// </summary>
        public SupersynkClient(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
            clientId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Retrieve node data of distant clients from the server
        /// </summary>
        public async Task<SupersynkClientDTOs?> GetAsync(string url)
        {
            // Perform async request
            var response = await httpClient.GetAsync(
                url, 
                CreateClientHeaders(),
                CreateServerTimeHeader());

            // Handle result
            if (response.Code == 0)
            {
                UpdateStatus(SupersynkClientStatus.CONNECTION_ERROR);
                return new SupersynkClientDTOs();
            }
            else if (response.Code != 200 && response.Code != 204)
            {
                UpdateStatus(SupersynkClientStatus.RESPONSE_ERROR);
                return new SupersynkClientDTOs();
            }

            UpdateStatus(SupersynkClientStatus.CONNECTED);

            // HTTP code 204 means 'late request'
            if (response.Code == 204)
            {
                return null;
            }

            // Check server time header, it can be a 'late response'
            double serverTime = ExtractServerTimeHeader(response.CustomHeaderValue);
            if (serverTime > 0) // server == -1 when no server time set 
            {
                if (serverTime < latestServerTime)
                {
                    return null;
                }
            }

            // Update previousServerTime
            latestServerTime = serverTime;

            // Convert JSON string into DTOs
            return SupersynkClientDTOs.FromJsonString(response.Text);
        }

        /// <summary>
        /// Send local node data to the server
        /// Retreive nodes data of distant clients from the server 
        /// </summary>
        public async Task<SupersynkClientDTOs?> PostAsync(string url, SupersynkClientDTO clientDTO)
        {
            // Convert DTO into JSON string
            string jsonString = clientDTO.ToJsonString();
            
            // Perform request
            var response = await httpClient.PostAsync(
                url,
                jsonString,
                CreateClientHeaders(),
                CreateServerTimeHeader());
            
            // Handle result
            if (response.Code == 0)
            {
                UpdateStatus(SupersynkClientStatus.CONNECTION_ERROR);
                return new SupersynkClientDTOs();
            }
            else if (response.Code != 200 && response.Code != 204)
            {
                UpdateStatus(SupersynkClientStatus.RESPONSE_ERROR);
                return new SupersynkClientDTOs();
            }

            UpdateStatus(SupersynkClientStatus.CONNECTED);

            // HTTP code 204 means 'late request'
            if (response.Code == 204)
            {
                return null;
            }

            // Check server time header, it can be a 'late response'
            double serverTime = ExtractServerTimeHeader(response.CustomHeaderValue);
            if (serverTime > 0) // server == -1 when no server time set 
            {
                if (serverTime < latestServerTime)
                {
                    return null;
                }
            }

            // Convert JSON string into DTO
            return SupersynkClientDTOs.FromJsonString(response.Text);
        }

        /// <summary>
        /// TODO : Add a delegate to be informed of status updates
        /// </summary>
        private void UpdateStatus(SupersynkClientStatus status)
        {
            if (status != this.Status)
            {
                this.Status = status;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<CustomRequestHeader>? CreateClientHeaders()
        {
            if (HandleLateMessages)
            {
                List<CustomRequestHeader> list = new List<CustomRequestHeader>();

                // Add Client-Id
                list.Add(new CustomRequestHeader(CLIENT_ID_HEADER, ClientId));

                // Add Client-Time
                var clientTime = (DateTime.Now - clientStartTime).TotalSeconds;
                var culture = System.Globalization.CultureInfo.InvariantCulture;
                var clientTimeAsString = clientTime.ToString("0.000", culture);
                // Log
                //Console.WriteLine("Set Client-Time to " + clientTimeAsString);
                list.Add(new CustomRequestHeader(CLIENT_TIME_HEADER, clientTimeAsString));

                return list;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly CustomResponseHeader serverTimeResponseHeader =
            new CustomResponseHeader(SERVER_TIME_HEADER);
        private CustomResponseHeader? CreateServerTimeHeader() 
        {
            if (HandleLateMessages)
            {
                return serverTimeResponseHeader;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        private double ExtractServerTimeHeader(string customHeaderValue)
        {
            if (HandleLateMessages || string.IsNullOrEmpty(customHeaderValue))
            {
                var culture = System.Globalization.CultureInfo.InvariantCulture;
                var numberStyle = System.Globalization.NumberStyles.Any;
                if (double.TryParse(customHeaderValue, numberStyle, culture, out var serverTime))
                {
                    return serverTime;
                }
            }
            return -1f;
        }
    }
}
