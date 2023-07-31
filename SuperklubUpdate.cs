using System.Collections.Generic;

namespace Superklub
{
    /// <summary>
    /// Takes two consecutive response from server and make a diff.
    /// Provides :
    /// - clients connected / disconnected
    /// - nodes to create / update / delete in the scene graph
    /// </summary>
    public class SuperklubUpdate
    {
        public List<string> newConnectedClients { get; private set; } = new List<string>();
        public List<string> disconnectedClients { get; private set; } = new List<string>();

        public List<SuperklubNodeRecord> nodesToCreate { get; private set; } = new List<SuperklubNodeRecord>();
        public List<SuperklubNodeRecord> nodesToUpdate { get; private set; } = new List<SuperklubNodeRecord>();
        public List<SuperklubNodeRecord> nodesToDelete { get; private set; } = new List<SuperklubNodeRecord>();

        public SuperklubUpdate(
            SupersynkClientDTOs oldDTOs,
            SupersynkClientDTOs newDTOs
            )
        {
            if (newDTOs == null)
            {
                return;
            }

            if (oldDTOs == null)
            {
                // All clients are new clients
                foreach (var newDTO in newDTOs.List)
                {
                    newConnectedClients.Add(newDTO.ClientId);
                }

                // All nodes are nodes to create
                nodesToCreate = ExtractNodes(newDTOs);
            }
            else
            {
                // Prepare clientIds for comparison
                HashSet<string> oldClientIds = ToClientIdHashset(oldDTOs);
                HashSet<string> newClientIds = ToClientIdHashset(newDTOs);

                // Compare
                newConnectedClients = GetClientIdsNotInHashset(newDTOs, oldClientIds);
                disconnectedClients = GetClientIdsNotInHashset(oldDTOs, newClientIds);

                // Extract Nodes from SupersynkClientDTO
                List<SuperklubNodeRecord> oldNodes = ExtractNodes(oldDTOs);
                List<SuperklubNodeRecord> newNodes = ExtractNodes(newDTOs);

                // Prepare nodeIds for comparison
                HashSet<string> oldNodeIds = ToNodeIdHashset(oldNodes);
                HashSet<string> newNodeIds = ToNodeIdHashset(newNodes);

                // Compare
                nodesToCreate = GetNodesNotInHashset(newNodes, oldNodeIds);
                nodesToDelete = GetNodesNotInHashset(oldNodes, newNodeIds);
                nodesToUpdate = GetNodesInHashset(newNodes, oldNodeIds);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static HashSet<string> ToClientIdHashset(SupersynkClientDTOs clientPropertiesDTOs)
        {
            HashSet<string> clientIdsHashset = new HashSet<string>();
            foreach (SupersynkClientDTO clientPropertiesDTO in clientPropertiesDTOs.List)
            {
                clientIdsHashset.Add(clientPropertiesDTO.ClientId);
            }
            return clientIdsHashset;
        }

        /// <summary>
        /// 
        /// </summary>
        public static List<string> GetClientIdsNotInHashset(
            SupersynkClientDTOs clientPropertiesDTOs,
            HashSet<string> clientIdsHashset)
        { 
            List<string> result = new List<string>();
            foreach (SupersynkClientDTO client in clientPropertiesDTOs.List)
            {
                if (!clientIdsHashset.Contains(client.ClientId))
                {
                    result.Add(client.ClientId);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<SuperklubNodeRecord> ExtractNodes(SupersynkClientDTOs DTOs)
        {
            List< SuperklubNodeRecord> result = new List< SuperklubNodeRecord>();
            foreach (SupersynkClientDTO DTO in DTOs.List)
            {
                result.AddRange(SuperklubNodeConverter.ConvertFromSupersynk(DTO));
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public static HashSet<string> ToNodeIdHashset(List<SuperklubNodeRecord> nodes)
        {
            HashSet<string> nodeIdsHashset = new HashSet<string>();
            foreach (var node in nodes)
            {
                nodeIdsHashset.Add(node.Id);
            }
            return nodeIdsHashset;
        }

        /// <summary>
        /// 
        /// </summary>
        public static List<SuperklubNodeRecord> GetNodesNotInHashset(
            List<SuperklubNodeRecord> nodes,
            HashSet<string> nodeIds)
        {
            List<SuperklubNodeRecord> result = new List<SuperklubNodeRecord>();
            foreach (var node in nodes)
            {
                if (!nodeIds.Contains(node.Id))
                {
                    result.Add(node);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public static List<SuperklubNodeRecord> GetNodesInHashset(
            List<SuperklubNodeRecord> nodes,
            HashSet<string> nodeIds)
        {
            List<SuperklubNodeRecord> result = new List<SuperklubNodeRecord>();
            foreach (var node in nodes)
            {
                if (nodeIds.Contains(node.Id))
                {
                    result.Add(node);
                }
            }
            return result;
        }
    }
}
