using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI.Utils.AttributeUtils.Models;

namespace VocalUtau.DirectUI.Utils.AttributeUtils.WavPartTools
{
    public class WavPartEditors
    {
        public class FileSelector : System.Drawing.Design.UITypeEditor
        {
            public FileSelector()
            {
            }
            public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Wav音频文件|*.wav|所有文件|*.*";
                ofd.CheckFileExists = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
                return value;
            }
        }
        public class TimeReload : System.Drawing.Design.UITypeEditor
        {
            public TimeReload()
            {
            }
            public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.Modal;
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (context.Instance is WavePartAttributes)
                {
                    WavePartAttributes wa = (WavePartAttributes)context.Instance;
                    if (wa.WavPart_RealFileDuring.ToString() != wa.WavPart_FileDuring)
                    {
                        if (MessageBox.Show("当前音频文件长度与段落长度不匹配，要重设段落长度为音频文件长度么？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            return wa.WavPart_RealFileDuring.ToString();
                        };
                    }
                }
                return value;
            }
        }
    }
}
