using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace KingsDamageMeter
{
    class Cfgenc
    {
        public bool CfgEncoding(string filepath1, string filepath2)
        {
            char chr;
            int bytes = 0;

            try
            {
                FileStream fsin = new FileStream(filepath1, FileMode.Open, FileAccess.Read);
                FileStream fsout = new FileStream(filepath2, FileMode.Create, FileAccess.Write);
                BinaryReader br = new BinaryReader(fsin, Encoding.ASCII);
                BinaryWriter bw = new BinaryWriter(fsout, Encoding.ASCII);

                byte[] temp = br.ReadBytes(33 + 96);
                bw.Write(temp);

                while (true)
                {
                    try
                    {
                        chr = (char)br.ReadByte();
                        if (chr != '\r' && chr != '\n')
                        {
                            chr = (char)~chr;
                        }
                        bytes++;
                         bw.BaseStream.WriteByte((byte)chr);
                    }
                    catch (System.Exception ex)
                    {
                        break;
                    }
                }

                br.Close();
                bw.Close();
                fsin.Close();
                fsout.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }            
            return true;
        }
    }
}
