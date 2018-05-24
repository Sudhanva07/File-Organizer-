using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;


namespace AlphaZero
{
    
    
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

       public static int NoOfFiles;
       
        public struct File_Index {
            
           public string filename;
           public int pos;
           
        }
        
        public static File_Index[] fs = new File_Index[500];
        public string FileName { get;set; }
        public string pos { get; set; }
        public String path;
        public string user_path;
        public int FilesCreated = 0;
        public string[] foldType = new string[] {
            "Document",
            "Videos",
            "Compressed",
            "Images",
            "Audio",
            };
        int sort_type;
        public string[] Alphabets = new string[] { "a", "b","c","d", "e" , "f" , "g" , "h" , "i" , "j" , "k" , "l" , "m" , "n" , "o" , "p","q","r","s","t","u","v","w","x","y","z"};

        public string[] Document = new string[]
        {
            "*.doc",
            "*.odt",
            "*.pdf",
            "*.rtf",
            "*.tex",
            "*.txt",
            "*.wks",
            "*.wpd",
            "*.wps"
        };

        public string[] Videos = new string[]
        {
            "*.3g2",
            "*.3gp",
            "*.avi",
            "*.flv",
            "*.h264",
            "*.m4v",
            "*.mkv",
            "*.mov",
            "*.mp4",
            "*.mpg",
            "*.mpeg",
            "*.rm",
            "*.swf",
            "*.vob",
            "*.wmv",

        };

        public string[] Images = new string[]
        {
            "*.ai",
            "*.bmp",
            "*.gif",
            "*.ico",
            "*.jpeg",
            "*.jpg",
            "*.png",
            "*.ps",
            "*.psd",
            "*.svg",
            "*.tif",
            "*.tiff",
        };

        public string[] Compressed = new string[]
        {
            "*.7z",
            "*.arj",
            "*.deb",
            "*.pkg",
            "*.rar",
            "*.rpm",
            "*.tar.gz",
            "*.tar",
            "*.z",
            "*.zip",
        };

        public string[] Audio = new string[]
        {
            "*.aif",
            "*.cda",
            "*.mid",
            "*.midi",
            "*.mp3",
            "*.mpa",
            "*.ogg",
            "*.wav",
            "*.wma",
            "*.wpl",
        };

        public string Document_path;
        public string Document_path_copy;
        public int Document_Done = 0;
        public int a_z_done = 0;
        public int type_wise_done = 0;
        Form2 form = new Form2();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Document_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                Document_path = Document_path + "\\" + "AlphaZero";
                if (File.Exists(Document_path + "\\fileList_Before.txt"))
                    File.Delete(Document_path + "\\fileList_Before.txt");

                if (File.Exists(Document_path + "\\fileList_After.txt"))
                    File.Delete(Document_path + "\\fileList_After.txt");

                Boolean Correct = ISCorrect();
                if(Correct)
                    create_Document();
                FolderBrowserDialog fd1 = new FolderBrowserDialog();
                //fd1.ShowDialog();
                
                if (fd1.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = fd1.SelectedPath;
                    path = textBox1.Text + "\\";
                    //MessageBox.Show(path);

                }

               
                
            }

            catch(IOException ex)
            {
                MessageBox.Show("Error Selecting the path "+ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                
                form.progressBar1.Minimum = 0;
                form.progressBar1.Maximum = 200;
                switch (sort_type)
                {
                    case 1: CreateFolder_Type_wise();break;
                    case 2: CreateFolder_a_z();break;
                    
                }
                form.Show();
                if(a_z_done == 1 || type_wise_done == 1)
                {
                    MessageBox.Show("Process complete");
                    a_z_done = 0;
                    type_wise_done = 0;
                    form.progressBar1.Value = 0;
                    form.Close();
                }

            }
            catch(IOException exc)
            {
                MessageBox.Show("Error Moving the files" + exc);
            }
        }
        void CreateFolder_Type_wise()
        {
            try
            {
                DirectoryInfo dinfo = new DirectoryInfo(@path);
                
                
                //string[] foldtype = File.ReadAllLines(@"C:\Users\Ryzen_vega\source\repos\AlphaZero\AlphaZero\FolderType.txt");
                foreach (string fold in foldType)
                {

                    // MessageBox.Show(fold);
                     string foldTypePath = Document_path +"\\" + fold + ".txt";
                     // MessageBox.Show(foldTypePath);
                     string[] filetype = File.ReadAllLines(foldTypePath);
                    

                    
                    foreach (string filet in filetype)
                    {
                        //MessageBox.Show(filet);
                        //MessageBox.Show("here");
                        FileInfo[] files = dinfo.GetFiles(filet);
                        foreach( FileInfo filets in files)
                        {
                            //string ss = filets.Name;
                           // MessageBox.Show("here1");
                            if ( filets != null)
                            {
                                
                              //  MessageBox.Show(filets.Name);
                                
                                if (!Directory.Exists(path + fold))
                                { 
                                    //MessageBox.Show("here");
                                    Directory.CreateDirectory(path + fold);


                                }
                            }
                        }
                    }

                    DirectoryInfo[] info = dinfo.GetDirectories();
                    if((info != null)&&(!Directory.Exists("Folder")))
                    {
                        Directory.CreateDirectory(path + "Folder");
                       
                    }
                    
                  
                }
                for (int i = 0; i < 100; i++)
                    form.progressBar1.Increment(1);
                
                SelectMove();
            }
            catch
            {
                MessageBox.Show("Error in creating a file");
            }
        }

       
        void SelectMove()
        {
            DirectoryInfo d = new DirectoryInfo(@path);
            //string[] folTyp = File.ReadAllLines(@"C:\Users\Ryzen_vega\source\repos\AlphaZero\AlphaZero\FolderType.txt");
            foreach (string folTy in foldType)
            {
                string foldTypePath = Document_path + "\\" + folTy + ".txt";
                string[] filetype = File.ReadAllLines(foldTypePath);
                String str = "";
                foreach (string fT in filetype )
                {
                    FileInfo[] files = d.GetFiles(fT);


                    foreach (FileInfo file in files)
                    {
                        str = file.Name;

                        File.Move(path + str, path + folTy + "\\" + str);
                    }
                }
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(@path);
            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            foreach (DirectoryInfo info in directories)
            {
                if ((info.Name == "Folder")||(info.Name == "Document")|| (info.Name == "Compressed")|| (info.Name == "Audio")|| (info.Name == "Videos")|| (info.Name == "Images"))
                    continue;
                else
                    Directory.Move(path + info.Name, path + "Folder\\" + info);
            }


            for (int i = 0; i < 100; i++)
                form.progressBar1.Increment(1);

            type_wise_done = 1;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            sort_type = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            sort_type = 2;
        }

        public void CreateFolder_a_z()
        {
            try
            {
                int ot_files = 0;
                DirectoryInfo dir = new DirectoryInfo(@path);
                foreach(string al in Alphabets)
                {
                    string alpType = al + "*";
                    FileInfo[] file = dir.GetFiles(alpType);
                    foreach(FileInfo testFile in file){
                        if( testFile != null)
                        {
                            if(!Directory.Exists(path + al))
                            {
                                Directory.CreateDirectory(path + al);
                            }
                        }
                    }
                }

                foreach(string al in Alphabets)
                {
                    string altype = al + "*";
                    DirectoryInfo[] directories = dir.GetDirectories(altype);
                    foreach(DirectoryInfo director in directories)
                    {
                        if(director != null)
                        {
                            if (!Directory.Exists(path + al))
                            {
                                Directory.CreateDirectory(path + al);
                            }
                        }
                    }
                }
                DirectoryInfo[] directoryInfo = dir.GetDirectories();
                for (int i = 0; i < directoryInfo.Length; i++) {
                    string str = directoryInfo[i].Name;
                    for (int j = 0; j < Alphabets.Length; j++)
                        if (!str.StartsWith(Alphabets[j]))
                            ot_files = 1;
                            
                }
                if (ot_files == 1)
                    Directory.CreateDirectory(path + "otherfiles");

                for (int i = 0; i < 100; i++)
                    form.progressBar1.Increment(1);
                selectMove_a_z();
            }
            catch(IOException exe)
            {
                MessageBox.Show("erroe" + exe);
            }
        }

        void selectMove_a_z()
        {
            DirectoryInfo dirSelect = new DirectoryInfo(@path);
            foreach(string al in Alphabets)
            {
                string str = al + "*";
                FileInfo[] fileSelect = dirSelect.GetFiles(str);
                foreach(FileInfo testFile in fileSelect)
                {
                    string fileName = testFile.Name;
                    File.Move(path + fileName, path + al + "\\"+ fileName);
                }

            }


            foreach (string al in Alphabets)
            {
                
                string str = al + "*";
                DirectoryInfo[] infos = dirSelect.GetDirectories(str);
                foreach (DirectoryInfo dirInfo in infos)
                {
                    
                    
                        if (dirInfo.Name == al )
                            continue;
                        else
                            Directory.Move(path + dirInfo.Name, path + al + "\\" + dirInfo.Name);
                    
                    


                }
                
               
            }




            for (int i = 0; i < 100; i++)
                    form.progressBar1.Increment(1);

            a_z_done = 1;
        }

        
        void create_Document()
        {
            
            
            

           // MessageBox.Show(Document_path);
            
              if (!Directory.Exists(Document_path))
                {
                    Directory.CreateDirectory(Document_path);


                }
                
                foreach(string folderT in foldType)
                {
                if (!File.Exists(Document_path + "\\"+folderT+".txt"))
                {
                    using (File.Create(Document_path + "\\" + folderT + ".txt")) ;
                }
                
                 
                }

            File.WriteAllLines(Document_path + "\\" + "Document.txt", Document);

            File.WriteAllLines(Document_path + "\\" + "Images.txt", Images);
            File.WriteAllLines(Document_path + "\\" + "Videos.txt", Videos);

            File.WriteAllLines(Document_path + "\\" + "Compressed.txt", Compressed);
            File.WriteAllLines(Document_path + "\\" + "Audio.txt", Audio);

            


            Document_Done = 1;

            
            
        }

       
        bool ISCorrect()
        {
            Document_path_copy = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            Document_path_copy = Document_path_copy + "\\AlphaZero";
                if (!Directory.Exists(Document_path_copy))
                return true;
            else
                return false;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
