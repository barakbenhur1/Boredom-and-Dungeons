using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoredomAndDungeons
{
    [DisallowMultipleComponent]
    public sealed class BDBossHealthGroup : MonoBehaviour
    {
        [SerializeField] private BDBossEncounterController encounter;
        [SerializeField] private List<BDBossHealthChannel> channels = new List<BDBossHealthChannel>();
        [SerializeField] private bool autoFindChildChannels = true;
        [SerializeField] private bool markVictoryWhenAllAtZero = true;

        public event Action AllChannelsReachedZero;

        public IReadOnlyList<BDBossHealthChannel> Channels => channels;
        public bool AllAtZero
        {
            get
            {
                if (channels.Count == 0)
                    return false;

                for (int i = 0; i < channels.Count; i++)
                {
                    BDBossHealthChannel channel = channels[i];
                    if (channel != null && !channel.IsAtZero)
                        return false;
                }

                return true;
            }
        }

        private void Awake()
        {
            if (encounter == null)
                encounter = GetComponent<BDBossEncounterController>();

            if (autoFindChildChannels)
                RefreshChannels();
        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        public void RefreshChannels()
        {
            Unsubscribe();
            channels.Clear();
            channels.AddRange(GetComponentsInChildren<BDBossHealthChannel>(includeInactive: true));
            Subscribe();
        }

        public BDBossHealthChannel FindChannel(string channelId)
        {
            if (string.IsNullOrWhiteSpace(channelId))
                return null;

            for (int i = 0; i < channels.Count; i++)
            {
                BDBossHealthChannel channel = channels[i];
                if (channel != null && string.Equals(channel.ChannelId, channelId, StringComparison.Ordinal))
                    return channel;
            }

            return null;
        }

        private void Subscribe()
        {
            for (int i = 0; i < channels.Count; i++)
            {
                BDBossHealthChannel channel = channels[i];
                if (channel == null)
                    continue;

                channel.ReachedZero -= HandleChannelReachedZero;
                channel.ReachedZero += HandleChannelReachedZero;
            }
        }

        private void Unsubscribe()
        {
            for (int i = 0; i < channels.Count; i++)
            {
                BDBossHealthChannel channel = channels[i];
                if (channel != null)
                    channel.ReachedZero -= HandleChannelReachedZero;
            }
        }

        private void HandleChannelReachedZero(BDBossHealthChannel channel)
        {
            if (!AllAtZero)
                return;

            AllChannelsReachedZero?.Invoke();

            if (markVictoryWhenAllAtZero && encounter != null)
                encounter.MarkVictory();
        }
    }
}
