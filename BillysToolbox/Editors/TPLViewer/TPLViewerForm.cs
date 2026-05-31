using kartlib.Imaging;
using kartlib.Serial;

namespace BillysToolbox.Editors
{
    public partial class TPLViewerForm : Form, IEditor
    {
        private TPL? FileInstance;
        private U8? ParentInstance;

        private int SelectedImage = 0;

        public TPLViewerForm(TPL fileInstance)
        {
            FileInstance = fileInstance;
            InitializeComponent();
            PopulateUI();
        }

        public TPLViewerForm(TPL fileInstance, U8? parentInstance)
        {
            FileInstance = fileInstance;
            ParentInstance = parentInstance;
            InitializeComponent();
            PopulateUI();
        }

        public void SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = Path.GetFileNameWithoutExtension(FileInstance.Filename);
            sfd.Filter = "TPL Files (*.tpl)|*.tpl";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                byte[] buffer = FileInstance.Write();
                File.WriteAllBytes(sfd.FileName, buffer);
            }
        }

        public void Save()
        {
            if (ParentInstance != null)
            {
                int index = ParentInstance.FindIndexFromName(FileInstance.Filename);
                if (index > 0)
                {
                    ParentInstance.Nodes[index].Data = FileInstance.Write();
                }
            }
            else if (!File.Exists(FileInstance.Filename))
            {
                SaveAs();
                return;
            }
            else
            {
                byte[] buffer = FileInstance.Write();
                File.WriteAllBytes(FileInstance.Filename, buffer);
            }
        }

        private string FormatToString(ImageFormatEnum format)
        {
            switch (format)
            {
                case ImageFormatEnum.I4:
                    return "I4";
                case ImageFormatEnum.I8:
                    return "I8";
                case ImageFormatEnum.IA4:
                    return "IA4";
                case ImageFormatEnum.IA8:
                    return "IA8";
                case ImageFormatEnum.RGB565:
                    return "RGB565";
                case ImageFormatEnum.RGB5A3:
                    return "RGB5A3";
                case ImageFormatEnum.RGBA8:
                    return "RGBA8";
                case ImageFormatEnum.C4:
                    return "C4";
                case ImageFormatEnum.C8:
                    return "C8";
                case ImageFormatEnum.C14:
                    return "C14";
                case ImageFormatEnum.CMPR:
                    return "CMPR";
                default:
                    return "";
            }
        }

        private void PopulateUI()
        {
            if (FileInstance == null) return;
            if (FileInstance.Images.Count <= 0) return;

            listBox1.Items.Clear();
            for (int i = 0; i < FileInstance.Images.Count; i++)
            {
                listBox1.Items.Add($"Image {i}");
            }

            TPL._Image image = FileInstance.Images[SelectedImage];

            imagePreview.BackgroundImage = image.Image;
            imageStatusStrip.Items[0].Text = $"Image Format: {FormatToString(image.GetFormat())} |";
            imageStatusStrip.Items[1].Text = $"{image.Image.Width}x{image.Image.Height} |";
        }

        private void addImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;|" +
                         "JPEG Image|*.jpg;*.jpeg|" +
                         "PNG Image|*.png|" +
                         "Bitmap Image|*.bmp|" +
                         "All Files|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string s in ofd.FileNames)
                    {
                        try
                        {
                            Image image = Image.FromFile(s);
                            Bitmap bmp = new Bitmap(image);
                            FileInstance.AddImage(bmp, ImageFormatEnum.CMPR);
                        }
                        catch (Exception ex)
                        {
                            return;
                        }
                    }

                    PopulateUI();
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedImage = listBox1.SelectedIndex;
            PopulateUI();
        }

        private void exportImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;|" +
                         "JPEG Image|*.jpg;*.jpeg|" +
                         "PNG Image|*.png|" +
                         "Bitmap Image|*.bmp|" +
                         "All Files|*.*";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        TPL._Image image = FileInstance.Images[SelectedImage];
                        image.Image.Save(sfd.FileName);
                    }
                    catch (Exception ex)
                    {
                        return;
                    }
                }
            }
        }
    }
}
