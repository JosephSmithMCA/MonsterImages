using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.Win32;

namespace MonsterImages {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        struct Monster {
            public string Name;
            public int   HP;
            public int   gold;
            public int   exp;
            public int   phyPower;
            public int   magiPower;
            public int   phyDefense;
            public int   magiDefense;
            public string family;
            public string weakAgainst;
            public string strongAgainst;
        }//end struct

        Monster[] globalMonsterData;
        int globalRecord;
        public MainWindow() {
            InitializeComponent();
        }//END MAIN
        #region EVENTS


        private void muiOpen_Click(object sender, RoutedEventArgs e) {


            //create the open file dialog object
            OpenFileDialog ofd = new OpenFileDialog();

            //open the dialog and wait for the user to make a selection
            bool fileSelected = (bool)ofd.ShowDialog();

            if (fileSelected == true) {
                //get record count
                globalRecord = RecordCount(ofd.FileName, true);
                //set slider to match
                sldRecord.Maximum = globalRecord - 1;
                //load data from csv & return array of person
                globalMonsterData = ProcessMonsterData(ofd.FileName, globalRecord, true);
                //update form with 1st persons data
                UpdateMonster(0);
            }//end if
        }//end event


        private void btnImageLoader_Click(object sender, RoutedEventArgs e) {
            //GET FILEPATH TO IMAGE
                
                string filePath = $"C:\\Users\\MCA\\source\\repos\\MonsterImages\\bin\\Debug\\net6.0-windows\\Monster Compendium\\images\\{txtFilePath.Text}.png";


            //DISPLAY IMAGE
                
                    LoadImage(imgMain, filePath);
              
        }//event
        private void sldRecord_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            string filePath = $"C:\\Users\\MCA\\source\\repos\\MonsterImages\\bin\\Debug\\net6.0-windows\\Monster Compendium\\images\\{txtFilePath.Text}.png";

            //check if no data has been loaded exit if this is the case
            if (globalMonsterData == null) {
                    sldRecord.Value = 0;
                    return;
                }//end if

            //create an int to snap slider value to
                int sliderInt = (int)sldRecord.Value;

            //update label showin the cerrently selceted record
                lblMonsterIndex.Content = (sliderInt + 1).ToString();
            //update form
                UpdateMonster(sliderInt);
                txtFilePath.Text = txtName.Text;
                LoadImage(imgMain,filePath);
        }//end event
        private void txtFilePath_TextChanged(object sender, TextChangedEventArgs e) {
            txtFilePath.Background = Brushes.White;
            txtFilePath.Foreground = Brushes.Black;


        }//END EVENT
        private void btnBegining_Click(object sender, RoutedEventArgs e) {
            sldRecord.Value = sldRecord.Minimum;
            string filePath = $"C:\\Users\\MCA\\source\\repos\\MonsterImages\\bin\\Debug\\net6.0-windows\\Monster Compendium\\images\\{txtName.Text}.png";
            LoadImage(imgMain, filePath);
            
        }//end event

        private void previous_Click(object sender, RoutedEventArgs e) {
            sldRecord.Value--;
            string filePath = $"C:\\Users\\MCA\\source\\repos\\MonsterImages\\bin\\Debug\\net6.0-windows\\Monster Compendium\\images\\{txtName.Text}.png";
            LoadImage(imgMain, filePath);
            
        }//end event

        private void next_Click(object sender, RoutedEventArgs e) {
            sldRecord.Value++;
            string filePath = $"C:\\Users\\MCA\\source\\repos\\MonsterImages\\bin\\Debug\\net6.0-windows\\Monster Compendium\\images\\{txtName.Text}.png";
            LoadImage(imgMain, filePath);
            
        }//end event

        private void End_Click(object sender, RoutedEventArgs e) {
            sldRecord.Value = sldRecord.Maximum;
            string filePath = $"C:\\Users\\MCA\\source\\repos\\MonsterImages\\bin\\Debug\\net6.0-windows\\Monster Compendium\\images\\{txtName.Text}.png";
            LoadImage(imgMain, filePath);
            
        }//end event
        #endregion

        #region METHODS/FUNCTION

        int RecordCount(string filepath, bool skipHeader) {

            int record = 0;

            StreamReader infile = new StreamReader(filepath);

            if (skipHeader) {
                infile.ReadLine();
            }//end if

            while (infile.EndOfStream == false) {
                infile.ReadLine();
                record++;
            }//end while

            infile.Close();

            return record;
        }//end function

        Monster[] ProcessMonsterData(string filepath, int recordCount, bool skipHeader) {

            Monster[] monsterData = new Monster[recordCount];
            int currentCounter = 0;
            int something = 0;
            StreamReader infile = new StreamReader (filepath);

            if(skipHeader) {
                infile.ReadLine();
            }//end if


            
            while (infile.EndOfStream == false && currentCounter < recordCount) { 

                string record = infile.ReadLine();
               
                string[] field = record.Split(",");
               
                
                

                monsterData[currentCounter].Name = field[0];
                monsterData[currentCounter].HP = MonsterParse(field[1]);
                monsterData[currentCounter].gold = MonsterParse(field[2]);
                monsterData[currentCounter].exp = MonsterParse(field[3]);
                monsterData[currentCounter].phyPower = MonsterParse(field[4]);
                monsterData[currentCounter].magiPower = MonsterParse(field[5]); 
                monsterData[currentCounter].phyDefense = MonsterParse( field[6]);
                monsterData[currentCounter].magiDefense = MonsterParse(field[1]);
                monsterData[currentCounter].family = field[8];
                monsterData[currentCounter].weakAgainst = field[9];
                monsterData[currentCounter].strongAgainst = field[10];
                
                currentCounter++;
            }//end while

            infile.Close ();

            return monsterData;

        }//end function

        int MonsterParse(string monsterData) {
            int parsedValue = 0;
            bool parsed = false;

            parsed = int.TryParse((monsterData), out parsedValue);
 
            return parsedValue;
        }//end function

        void UpdateMonster(int monsterIndex) {
            //grab a person form the global array
            Monster currentMonster = globalMonsterData[monsterIndex];
            //update textboxes on the form
            txtName.Text          = currentMonster.Name;
            txtHP.Text            = currentMonster.HP.ToString();
            txtGold.Text          = currentMonster.gold.ToString();
            txtExp.Text           = currentMonster.exp.ToString();
            txtPhyPower.Text      = currentMonster.phyPower.ToString();
            txtMagiPower.Text     = currentMonster.magiPower.ToString();
            txtPhyDefense.Text    = currentMonster.phyDefense.ToString();
            txtMagiDefense.Text   = currentMonster.magiDefense.ToString();
            txtFamily.Text        = currentMonster.family;
            txtWeakAgainst.Text   = currentMonster.weakAgainst;
            txtStrongAgainst.Text = currentMonster.strongAgainst;


            

        }//end function

        private void LoadImage(Image imgTarget , string imageFilePath) {

            //CHECK IF THE FILE IMAGE EXISTS
            if (File.Exists(imageFilePath) == false) {
                //CREATE BITMAP
                BitmapImage bmpNoImage = new BitmapImage();
                //SET BITMAP FOR EDITING
                bmpNoImage.BeginInit();
                bmpNoImage.UriSource = new Uri("C:\\Users\\MCA\\source\\repos\\MonsterImages\\bin\\Debug\\net6.0-windows\\Monster Compendium\\images\\no image.png");// LOAD IMAGE
                bmpNoImage.EndInit();

                // SET THE SOURCE OF THE IMAGE CONTROL TO THE BITMAP
                imgTarget.Source = bmpNoImage;
            } else {

                //CREATE BITMAP
                BitmapImage bmpImage = new BitmapImage();
                //SET BITMAP FOR EDITING
                bmpImage.BeginInit();
                bmpImage.UriSource = new Uri(imageFilePath);// LOAD IMAGE
                bmpImage.EndInit();

                // SET THE SOURCE OF THE IMAGE CONTROL TO THE BITMAP
                imgTarget.Source = bmpImage;
            }
        }//END FUNCTION
        #endregion


    }//END CLASS
}//END NAMESPACE
