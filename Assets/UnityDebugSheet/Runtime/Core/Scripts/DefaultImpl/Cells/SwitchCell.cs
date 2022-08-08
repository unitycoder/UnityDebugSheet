﻿using System;
using UnityDebugSheet.Runtime.Core.Scripts.DefaultImpl.CellParts;
using UnityEngine;
using UnityEngine.UI;

namespace UnityDebugSheet.Runtime.Core.Scripts.DefaultImpl.Cells
{
    public sealed class SwitchCell : Cell<SwitchCellModel>
    {
        [SerializeField] private LayoutElement _layoutElement;
        [SerializeField] private RectTransform _contents;

        public CellIcon icon;
        public CellTexts cellTexts;
        public Toggle toggle;

        protected override void SetModel(SwitchCellModel model)
        {
            // Icon
            icon.Setup(model.Icon);
            icon.gameObject.SetActive(model.Icon.Sprite != null);

            //Texts
            cellTexts.Setup(model.CellTexts);

            // Toggle
            toggle.SetIsOnWithoutNotify(model.Value);
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(x =>
            {
                model.Value = x;
                model.InvokeToggled(x);
            });

            // Height
            var height = model.UseSubTextOrIcon ? 68 : 42; // Texts
            height += 36; // Padding
            height += 1; // Border
            _contents.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            _layoutElement.preferredHeight = height; // Set the preferred height for the recycler view.
        }
    }

    public sealed class SwitchCellModel : CellModel
    {
        public SwitchCellModel(bool useSubTextOrIcon)
        {
            UseSubTextOrIcon = useSubTextOrIcon;
        }

        public CellIconModel Icon { get; } = new CellIconModel();

        public bool UseSubTextOrIcon { get; }

        public bool Value { get; set; }

        public CellTextsModel CellTexts { get; } = new CellTextsModel();

        public event Action<bool> ValueChanged;

        internal void InvokeToggled(bool isOn)
        {
            ValueChanged?.Invoke(isOn);
        }
    }
}
