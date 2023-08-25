using System.Collections.Generic;

namespace Superklub
{
    /// <summary>
    /// Takes two consecutive response from the server and make a diff.
    /// Provides :
    /// - clients connected / disconnected
    /// - nodes to create / update / delete in the scene graph
    /// </summary>
    public class SuperklubUpdate
    {
        // Diff related to clients
        public List<string> newConnectedClients { get; private set; } = new List<string>();
        public List<string> disconnectedClients { get; private set; } = new List<string>();

        // Diff related to nodes
        public List<SuperklubNodeRecord> nodesToCreate { get; private set; } = new List<SuperklubNodeRecord>();
        public List<SuperklubNodeRecord> nodesToUpdate { get; private set; } = new List<SuperklubNodeRecord>();
        public List<SuperklubNodeRecord> nodesToDelete { get; private set; } = new List<SuperklubNodeRecord>();

        /// <summary>
        /// Use this constructor when no update is required
        /// </summary>
        public SuperklubUpdate()
        {
        }

        /// <summary>
        /// Use this constructor to detect differences between two requests on the server
        /// </summary>
        public SuperklubUpdate(
        SupersynkClientDTOs oldDTOs,
        SupersynkClientDTOs newDTOs)
        {
            // Compare clients in old and new list of SupersynkClientDTO
            newConnectedClients = GetClientIdsInListAButNotInListB(newDTOs, oldDTOs);
            disconnectedClients = GetClientIdsInListAButNotInListB(oldDTOs, newDTOs);

            // Extract Nodes from SupersynkClientDTO
            List<SuperklubNodeRecord> oldNodes = ExtractNodes(oldDTOs);
            List<SuperklubNodeRecord> newNodes = ExtractNodes(newDTOs);

            // Compare nodes in old and new list of nodes
            nodesToCreate = GetNodesInListAButNotInListB(newNodes, oldNodes);
            nodesToUpdate = GetNodesInListAButAlsoInListB(newNodes, oldNodes);
            nodesToDelete = GetNodesInListAButNotInListB(oldNodes, newNodes);
        }

        /// <summary>
        /// Get a list of client ids belonging to list A
        /// but missing from list B
        /// </summary>
        public static List<string> GetClientIdsInListAButNotInListB(
            SupersynkClientDTOs listA,
            SupersynkClientDTOs listB
            )
        {
            // Convert list B into a hashset of client Ids
            var clientIdsInListB = ToClientIdHashset(listB);

            List<string> clientIds = new List<string>();
            foreach (var client in listA)
            {
                if (!clientIdsInListB.Contains(client.ClientId))
                {
                    clientIds.Add(client.ClientId);
                }
            }
            return clientIds;
        }

        /// <summary>
        /// Convert a list of SupersynkClientDTOs into a Hashset of client Ids
        /// </summary>
        public static HashSet<string> ToClientIdHashset(SupersynkClientDTOs clientPropertiesDTOs)
        {
            HashSet<string> clientIdsHashset = new HashSet<string>();
            foreach (SupersynkClientDTO clientPropertiesDTO in clientPropertiesDTOs)
            {
                clientIdsHashset.Add(clientPropertiesDTO.ClientId);
            }
            return clientIdsHashset;
        }

        /// <summary>
        /// Extract nodes of every client DTOs  
        /// </summary>
        public List<SuperklubNodeRecord> ExtractNodes(SupersynkClientDTOs DTOs)
        {
            List<SuperklubNodeRecord> nodes = new List< SuperklubNodeRecord>();
            foreach (SupersynkClientDTO DTO in DTOs)
            {
                nodes.AddRange(SuperklubNodeConverter.ConvertFromSupersynk(DTO));
            }
            return nodes;
        }

        /// <summary>
        /// Get the nodes belonging to list A
        /// but missing from list B
        /// </summary>
        public static List<SuperklubNodeRecord> GetNodesInListAButNotInListB(
            List<SuperklubNodeRecord> listA,
            List<SuperklubNodeRecord> listB)
        {
            // Convert list B into a hashset of node Ids
            HashSet<string> nodeIdsInListB = ToNodeIdHashset(listB);

            List<SuperklubNodeRecord> nodes = new List<SuperklubNodeRecord>();
            foreach (var node in listA)
            {
                if (!nodeIdsInListB.Contains(node.Id))
                {
                    nodes.Add(node);
                }
            }
            return nodes;
        }

        /// <summary>
        /// Convert a list of nodes into a Hashset of node Ids
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
        /// Get the list of nodes belonging to list A and list B
        /// </summary>
        public static List<SuperklubNodeRecord> GetNodesInListAButAlsoInListB(
            List<SuperklubNodeRecord> listA,
            List<SuperklubNodeRecord> listB
            )
        {
            // Convert list B into a hashset of node Ids
            HashSet<string> nodeIdsInListB = ToNodeIdHashset(listB);

            List<SuperklubNodeRecord> nodes = new List<SuperklubNodeRecord>();
            foreach (var node in listA)
            {
                if (nodeIdsInListB.Contains(node.Id))
                {
                    nodes.Add(node);
                }
            }
            return nodes;
        }
    }
}
