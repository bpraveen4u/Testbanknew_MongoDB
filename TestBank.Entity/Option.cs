using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace TestBank.Entity
{
    [DefaultValue(RadioButton)]
    public enum OptionType
    {
        None = 0,
        RadioButton = 1,
        CheckBox = 2,
        DropDown = 3,
        Text = 4
    }

    public class Option : IEntity<string>
    {
        public string Id { get; set; }
        public OptionType Type { get; set; }
        public string Description { get; set; }
        public bool IsCorrect { get; set; }
        //public virtual Question Question { get; set; }
    }
}
