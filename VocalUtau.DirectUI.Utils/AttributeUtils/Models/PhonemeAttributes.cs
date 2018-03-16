using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace VocalUtau.DirectUI.Utils.AttributeUtils.Models
{
    [DefaultProperty("Phoneme_Atom")]
    public class PhonemeAttributes
    {
        [CategoryAttribute("音符信息"), DisplayName("发音符号")]
        public string TEST
        {
            get
            {
                return "TEST";
            }
        }
    }
}
