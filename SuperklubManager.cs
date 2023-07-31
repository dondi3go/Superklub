﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Superklub
{
    /// <summary>
    /// The class to use inorder to have synchronization 
    /// between 3D scenes for several distributed users
    /// </summary>
    public class SuperklubManager
    {
        // Client Id should unique among clients
        private string clientId;

        // Local nodes
        private List<ISuperklubNode> localNodes = new List<ISuperklubNode>();

        // Supersynk Client
        private SupersynkClient supersynkClient;

        // Keep the previous response from the server
        SupersynkClientDTOs oldDistantData = new SupersynkClientDTOs();

        /// <summary>
        /// SuperklubManager requires a SupersynkClient
        /// </summary>
        public SuperklubManager(SupersynkClient supersynkClient)
        { 
            this.supersynkClient = supersynkClient;

            // TODO
            clientId = "ada";
        }

        /// <summary>
        /// Call this method once for each local node at the begining of the session
        /// </summary>
        public void RegisterLocalNode(ISuperklubNode node)
        {
            localNodes.Add(node);
        }

        /// <summary>
        /// Call this method in the main loop :
        /// - Send local nodes to the server (if any)
        /// - Receive distant nodes data
        /// </summary>
        public async Task<SuperklubUpdate> SynchronizeLocalAndDistantNodes()
        {
            // Perform request
            SupersynkClientDTOs newDistantData;
            if (localNodes.Count == 0)
            {
                newDistantData = await supersynkClient.GetAsync();
            }
            else
            {
                var localData = SuperklubNodeConverter.ConvertToSupersynk(clientId, localNodes);
                newDistantData = await supersynkClient.PostAsync(localData);
            }

            // Handle data from distant clients :
            // 1/ Detect new clients / disconnected clients
            // 2/ Detect nodes to Create / Update / Delete

            SuperklubUpdate update = new SuperklubUpdate(oldDistantData, newDistantData);

            oldDistantData = newDistantData;

            return update;
        }
    }
}