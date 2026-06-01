using kartlib.Serial;

namespace BillysToolbox.Editors
{
    public class EditorFactory
    {
        public static Form? GetEditor(string fileName)
        {
            byte[] buffer = File.ReadAllBytes(fileName);
            return GetEditor(buffer, fileName, null);
        }

        public static Form? GetEditor(byte[] buffer, string fileName, U8? parentInstance)
        {
            string extension = Path.GetExtension(fileName);
            switch(extension.ToLower())
            {
                case ".arc":
                    U8 arc = new U8(buffer, fileName);
                    return new U8EditorForm(arc);
                case ".u8":
                    U8 u8 = new U8(buffer, fileName);
                    return new U8EditorForm(u8, false);
                case ".szs":
                    U8 szs = new U8(YAZ0.Decompress(buffer), fileName);
                    return new U8EditorForm(szs);
                case ".bmm":
                    BMM bmm = new BMM(buffer, fileName);
                    return new BMMEditorForm(bmm, parentInstance);
                case ".kmp":
                    KMP kmp = new KMP(buffer, fileName);
                    return new KMPEditorForm(kmp, parentInstance);
                case ".kcl":
                    KCL kcl = new KCL(buffer, fileName);
                    return new KCLEditorForm(kcl, parentInstance);
                case ".blight":
                case ".blight1":
                case ".blight2":
                    BLIGHT blight = new BLIGHT(buffer, fileName);
                    return new BLIGHTEditorForm(blight, parentInstance);
                case ".bdof":
                case ".bdof1":
                case ".bdof2":
                    BDOF bdof = new BDOF(buffer, fileName);
                    return new BDOFEditorForm(bdof, parentInstance);
                case ".tpl":
                    TPL tpl = new TPL(buffer, fileName);
                    return new TPLViewerForm(tpl, parentInstance);
                default:
                    return null;
            }
        }
    }
}
