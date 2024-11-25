using System.Drawing;

namespace Demo.PL.Helper
{
    public static class DocumentSetting
    {
        public static String UploadFile(IFormFile file , string FolderName)
        {
            //1.get located folder path 
            string FolderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\" +FolderName;

            //2.get filename and make it unique 
            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            //3. Get file path [filepath , foldername ]
            String FilePath = Path.Combine(FolderPath,FileName);

            //4.save path as streams [streams : data per time ]
            using var FS = new FileStream(FilePath , FileMode.Create);
            file.CopyTo(FS);
            //5.return name of file
            return FileName;

        }
        public static void Delete(string FileName , string FolderName)
        {
            //1.get file Path 
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files",FileName);

            //2.Check 
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }

    }

 
}
