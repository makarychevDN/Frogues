using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace FroguesFramework
{
    public class AbilityHint : Hint
    {
        [SerializeField] private SerializedDictionary<string, SubHintsData> mechanicDescriptionsByTags;
        [SerializeField] private List<Hint> additionalHints;

        public override void Init(string header, List<string> textBlocks, Transform button, Vector2 pivot, Vector2 positionRelativeToButton)
        {
            InitSubHint(GetSubHintsData(textBlocks), additionalHints);
            base.Init(header, textBlocks, button, pivot, positionRelativeToButton);
        }

        private List<SubHintsData> GetSubHintsData(List<string> textBlocks)
        {
            List<SubHintsData> subHintsData = new List<SubHintsData>();

            for (int i = 0; i < textBlocks.Count; i++)
            {
                foreach (var mechanicDescription in mechanicDescriptionsByTags)
                {
                    if (textBlocks[i].Contains(mechanicDescription.Key) && !subHintsData.Contains(mechanicDescription.Value))
                    {
                        subHintsData.Add(mechanicDescription.Value);
                    }
                }
            }

            return subHintsData;
        }

        private void InitSubHint(List<SubHintsData> subHintsData, List<Hint> additionalHints)
        {
            if (subHintsData.Count > additionalHints.Count)
            {
                Debug.LogError("need more subhints!");
                return;
            }

            for (int i = 0; i < additionalHints.Count; i++)
            {
                additionalHints[i].gameObject.SetActive(i < subHintsData.Count);

                if (i >= subHintsData.Count)
                    continue;

                additionalHints[i].Init(subHintsData[i].header.GetLocalizedString(), new List<string> { subHintsData[i].body.GetLocalizedString() });
            }
        }
    }

    [Serializable]
    public struct SubHintsData
    {
        public LocalizedString header;
        public LocalizedString body;
    }
}