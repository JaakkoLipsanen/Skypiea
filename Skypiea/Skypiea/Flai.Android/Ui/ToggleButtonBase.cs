
using System;
using System.Collections.Generic;

namespace Flai.Ui
{
    public abstract class ToggleButtonBase : ButtonBase
    {
        public bool IsToggled { get; set; }
        public event GenericEvent<bool> Toggled;

        protected ToggleButtonBase(RectangleF area)
            : this(area, true)
        {
        }

        protected ToggleButtonBase(RectangleF area, bool isToggled)
            : base(area)
        {
            this.IsToggled = isToggled;
        }

        protected override void OnClick()
        {
            this.IsToggled = !this.IsToggled;
            this.Toggled.InvokeIfNotNull(this.IsToggled);
            base.OnClick();
        }
    }

    public abstract class MultiToggleButtonBase<T> : ButtonBase
    {
        public event GenericEvent<T> Toggled;

        private readonly IList<T> _values;
        private int _selectedIndex;

        public T SelectedValue
        {
            get { return _values[_selectedIndex]; }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
        }

        protected MultiToggleButtonBase(RectangleF area, IList<T> values)
            : base(area)
        {
            Ensure.NotNull(values);
            Ensure.True(values.Count > 1);

            _values = values;
        }

        protected override void OnClick()
        {
            _selectedIndex++;
            if (_selectedIndex >= _values.Count)
            {
                _selectedIndex = 0;
            }

            this.Toggled.InvokeIfNotNull(this.SelectedValue);
            base.OnClick();
        }

        public void SetSelectedIndex(int index)
        {
            Ensure.WithinRange(index, 0, _values.Count);
            _selectedIndex = index;
        }

        public void SetSelectedValue(T value)
        {
            // !! if the _values contains duplicates, then this return the index of the first value !!
            this.SetSelectedIndex(_values.IndexOf(value));
        }
    }
}
