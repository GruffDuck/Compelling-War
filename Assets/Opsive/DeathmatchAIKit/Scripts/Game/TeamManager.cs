/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// The TeamManager coordinates a team and its leader.
    /// </summary>
    public class TeamManager : MonoBehaviour
    {
        /// <summary>
        /// A formation group represents a group of players who search for enemies together.
        /// </summary>
        private class FormationGroup
        {
            private GameObject m_Leader;
            private List<Behavior> m_Followers = new List<Behavior>();

            public GameObject Leader { get { return m_Leader; } set { m_Leader = value; } }

            /// <summary>
            /// Adds the specified player to the formation group.
            /// </summary>
            /// <param name="player">The player to add to the formation group.</param>
            /// <returns>The index of the player within the formation group.</returns>
            public int AddToFormation(Behavior player)
            {
                m_Followers.Add(player);
                return m_Followers.Count - 1;
            }

            /// <summary>
            /// Removes the specified player from the formation group.
            /// </summary>
            /// <param name="player"></param>
            public void RemoveFromFormation(Behavior player)
            {
                m_Followers.Remove(player);
                for (int i = 0; i < m_Followers.Count; ++i) {
                    m_Followers[i].SendEvent("FormationUpdated", i);
                }
            }
        }

        private static TeamManager s_Instance;
        private static TeamManager Instance
        {
            get
            {
                if (s_Instance == null) {
                    s_Instance = new GameObject("Team Manager").AddComponent<TeamManager>();
                }
                return s_Instance;
            }
        }

        private List<List<Behavior>> m_TeamMembers = new List<List<Behavior>>();
        private List<HashSet<GameObject>> m_TeamMembership = new List<HashSet<GameObject>>();
        private List<List<FormationGroup>> m_FormationGroups = new List<List<FormationGroup>>();

        public static bool IsInstantiated { get { return s_Instance != null; } }

        /// <summary>
        /// Initializes the default values.
        /// </summary>
        private void Awake()
        {
            s_Instance = this;
        }

        /// <summary>
        /// Adds a new player to the team.
        /// </summary>
        /// <param name="player">The player to add.</param>
        /// <param name="teamIndex">The index of the team.</param>
        public static void AddTeamMember(GameObject player, int teamIndex)
        {
            Instance.AddTeamMemberInternal(player, teamIndex);
        }

        /// <summary>
        /// Internal method which adds a new player to the team.
        /// </summary>
        /// <param name="player">The player to add.</param>
        /// <param name="teamIndex">The index of the team.</param>
        private void AddTeamMemberInternal(GameObject player, int teamIndex)
        {
            // Create space if this is the first player on the specified team index.
            if (teamIndex >= m_TeamMembers.Count) {
                m_TeamMembers.Add(new List<Behavior>());
                m_TeamMembership.Add(new HashSet<GameObject>());
                m_FormationGroups.Add(new List<FormationGroup>());
            }

            m_TeamMembership[teamIndex].Add(player);
            // The behavior tree will be null for player-controlled characters.
            var behaviorTree = player.GetComponent<BehaviorTree>();
            if (behaviorTree != null) {
                m_TeamMembers[teamIndex].Add(player.GetComponent<BehaviorTree>());
            } else {
                // The behavior tree is null so the player character is forced to be a leader.
                SetLeaderInternal(player, true);
            }
        }

        /// <summary>
        /// Are the two players teammates?
        /// </summary>
        /// <param name="firstPlayer">The first player.</param>
        /// <param name="secondPlayer">The second player.</param>
        /// <returns>True if the players are teammates.</returns>
        public static bool IsTeammate(GameObject firstPlayer, GameObject secondPlayer)
        {
            return Instance.IsTeammateInternal(firstPlayer, secondPlayer);
        }

        /// <summary>
        /// Internal method which determines if the two players teammates.
        /// </summary>
        /// <param name="firstPlayer">The first player.</param>
        /// <param name="secondPlayer">The second player.</param>
        /// <returns>True if the players are teammates.</returns>
        private bool IsTeammateInternal(GameObject firstPlayer, GameObject secondPlayer)
        {
            return TeamIndexForPlayer(firstPlayer) == TeamIndexForPlayer(secondPlayer);
        }

        /// <summary>
        /// Returns the leader of the team that the player is on.
        /// </summary>
        /// <param name="player">The player on the interested team.</param>
        /// <returns>The leader of the team that the player is on.</returns>
        public static GameObject GetLeader(GameObject player)
        {
            return Instance.GetLeaderInternal(player);
        }

        /// <summary>
        /// Internal method which returns the leader of the team that the player is on.
        /// </summary>
        /// <param name="player">The player on the interested team.</param>
        /// <returns>The leader of the team that the player is on.</returns>
        private GameObject GetLeaderInternal(GameObject player)
        {
            var teamIndex = TeamIndexForPlayer(player);

            // Find the closest leader to the player.
            GameObject closestLeader = null;
            var minDistance = float.MaxValue;
            float distance;
            for (int i = 0; i < m_FormationGroups[teamIndex].Count; ++i) {
                if ((distance = (m_FormationGroups[teamIndex][i].Leader.transform.position - player.transform.position).sqrMagnitude) < minDistance) {
                    minDistance = distance;
                    closestLeader = m_FormationGroups[teamIndex][i].Leader;
                }
            }
            return closestLeader;
        }

        /// <summary>
        /// Sets the leader of the team that the player is on.
        /// </summary>
        /// <param name="player">The player on the interested team.</param>
        /// <param name="start">Is the player starting to be the leader?</param>
        public static void SetLeader(GameObject player, bool start)
        {
            Instance.SetLeaderInternal(player, start);
        }

        /// <summary>
        /// Internal method which sets the leader of the team that the player is on.
        /// </summary>
        /// <param name="player">The player on the interested team.</param>
        /// <param name="start">Is the player starting to be the leader?</param>
        private void SetLeaderInternal(GameObject player, bool start)
        {
            var teamIndex = TeamIndexForPlayer(player);
            FormationGroup formationGroup = null;
            if (start) {
                // A new formation requires a new formation group.
                formationGroup = ObjectPool.Get<FormationGroup>();
                formationGroup.Leader = player;
                m_FormationGroups[teamIndex].Add(formationGroup);
            } else {
                // If the player is no longer starting to be the leader then the formation group should be removed. Find the formation group within the list.
                for (int i = 0; i < m_FormationGroups[teamIndex].Count; ++i) {
                    if (m_FormationGroups[teamIndex][i].Leader == player) {
                        formationGroup = m_FormationGroups[teamIndex][i];
                        m_FormationGroups[teamIndex].RemoveAt(i);
                        break;
                    }
                }
                if (formationGroup != null) {
                    // Set the leader to null to notify the followers and then cleanup.
                    formationGroup.Leader = null;
                    ObjectPool.Return(formationGroup);
                } else {
                    Debug.LogError("Error: Unable to find formation group with leader " + player);
                }
            }
        }

        /// <summary>
        /// Adds the player to a new formation.
        /// </summary>
        /// <param name="leader">The leader to follow.</param>
        /// <param name="player">The player to add to the formation.</param>
        /// <returns>The index of the player within the formation.</returns>
        public static int AddToFormation(GameObject leader, Behavior player)
        {
            return Instance.AddToFormationInternal(leader, player);
        }

        /// <summary>
        /// Internal method which adds the player to a new formation.
        /// </summary>
        /// <param name="leader">The leader to follow.</param>
        /// <param name="player">The player to add to the formation.</param>
        /// <returns>The index of the player within the formation.</returns>
        private int AddToFormationInternal(GameObject leader, Behavior player)
        {
            // Add the player to the formation.
            var teamIndex = TeamIndexForPlayer(player.gameObject);
            for (int i = 0; i < m_FormationGroups[teamIndex].Count; ++i) {
                if (m_FormationGroups[teamIndex][i].Leader == leader) {
                    return m_FormationGroups[teamIndex][i].AddToFormation(player);
                }
            }
            return -1; 
        }

        /// <summary>
        /// Removes the player from the formation.
        /// </summary>
        /// <param name="leader">The leader to follow.</param>
        /// <param name="player">The player to remove from the formation.</param>
        public static void RemoveFromFormation(GameObject leader, Behavior player)
        {
            // The Instance may be null if the TeamManager has already been removed when the game is stopping.
            if (Instance == null) {
                return;
            }
            Instance.RemoveFromFormationInternal(leader, player);
        }

        /// <summary>
        /// Removes the player from the formation.
        /// </summary>
        /// <param name="leader">The leader to follow.</param>
        /// <param name="player">The player to remove from the formation.</param>
        private void RemoveFromFormationInternal(GameObject leader, Behavior player)
        {
            // Remove the player from the formation and notify the remaining players that their formation index has updated.
            var teamIndex = TeamIndexForPlayer(player.gameObject);
            for (int i = 0; i < m_FormationGroups[teamIndex].Count; ++i) {
                if (m_FormationGroups[teamIndex][i].Leader == leader) {
                    m_FormationGroups[teamIndex][i].RemoveFromFormation(player);
                }
            }
        }

        /// <summary>
        /// The specified player needs backup. Notify all of the teammates.
        /// </summary>
        /// <param name="player">The player that needs backup.</param>
        /// <param name="target">The target of the player that needs backup.</param>
        public static void RequestBackup(GameObject player, GameObject target)
        {
            Instance.RequestBackupInternal(player, target);
        }

        /// <summary>
        /// Internal method indicating that the specified player needs backup. Notify all of the teammates.
        /// </summary>
        /// <param name="player">The player that needs backup.</param>
        /// <param name="target">The target of the player that needs backup.</param>
        private void RequestBackupInternal(GameObject player, GameObject target)
        {
            var teamIndex = TeamIndexForPlayer(player.gameObject);
            for (int i = 0; i < m_TeamMembers[teamIndex].Count; ++i) {
                if (m_TeamMembers[teamIndex][i].gameObject != player) {
                    m_TeamMembers[teamIndex][i].SendEvent<object, object>("RequestBackup", player.gameObject, target);
                }
            }
        }

        /// <summary>
        /// The specified player has updated their target. Notify all of the teammates.
        /// </summary>
        /// <param name="player">The player that needs backup.</param>
        /// <param name="target">The target of the player that needs backup.</param>
        public static void UpdateBackupRequest(GameObject player, GameObject target)
        {
            Instance.UpdateBackupRequestInternal(player, target);
        }

        /// <summary>
        /// Internal method indicating that the specified player has updated their target. Notify all of the teammates.
        /// </summary>
        /// <param name="player">The player that needs backup.</param>
        /// <param name="target">The target of the player that needs backup.</param>
        private void UpdateBackupRequestInternal(GameObject player, GameObject target)
        {
            var teamIndex = TeamIndexForPlayer(player.gameObject);
            for (int i = 0; i < m_TeamMembers[teamIndex].Count; ++i) {
                if (m_TeamMembers[teamIndex][i].gameObject != player) {
                    m_TeamMembers[teamIndex][i].SendEvent<object, object>("UpdateBackupRequest", player.gameObject, target);
                }
            }
        }

        /// <summary>
        /// The specified player no longer needs backup. Notify all of the teammates.
        /// </summary>
        /// <param name="player">The player that no longer needs backup.</param>
        public static void CancelBackupRequest(GameObject player)
        {
            Instance.CancelBackupRequestInternal(player);
        }

        /// <summary>
        /// Internal method indicating that the specified player no longer needs backup. Notify all of the teammates.
        /// </summary>
        /// <param name="player">The player that no longer needs backup.</param>
        private void CancelBackupRequestInternal(GameObject player)
        {
            var teamIndex = TeamIndexForPlayer(player.gameObject);
            for (int i = 0; i < m_TeamMembers[teamIndex].Count; ++i) {
                if (m_TeamMembers[teamIndex][i].gameObject != player) {
                    m_TeamMembers[teamIndex][i].SendEvent("CancelBackupRequest");
                }
            }
        }
        
        /// <summary>
        /// Returns the team index for the specified player.
        /// </summary>
        /// <param name="player">The player to get the team index of.</param>
        /// <returns>The team index for the specified player.</returns>
        private int TeamIndexForPlayer(GameObject player)
        {
            for (int i = 0; i < m_TeamMembership.Count; ++i) {
                if (m_TeamMembership[i].Contains(player)) {
                    return i;
                }
            }

            // How'd this happen?
            return -1;
        }

        /// <summary>
        /// The instance has been destroyed.
        /// </summary>
        private void OnDestroy()
        {
            s_Instance = null;
        }
    }
}