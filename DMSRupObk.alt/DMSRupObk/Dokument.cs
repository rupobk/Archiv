using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSRupObk
{
    class Dokument
    {
        public enum Dokstatus { erledigt, zu_bearbeiten };
        public enum Dattyp { jpg, png, pdf, txt, doc, docx, xls, xlsx, ppt, pptx, xml}
        public Guid Nr { get; private set; }
        public string Pfad { get; set; }
        public string Dateiname { get; set; }
        public string Dateityp { get; set; }
        public DateTime Archivierungsdatum { get; set; }
        public DateTime Aenderungsdatum { get; set; }
        public int Jahr { get; set; }
        public string Periode { get; set; }
        public string Dokumentenart { get; set; }
        public string Lieferant { get; set; }
        public string Betreff { get; set; }
        public string Verschlagwortung { get; set; }
        public Dokstatus Dokumentenstatus { get; set; }
        public string Volltextindex { get; set; }

        public Dokument(string Pfad, string Dateiname, string Dateityp, 
                        DateTime Archivierungsdatum, DateTime Aenderungsdatum,
                        int Jahr, string Periode, string Dokumentenart,
                        string Lieferant, string Betreff, string Verschlagwortung,
                        string Dokumentenstatus, string Volltextindex) // nur Pflichtfelder angeben, Optionalfelder unter New Document machen
        {
            this.Nr = Guid.NewGuid();
            //
        }

        public static void CreateDokument(string tempPfad)
        {
            // alle Kontrollroutinen
            // verschiebt Dokument in richtigen Ordner
            // zum Schluss erstellt Objekt Dokument
        }

        public void OpenDokument()
        {

        }

        public void ArchivierenDokument()
        { }
    }

}
