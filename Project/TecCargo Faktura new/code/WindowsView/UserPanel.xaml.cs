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
using System.Windows.Shapes;
using TecCargo_Faktura.Controls;
using System.Windows.Threading;

namespace TecCargo_Faktura.WindowsView
{
    /// <summary>
    /// Interaction logic for UserPanel.xaml
    /// </summary>
    public partial class UserPanel : Window
    {
        private Class.XML_Files.DocData.Layout.DocumentData FileDocument = new Class.XML_Files.DocData.Layout.DocumentData();

        private List<string> imagePostionPng = new List<string>();
        private List<bool> gridPostionStatus = new List<bool>();

        private bool isSaved = true;
        private int viewId = 0;
        private int documentStatus = document_FragtInfo;
        private Dictionary<int, bool> viewHasBeenChecked = new Dictionary<int, bool>()
        {
            {View_Contact, false },
            {View_GenereltFragt, false },
            {View_TransportFragt, false },
            {View_Notifications, false },
            {View_CloseFragt, false },
            {View_GenereltFaktura, false },
            {View_TransportFaktura, false },
            {View_Gebyr, false }
        };




        private byte[] image_Color_Selected = { 255, 255, 255 };
        private byte[] image_Color_NotDone = { 255, 0, 0 };
        private byte[] image_Color_Done = { 0, 255, 0 };
        private byte[] image_Color_NoAccess = { 65, 65, 65 };
        private byte[] image_Color_Info = { 9, 89, 168 };
       

        private DispatcherTimer SaveTimer;
        

        //gør det lettere at læse
        const int View_Contact = 0;
        const int View_GenereltFragt = 1;
        const int View_TransportFragt = 2;
        const int View_Notifications = 3;
        const int View_StatusFragt = 4;
        const int View_PDFFragt = 5;
        const int View_CloseFragt = 6;
        const int View_GenereltFaktura = 7;
        const int View_TransportFaktura = 8;
        const int View_Gebyr = 9;
        const int View_StatusFaktura = 10;
        const int View_CloseFaktura = 11;
        const int View_PDFFaktura = 12;

        const int c_Transport_Kurer = 0;
        const int c_Transport_Pakke = 1;
        const int c_Transport_Gods = 2;

        const int document_FragtInfo = 0;
        const int document_FragtClose = 1;
        const int document_FakturaInfo = 2;
        const int document_FakturaClose = 3;

        public UserPanel()
        {
            InitializeComponent();
            
            ViewSelectorCreate();
            UpdateImagePositionColorAll(); //opdatere all postion images
            UpdatePriceList();
            PakkeControl_Transport.ValueChange += PakkeControl_Transport_ValueChange;
            Gebyr_Ekstra.ValueChange += PakkeControl_Transport_ValueChange;
            //MainFragtBrev part2 = new MainFragtBrev("ss");
            //part2.Show();

            MyDocumentViewer_PDFViewer.ReloadClick += MyDocumentViewer_CreatePDF_ReloadClick;
            SetSaveTimer(5);
            //docviewer.Document
            SetElementChangeEventAll();

        }

        #region Auto Save og Change Event
        private void SetSaveTimer(int minutes)
        {
            SaveTimer = new DispatcherTimer();
            SaveTimer.Interval = TimeSpan.FromMinutes(minutes);

            SaveTimer.Tick += SaveTimer_Tick;
            SaveTimer.Start();
        }

        private void SaveTimer_Tick(object sender, EventArgs e)
        {
            if (!this.isSaved)
                SaveDoc();
        }


        /// <summary>
        /// Ændre save billede icon
        /// </summary>
        private void UpdateSaveStatus(bool status)
        {
            string imageUrl;

            if (status)
                imageUrl = "/Images/Icons/save_checkv2.png";
            else
                imageUrl = "/Images/Icons/save_uncheck.png";

            BitmapImage bitImage = new BitmapImage();
            bitImage.BeginInit();
            bitImage.UriSource = new Uri(imageUrl, UriKind.Relative);
            bitImage.EndInit();

            (Button_Save.Content as Image).Stretch = Stretch.Fill;
            (Button_Save.Content as Image).Source = bitImage;
            this.isSaved = status;
        }

        private void SetElementChangeEventAll()
        {

            foreach (Grid item in Grid_Viewbox.Children)
            {
                foreach (Address itemChild in FindVisualChildren<Address>(item))
                {
                    itemChild.Textbox_ContactTlf.TextChanged += ElementChange_Changed;
                    itemChild.Textbox_FirmName.TextChanged += ElementChange_Changed;
                    itemChild.Textbox_ContactPer.TextChanged += ElementChange_Changed;
                    itemChild.Textbox_Address.TextChanged += ElementChange_Changed;
                    itemChild.Textbox_ZipCode.TextChanged += ElementChange_Changed;
                    itemChild.CheckBox_Pay.Changed += ElementChange_Changed;
                }

                foreach (MyTextbox itemChild in FindVisualChildren<MyTextbox>(item))
                {
                    itemChild.TextChanged += ElementChange_Changed;
                }
                foreach (MyTextBlock itemChild in FindVisualChildren<MyTextBlock>(item))
                {
                    itemChild.Input.TextChanged += ElementChange_Changed;
                }
                foreach (Selector itemChild in FindVisualChildren<Selector>(item))
                {
                    itemChild.SelectionChanged += ElementChange_Changed;
                }

                foreach (PakkeControl itemChild in FindVisualChildren<PakkeControl>(item))
                {
                    itemChild.ValueChange += ElementChange_Changed;
                }

                foreach (DateTimePicker itemChild in FindVisualChildren<DateTimePicker>(item))
                {
                    itemChild.Changed += ElementChange_Changed;
                }
            }
        }

        /// <summary>
        /// Gør det muligt at hente flere element af samme slag
        /// </summary>
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        private void ElementChange_Changed(object sender, EventArgs e)
        {
            if (!this.IsLoaded)
                return;

            UpdateSaveStatus(false);
        }


        #endregion
        
        #region ViewSelector skift og iconer
        /// <summary>
        /// Denne function bliver kaldt i ViewSelectorCreate()
        /// og laver inddeling mellerum fragtbrev og faktura
        /// </summary>
        private void SetPostionForViewSelectorLines()
        {
            //billede knapper
            int fragtbrevViews = 8;
            int fakturaViews = 7;

            //pile i mellmeum
            int fragtbrevArrows = 6;
            int fakturaArrows = 5;

            int imageViewsSize = 46;
            int imageArrowsSize = 23;
            int removeExtraSpace = 5;

            double fragtbrevFullLengt = (fragtbrevViews * imageViewsSize) + (fragtbrevArrows * imageArrowsSize);
            double fragtLineWidth = fragtbrevFullLengt - TextBlock_ViewSelector_Fragt.ActualHeight;
            fragtLineWidth = fragtLineWidth / 2;
            fragtLineWidth = fragtLineWidth - imageArrowsSize - imageViewsSize;
            fragtLineWidth = fragtLineWidth - removeExtraSpace;


            double fakturaFullLengt = (fakturaViews * imageViewsSize) + (fakturaArrows * imageArrowsSize);
            double fakturaLineWidth = fakturaFullLengt - TextBlock_ViewSelector_Faktura.ActualHeight;
            fakturaLineWidth = fakturaLineWidth / 2;
            fakturaLineWidth = fakturaLineWidth - imageArrowsSize - imageViewsSize;
            //fakturaLineWidth = fakturaLineWidth - removeExtraSpace;

            Line_ViewSelector_FragtFirstH.Width = fragtLineWidth;
            Line_ViewSelector_FragtLastH.Width = fragtLineWidth;
            Line_ViewSelector_FakturaFirstH.Width = fakturaLineWidth;
            Line_ViewSelector_FakturaLastH.Width = fakturaLineWidth;
        }

        /// <summary>
        /// Denne function opertter ViewSelector menu i toppen
        /// </summary>
        private void ViewSelectorCreate()
        {
            string selectorArrowImagePath = "FragtBrev-Status-arrow.png";
            //Button_Image_Position_Click //Image_Position_8
            string[] selectorImagesPath = {
                "FragtBrev-Status-Contact.png",
                "FragtBrev-Status-Generalt.png",
                "FragtBrev-Status-Truck.png",
                "FragtBrev-Status-Notification.png",
                "FragtBrev-Status-Check.png",
                "FragtBrev-Status-Pdf.png",
                "FragtBrev-Status-Close.png",
                "FragtBrev-Status-Generalt.png",
                "FragtBrev-Status-Truck.png",
                "FragtBrev-Status-Gebyr.png",
                "FragtBrev-Status-Check.png",
                "FragtBrev-Status-Close.png",
                "FragtBrev-Status-Pdf.png",
            };

            int separated = 6;

            Binding size = new Binding("Height");
            size.ElementName = "Border_ViewSelector";



            int columnIdStart = 0;
            for (int i = 0; i < selectorImagesPath.Length; i++)
            {

                Grid_ViewSelectorContainer.ColumnDefinitions.Add(new ColumnDefinition());
                Grid_ViewSelectorContainer.ColumnDefinitions[columnIdStart].Width = GridLength.Auto;

                //opret knap
                Button selectButton = new Button();
                selectButton.Click += Button_ViewSelectorImagePosition_Click;
                selectButton.Style = FindResource(ToolBar.ButtonStyleKey) as Style;
                selectButton.SetBinding(HeightProperty, size);
                selectButton.SetBinding(WidthProperty, size);

                //opret select billede
                Image selectImage = new Image();
                selectImage.Name = "Image_Position_" + i;
                selectImage.Source = new BitmapImage(new Uri("/Images/Icons/" + selectorImagesPath[i], UriKind.Relative));

                //gør så man vil kunne ændre på billede farve
                this.RegisterName(selectImage.Name, selectImage);

                //tilføj billed til knap
                selectButton.Content = selectImage;

                //tilføj knap
                Grid.SetColumn(selectButton, columnIdStart);
                columnIdStart++;
                Grid_ViewSelectorContainer.Children.Add(selectButton);




                if (i == separated)
                {
                    Binding colorBinding = new Binding("Background");
                    colorBinding.ElementName = "Border_ViewSelectorBox";

                    Canvas separatedFiller = new Canvas();
                    separatedFiller.HorizontalAlignment = HorizontalAlignment.Center;

                    //filler.Width = 23;
                    Rectangle separatedFillerBack = new Rectangle();
                    separatedFillerBack.SetBinding(Rectangle.FillProperty, colorBinding);
                    separatedFillerBack.Height = 42;
                    separatedFillerBack.Width = 4;

                    separatedFiller.Children.Add(separatedFillerBack);

                    Grid_ViewSelectorContainer.ColumnDefinitions.Add(new ColumnDefinition());
                    Grid_ViewSelectorContainer.ColumnDefinitions[columnIdStart].Width = new GridLength(23);
                    Grid.SetColumn(separatedFiller, columnIdStart);
                    columnIdStart++;
                    Grid_ViewSelectorContainer.Children.Add(separatedFiller);

                }
                //hvis det ikke er den sidste billede skal der også være plads til en pil
                else if (i != selectorImagesPath.Length - 1)
                {
                    Image arrowImage = new Image();
                    arrowImage.Source = new BitmapImage(new Uri("/Images/Icons/" + selectorArrowImagePath, UriKind.Relative));
                    //arrowImage.SetBinding(HeightProperty, size);
                    //arrowImage.SetBinding(WidthProperty, size);
                    arrowImage.Height = 23;
                    arrowImage.Width = 23;
                    Grid_ViewSelectorContainer.ColumnDefinitions.Add(new ColumnDefinition());
                    Grid_ViewSelectorContainer.ColumnDefinitions[columnIdStart].Width = GridLength.Auto;
                    Grid.SetColumn(arrowImage, columnIdStart);
                    columnIdStart++;
                    Grid_ViewSelectorContainer.Children.Add(arrowImage);
                }


                this.imagePostionPng.Add("/Images/Icons/" + selectorImagesPath[i]); //sæt default
                gridPostionStatus.Add(false); //sæt default
            }

            //int lastColumnId = columnIdStart;
            //Grid.SetColumn(Button_ViewSelectorNext, lastColumnId);
            SetPostionForViewSelectorLines();
        }
        
        /// <summary>
        /// Når man klikker på et billede i ViewSelector
        /// skal den vise den valgte view
        /// </summary>
        private void Button_ViewSelectorImagePosition_Click(object sender, RoutedEventArgs e)
        {
            if (this.viewId < 0)
                this.viewId = 0;
            else if (this.viewId >= this.gridPostionStatus.Count)
                this.viewId = this.gridPostionStatus.Count - 1;

            ScrollViewBox.Focus();//fjern focus fra knap
            this.gridPostionStatus[this.viewId] = GetViewStatus(this.viewId); //opdatere status for sidste vindue

            //hent det nye vindue Id
            Image buttonImage = (sender as Button).Content as Image;
            this.viewId = int.Parse(buttonImage.Name.Replace("Image_Position_", ""));

            if (this.UseFullView)
                ShowViewInFullMode();
            else
             SetUpGridView(this.viewId); //vis vindue
        }
        
        /// <summary>
        /// skift imellem vinduer/kategorier
        /// </summary>
        private void Button_ViewSelectorChangeView_Click(object sender, RoutedEventArgs e)
        {
            if (this.viewId < 0)
                this.viewId = 0;
            else if (this.viewId >= this.gridPostionStatus.Count)
                this.viewId = this.gridPostionStatus.Count -1;

            const string ButtonBack = "Button_ViewSelectorBack";
            const string ButtonNext = "Button_ViewSelectorNext";
            const string ButtonGridBack = "Button_GridViewBack";
            const string ButtonGridNext = "Button_GridViewNext";

            Button senderB = (sender as Button);
            string buttonId = senderB.Name;

            int lastView = this.viewId;
            this.gridPostionStatus[this.viewId] = GetViewStatus(this.viewId);


            if(this.UseFullView)
            switch (lastView)
            {
                case View_Contact:
                case View_GenereltFragt:
                case View_TransportFragt:
                case View_Notifications:
                        switch (buttonId)
                        {
                            case ButtonBack:
                            case ButtonGridBack:
                                this.viewId--;
                                break;

                            case ButtonNext:
                            case ButtonGridNext:
                                this.viewId = View_StatusFragt;
                                break;
                        }                        
                    break;

                case View_StatusFragt:
                case View_CloseFragt:
                case View_PDFFragt:
                case View_StatusFaktura:
                case View_CloseFaktura:
                case View_PDFFaktura:
                        switch (buttonId)
                        {
                            case ButtonBack:
                            case ButtonGridBack:
                                this.viewId--;
                                break;

                            case ButtonNext:
                            case ButtonGridNext:
                                this.viewId++;
                                break;
                        }

                    break;

                case View_GenereltFaktura:
                case View_TransportFaktura:
                case View_Gebyr:
                        switch (buttonId)
                        {
                            case ButtonGridBack:
                            case ButtonBack:
                                this.viewId--;
                                break;

                            case ButtonNext:
                            case ButtonGridNext:
                                this.viewId = View_StatusFaktura;
                                break;
                        }
                        break;
            }
            else
            switch (buttonId)
                {
                    case ButtonGridBack:
                    case ButtonBack:
                    this.viewId--;
                    break;

                case ButtonNext:
                case ButtonGridNext:
                    this.viewId++;
                    break;
            }

            if (this.UseFullView)
                ShowViewInFullMode();
            else
                SetUpGridView(this.viewId);
        }

        /// <summary>
        /// opdatere alle postion billeder farver
        /// </summary>
        private void UpdateImagePositionColorAll()
        {
            for (int i = 0; i < this.imagePostionPng.Count; i++)
            {
                UpdateImagePositionColor(i); //opdatere billede farve
            }
        }

        /// <summary>
        /// sæt en farve på billede
        /// </summary>
        private void UpdateImagePositionColor(int imageId)
        {
            //hent billede element
            Image thisImage = FindName("Image_Position_" + imageId) as Image;
            byte[] colorId = image_Color_Selected;
            
            

            //tjek hvad farve det skal være
            switch (imageId)
            {
                case View_Contact:
                case View_GenereltFragt:
                case View_TransportFragt:
                case View_Notifications:
                case View_CloseFragt:

                    if (imageId == this.viewId)
                        colorId = image_Color_Selected;
                    else if (imageId == View_CloseFragt && this.documentStatus < document_FragtClose)
                        colorId = image_Color_NoAccess;
                    else if (this.gridPostionStatus[imageId])
                        colorId = image_Color_Done;
                    else
                        colorId = image_Color_NotDone;

                    
                    break;

                case View_StatusFragt:
                case View_PDFFragt:
                case View_StatusFaktura:
                case View_PDFFaktura:
                    if (imageId == this.viewId)
                        colorId = image_Color_Selected;
                    else if (
                        (imageId == View_PDFFragt && this.documentStatus < document_FragtClose) ||
                        (imageId == View_StatusFaktura && this.documentStatus < document_FakturaInfo) ||
                        (imageId == View_PDFFaktura && this.documentStatus < document_FakturaClose)
                    )
                        colorId = image_Color_NoAccess;
                    else
                        colorId = image_Color_Info;
                    
                    break;


                case View_GenereltFaktura:
                case View_TransportFaktura:
                case View_Gebyr:
                case View_CloseFaktura:
                    if (this.documentStatus >= document_FakturaInfo)
                    {
                        if (imageId == this.viewId)
                            colorId = image_Color_Selected;
                        else if (imageId == View_CloseFaktura && this.documentStatus < document_FakturaClose)
                            colorId = image_Color_NoAccess;
                        else if (this.gridPostionStatus[imageId])
                            colorId = image_Color_Done;
                        else
                            colorId = image_Color_NotDone;
                    }
                    else {
                        colorId = image_Color_NoAccess;
                    }
                    break;
            }

            //opdatere billede
            ChangeImageColor(thisImage, this.imagePostionPng[imageId], colorId);
        }
        
        /// <summary>
        /// visser et grid vindue 
        /// med indstillinger
        /// </summary>
        private void SetUpGridView(int gridViewId, bool hideOthers = true)
        {
            //gør at man skal have adgang til view for det den
            if ((documentStatus == document_FragtInfo && !(gridViewId >= View_Contact && gridViewId <= View_StatusFragt)) ||
                (documentStatus == document_FragtClose && !(gridViewId >= View_Contact && gridViewId <= View_PDFFragt)) ||
                (documentStatus == document_FakturaInfo && !(gridViewId >= View_Contact && gridViewId <= View_StatusFaktura)) ||
                (documentStatus == document_FakturaClose && !(gridViewId >= View_Contact && gridViewId <= View_PDFFaktura)))
                return;

            //sæt hak ved at man har kigget på siden
            if (this.viewHasBeenChecked.ContainsKey(gridViewId))
                this.viewHasBeenChecked[gridViewId] = true;


            //grid/vindue Id kan ændres sig hvis
            //der er to der bruge de samme ting
            int showGridId = gridViewId;

            //sæt indstillinger for vinduer
            switch (gridViewId)
            {
                case View_Contact:
                    break;
                case View_GenereltFragt:
                    break;
                case View_TransportFragt:
                    PakkeControl_Transport.IsFaktura = false;
                    PakkeControl_Transport.Update();
                    break;
                case View_Notifications:
                    break;
                case View_StatusFragt:
                    break;
                case View_CloseFragt:
                    break;
                case View_GenereltFaktura:
                    Address_Faktura_Update();
                    break;

                case View_TransportFaktura:
                    showGridId = View_TransportFragt;//vis pakkecontrol som også bliver brugt i fragtbrev
                    PakkeControl_Transport.IsFaktura = true;
                    PakkeControl_Transport.Update();
                    break;
                case View_StatusFaktura:
                    showGridId = View_StatusFragt;//vis status som også bliver brugt i fragtbrev
                    break;
                case View_PDFFaktura:
                    showGridId = View_PDFFragt;//vis pdf som også bliver brugt i fragtbrev
                    break;
            }

            ShowViewElement(showGridId, hideOthers);
            UpdateImageButtomStatus();
            UpdateChangeViewButtonVisibility();
        }

        /// <summary>
        /// tjek valgte vindue status
        /// </summary>
        private bool GetViewStatus(int id)
        {

            //hvad grid status function der skal køre
            switch (id)
            {
                case View_Contact:
                    return Grid_ContactInfomation_Check();
                case View_GenereltFragt:
                    return Grid_GenereltInfomation_Check();
                case View_TransportFragt:
                    return Grid_TransportInfomation_Check();
                case View_TransportFaktura:
                    return true;
                case View_Gebyr:
                    return true;

                case View_Notifications:
                case View_CloseFragt:
                    return this.viewHasBeenChecked[id];
                default:
                    return false;
            }
        }

        private void UpdateViewStatusAll()
        {
            for (int i = 0; i < this.imagePostionPng.Count; i++)
            {
                gridPostionStatus[i] = GetViewStatus(i);
                UpdateImagePositionColor(i); //opdatere billede farve
            }
        }
        private void UpdateDocumentAccess()
        {
            this.documentStatus = document_FragtInfo;

            for (int i = 0; i < this.imagePostionPng.Count; i++)
            {
                //stop loop må ikke kunne gå vidre
                if (!gridPostionStatus[i])
                    break;


                switch (i)
                {
                    case View_CloseFragt:
                        this.documentStatus = document_FragtClose;
                        break;
                    case View_GenereltFragt:
                        this.documentStatus = document_FakturaInfo;
                        break;
                    case View_CloseFaktura:
                        this.documentStatus = document_FakturaClose;
                        break;
                }                
            }


            //åben for alt
            UnLockFragtBrev(true);

            //luk for færdige views
            switch (this.documentStatus)
            {
                case document_FragtClose:
                    LockFragtBrev(false);
                    break;
                case document_FakturaInfo:
                    LockFragtBrev(true);
                    break;
                case document_FakturaClose:
                    break;
            }
        }

        /// <summary>
        /// viser valgte vindue og 
        /// skjuler de andre
        /// </summary>
        private void ShowViewElement(int viewId, bool hideOthers = true)
        {
            int viewCount = Grid_Viewbox.Children.Count; //antal view der er

            //skjul element og vis valgte element
            for (int i = 0; i < viewCount -1; i++)
            {
                Grid thisElement = (Grid_Viewbox.Children[i] as Grid);

                if (viewId == i)
                    thisElement.Visibility = Visibility.Visible;
                else if (hideOthers)
                    thisElement.Visibility = Visibility.Collapsed;
            }
            UpdateImagePositionColorAll(); //opdatere status for postion billeder
        }


        #endregion

        #region View Check Status Func

        private bool Grid_ContactInfomation_Check()
        {
            object[] contactElements = { Address_Afsender, Address_Modtager, Address_Modtager2 };
            bool payerIsFound = false;

            foreach (Address element in contactElements)
            {
                if (element.HeaderIsCheckButton && !element.Checked)
                    continue;

                if (element.Textbox_ContactTlf.Text.Length == 0)
                    return false;
                if (element.Textbox_FirmName.Text.Length == 0)
                    return false;
                if (element.Textbox_ContactPer.Text.Length == 0)
                    return false;
                if (element.Textbox_Address.Text.Length == 0)
                    return false;
                if (element.Textbox_ZipCode.Text.Length == 0)
                    return false;

                if (!payerIsFound && element.CheckBox_Pay.Checked)
                    payerIsFound = true;
            }

            return payerIsFound;
        }
        private bool Grid_GenereltInfomation_Check()
        {

            if (MyTextbox_Generelt_Reference.Text.Length == 0)
                return false;
            if (!DateTimePicker_Generelt_Rute_1.HasBeenSet)
                return false;
            if (MyTextbox_Generelt_Rute_1.Text.Length == 0)
                return false;
            if (Selector_Generelt_Forsikringstype.SelectId == -1)
                return false;

            return true;
        }
        private bool Grid_TransportInfomation_Check()
        {
            //gør det lettere at læse
            const int transportKurer = 0;
            //const int transportPakke = 1;
            //const int transportGods = 2;

            if (Selector_Transport1.SelectId == -1)
                return false;
            if (Selector_Transport2.SelectId == -1)
                return false;
            if (Selector_Transport1.SelectId == transportKurer && Selector_Transport3.SelectId == -1)
                return false;

            foreach (var item in PakkeControl_Transport.Items)
            {
                if (!item.IsDoneFragt)
                    return false;
            }

            return true;
        }

        
        

        #endregion


        private bool testdsgf = false;
        private void ButtonAdmin_Startmenu_Click(object sender, RoutedEventArgs e)
        {
            SaveDoc();
            var funcPDF = new Class.MakePDF();
            funcPDF.CreateFragtBrev(this.FileDocument);
            //funcPDF.CreateFaktura(this.FileDocument);
        }

        

        #region Document - indlæs, gem og lås

        /// <summary>
        /// Lås fragtbrev elementer 
        /// så man ikke kan ændre i dem
        /// </summary>
        /// <param name="FullLock">Om afslut delen også skal være med</param>
        private void LockFragtBrev(bool FullLock = false)
        {
            //Lås Adresse
            Address_Afsender.IsUnLock = false;
            Address_Modtager.IsUnLock = false;
            Address_Modtager2.IsUnLock = false;

            //Lås Generelt
            MyTextbox_Generelt_Reference.IsUnLock = false;
            MyTextbox_Generelt_Fragtmand.IsUnLock = false;

            DateTimePicker_Generelt_Rute_1.IsUnLock = false;
            DateTimePicker_Generelt_Rute_2.IsUnLock = false;
            MyTextbox_Generelt_Rute_1.IsUnLock = false;
            MyTextbox_Generelt_Rute_2.IsUnLock = false;

            Selector_Generelt_Forsikringstype.IsEnabled = false;

            //Lås Byttepaller
            CheckBoxButton_Byttepaller.IsEnabled = false;
            MyTextbox_Byttepaller_1.IsUnLock = false;
            MyTextbox_Byttepaller_2.IsUnLock = false;
            MyTextbox_Byttepaller_3.IsUnLock = false;

            //Lås Efterkrav
            CheckBoxButton_Efterkrav.IsEnabled = false;
            MyTextbox_Efterkrav_1.IsUnLock = false;
            MyTextbox_Efterkrav_2.IsUnLock = false;
            MyTextbox_Efterkrav_3.IsUnLock = false;
            MyTextbox_Efterkrav_4.IsUnLock = false;

            //Lås Bemærkninger
            MyTextBlock_Notifications.IsUnLock = false;

            //Lås Transport
            Selector_Transport1.IsEnabled = false;
            Selector_Transport2.IsEnabled = false;
            Selector_Transport3.IsEnabled = false;
            PakkeControl_Transport.IsUnLock = false;

            //Lås Afslut fragtbrev
            if (FullLock)
            {
                //Lås Generelt
                DateTimePicker_Afslut_Leveringsdato.IsEnabled = false;
                MyTextbox_Afslut_Rabat.IsEnabled = false;
                MyTextbox_Afslut_Kilometer.IsEnabled = false;

                //Lås Tidsforbrug
                MyTextbox_Afslut_Tidsforbrug_1.IsEnabled = false;
                MyTextbox_Afslut_Tidsforbrug_2.IsEnabled = false;
                MyTextbox_Afslut_Tidsforbrug_3.IsEnabled = false;
                MyTextbox_Afslut_Tidsforbrug_4.IsEnabled = false;

                //Lås Kommentar
                MyTextBlock_Afslut_Kommentar.IsEnabled = false;
            }
        }


        /// <summary>
        /// åben fragtbrev elementer 
        /// så man kan ændre i dem
        /// </summary>
        /// <param name="FullUnLock">Om afslut delen også skal være med</param>
        private void UnLockFragtBrev(bool FullUnLock = false)
        {
            //åben Adresse
            Address_Afsender.IsUnLock = true;
            Address_Modtager.IsUnLock = true;
            Address_Modtager2.IsUnLock = true;

            //åben Generelt
            MyTextbox_Generelt_Reference.IsUnLock = true;
            MyTextbox_Generelt_Fragtmand.IsUnLock = true;

            DateTimePicker_Generelt_Rute_1.IsUnLock = true;
            DateTimePicker_Generelt_Rute_2.IsUnLock = true;
            MyTextbox_Generelt_Rute_1.IsUnLock = true;
            MyTextbox_Generelt_Rute_2.IsUnLock = true;

            Selector_Generelt_Forsikringstype.IsEnabled = true;

            //åben Byttepaller
            CheckBoxButton_Byttepaller.IsEnabled = true;
            MyTextbox_Byttepaller_1.IsUnLock = true;
            MyTextbox_Byttepaller_2.IsUnLock = true;
            MyTextbox_Byttepaller_3.IsUnLock = true;

            //åben Efterkrav
            CheckBoxButton_Efterkrav.IsEnabled = true;
            MyTextbox_Efterkrav_1.IsUnLock = true;
            MyTextbox_Efterkrav_2.IsUnLock = true;
            MyTextbox_Efterkrav_3.IsUnLock = true;
            MyTextbox_Efterkrav_4.IsUnLock = true;

            //åben Bemærkninger
            MyTextBlock_Notifications.IsUnLock = true;

            //åben Transport
            Selector_Transport1.IsEnabled = true;
            Selector_Transport2.IsEnabled = true;
            Selector_Transport3.IsEnabled = true;
            PakkeControl_Transport.IsUnLock = true;
            
            //åben Afslut fragtbrev
            if (FullUnLock)
            {
                //åben Generelt
                DateTimePicker_Afslut_Leveringsdato.IsEnabled = true;
                MyTextbox_Afslut_Rabat.IsEnabled = true;
                MyTextbox_Afslut_Kilometer.IsEnabled = true;

                //åben Tidsforbrug
                MyTextbox_Afslut_Tidsforbrug_1.IsEnabled = true;
                MyTextbox_Afslut_Tidsforbrug_2.IsEnabled = true;
                MyTextbox_Afslut_Tidsforbrug_3.IsEnabled = true;
                MyTextbox_Afslut_Tidsforbrug_4.IsEnabled = true;

                //åben Kommentar
                MyTextBlock_Afslut_Kommentar.IsEnabled = true;
            }
        }
        private void LoadDoc() {
            
            Class.XML_Files.DocData.Layout.DocumentData MyDocument = this.FileDocument;
            
            //sæt afsender info
            Address_Afsender.Textbox_ContactTlf.Text = MyDocument.Afsender.telefon;
            Address_Afsender.Textbox_ContactPer.Text = MyDocument.Afsender.kontaktPerson;
            Address_Afsender.Textbox_FirmName.Text = MyDocument.Afsender.firma;
            Address_Afsender.Textbox_Address.Text = MyDocument.Afsender.vej;
            Address_Afsender.Textbox_ZipCode.Text = MyDocument.Afsender.zipCode;
            Address_Afsender.CheckBox_Pay.Checked = MyDocument.Afsender.isPayer;

            //sæt modtager info
            Address_Modtager.Textbox_ContactTlf.Text = MyDocument.Modtager.telefon;
            Address_Modtager.Textbox_ContactPer.Text = MyDocument.Modtager.kontaktPerson;
            Address_Modtager.Textbox_FirmName.Text = MyDocument.Modtager.firma;
            Address_Modtager.Textbox_Address.Text = MyDocument.Modtager.vej;
            Address_Modtager.Textbox_ZipCode.Text = MyDocument.Modtager.zipCode;
            Address_Modtager.CheckBox_Pay.Checked = MyDocument.Modtager.isPayer;

            //sæt anden modtager info
            Address_Modtager2.CheckedHeader = MyDocument.Modtager2.isActive;
            Address_Modtager2.Textbox_ContactTlf.Text = MyDocument.Modtager2.telefon;
            Address_Modtager2.Textbox_ContactPer.Text = MyDocument.Modtager2.kontaktPerson;
            Address_Modtager2.Textbox_FirmName.Text = MyDocument.Modtager2.firma;
            Address_Modtager2.Textbox_Address.Text = MyDocument.Modtager2.vej;
            Address_Modtager2.Textbox_ZipCode.Text = MyDocument.Modtager2.zipCode;
            Address_Modtager2.CheckBox_Pay.Checked = MyDocument.Modtager2.isPayer;

            //sæt fragtbrev generelt info
            MyTextbox_Generelt_Reference.Text = MyDocument.Generelt.reference;
            MyTextbox_Generelt_Fragtmand.Text = MyDocument.Generelt.fragtmand;
            Selector_Generelt_Forsikringstype.SelectId = MyDocument.Generelt.forsikringstype;
            DateTimePicker_Generelt_Rute_1.SelecteDate = MyDocument.Generelt.datoRute1;
            DateTimePicker_Generelt_Rute_1.HasBeenSet = MyDocument.Generelt.isSetDatoRute1;
            DateTimePicker_Generelt_Rute_1.SetDatetimeTextboxText();
            MyTextbox_Generelt_Rute_1.Text = MyDocument.Generelt.zipRute1;
            DateTimePicker_Generelt_Rute_2.SelecteDate = MyDocument.Generelt.datoRute2;
            DateTimePicker_Generelt_Rute_2.HasBeenSet = MyDocument.Generelt.isSetDatoRute2;
            DateTimePicker_Generelt_Rute_2.SetDatetimeTextboxText();
            MyTextbox_Generelt_Rute_2.Text = MyDocument.Generelt.zipRute2;

            //sæt byttepaller info
            CheckBoxButton_Byttepaller.Checked = MyDocument.Byttepaller.useByttepalle;
            MyTextbox_Byttepaller_1.Text = MyDocument.Byttepaller.palle1_1;
            MyTextbox_Byttepaller_2.Text = MyDocument.Byttepaller.palle1_2;
            MyTextbox_Byttepaller_3.Text = MyDocument.Byttepaller.palle1_4;

            //sæt efterkrav info
            CheckBoxButton_Efterkrav.Checked = MyDocument.Efterkrav.useEfterkrav;
            MyTextbox_Efterkrav_1.Text = MyDocument.Efterkrav.efterkravGebyr;
            MyTextbox_Efterkrav_2.Text = MyDocument.Efterkrav.forsikringssum;
            MyTextbox_Efterkrav_3.Text = MyDocument.Efterkrav.premium;
            MyTextbox_Efterkrav_4.Text = MyDocument.Efterkrav.total;

            Selector_Transport1.SelectId = MyDocument.Transport.transportType[0];
            Selector_Transport_SelectionChanged(Selector_Transport1, null);
            Selector_Transport2.SelectId = MyDocument.Transport.transportType[1];
            Selector_Transport_SelectionChanged(Selector_Transport2, null);
            Selector_Transport3.SelectId = MyDocument.Transport.transportType[2];
            Selector_Transport_SelectionChanged(Selector_Transport3, null);



            PakkeControl_Transport.Items = MyDocument.Transport.pakker;

            //sæt ekstra bemærkning info
            MyTextBlock_Notifications.Text = MyDocument.FragtbrevNotifications;

            //sæt afslut fragtbrev info
            DateTimePicker_Afslut_Leveringsdato.SelecteDate = MyDocument.LukFragtbrev.leveringsdato;
            DateTimePicker_Afslut_Leveringsdato.HasBeenSet = MyDocument.LukFragtbrev.isSetLeveringsdato;
            DateTimePicker_Afslut_Leveringsdato.SetDatetimeTextboxText();
            MyTextbox_Afslut_Rabat.Text = MyDocument.LukFragtbrev.rabat;
            MyTextbox_Afslut_Kilometer.Text = MyDocument.LukFragtbrev.kilometer;
            MyTextbox_Afslut_Tidsforbrug_1.Text = MyDocument.LukFragtbrev.tidsforbrug1;
            MyTextbox_Afslut_Tidsforbrug_2.Text = MyDocument.LukFragtbrev.tidsforbrug2;
            MyTextbox_Afslut_Tidsforbrug_3.Text = MyDocument.LukFragtbrev.tidsforbrug3;
            MyTextbox_Afslut_Tidsforbrug_4.Text = MyDocument.LukFragtbrev.tidsforbrug4;

            //sæt ekstra gebyr info
            for (int i = 0; i < MyDocument.EkstraGebyr.buttonsIsCheck.Length; i++)
            {
                (Gebyr_Ekstra.FindName("CheckBoxButton_Gebyr_" + i) as CheckBoxButton).Checked = MyDocument.EkstraGebyr.buttonsIsCheck[i];
            }
            Gebyr_Ekstra.MyTextbox_Gebyr_0.Text = MyDocument.EkstraGebyr.textboxsValues[0];
            Gebyr_Ekstra.MyTextbox_Gebyr_1.Text = MyDocument.EkstraGebyr.textboxsValues[1];
            Gebyr_Ekstra.MyTextbox_Gebyr_6.Text = MyDocument.EkstraGebyr.textboxsValues[2];
            Gebyr_Ekstra.MyTextbox_Gebyr_7.Text = MyDocument.EkstraGebyr.textboxsValues[3];
            Gebyr_Ekstra.MyTextbox_Gebyr_9.Text = MyDocument.EkstraGebyr.textboxsValues[4];
            Gebyr_Ekstra.MyTextbox_Gebyr_10.Text = MyDocument.EkstraGebyr.textboxsValues[5];
            Gebyr_Ekstra.MyTextbox_Gebyr_11.Text = MyDocument.EkstraGebyr.textboxsValues[6];

            UpdateViewStatusAll();
            UpdateDocumentAccess();
        }
        private void SaveDoc()
        {
            UpdateSaveStatus(true);

            Class.XML_Files.DocData.Layout.DocumentData MyDocument = new Class.XML_Files.DocData.Layout.DocumentData();
            Class.XML_Files.DocData FuncDocData = new Class.XML_Files.DocData();

            MyDocument.Invoice = this.FileDocument.Invoice;

            //hent afsender info
            MyDocument.Afsender.telefon = Address_Afsender.Textbox_ContactTlf.Text;
            MyDocument.Afsender.kontaktPerson = Address_Afsender.Textbox_ContactPer.Text;
            MyDocument.Afsender.firma = Address_Afsender.Textbox_FirmName.Text;
            MyDocument.Afsender.vej = Address_Afsender.Textbox_Address.Text;
            MyDocument.Afsender.zipCode = Address_Afsender.Textbox_ZipCode.Text;
            MyDocument.Afsender.isPayer = Address_Afsender.CheckBox_Pay.Checked;
            Address_Afsender.SaveCustomer();

            //hent modtager info
            MyDocument.Modtager.telefon = Address_Modtager.Textbox_ContactTlf.Text;
            MyDocument.Modtager.kontaktPerson = Address_Modtager.Textbox_ContactPer.Text;
            MyDocument.Modtager.firma = Address_Modtager.Textbox_FirmName.Text;
            MyDocument.Modtager.vej = Address_Modtager.Textbox_Address.Text;
            MyDocument.Modtager.zipCode = Address_Modtager.Textbox_ZipCode.Text;
            MyDocument.Modtager.isPayer = Address_Modtager.CheckBox_Pay.Checked;
            Address_Modtager.SaveCustomer();

            //hent anden modtager info
            MyDocument.Modtager2.isActive = Address_Modtager2.CheckedHeader;
            MyDocument.Modtager2.telefon = Address_Modtager2.Textbox_ContactTlf.Text;
            MyDocument.Modtager2.kontaktPerson = Address_Modtager2.Textbox_ContactPer.Text;
            MyDocument.Modtager2.firma = Address_Modtager2.Textbox_FirmName.Text;
            MyDocument.Modtager2.vej = Address_Modtager2.Textbox_Address.Text;
            MyDocument.Modtager2.zipCode = Address_Modtager2.Textbox_ZipCode.Text;
            MyDocument.Modtager2.isPayer = Address_Modtager2.CheckBox_Pay.Checked;
            Address_Modtager2.SaveCustomer();

            //hent fragtbrev generelt info
            MyDocument.Generelt.reference = MyTextbox_Generelt_Reference.Text;
            MyDocument.Generelt.fragtmand = MyTextbox_Generelt_Fragtmand.Text;
            MyDocument.Generelt.forsikringstype = Selector_Generelt_Forsikringstype.SelectId;
            MyDocument.Generelt.datoRute1 = DateTimePicker_Generelt_Rute_1.SelecteDate;
            MyDocument.Generelt.isSetDatoRute1 = DateTimePicker_Generelt_Rute_1.HasBeenSet;
            MyDocument.Generelt.zipRute1 = MyTextbox_Generelt_Rute_1.Text;
            MyDocument.Generelt.datoRute2 = DateTimePicker_Generelt_Rute_2.SelecteDate;
            MyDocument.Generelt.isSetDatoRute2 = DateTimePicker_Generelt_Rute_2.HasBeenSet;
            MyDocument.Generelt.zipRute2 = MyTextbox_Generelt_Rute_2.Text;

            //hent byttepaller info
            MyDocument.Byttepaller.useByttepalle = CheckBoxButton_Byttepaller.Checked;
            MyDocument.Byttepaller.palle1_1 = MyTextbox_Byttepaller_1.Text;
            MyDocument.Byttepaller.palle1_2 = MyTextbox_Byttepaller_2.Text;
            MyDocument.Byttepaller.palle1_4 = MyTextbox_Byttepaller_3.Text;

            //hent efterkrav info
            MyDocument.Efterkrav.useEfterkrav = CheckBoxButton_Efterkrav.Checked;
            MyDocument.Efterkrav.efterkravGebyr = MyTextbox_Efterkrav_1.Text;
            MyDocument.Efterkrav.forsikringssum = MyTextbox_Efterkrav_2.Text;
            MyDocument.Efterkrav.premium = MyTextbox_Efterkrav_3.Text;
            MyDocument.Efterkrav.total = MyTextbox_Efterkrav_4.Text;

            //hent transport info
            MyDocument.Transport.transportType[0] = Selector_Transport1.SelectId;
            MyDocument.Transport.transportType[1] = Selector_Transport2.SelectId;
            MyDocument.Transport.transportType[2] = Selector_Transport3.SelectId;
            MyDocument.Transport.pakker = PakkeControl_Transport.Items;

            //hent ekstra bemærkning info
            MyDocument.FragtbrevNotifications = MyTextBlock_Notifications.Text;

            //hent afslut fragtbrev info
            MyDocument.LukFragtbrev.leveringsdato = DateTimePicker_Afslut_Leveringsdato.SelecteDate;
            MyDocument.LukFragtbrev.isSetLeveringsdato = DateTimePicker_Afslut_Leveringsdato.HasBeenSet;
            MyDocument.LukFragtbrev.rabat = MyTextbox_Afslut_Rabat.Text;
            MyDocument.LukFragtbrev.kilometer = MyTextbox_Afslut_Kilometer.Text;
            MyDocument.LukFragtbrev.tidsforbrug1 = MyTextbox_Afslut_Tidsforbrug_1.Text;
            MyDocument.LukFragtbrev.tidsforbrug2 = MyTextbox_Afslut_Tidsforbrug_2.Text;
            MyDocument.LukFragtbrev.tidsforbrug3 = MyTextbox_Afslut_Tidsforbrug_3.Text;
            MyDocument.LukFragtbrev.tidsforbrug4 = MyTextbox_Afslut_Tidsforbrug_4.Text;

            //hent ekstra gebyr info
            for (int i = 0; i < MyDocument.EkstraGebyr.buttonsIsCheck.Length; i++)
            {
                MyDocument.EkstraGebyr.buttonsIsCheck[i] = (Gebyr_Ekstra.FindName("CheckBoxButton_Gebyr_" + i) as CheckBoxButton).Checked;
            }
            MyDocument.EkstraGebyr.textboxsValues[0] = Gebyr_Ekstra.MyTextbox_Gebyr_0.Text;
            MyDocument.EkstraGebyr.textboxsValues[1] = Gebyr_Ekstra.MyTextbox_Gebyr_1.Text;
            MyDocument.EkstraGebyr.textboxsValues[2] = Gebyr_Ekstra.MyTextbox_Gebyr_6.Text;
            MyDocument.EkstraGebyr.textboxsValues[3] = Gebyr_Ekstra.MyTextbox_Gebyr_7.Text;
            MyDocument.EkstraGebyr.textboxsValues[4] = Gebyr_Ekstra.MyTextbox_Gebyr_9.Text;
            MyDocument.EkstraGebyr.textboxsValues[5] = Gebyr_Ekstra.MyTextbox_Gebyr_10.Text;
            MyDocument.EkstraGebyr.textboxsValues[6] = Gebyr_Ekstra.MyTextbox_Gebyr_11.Text;

            FuncDocData.SaveFile(MyDocument);
            this.FileDocument = MyDocument;
        }

        #endregion

        #region Document - Function og Events

        private void Address_Faktura_Update()
        {
            Address fakturaInfo = null;

            //find ud at hvem der skal stå på fakturaen
            if (Address_Afsender.CheckBox_Pay.Checked)
                fakturaInfo = Address_Afsender;
            else if (Address_Modtager.CheckBox_Pay.Checked)
                fakturaInfo = Address_Modtager;
            else if (Address_Modtager2.CheckBox_Pay.Checked)
                fakturaInfo = Address_Modtager2;
            else
                return;

            //sæt data
            Address_Faktura.Textbox_ContactTlf.Text = fakturaInfo.Textbox_ContactTlf.Text;
            Address_Faktura.Textbox_ContactPer.Text = fakturaInfo.Textbox_ContactPer.Text;
            Address_Faktura.Textbox_FirmName.Text = fakturaInfo.Textbox_FirmName.Text;
            Address_Faktura.Textbox_Address.Text = fakturaInfo.Textbox_Address.Text;
            Address_Faktura.Textbox_ZipCode.Text = fakturaInfo.Textbox_ZipCode.Text;
        }
        
        /// <summary>
        /// gør så der kun kan være en 
        /// hakket af i adresse information
        /// </summary>
        private void Address_Contact_PayClick(object sender, RoutedEventArgs e)
        {
            Address_Afsender.CheckBox_Pay.Checked = false;
            Address_Modtager.CheckBox_Pay.Checked = false;
            Address_Modtager2.CheckBox_Pay.Checked = false;

            (sender as CheckBoxButton).Checked = true;
        }
        private void PakkeControl_Transport_ValueChange(object sender, EventArgs e)
        {
            UpdatePriceList();
        }

        /// <summary>
        /// her vælger vi hvilken transport
        /// type det er
        /// den bliver brugt på alle tre
        /// selector
        /// </summary>
        private void Selector_Transport_SelectionChanged(object sender, EventArgs e)
        {
            //gør det lettere at læse
            const int transportKurer = 0;
            const int transportPakke = 1;
            const int transportGods = 2;

            Selector thisSelector = (sender as Selector); //denne selector
            Selector NextSelector = null; //næste selector
            List<string> itemsSource = new List<string>();
            int valueId = Selector_Transport1.SelectId; //Trin 1 Id

            //sæt næste selector
            if (thisSelector == Selector_Transport1)
                NextSelector = Selector_Transport2;
            else if (thisSelector == Selector_Transport2)
                NextSelector = Selector_Transport3;

            //gør så hvis man ændre mening
            //vil de andre trin ikke blive
            //vist før de skal
            if (thisSelector == Selector_Transport1)
                Selector_Transport2.Visibility = Visibility.Collapsed;
            if (thisSelector == Selector_Transport1 || thisSelector == Selector_Transport2)
                Selector_Transport3.Visibility = Visibility.Collapsed;

            if (thisSelector == Selector_Transport3 && Selector_Transport1.SelectId != 0)
                return;

            PakkeControl_Transport.Visibility = Visibility.Collapsed;




            //hvis der ikke er valgt noget id for trin 1
            if (valueId == -1)
                return;

            //lav item source for næste selector
            switch (valueId)
            {
                case transportKurer:
                    if (thisSelector == Selector_Transport1)
                        itemsSource = new List<string>() { "GoRush", "GoFlex", "GoVIP" };
                    else if (thisSelector == Selector_Transport2)
                        itemsSource = new List<string>() { "Grp. 1", "Grp. 2", "Grp. 3", "Grp. 4" };
                    break;

                case transportPakke:
                    if (thisSelector == Selector_Transport1)
                        itemsSource = new List<string>() { "GoGreen", "GoPlus" };
                    break;

                case transportGods:
                    if (thisSelector == Selector_Transport1)
                        itemsSource = new List<string>() { "GoFull", "GoPart" };
                    break;
            }

            //tjek om den skal vise PakkeControl eller
            //næste selector
            if ((NextSelector != null && thisSelector != Selector_Transport2) ||
                (thisSelector == Selector_Transport2 && valueId == transportKurer) && thisSelector.SelectId != -1)
            {
                NextSelector.SelectId = -1;
                NextSelector.Items = itemsSource;
                NextSelector.Visibility = Visibility.Visible;
            }
            else if (((thisSelector == Selector_Transport2 && Selector_Transport1.SelectId != 0) || 
                (thisSelector == Selector_Transport3 && Selector_Transport1.SelectId == 0)) && 
                thisSelector.SelectId != -1)
            {
                PakkeControl_Transport.SetTransportType = valueId;
                PakkeControl_Transport.Visibility = Visibility.Visible;
                PakkeControl_Transport.Update();
            }
        }

        /// <summary>
        /// Function der skal tjekke om
        /// de andre fragtbrev vinduer er 
        /// udflydt så man kan forsætte
        /// </summary>
        private void Grid_StatusCheck_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Grid gridSender = sender as Grid;
            int viewStartId = 0;
            byte[] statusColor = image_Color_Done;
            string statusImage = "/Images/Icons/CheckWhite.png";
            List<string> viewTextList = new List<string>();
            string headerText = "";
            bool isDone = true;

            //skal kun opdateres når den er synlig
            if (gridSender.Visibility != Visibility.Visible || !this.IsLoaded)
                return;

            UpdateImagePositionColorAll();
            Grid_CheckStatusContainer.Children.Clear();
            Grid_CheckStatusContainer.RowDefinitions.Clear();
            
            if (this.viewId == View_StatusFragt)
            {
                headerText = "Fragtbrev er færdig";
                viewStartId = View_Contact;
                viewTextList.Add("Kontakt Informationer");
                viewTextList.Add("Generelt");
                viewTextList.Add("Transport");
                viewTextList.Add("Bemærkninger");
            }
            else //if (this.viewId == View_StatusFaktura)            
            {
                headerText = "Faktura er færdig";
                viewStartId = View_GenereltFaktura;
                viewTextList.Add("Generelt");
                viewTextList.Add("Transport");
                viewTextList.Add("Gebyr");
            }

            for (int i = 0; i < viewTextList.Count + viewStartId; i++)
            {
                if (!GetViewStatus(i))
                {
                    if (this.viewId == View_StatusFragt)
                        headerText = "Fragtbrev er ikke færdig";
                    else
                        headerText = "Faktura er ikke færdig";

                    isDone = false;
                    statusColor = image_Color_NotDone;
                    statusImage = "/Images/Icons/UnCheckWhite.png";
                    break;
                }
            }

            //tilføj row
            int headerRowId = Grid_CheckStatusContainer.RowDefinitions.Count;
            Grid_CheckStatusContainer.RowDefinitions.Add(new RowDefinition());
            Grid_CheckStatusContainer.RowDefinitions[headerRowId].Height = GridLength.Auto;

            Image headerViewIcon = new Image();
            headerViewIcon.Height = 100;
            headerViewIcon.Width = 100;
            TextBlock headerViewText = new TextBlock();
            headerViewText.Text = "Status: " + headerText;
            headerViewText.FontSize = 25;
            headerViewText.FontWeight = FontWeights.Bold;
            headerViewText.Margin = new Thickness(25, 0, 0, 0);
            headerViewText.VerticalAlignment = VerticalAlignment.Center;
            

            ChangeImageColor(headerViewIcon, statusImage, statusColor);


            Grid.SetColumn(headerViewIcon, 0);
            Grid.SetColumn(headerViewText, 1);

            Grid.SetRow(headerViewIcon, headerRowId);
            Grid.SetRow(headerViewText, headerRowId);

            Grid_CheckStatusContainer.Children.Add(headerViewIcon);
            Grid_CheckStatusContainer.Children.Add(headerViewText);

            for (int i = viewStartId; i < viewTextList.Count + viewStartId; i++)
            {
                //opdatere view status
                this.gridPostionStatus[i] = GetViewStatus(i);

                //tilføj row
                int rowId = Grid_CheckStatusContainer.RowDefinitions.Count;
                Grid_CheckStatusContainer.RowDefinitions.Add(new RowDefinition());
                Grid_CheckStatusContainer.RowDefinitions[rowId].Height = GridLength.Auto;

                Image viewIcon = new Image();
                viewIcon.Height = 80;
                viewIcon.Width = 80;
                TextBlock viewText = new TextBlock();
                viewText.Text = viewTextList[i - viewStartId];
                viewText.FontSize = 20;
                viewText.FontWeight = FontWeights.Bold;
                viewText.Margin = new Thickness(20, 0, 0, 0);
                viewText.VerticalAlignment = VerticalAlignment.Center;

                //billede status farve
                byte[] colorId;
                if (this.gridPostionStatus[i])
                    colorId = image_Color_Done;
                else
                    colorId = image_Color_NotDone;

                ChangeImageColor(viewIcon, this.imagePostionPng[i], colorId);

                
                Grid.SetColumn(viewIcon,0);
                Grid.SetColumn(viewText,1);

                Grid.SetRow(viewIcon, rowId);
                Grid.SetRow(viewText, rowId);

                Grid_CheckStatusContainer.Children.Add(viewIcon);
                Grid_CheckStatusContainer.Children.Add(viewText);
            }

            if (isDone)
            {
                if (this.viewId == View_StatusFragt)
                    this.documentStatus = document_FragtClose;
                else
                    this.documentStatus = document_FakturaClose;
            }
        }

        private void UpdatePriceList()
        {
            int transport1 = Selector_Transport1.SelectId;
            int transport2 = Selector_Transport2.SelectId;
            int transport3 = Selector_Transport3.SelectId;
            int gebyrId_1 = 0;
            int gebyrId_2 = 0;

            int pakkeCount = 0;
            double nettofragt = 0;

            double rabat = 0;
            double kilometer = 0;

            double.TryParse(MyTextbox_Afslut_Rabat.Text, out rabat);
            double.TryParse(MyTextbox_Afslut_Kilometer.Text, out kilometer);

            List<PrisList.pricelistItem> priceSource = new List<PrisList.pricelistItem>();


            switch (transport1)
            {
                case c_Transport_Kurer:
                    priceSource.Add(new PrisList.pricelistItem() { isHeader = true, name = "Nettofragt" });
                    gebyrId_1 = transport2;
                    gebyrId_2 = transport3;

                    double d_startGebyr = Models.FakturaPrisliste.kurerPriser.startgebyr[transport2][transport3];
                    double d_kilometer = kilometer * Models.FakturaPrisliste.kurerPriser.kilometer[transport2][transport3];
                    double d_minimun = Models.FakturaPrisliste.kurerPriser.minimun[transport2][transport3];
                    d_minimun -= (d_startGebyr + d_kilometer);

                    priceSource.Add(new PrisList.pricelistItem() { name = "Startgebyr", price = d_startGebyr.ToString() });

                    if (d_kilometer > 0)
                        priceSource.Add(new PrisList.pricelistItem() { name = "Kilometertakst. " + kilometer + " km", price = d_kilometer.ToString() });

                    if (d_minimun > 0)
                        priceSource.Add(new PrisList.pricelistItem() { name = "Minimun pris pr. tur", price = d_minimun.ToString() });


                    nettofragt += d_startGebyr;
                    nettofragt += d_kilometer;
                    if (d_minimun > 0)
                        nettofragt += d_minimun;

                    break;
                case c_Transport_Pakke:
                    priceSource.Add(new PrisList.pricelistItem() { isHeader = true, name = "Pakker" });

                    List<int> pakkeStrCount = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
                    List<string> pakkeStrHeader = new List<string>() { "XS", "S", "M", "L", "XL", "XXL", "XXL" };


                    foreach (var item in PakkeControl_Transport.Items)
                    {
                        int pakkeStr = item.transportTypeId;

                        if (pakkeStr == -1)
                            continue;

                        pakkeStrCount[pakkeStr] += item.countI;
                        pakkeCount += item.countI;
                    }

                    for (int i = 0; i < pakkeStrCount.Count; i++)
                    {
                        if (transport2 == -1)
                            break;
                        int _antal = pakkeStrCount[i];
                        string _header = pakkeStrHeader[i];
                        double _price = _antal * Models.FakturaPrisliste.pakkePriser.Prices[i][transport2];

                        if (_antal > 0)
                            priceSource.Add(new PrisList.pricelistItem() { name = _antal + "x Pakke str." + _header, price = _price.ToString("N2") });
                    }

                    break;
                case c_Transport_Gods:
                    var godsFunc = new Class.GodsFunction();
                    priceSource.Add(new PrisList.pricelistItem() { isHeader = true, name = "Nettofragt" });
                    
                    foreach (var item in PakkeControl_Transport.Items)
                    {
                        for (int i = 0; i < item.countI; i++)
                        {
                            if (item.IsDoneFaktura == null || item.IsDoneFaktura.Count != item.countI)
                            {
                                break;
                            }

                            double kiloCal = godsFunc.GetCalKilo(item.takstId[i], item.volumeL, item.volumeB, item.volumeH);
                            double kiloUse = kiloCal >= item.weightD ? kiloCal : item.weightD;

                            nettofragt += godsFunc.GetPrice(item.takstId[i], kiloUse);
                        }
                    }


                    priceSource.Add(new PrisList.pricelistItem() { name = "Pakker", price = nettofragt.ToString("N2") });

                    gebyrId_1 = 0;
                    gebyrId_2 = 3;

                    break;
            }
            priceSource.Add(new PrisList.pricelistItem() { isSpace = true });

            if (transport1 == c_Transport_Kurer ||
                transport1 == c_Transport_Gods)
            {
                priceSource.Add(new PrisList.pricelistItem() { isHeader = true, name = "Tid / Minut" });


                int time_Load = 0;
                int time_UnLoad = 0;
                int time_Wait = 0;
                int time_Helper = 0;

                int.TryParse(MyTextbox_Afslut_Tidsforbrug_1.Text, out time_Load);
                int.TryParse(MyTextbox_Afslut_Tidsforbrug_2.Text, out time_UnLoad);
                int.TryParse(MyTextbox_Afslut_Tidsforbrug_3.Text, out time_Wait);
                int.TryParse(MyTextbox_Afslut_Tidsforbrug_4.Text, out time_Helper);

                //hvis der er blevet brugt mere end 20 min
                int countTime = time_Load + time_UnLoad + time_Wait;
                if (countTime > 20)
                {
                    int ekstraTid = countTime - 20;
                    double timePrice = (Models.FakturaPrisliste.kurerPriser.ekstraTidforbrug[gebyrId_1][gebyrId_2] * ekstraTid);

                    priceSource.Add(new PrisList.pricelistItem() { name = "Tidsforbrug(" + ekstraTid.ToString() + "min)", price = timePrice.ToString("N2") });

                }

                //medhjælper
                if (Gebyr_Ekstra.CheckBoxButton_Gebyr_0.Checked)
                {
                    int helperInUse = 1;
                    int.TryParse(Gebyr_Ekstra.MyTextbox_Gebyr_0.Text, out helperInUse);

                    double countHelpTime = Math.Ceiling(time_Helper / 30f);
                    double allHelperTime = 30 * countHelpTime;

                    double helperPrice = (Models.FakturaPrisliste.kurerPriser.medhjaelper[gebyrId_1][gebyrId_2] * countHelpTime) * helperInUse;
                    priceSource.Add(new PrisList.pricelistItem() { name = "Medhjælper (" + allHelperTime + " min)", price = helperPrice.ToString("N2") });
                }
                priceSource.Add(new PrisList.pricelistItem() { isSpace = true });
            }

            if (transport1 == c_Transport_Kurer ||
                transport1 == c_Transport_Gods)
            {
                int[] checkboxWithTextboxCount = { 0, 1, 6, 7 };
                int[] checkboxWithTextboxPay = { 9, 10, 11 };
                int[] checkboxUseNettofragt = { 5 };
                int[] checkboxUse = { 0 };
                bool foundOneGebyr = false;

                for (int i = 0; i < 12; i++)
                {
                    if (checkboxUse.Contains(i))
                        continue;

                    CheckBoxButton myCheckbox = Gebyr_Ekstra.FindName("CheckBoxButton_Gebyr_" + i) as CheckBoxButton;

                    if (myCheckbox.Checked)
                    {
                        string gebyrName = myCheckbox.Text;
                        double gebyrPrice = 0;

                        if (checkboxWithTextboxCount.Contains(i))
                        {
                            MyTextbox countTextbox = Gebyr_Ekstra.FindName("MyTextbox_Gebyr_" + i) as MyTextbox;
                            int priceCount = 1;

                            int.TryParse(countTextbox.Text, out priceCount);
                            gebyrPrice = priceCount * Models.FakturaPrisliste.EkstraGebyrPrice.prices[i][gebyrId_1][gebyrId_2];
                        }
                        else if (checkboxWithTextboxPay.Contains(i))
                        {
                            MyTextbox countTextbox = Gebyr_Ekstra.FindName("MyTextbox_Gebyr_" + i) as MyTextbox;
                            double newPrice = 0;

                            double.TryParse(countTextbox.Text, out newPrice);
                            gebyrPrice = newPrice;
                        }
                        else if (checkboxUseNettofragt.Contains(i))
                            gebyrPrice = nettofragt * (Models.FakturaPrisliste.EkstraGebyrPrice.prices[i][gebyrId_1][gebyrId_2] / 100);
                        else
                            gebyrPrice = Models.FakturaPrisliste.EkstraGebyrPrice.prices[i][gebyrId_1][gebyrId_2];


                        if (!foundOneGebyr && gebyrPrice != 0)
                        {
                            priceSource.Add(new PrisList.pricelistItem() { isHeader = true, name = "Tillæg for særlige ydelser" });
                            foundOneGebyr = true;
                        }
                        if (gebyrPrice != 0)
                        {
                            priceSource.Add(new PrisList.pricelistItem() { name = gebyrName, price = gebyrPrice.ToString("N2") });
                        }
                    }
                }

                if (foundOneGebyr)
                {
                    priceSource.Add(new PrisList.pricelistItem() { isSpace = true });
                }

                priceSource.Add(new PrisList.pricelistItem() { isHeader = true, name = "Gebyr" });

                if (transport1 == c_Transport_Pakke)
                {
                    if (pakkeCount > 0)
                    {
                        double _price = pakkeCount * Models.FakturaPrisliste.pakkePriser.Prices[7][transport2];
                        priceSource.Add(new PrisList.pricelistItem() { name = "Servicegebyr pr.pakke (" + pakkeCount + ")", price = _price.ToString("N2") });
                    }
                }

                else {

                    double gebyrBraend = nettofragt * (Models.FakturaPrisliste.kurerPriser.braendstof[gebyrId_1][gebyrId_2] / 100);
                    double gebyrMiljo = nettofragt * (Models.FakturaPrisliste.kurerPriser.miljoegebyr[gebyrId_1][gebyrId_2] / 100);
                    double adminGebyr = Models.FakturaPrisliste.kurerPriser.Adminnistrationsgebyr[gebyrId_1][gebyrId_2];

                    priceSource.Add(new PrisList.pricelistItem() { name = "Brændstofgebyr Beregnes af nettofragt", price = gebyrBraend.ToString("N2") });
                    priceSource.Add(new PrisList.pricelistItem() { name = "Miljøgebyr beregnes af nettofragt", price = gebyrMiljo.ToString("N2") });
                    priceSource.Add(new PrisList.pricelistItem() { name = "Adminnistrationsgebyr pr. faktura", price = adminGebyr.ToString("N2") });

                }

                priceSource.Add(new PrisList.pricelistItem() { isSpace = true });
            }

            Prislist_faktura.rabatProcent = rabat;
            Prislist_faktura.items = priceSource;
            Prislist_faktura.Update();
        }

        #endregion

        private void ShowViewInFullMode()
        {
            ViewRowReset();
            
            switch (this.viewId)
            {
                case View_Contact:
                case View_GenereltFragt:
                case View_TransportFragt:
                case View_Notifications:
                    SetUpGridView(View_Contact, true); //bruger true da vi ikke vil have tidligere ting vis
                    SetUpGridView(View_GenereltFragt, false);
                    SetUpGridView(View_TransportFragt, false);
                    SetUpGridView(View_Notifications, false);
                    break;

                case View_StatusFragt:
                case View_CloseFragt:
                case View_PDFFragt:
                case View_StatusFaktura:
                case View_CloseFaktura:
                case View_PDFFaktura:
                    SetUpGridView(this.viewId, true); //bruger true da vi ikke vil have tidligere ting vis
                    
                    break;

                case View_GenereltFaktura:
                case View_TransportFaktura:
                case View_Gebyr:
                    SetUpGridView(View_GenereltFaktura, true); //bruger true da vi ikke vil have tidligere ting vis
                    SetUpGridView(View_TransportFaktura, false);
                    SetUpGridView(View_Gebyr, false);
                    break;
            }

        }
        
        #region ToolBar

        public static readonly DependencyProperty ChangeHeaderInfoStateProperty =
            DependencyProperty.Register("ChangeHeaderInfoState", typeof(bool), typeof(UserPanel), new PropertyMetadata(false));
        public static readonly DependencyProperty ShowPricelistProperty =
            DependencyProperty.Register("ShowPricelist", typeof(bool), typeof(UserPanel), new PropertyMetadata(false));
        public bool ChangeHeaderInfoState
        {
            get
            {
                return (bool)GetValue(ChangeHeaderInfoStateProperty);
            }
            set
            {
                SetValue(ChangeHeaderInfoStateProperty, value);
            }
        }
        public bool ShowPricelist
        {
            get
            {
                return (bool)GetValue(ShowPricelistProperty);
            }
            set
            {
                SetValue(ShowPricelistProperty, value);
            }
        }

        private bool UseFullView = false;
        private void ToolBarButton_ChangeOptions_Click(object sender, RoutedEventArgs e)
        {
            int buttonId = Grid.GetColumn((sender as System.Windows.Controls.Primitives.ToggleButton));
            const int Button_ChangePageView = 0;
            const int Button_ChangePlaceholder = 1;
            const int Button_ChangePricelist = 2;

            switch (buttonId)
            {
                case Button_ChangePageView:
                    this.UseFullView = !this.UseFullView;

                    if (this.UseFullView)
                        ShowViewInFullMode();
                    else
                        ShowViewElement(this.viewId, true);
                    break;
                case Button_ChangePlaceholder:
                    ChangeHeaderInfoState = !ChangeHeaderInfoState;

                    Thickness byttepallerMargin = CheckBoxButton_Byttepaller.Margin;
                    if (ChangeHeaderInfoState)
                    {
                        byttepallerMargin.Top += 38;                       
                    }
                    else
                    {
                        byttepallerMargin.Top -= 38;
                    }

                    CheckBoxButton_Byttepaller.Margin = byttepallerMargin;

                    break;
                case Button_ChangePricelist:
                    ShowPricelist = !ShowPricelist;
                    break;
            }
        }
        private void toolbarButton_new_Click(object sender, RoutedEventArgs e)
        {
            UpdatePriceList();
        }
        
        private void ToolsbarButton_Filer_Click(object sender, RoutedEventArgs e)
        {
        }
        private void toolbarButton_PDF_Click(object sender, RoutedEventArgs e)
        {
            var saveFileFunc = new Class.XML_Files.DocData();
            this.FileDocument = saveFileFunc.ReadFile("document-1000.xml");
            LoadDoc();
        }

        private void toolbarButton_save_Click(object sender, RoutedEventArgs e)
        {
            if (!this.isSaved)
                SaveDoc();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double size = (sender as Slider).Value / 100;
            ((ScrollViewBox.Content as Grid).Children[0] as Grid).LayoutTransform = new ScaleTransform(size, size);
        }
        #endregion

        #region Function

        /// <summary>
        /// skifter farven hvid ud med valgte farve
        ///  denne function er hentet fra nettet af
        ///  hvor jeg har rettet den lidt til
        /// </summary>
        private void ChangeImageColor(object imageObj, string imagePicDefault, byte[] colorType)
        {
            // Copy pixel colour values from existing image.
            // (This loads them from an embedded resource. BitmapDecoder can work with any Stream, though.)
            System.Windows.Resources.StreamResourceInfo x =
                Application.GetResourceStream(
                    new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), imagePicDefault));
            BitmapDecoder dec = BitmapDecoder.Create(x.Stream, BitmapCreateOptions.None, BitmapCacheOption.Default);
            BitmapFrame image = dec.Frames[0];
            byte[] pixels = new byte[image.PixelWidth * image.PixelHeight * 4];
            image.CopyPixels(pixels, image.PixelWidth * 4, 0);

            // Modify the white pixels
            for (int i = 0; i < pixels.Length / 4; ++i)
            {
                byte b = pixels[i * 4];
                byte g = pixels[i * 4 + 1];
                byte r = pixels[i * 4 + 2];
                byte a = pixels[i * 4 + 3];

                if (r == 255 &&
                    g == 255 &&
                    b == 255 &&
                    a == 255)
                {
                    r = colorType[0];
                    g = colorType[1];
                    b = colorType[2];                    


                    pixels[i * 4] = b;
                    pixels[i * 4 + 1] = g;
                    pixels[i * 4 + 2] = r;
                }
            }

            // Write the modified pixels into a new bitmap and use that as the source of an Image
            var bmp = new WriteableBitmap(image.PixelWidth, image.PixelHeight, image.DpiX, image.DpiY, PixelFormats.Bgra32, null);
            bmp.WritePixels(new Int32Rect(0, 0, image.PixelWidth, image.PixelHeight), pixels, image.PixelWidth * 4, 0);

            (imageObj as Image).Source = bmp;

        }

        #endregion Function

        
        private void ViewRowReset()
        {
            int viewCount = Grid_Viewbox.Children.Count; //antal view der er

            //kør igennem alle elementer
            for (int i = 0; i < viewCount -1; i++)
            {
                Grid thisElement = (Grid_Viewbox.Children[i] as Grid);
                thisElement.SetValue(Grid.RowProperty, i);
            }
        }
        private void ViewRowChange(int viewId, int newRowId)
        {
            Grid viewElement = Grid_Viewbox.Children[viewId] as Grid;
            viewElement.SetValue(Grid.RowProperty, newRowId);
        }

        private void MyDocumentViewer_CreatePDF_ReloadClick(object sender, RoutedEventArgs e)
        {
            SaveDoc();

            bool savePaper = false;

            try
            {
                if (System.IO.File.Exists(MyDocumentViewer_PDFViewer.Document))
                    System.IO.File.Delete(MyDocumentViewer_PDFViewer.Document);

            }
            catch (Exception)
            {
                return;
            }


            System.Threading.Thread createPDF = new System.Threading.Thread(() =>
            {
                //opret pdf fil
                var funcPDF = new Class.MakePDF();
                if (this.viewId == View_PDFFragt)
                    funcPDF.CreateFragtBrev(this.FileDocument);
                else
                    funcPDF.CreateFaktura(this.FileDocument);


                MyDocumentViewer_PDFViewer.FileIsCreated = true;
                MyDocumentViewer_PDFViewer.ReloadDocument();

                //if (System.Threading.SynchronizationContext.Current == null)
                //    return;

                //System.Threading.SynchronizationContext.Current.Post(delegate {

                //    MyDocumentViewer_PDFViewer.FileIsCreated = true;
                //    MyDocumentViewer_PDFViewer.ReloadDocument();
                //},null);

            });
            createPDF.Start();
            

        }


        private void UpdateImageButtomStatus()
        {
            string imageCheck = "/Images/Icons/CheckWhite.png";
            string imageUnCheck = "/Images/Icons/UnCheckWhite.png";

            switch (this.documentStatus)
            {
                case document_FragtInfo:
                    ChangeImageColor(Image_ButtomStatusFragt, imageUnCheck, image_Color_NotDone);
                    ChangeImageColor(Image_ButtomStatusFaktura, imageUnCheck, image_Color_NotDone);
                    break;

                case document_FakturaInfo:
                    ChangeImageColor(Image_ButtomStatusFragt, imageCheck, image_Color_Done);
                    ChangeImageColor(Image_ButtomStatusFaktura, imageUnCheck, image_Color_NotDone);
                    break;

                case document_FakturaClose:
                    ChangeImageColor(Image_ButtomStatusFragt, imageCheck, image_Color_Done);
                    ChangeImageColor(Image_ButtomStatusFaktura, imageCheck, image_Color_Done);
                    break;
            }

        }

        private void Grid_PDFView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((sender as Grid).Visibility != Visibility.Visible)
                return;

            this.FileDocument.Invoice = 1000;
            MyDocumentViewer_PDFViewer.IsLoading = true;
            MyDocumentViewer_PDFViewer.FileIsCreated = false;

            if ((sender as Grid).Visibility != Visibility.Visible)
                return;
            
            if (this.viewId == View_PDFFragt)
                MyDocumentViewer_PDFViewer.Document = Models.ImportantData.g_FolderPdf + "Fragtbrev-" + this.FileDocument.Invoice + ".pdf";
            else if (this.viewId == View_PDFFaktura)
                MyDocumentViewer_PDFViewer.Document = Models.ImportantData.g_FolderPdf + "Faktura-" + this.FileDocument.Invoice + ".pdf";
            else return;
            
            MyDocumentViewer_CreatePDF_ReloadClick(MyDocumentViewer_PDFViewer, null);
        }

        private void ScrollViewBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            double pricelistMarginEkstra = 10;
            Thickness pricelistMargin = Prislist_faktura.Margin;
            pricelistMargin.Top = (sender as ScrollViewer).VerticalOffset + pricelistMarginEkstra;
            Prislist_faktura.Margin = pricelistMargin;


        }

        private void UpdateChangeViewButtonVisibility()
        {
            //start med at være vis
            Button_GridViewBack.Visibility = Visibility.Visible;
            Button_GridViewNext.Visibility = Visibility.Visible;

            
            switch (this.viewId)
            {
                case View_Contact:
                    Button_GridViewBack.Visibility = Visibility.Hidden;
                    break;

                    
                case View_GenereltFragt:
                case View_TransportFragt:
                case View_Notifications:
                    if(this.UseFullView)
                        Button_GridViewBack.Visibility = Visibility.Hidden;
                    break;




                case View_StatusFragt:
                    if (GetHighestDocumentAccess() == document_FragtInfo)
                        Button_GridViewNext.Visibility = Visibility.Hidden;
                    break;

                case View_StatusFaktura:
                    if (GetHighestDocumentAccess() == document_FakturaInfo)
                        Button_GridViewNext.Visibility = Visibility.Hidden;
                    break;

                case View_CloseFaktura:
                        Button_GridViewNext.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private int GetHighestDocumentAccess()
        {
            if (GetViewStatus(View_TransportFaktura) &&
                GetViewStatus(View_Gebyr))
                return document_FakturaClose;
            else if (GetViewStatus(View_CloseFragt))
                return document_FakturaInfo;
            else if (GetViewStatus(View_Contact) &&
                    GetViewStatus(View_GenereltFragt) &&
                    GetViewStatus(View_TransportFragt) &&
                    GetViewStatus(View_Notifications))
                return document_FragtClose;
            else
                return document_FragtInfo;
        }

        private void ToolBarButton_Test(object sender, RoutedEventArgs e)
        {
            int index = Grid.GetColumn((sender as System.Windows.Controls.Primitives.ToggleButton));
            bool isChecked = (sender as System.Windows.Controls.Primitives.ToggleButton).IsChecked.Value;

            switch (index)
            {
                case 0:
                    if (isChecked)
                    {
                        Address_Afsender.Textbox_ContactTlf.Text = "10 20 30 40";
                        Address_Afsender.Textbox_ContactPer.Text = "Afsender";
                        Address_Afsender.Textbox_FirmName.Text = "Afsender A/S";
                        Address_Afsender.Textbox_ZipCode.Text = "1000";
                        Address_Afsender.Textbox_Address.Text = "Afsender 128";
                        Address_Afsender.CheckBox_Pay.Checked = true;


                        Address_Modtager.Textbox_ContactTlf.Text = "50 60 70 80 90";
                        Address_Modtager.Textbox_ContactPer.Text = "Modtager";
                        Address_Modtager.Textbox_FirmName.Text = "Modtager A/S";
                        Address_Modtager.Textbox_ZipCode.Text = "2000";
                        Address_Modtager.Textbox_Address.Text = "Modtager 128";

                        Address_Modtager2.Textbox_ContactTlf.Text = "10 10 50 50";
                        Address_Modtager2.Textbox_ContactPer.Text = "Modtager2";
                        Address_Modtager2.Textbox_FirmName.Text = "Modtager2 A/S";
                        Address_Modtager2.Textbox_ZipCode.Text = "3000";
                        Address_Modtager2.Textbox_Address.Text = "Modtager2 128";


                        this.viewHasBeenChecked[View_Notifications] = true;

                    }
                    else {

                        Address_Afsender.Textbox_ContactTlf.Text = "";
                        Address_Afsender.Textbox_ContactPer.Text = "";
                        Address_Afsender.Textbox_FirmName.Text = "";
                        Address_Afsender.Textbox_ZipCode.Text = "";
                        Address_Afsender.Textbox_Address.Text = "";
                        Address_Afsender.CheckBox_Pay.Checked = false;

                        Address_Modtager.Textbox_ContactTlf.Text = "";
                        Address_Modtager.Textbox_ContactPer.Text = "";
                        Address_Modtager.Textbox_FirmName.Text = "";
                        Address_Modtager.Textbox_ZipCode.Text = "";
                        Address_Modtager.Textbox_Address.Text = "";
                        Address_Modtager.CheckBox_Pay.Checked = false;

                        Address_Modtager2.Textbox_ContactTlf.Text = "";
                        Address_Modtager2.Textbox_ContactPer.Text = "";
                        Address_Modtager2.Textbox_FirmName.Text = "";
                        Address_Modtager2.Textbox_ZipCode.Text = "";
                        Address_Modtager2.Textbox_Address.Text = "";
                        Address_Modtager2.CheckBox_Pay.Checked = false;

                        this.viewHasBeenChecked[View_Notifications] = false;
                    }
                    break;
                case 1:
                    if (isChecked)
                    {
                        MyTextbox_Generelt_Reference.Text = "Reference";
                        MyTextbox_Generelt_Fragtmand.Text = "Fragtmand";
                        MyTextbox_Generelt_Rute_1.Text = "8000";
                        MyTextbox_Generelt_Rute_2.Text = "9000";

                        DateTimePicker_Generelt_Rute_1.SelecteDate = DateTime.Now;
                        DateTimePicker_Generelt_Rute_2.SelecteDate = DateTime.Now;
                        DateTimePicker_Generelt_Rute_1.SetDatetimeTextboxText();
                        DateTimePicker_Generelt_Rute_2.SetDatetimeTextboxText();

                        MyTextbox_Byttepaller_1.Text = "3";
                        MyTextbox_Byttepaller_2.Text = "2";
                        MyTextbox_Byttepaller_3.Text = "5";

                        Selector_Generelt_Forsikringstype.SelectId = 2;

                        CheckBoxButton_Efterkrav.Checked = true;
                        MyTextbox_Efterkrav_1.Text = "1000";
                        MyTextbox_Efterkrav_2.Text = "2000";
                        MyTextbox_Efterkrav_3.Text = "3000";
                        MyTextbox_Efterkrav_4.Text = "4000";
                    }
                    else {

                        MyTextbox_Generelt_Reference.Text = "";
                        MyTextbox_Generelt_Fragtmand.Text = "";
                        MyTextbox_Generelt_Rute_1.Text = "";
                        MyTextbox_Generelt_Rute_2.Text = "";

                        Selector_Generelt_Forsikringstype.SelectId = -1;

                        DateTimePicker_Generelt_Rute_1.SelecteDate = DateTime.Now;
                        DateTimePicker_Generelt_Rute_2.SelecteDate = DateTime.Now;

                        DateTimePicker_Generelt_Rute_1.HasBeenSet = false;
                        DateTimePicker_Generelt_Rute_2.HasBeenSet = false;

                        MyTextbox_Byttepaller_1.Text = "";
                        MyTextbox_Byttepaller_2.Text = "";
                        MyTextbox_Byttepaller_3.Text = "";

                        CheckBoxButton_Efterkrav.Checked = false;
                        MyTextbox_Efterkrav_1.Text = "";
                        MyTextbox_Efterkrav_2.Text = "";
                        MyTextbox_Efterkrav_3.Text = "";
                        MyTextbox_Efterkrav_4.Text = "";

                    }
                    break;
                case 2:
                    if (isChecked) {

                        Selector_Transport1.SelectId = 0;
                        Selector_Transport_SelectionChanged(Selector_Transport1, null);
                        Selector_Transport2.SelectId = 0;
                        Selector_Transport_SelectionChanged(Selector_Transport2, null);
                        Selector_Transport3.SelectId = 3;
                        Selector_Transport_SelectionChanged(Selector_Transport3, null);
                        PakkeControl_Transport.Items.Clear();

                        for (int i = 0; i < 10; i++)
                        {
                            PakkeControl_Transport.Items.Add(new PakkeControlItem() {
                                IsDoneFragt = true,
                                mrkNumb = (1000 + i).ToString(),
                                contains = "Dette er en test " + i,
                                weightS = "20",
                                weightD = 20,
                                countI = 4,
                                countS = "4",
                                artId = 1,
                                artName = "Test",
                                volume = "10*10*10",
                                volumeL = 10,
                                volumeB = 10,
                                volumeH = 10,
                                transportTypeId = 1,
                                

                            });
                        }
                        PakkeControl_Transport.Update();
                    }
                    else {

                        Selector_Transport1.SelectId = -1;
                        Selector_Transport_SelectionChanged(Selector_Transport1, null);
                        PakkeControl_Transport.Items.Clear();

                    }
                    break;
                case 3:
                    if (isChecked) { }
                    else { }
                    break;
                case 4:
                    if (isChecked) { }
                    else { }
                    break;
                case 5:
                    if (isChecked) { }
                    else { }
                    break;
                case 6:
                    if (isChecked) { }
                    else { }
                    break;
                case 7:
                    if (isChecked) { }
                    else { }
                    break;
                case 8:
                    if (isChecked) { }
                    else { }
                    break;
            }
        }
    }
}
