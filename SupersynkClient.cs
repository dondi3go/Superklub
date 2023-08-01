using System.Threading.Tasks;

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

        // Public properties
        public SupersynkClientStatus Status { get; private set; } = SupersynkClientStatus.NOT_SET;

        /// <summary>
        /// 
        /// </summary>
        public SupersynkClient(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Retrieve node data of distant clients from the server
        /// </summary>
        public async Task<SupersynkClientDTOs> GetAsync()
        {
            // Perform async request
            var response = await httpClient.GetAsync();

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

            // Convert JSON string into DTOs
            return SupersynkClientDTOs.FromJsonString(response.Text);
        }

        /// <summary>
        /// Send local node data to the server
        /// Retreive nodes data of distant clients from the server 
        /// </summary>
        public async Task<SupersynkClientDTOs> PostAsync(SupersynkClientDTO clientDTO)
        {
            // Convert DTO into JSON string
            string jsonString = clientDTO.ToJsonString();
            
            // Perform request
            var response = await httpClient.PostAsync(jsonString);
            
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
    }
}
