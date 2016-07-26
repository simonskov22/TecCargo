using System;
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

namespace TecCargo_Faktura.Controls
{
    /// <summary>
    /// Interaction logic for PakkeControl.xaml
    /// </summary>
    public partial class PakkeControl : UserControl
    {
        public static readonly DependencyProperty ShowAlwaysPlaceholderOnTopProperty =
            DependencyProperty.Register("ShowAlwaysPlaceholderOnTop", typeof(bool), typeof(PakkeControl), new PropertyMetadata(false));
        public static readonly DependencyProperty IsUnLockProperty =
            DependencyProperty.Register("IsUnLock", typeof(bool), typeof(PakkeControl), new PropertyMetadata(true, PropertyChangedCallback_IsUnlock));

        public bool ShowAlwaysPlaceholderOnTop
        {
            get { return (bool)GetValue(ShowAlwaysPlaceholderOnTopProperty); }
            set { SetValue(ShowAlwaysPlaceholderOnTopProperty, value); }
        }
        public bool IsUnLock
        {
            get { return (bool)GetValue(IsUnLockProperty); }
            set { SetValue(IsUnLockProperty, value); }
        }

        public List<PakkeControlItem> Items = new List<PakkeControlItem>();
        public int SetTransportType
        {
            set
            {
                UpdateTransportType(value);
                TranssportId = value;
            }
        }
        public bool IsFaktura = true;
        private bool FirstFakturaItemIsSet = false;
        private int TranssportId = -1;
        private int TranssportId2 = -1;
        private List<int> LastSelectedRowID = new List<int>() { 0, 0 };
        private bool VolumeHasUseRedBorder = false;
        public EventHandler ValueChange;

        const int c_TransportKurer = 0;
        const int c_TransportPakke = 1;
        const int c_TransportGods = 2;

        public PakkeControl()
        {
            InitializeComponent();

            this.Items.Add(new PakkeControlItem() { IsDoneFragt = true, mrkNumb = "1000", contains = "Bob", countI = 5, countS = "2", weightD = 20, weightS = "20", artId = 1, artName = "Ingen", transportTypeId = 1, volume = "50*50*50" , volumeL = 50, volumeB = 50, volumeH = 50 });
            this.Items.Add(new PakkeControlItem() { IsDoneFragt = true, mrkNumb = "3000", contains = "Simon", countI = 11, countS = "2", weightD = 20, weightS = "20", artId = 5, artName = "Ingen", transportTypeId = 5, volume = "50*50*50", volumeL = 50, volumeB = 50, volumeH = 50 });
            this.Items.Add(new PakkeControlItem() { IsDoneFragt = true, mrkNumb = "7000", contains = "Skov", countI = 3, countS = "2", weightD = 20, weightS = "20", artId = 3, artName = "Ingen", transportTypeId = 3, volume = "50*50*50", volumeL = 50, volumeB = 50, volumeH = 50 });
        }

        private static void PropertyChangedCallback_IsUnlock(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            PakkeControl myPakkeControl = dependencyObject as PakkeControl;

            myPakkeControl.Update();
            myPakkeControl.Grid_editItem1.IsEnabled = myPakkeControl.IsUnLock;
            myPakkeControl.grid_faktura.IsEnabled = myPakkeControl.IsUnLock;
        }

        private void ValueHasChanges()
        {
            if (ValueChange != null)
            {
                ValueChange(this, new EventArgs());
            }
        }

        /// <summary>
        /// hent transport type
        /// </summary>
        private void UpdateTransportType(int transportId)
        {
            if (IsFaktura)
            {
                ResetFakturaPackets();
                if (transportId == c_TransportGods)
                {

                }
                else
                {
                    for (int i = 0; i < this.Items.Count; i++)
                    {
                        this.Items[i].IsDoneFaktura = new List<bool>();
                        for (int a = 0; a < this.Items[i].countI; a++)
                        {
                            this.Items[i].IsDoneFaktura.Add(true);
                        }
                    }
                    FirstFakturaItemIsSet = true; //så faktura delen ikke bliver vist
                    Update();
                }
            }

            if (transportId != c_TransportPakke && transportId != c_TransportGods && transportId != c_TransportKurer)
                return;

            if (transportId == c_TransportPakke)
                _transportType.Items = new List<string>() { "XS", "S", "M", "L", "XL", "2XL", "3XL" };
            else if (transportId == c_TransportGods)
                _transportType.Items = new List<string>() { "LDM", "PLL", "M\u00b3" };

            if (transportId == c_TransportPakke || transportId == c_TransportGods)
                _transportType.Visibility = Visibility.Visible;
            else
                _transportType.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// opdatere/loader pakker
        /// </summary>
        public void Update()
        {

            var funcClass = new Class.Functions.others();
            _ItemsContains.Children.Clear();
            _ItemsContains.RowDefinitions.Clear();

            grid_faktura.Visibility = Visibility.Hidden;

            LockFragtbrevStatus(IsFaktura);

            int itemCount = 0;
            int loopRun1 = this.Items.Count;
            bool styleChanger = false;
            int itemMax = GetPacketMaxIndex();
            

            //opret pakker i listview
            for (int i = 0; i < loopRun1; i++)
            {

                //så den kan visse alle pakker hvis faktura er valgt
                int loopRun2 = 1;

                if (IsFaktura)
                    loopRun2 = this.Items[i].countI;              
                

                for (int a = 0; a < loopRun2; a++)
                {
                    //baground farve "converter" på row
                    styleChanger = !styleChanger;

                    //opret rækker
                    _ItemsContains.RowDefinitions.Add(new RowDefinition());
                    int rowDefId = _ItemsContains.RowDefinitions.Count - 1;
                    _ItemsContains.RowDefinitions[rowDefId].Height = GridLength.Auto;

                    //lav elementer
                    Grid _newGrid = new Grid();
                    TextBlock _newHeader = new TextBlock();
                    TextBlock _newIndex = new TextBlock();
                    Button _newEdit = new Button();
                    Button _newDelete = new Button();
                    Border _border = new Border();
                    Border _borderIndex = new Border();

                    //lav kolonner
                    _newGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    _newGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    _newGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    _newGrid.ColumnDefinitions.Add(new ColumnDefinition());
                    _newGrid.ColumnDefinitions[0].Width = GridLength.Auto;
                    _newGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                    _newGrid.ColumnDefinitions[2].Width = GridLength.Auto;
                    _newGrid.ColumnDefinitions[3].Width = GridLength.Auto;
                    
                    //tilføj klik event
                    _newEdit.Click += _EditButton_click;
                    _newDelete.Click += _DeleteButton_click;

                    //skjul slet knap
                    if (this.IsFaktura)
                    {
                        //giv edit knap delete plads
                        Grid.SetColumnSpan(_newEdit, 2);
                        Panel.SetZIndex(_newEdit, 5);
                        _newEdit.HorizontalAlignment = HorizontalAlignment.Right;
                        _newDelete.Visibility = Visibility.Collapsed;
                    }                    


                    if (IsFaktura && this.Items[i].IsDoneFaktura == null)
                    {
                        ResetFakturaPackets();
                    }

                    //lav rammer
                    _border.BorderBrush = Brushes.Black;
                    _borderIndex.BorderBrush = Brushes.Black;
                    _border.BorderThickness = new Thickness(2, 1, 2, 1);
                    _borderIndex.BorderThickness = new Thickness(0, 0, 2, 0);

                    //update status
                    this.Items[i].IsDoneFragt = PacketIsDoneFragt(this.Items[i]);

                    //skift farve på teskt hvis ikke færdig
                    if (!this.Items[i].IsDoneFragt || (IsFaktura && !this.Items[i].IsDoneFaktura[a]))
                    {
                       // _border.BorderBrush = funcClass.ColorBrushHex("#FF0000");
                        //_borderIndex.BorderBrush = funcClass.ColorBrushHex("#FF0000");
                        _newIndex.Foreground = funcClass.ColorBrushHex("#FF0000");
                        _newHeader.Foreground = funcClass.ColorBrushHex("#FF0000");
                    }

                    _newEdit.IsEnabled = true;

                    //dette vil blive brugt som Id for pakkerne
                    _newEdit.Name = "Button_PakkeGrid_Edit_"+i+"_" +a;
                    _newDelete.Name = "Button_PakkeGrid_Delete_" + i + "_" + a; ;

                    //sæt størrelse
                    _newEdit.Width = 35;
                    _newEdit.Height = 35;
                    _newDelete.Width = 35;
                    _newDelete.Height = 35;

                    //giv billede bagrund
                    _newEdit.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/TecCargo Faktura System;component/Images/Icons/editpen.png")));
                    _newDelete.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/TecCargo Faktura System;component/Images/Icons/deletecan.png")));

                    _newIndex.FontSize = 20;
                    _newIndex.FontWeight = FontWeights.Bold;
                    _newIndex.Width = itemMax.ToString().Length * 15;
                    _newIndex.TextAlignment = TextAlignment.Center;
                    _newIndex.Text = (itemCount + 1).ToString();
                    _borderIndex.Child = _newIndex;

                    //header indstilinger
                    _newHeader.FontSize = 20;
                    _newHeader.Width = 250;
                    _newHeader.Text = this.Items[i].mrkNumb;

                    //lav padding
                    _newIndex.Padding = new Thickness(2,5,2,5);
                    _newHeader.Padding = new Thickness(5);
                    _newEdit.Padding = new Thickness(5);
                    _newDelete.Padding = new Thickness(5);

                    //fjern default style
                    _newEdit.Style = (Style)this.FindResource(ToolBar.ButtonStyleKey);
                    _newDelete.Style = (Style)this.FindResource(ToolBar.ButtonStyleKey);

                    //giv bagrund farve
                    if (styleChanger)
                        _newGrid.Background = (Brush)this.FindResource("Row_1");
                    else
                        _newGrid.Background = (Brush)this.FindResource("Row_2");


                    if (!this.IsUnLock)
                    {
                        _borderIndex.IsEnabled = false;
                        _newHeader.IsEnabled = false;
                        _newDelete.IsEnabled = false;

                        //disable color
                        if (styleChanger)
                            _newGrid.Background = (Brush)this.FindResource("Row_1_Disable");
                        else
                            _newGrid.Background = (Brush)this.FindResource("Row_2_Disable");
                        
                        _newIndex.Foreground = funcClass.ColorBrushHex("#FF858585");
                        _newHeader.Foreground = funcClass.ColorBrushHex("#FF858585");                        
                    }


                    //lav row og column
                    Grid.SetRow(_border, rowDefId);
                    
                    Grid.SetColumn(_borderIndex, 0);
                    Grid.SetColumn(_newHeader, 1);
                    Grid.SetColumn(_newEdit, 2);
                    Grid.SetColumn(_newDelete, 3);

                    //tilføj pakke til listview
                    _newGrid.Children.Add(_borderIndex);
                    _newGrid.Children.Add(_newHeader);
                    _newGrid.Children.Add(_newEdit);
                    _newGrid.Children.Add(_newDelete);

                    _border.Child = _newGrid;

                    _ItemsContains.Children.Add(_border);

                    itemCount++;
                }
            }

            _packCount.Content = "Antal: " + itemCount;

            //tjek om det er faktura delen og
            //om den er blivet sat en gang
            if (IsFaktura && !FirstFakturaItemIsSet)
            {
                FirstFakturaItemIsSet = true;
                SetElementPacketData(0, 0);

            }
            else if (!IsFaktura)
            {
                FirstFakturaItemIsSet = false;
            }

            HighLightSelected();
            ValueHasChanges();
        }

        /// <summary>
        /// lav ekstra indstillinger
        /// </summary>
        private void _PakkeControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Update();//opdatere pakke listview
        }


        private bool PacketIsDoneFragt(PakkeControlItem packetItem)
        {
            int[] nullVolume;
            bool volumeStatus = CheckVolume(packetItem.volume, out nullVolume);


            if (packetItem.mrkNumb == "" || packetItem.contains == "" ||
                packetItem.countI <= 0 || packetItem.weightD <= 0 ||
                packetItem.artId == -1 || packetItem.transportTypeId == -1)
                return false;
            else if (this.TranssportId == c_TransportGods && !volumeStatus ||
                     !volumeStatus && packetItem.volume != "")
                return false;
            else
                return true;
        }

        /// <summary>
        /// tilføj pakke til list
        /// </summary>
        private void _AddButton_click(object sender, RoutedEventArgs e)
        {
            List<int> hiddenId = GetPacketIndexs(_hiddenId.Text);
            _hiddenId.Text = "-1"; //sæt id for næste pakke
            PakkeControlItem newItem = new PakkeControlItem();

            int count = 0;
            double weight = 0;
            
            int.TryParse(_antal.Text, out count);
            double.TryParse(_weight.Text, out weight);

            //hent data
            newItem.IsDoneFragt = false;
            newItem.mrkNumb = _mrkNumb.Text;
            newItem.contains = _indhold.Text;
            newItem.countI = count;
            newItem.countS = _antal.Text;
            newItem.weightD = weight;
            newItem.weightS = _weight.Text;
            newItem.artId = _artType.SelectId;
            newItem.artName = _artType.SelectName;
            newItem.volume = _volume.Text;
            newItem.transportTypeId = _transportType.SelectId;
            newItem.transportTypeName = _transportType.SelectName;

            //check volume
            if (newItem.volume != "" || this.TranssportId == c_TransportGods) {
                int[] volumeValue;
                CheckVolume(newItem.volume, out volumeValue);
                
                newItem.volumeL = volumeValue[0];
                newItem.volumeB = volumeValue[1];
                newItem.volumeH = volumeValue[2];
            }
            
            newItem.IsDoneFragt = PacketIsDoneFragt(newItem);

            //man må ikke kunne tilføje helt tomme pakker
            if (!PacketIsEmpty(newItem))
            {
                //nulstil faktura delen
                newItem.IsDoneFaktura = new List<bool>();
                newItem.takstId = new List<int>();
                newItem.takstName = new List<string>();
                newItem.beregningstypeId = new List<int>();
                newItem.beregningstypeName = new List<string>();

                //giv faktura delen en default værdi
                for (int i = 0; i < newItem.countI; i++)
                {
                    newItem.IsDoneFaktura.Add(false);
                    newItem.takstId.Add(-1);
                    newItem.takstName.Add("");
                    newItem.beregningstypeId.Add(-1);
                    newItem.beregningstypeName.Add("");
                }

                // om det er en ny pakke
                if (hiddenId[0] == -1)
                    Items.Add(newItem);
                else
                    Items[hiddenId[0]] = newItem;

                Update();//opdatere listview
                _CancelButton_click(sender, e); //nulstil tekstbokse
            }
        }

        /// <summary>
        /// knap til at annullere pakke indhold
        /// </summary>
        private void _CancelButton_click(object sender, RoutedEventArgs e)
        {
            //sæt textboxen til null
            _mrkNumb.Text = "";
            _indhold.Text = "";
            _antal.Text = "";
            _weight.Text = "";
            _artType.SelectId = -1;
            _volume.Text = "";
            _transportType.SelectId = -1;
            _hiddenId.Text = "-1";

            _volume.RequireEnableRedBorder = false;
            this.VolumeHasUseRedBorder = false;

            grid_faktura.Visibility = Visibility.Hidden; //skjul faktura delen
            _CancelButton.Visibility = Visibility.Hidden; //skjul annuller knap
            _AddButtonText.Content = "Tilføj"; //ændre tekst på tilføj knap
            HighLightSelected(); //fjern highligh
        }

        /// <summary>
        /// ændre på en pakke
        /// </summary>
        private void _EditButton_click(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            List<int> pakkeId = GetPacketIndexs(thisButton.Name);

            SetElementPacketData(pakkeId[0], pakkeId[1]); //sæt textbox elmenter

            if (IsFaktura)
            {
                //skjult fragtbrev knapper
                _AddButtonText.Visibility = Visibility.Collapsed; 
                _CancelButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                _AddButtonText.Visibility = Visibility.Visible; //vær sikker på at man kan se knappen
                _CancelButton.Visibility = Visibility.Visible;//vis annuller knap
                _AddButtonText.Content = "Gem";//ændre navn på tilføj knap
                grid_faktura.Visibility = Visibility.Collapsed;//vær sikker på at den ikke er vist
            }

            HighLightSelected();
        }
        
        /// <summary>
        /// gå til næste faktura pakke
        /// </summary>
        private void _NextButton_click(object sender, RoutedEventArgs e)
        {
            List<int> pakkeId = GetPacketIndexs(_hiddenId.Text);//hent id
            PakkeControlItem pakkeItem = this.Items[pakkeId[0]]; //hent pakke data

            //hent faktura pakke data
            pakkeItem.takstId[pakkeId[1]] = Selector_Faktura_Takst.SelectId;
            pakkeItem.takstName[pakkeId[1]] = Selector_Faktura_Takst.SelectName;
            pakkeItem.beregningstypeId[pakkeId[1]] = Selector_Faktura_Beregn.SelectId;
            pakkeItem.beregningstypeName[pakkeId[1]] = Selector_Faktura_Beregn.SelectName;
            
            //tjek om pakken er færdig
            if (Selector_Faktura_Takst.SelectId != -1 && Selector_Faktura_Beregn.SelectId != -1)
                pakkeItem.IsDoneFaktura[pakkeId[1]] = true;
            else
                pakkeItem.IsDoneFaktura[pakkeId[1]] = false;

            //opdatere layout
            Update();

            int ItemCount = this.Items.Count;
            int ItemfakturaCount = this.Items[pakkeId[0]].countI;

            if (ItemfakturaCount != (pakkeId[1] +1))
            {
                SetElementPacketData(pakkeId[0], (pakkeId[1] + 1));
            }

            else if (ItemCount != (pakkeId[0] + 1))
            {
                SetElementPacketData((pakkeId[0] + 1), 0);
            }
            else
            {
                _hiddenId.Text = "-1";
                grid_faktura.Visibility = Visibility.Hidden;
            }
            HighLightSelected();
        }

        /// <summary>
        /// slet pakke fra list view
        /// </summary>
        private void _DeleteButton_click(object sender, RoutedEventArgs e)
        {
            Button thisButton = sender as Button;
            List<int> packetId = GetPacketIndexs(thisButton.Name);
            List<int> hiddenId = GetPacketIndexs(_hiddenId.Text);

            //vis man er igang med at ændre en pakke der ligger
            //efter den man har slettet så gør hidden id 1 mindre
            if (packetId[0] > hiddenId[0])
                _hiddenId.Text = "HiddenId_" + (hiddenId[0]-1) + "_" + hiddenId[1];

            //hvis man sletter den pakker man er igang med at ændre i 
            else if (packetId[0] == hiddenId[0])
                _hiddenId.Text = "-1";

            this.Items.RemoveAt(packetId[0]);//fjern pakke fra source
            Update(); //opdatere pakkere

            HighLightSelected();
        }

        /// <summary>
        /// vis annuller knap
        /// </summary>
        private void Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            _CancelButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// man skal kun kunne gå til næste pakke når 
        /// man har valgt både takst og beregningstype
        /// </summary>
        private void Selector_CheckFakturaStatus_SelectionChanged(object sender, EventArgs e)
        {
            if (Selector_Faktura_Takst.IsChecked && Selector_Faktura_Beregn.IsChecked)
                _NextButton.Visibility = Visibility.Visible;
            else
                _NextButton.Visibility = Visibility.Collapsed;

            int packetId = GetPacketOneIndex(this.LastSelectedRowID[0], this.LastSelectedRowID[1]);
            int packetMax = GetPacketMaxIndex();

            if (packetId == packetMax)
                _NextButtonText.Content = "Gem";
            else
                _NextButtonText.Content = "Gem og forsæt";

            SetPriceTextBlock(Selector_Faktura_Takst.SelectId, Selector_Faktura_Beregn.SelectId);
        }

        /// <summary>
        /// henter pakke data og indæstter det i tekst bokse
        /// </summary>
        private bool SetElementPacketData(int pakkeId, int fakturaId)
        {

            grid_faktura.Visibility = Visibility.Hidden;
            _NextButton.Visibility = Visibility.Collapsed;

            int pakkeCount = this.Items.Count;

            int packIndex = GetPacketOneIndex(pakkeId, fakturaId);
            int packIndexMax = GetPacketMaxIndex();


            _Button_Arrow_Next.Visibility = Visibility.Visible;
            _Button_Arrow_Back.Visibility = Visibility.Visible;

            if (packIndex == 0)
                _Button_Arrow_Back.Visibility = Visibility.Collapsed;

            if (packIndex == packIndexMax)
                _Button_Arrow_Next.Visibility = Visibility.Collapsed;

            //tjek om pakke findes
            if (packIndex == -1)
            {
                _hiddenId.Text = "-1";//sæt næste pakke id
                return false;
            }
            
            //hent pakke data
            PakkeControlItem pakkeItem = this.Items[pakkeId]; 
            
            //sæt fragtbrev data
            _mrkNumb.Text = pakkeItem.mrkNumb;
            _indhold.Text = pakkeItem.contains;
            _antal.Text = pakkeItem.countS;
            _weight.Text = pakkeItem.weightS;
            _artType.SelectId = pakkeItem.artId;
            _volume.Text = pakkeItem.volume;
            _transportType.SelectId = pakkeItem.transportTypeId;

            //check volume
            bool volumeStatus = true;
            if (pakkeItem.volume != "" || this.TranssportId == c_TransportGods)
            {
                int[] volumeValue;
                volumeStatus = CheckVolume(pakkeItem.volume, out volumeValue);                
            }
            //sæt/fjern rød ramme
            _volume.RequireEnableRedBorder = !volumeStatus;
            this.VolumeHasUseRedBorder = !volumeStatus;


            _hiddenId.Text = "HiddenId_" + pakkeId + "_0";//sæt næste pakke id

            //sæt faktura data
            if (IsFaktura && fakturaId != -1)
            {
                _hiddenId.Text = "HiddenId_" + pakkeId + "_" + fakturaId;//sæt næste pakke id
                _antal.Text = "1";//skal kun sige der er en
                Selector_Faktura_Takst.SelectId = pakkeItem.takstId[fakturaId];
                Selector_Faktura_Beregn.SelectId = pakkeItem.beregningstypeId[fakturaId];
            }

            if (IsFaktura && this.TranssportId == c_TransportGods)
            {
                grid_faktura.Visibility = Visibility.Visible;
                Selector_Faktura_Takst.Visibility = Visibility.Visible;
                Selector_Faktura_Beregn.Visibility = Visibility.Visible;
            }
            else if(IsFaktura && this.TranssportId == c_TransportPakke)
            {
                grid_faktura.Visibility = Visibility.Visible;
                Selector_Faktura_Takst.Visibility = Visibility.Collapsed;
                Selector_Faktura_Beregn.Visibility = Visibility.Collapsed;
            }

            SetPriceTextBlock();

            ScrollToPacket();

            return true;
        }

        private void SetPriceTextBlock(int Takst = -1, int Beregning = -1)
        {
            List<int> pakkeId = GetPacketIndexs(_hiddenId.Text);//hent id
            PakkeControlItem pakkeItem = this.Items[pakkeId[0]];


            if (!IsFaktura || this.TranssportId == c_TransportKurer ||
                pakkeId[0] == -1 || pakkeId[1] == -1 ||
                (this.TranssportId == c_TransportKurer && (pakkeItem.takstId[pakkeId[1]] == -1 || 
                pakkeItem.beregningstypeId[pakkeId[1]] == -1)))
            {
                TextBlock_PacketPrice.Text = "";
                return;
            }

            double price = 0;

            switch (this.TranssportId)
            {
                case c_TransportPakke:

                    price = Models.FakturaPrisliste.pakkePriser.Prices[pakkeItem.transportTypeId][this.TranssportId2];

                    break;
                case c_TransportGods:

                    

                    Takst = Takst == -1 ? pakkeItem.takstId[pakkeId[1]] : Takst;
                    Beregning = Beregning == -1 ? pakkeItem.beregningstypeId[pakkeId[1]] : Beregning;

                    if (Takst == -1 || Beregning == -1)
                    {
                        TextBlock_PacketPrice.Text = "";
                        return;
                    }

                    var godsFunc = new Class.GodsFunction();

                    double kiloCal = godsFunc.GetCalKilo(Beregning, pakkeItem.volumeL, pakkeItem.volumeB, pakkeItem.volumeH);
                    double kiloUse = kiloCal >= pakkeItem.weightD ? kiloCal : pakkeItem.weightD;


                    price = godsFunc.GetPrice(Takst, kiloUse);
                    break;
            }

            if (price != 0)
                TextBlock_PacketPrice.Text = price.ToString("N2") + " kr.";
        }

        /// <summary>
        /// tjek om det er en tom pakke
        /// </summary>
        private bool PacketIsEmpty(PakkeControlItem packetItem)
        {
            PakkeControlItem checkNewElement = new PakkeControlItem();
            

            if (
                packetItem.mrkNumb == checkNewElement.mrkNumb &&
                packetItem.contains == checkNewElement.contains &&
                packetItem.countI == checkNewElement.countI &&
                packetItem.weightD == checkNewElement.weightD &&
                packetItem.artId == checkNewElement.artId &&
                packetItem.volume == checkNewElement.volume &&
                packetItem.transportTypeId == checkNewElement.transportTypeId &&
                packetItem.takstId == checkNewElement.takstId &&
                packetItem.beregningstypeId == checkNewElement.beregningstypeId
                )
                return true;
            else
                return false;            
        }

        /// <summary>
        /// hent to id`er udfra string
        /// </summary>
        /// <param name="source">Button_10_20</param>
        /// <returns>10 og 20</returns>
        private List<int> GetPacketIndexs(string source)
        {
            bool underScoreIsFound = false;
            string numbers = "0123456789";
            string indexNumber = "";
            List<int> values = new List<int>();
            int loopRun = source.Length;
            

            for (int i = 0; i < loopRun; i++)
            {
                string letter = source.Substring(i, 1);


                if (letter == "_")
                    underScoreIsFound = true;
                else if(underScoreIsFound && numbers.Contains(letter))
                    indexNumber += letter;
                else
                    underScoreIsFound = false;
                
                //tjek om der er fundet index
                if (indexNumber != "" && (!underScoreIsFound || letter == "_" || i == loopRun - 1))
                {
                    values.Add(int.Parse(indexNumber));
                    indexNumber = "";
                }
            }

            //skal være 2 værdier
            while (values.Count < 2)
                values.Add(-1);


            //updater valgte
            if (values[0] != -1)
                this.LastSelectedRowID = values;


            return values;
        }
        
        /// <summary>
        /// nulstil faktura pakke delen
        /// </summary>
        private void ResetFakturaPackets()
        {
            for (int i = 0; i < this.Items.Count; i++)
            {
                this.Items[i].IsDoneFaktura = new List<bool>();
                this.Items[i].takstId = new List<int>();
                this.Items[i].takstName = new List<string>();
                this.Items[i].beregningstypeId = new List<int>();
                this.Items[i].beregningstypeName = new List<string>();

                for (int a = 0; a < this.Items[i].countI; a++)
                {
                    this.Items[i].IsDoneFaktura.Add(false);
                    this.Items[i].takstId.Add(-1);
                    this.Items[i].takstName.Add("");
                    this.Items[i].beregningstypeId.Add(-1);
                    this.Items[i].beregningstypeName.Add("");
                }
            }
        }

        /// <summary>
        /// viser hvad for en pakke man ændre på
        /// </summary>
        private void HighLightSelected()
        {
            var funcClass = new Class.Functions.others();
            List<int> pakkeId = GetPacketIndexs(_hiddenId.Text);
           
            //find pakke grid index i listen
            int index = GetPacketOneIndex(pakkeId[0], pakkeId[1]);

            //sæt bagground farve på valgte
            for (int i = 0; i < _ItemsContains.Children.Count; i++)
            {
                Grid item = (_ItemsContains.Children[i] as Border).Child as Grid;
                
                if (i == index)
                    item.Background = funcClass.ColorBrushHex("#FF36A3FD");
                else
                    item.Background = Brushes.Transparent;
            }
        }

        private void LockFragtbrevStatus(bool IsLock)
        {
            _mrkNumb.IsUnLock = !IsLock;
            _indhold.IsUnLock = !IsLock;
            _antal.IsUnLock = !IsLock;
            _weight.IsUnLock = !IsLock;
            _volume.IsUnLock = !IsLock;
            _artType.IsEnabled = !IsLock;
            _transportType.IsEnabled = !IsLock;


            if (IsLock)
                _AddButton.Visibility = Visibility.Collapsed;
            else
                _AddButton.Visibility = Visibility.Visible;
        }

        private void Button_ChangeEditArrow_Click(object sender, RoutedEventArgs e)
        {
            const int Button_Back = 0;
            const int Button_Next = 1;

            int buttonType;

            //finder ud af om man skal gå frem eller tilbage mellem pakkerne
            if ((sender as Button).HorizontalAlignment == HorizontalAlignment.Left)
                buttonType = Button_Back;
            else
                buttonType = Button_Next;


            //tjek om man skal bruge hidden id hvis man er igang med at ændre i en pakken
            //eller om den bare skal bruge den site valgte
            GetPacketIndexs(_hiddenId.Text);
            
            
            int nextVal_0 = this.LastSelectedRowID[0];
            int nextVal_1 = this.LastSelectedRowID[1];

            switch (buttonType)
            {
                case Button_Back:

                    if (nextVal_1 > 0)
                    {
                        nextVal_1--;
                    }
                    else
                    {
                        nextVal_0--;
                        nextVal_1 = this.Items[nextVal_0].countI - 1;
                    }
                    break;

                case Button_Next:
                    
                    if (nextVal_1 < this.Items[nextVal_0].countI - 1 && this.IsFaktura)
                    {
                        nextVal_1++;
                    }
                    else
                    {
                        nextVal_0++;
                        nextVal_1 = 0;
                    }

                    break;
            }

            SetElementPacketData(nextVal_0, nextVal_1);
            HighLightSelected();
        }

        private int GetPacketOneIndex(int pakkeId, int fakturaId)
        {
            int index = -1;

            //find pakkerende grid index i listen
            if (IsFaktura && pakkeId >= 0)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {

                    if (pakkeId == i)
                        break;
                    else
                        index += this.Items[i].countI;
                }
                index += fakturaId;
                index++;

            }
            else
            {
                index = pakkeId;
            }

            return index;
        }

        private int GetPacketMaxIndex()
        {
            int index = 0;

            //find pakkerende grid index i listen
            if (IsFaktura)
            {
                for (int i = 0; i < this.Items.Count; i++)
                {
                    index += this.Items[i].countI;
                }
            }
            else
            {
                index = this.Items.Count;
            }

            //fjern 1 da 0 tæller med i loop
            index--;

            return index;
        }

        private void ScrollToPacket()
        {
            double scrollPostion = _ScrollViewer_Packet.VerticalOffset;
            double scrollHeight = 38.5;
            int packIndex = GetPacketOneIndex(this.LastSelectedRowID[0], this.LastSelectedRowID[1]);
            double scrollLinePostion = Math.Ceiling(scrollPostion / scrollHeight);
            double scrollNext = 0;
            
            scrollNext = scrollHeight * (packIndex -1) ;
            _ScrollViewer_Packet.ScrollToVerticalOffset(scrollNext);
        }

        private bool CheckVolume(string volume, out int[] singleVolumn)
        {
            int[] values = { 0, 0, 0 };
            singleVolumn = values;

            int index = 0;
            string oneValue = "";
            for (int i = 0; i < volume.Length; i++)
            {
                string oneLetter = volume.Substring(i, 1).ToLower();

                if (oneLetter == "x" || oneLetter == "*" || i == volume.Length -1)
                {
                    //tilføj sidste tal
                    if(i == volume.Length - 1)
                        oneValue += oneLetter;

                    //ikke et tal
                    if (!int.TryParse(oneValue, out values[index]))
                        return false;

                    //gør klar til næste tal
                    oneValue = "";
                    index++;
                }
                else
                {
                    oneValue += oneLetter;
                }
            }

            //hvis den ikke har tre værdiger
            if (index != 3)
                return false;

            //sæt nye værdier
            singleVolumn = values;
            return true;
        }

        private void _volume_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.VolumeHasUseRedBorder)
            {
                int[] nullValue;
                _volume.RequireEnableRedBorder = !CheckVolume((sender as TextBox).Text, out nullValue);
            }
        }

        private void _PakkeControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_PakkeControl.Visibility != Visibility.Visible)
                return;

            _CancelButton_click(_CancelButton, null);
        }
    }

    public class PakkeControlItem
    {
        public bool IsDoneFragt = false;
        public List<bool> IsDoneFaktura;

        public string mrkNumb = "";
        public string contains = "";
        public int countI = 0;
        public string countS = "";
        public double weightD = 0;
        public string weightS = "";
        public int artId = -1;
        public string artName = "";
        public string volume = "";
        public int volumeL = -1;
        public int volumeB = -1;
        public int volumeH = -1;
        public int transportTypeId = -1;
        public string transportTypeName = "";


        public List<int> takstId;
        public List<string> takstName;
        public List<int> beregningstypeId;
        public List<string> beregningstypeName;


    }
}
