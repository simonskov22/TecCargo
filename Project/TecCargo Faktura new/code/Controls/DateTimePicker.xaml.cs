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
using System.ComponentModel;
using System.Windows.Threading;



namespace TecCargo_Faktura.Controls
{
    /// <summary>
    /// Interaction logic for DateTimePicker.xaml
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        #region DependencyProperty

        public static readonly DependencyProperty SelecteDateProperty =
            DependencyProperty.Register("SelecteDate", typeof(DateTime), typeof(DateTimePicker),
            new PropertyMetadata(DateTime.Now));
        public static readonly DependencyProperty FormatDateProperty =
            DependencyProperty.Register("Format", typeof(string), typeof(DateTimePicker),
            new PropertyMetadata("dd/MM/yyyy"));
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(DateTimePicker),
            new PropertyMetadata("Vælg Dato"));
        public static readonly DependencyProperty TimeVisibilityProperty =
            DependencyProperty.Register("TimeVisibility", typeof(Visibility), typeof(DateTimePicker),
            new PropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty IsUnLockProperty =
            DependencyProperty.Register("IsUnLock", typeof(bool), typeof(DateTimePicker), new PropertyMetadata(true));
        public static readonly DependencyProperty HasBeenSetProperty =
            DependencyProperty.Register("HasBeenSet", typeof(bool), typeof(DateTimePicker), new PropertyMetadata(false));
        public static readonly DependencyProperty ShowAlwaysPlaceholderOnTopProperty =
            DependencyProperty.Register("ShowAlwaysPlaceholderOnTop", typeof(bool), typeof(DateTimePicker), new PropertyMetadata(false, PropertyChangedCallback));


        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            DateTimePicker thisDateTime = dependencyObject as DateTimePicker;
            thisDateTime.UpdateHoverSize();
            Thickness textboxMargin = thisDateTime.Margin;
            thisDateTime._hovertext.Visibility = Visibility.Visible;

            //gem margin top engang så den sider rigtig
            if (thisDateTime.marginTop == -1)
                thisDateTime.marginTop = textboxMargin.Top;

            //flyt på textboxen
            if (thisDateTime.ShowAlwaysPlaceholderOnTop)
                textboxMargin.Top = thisDateTime._hovertext.ActualHeight + thisDateTime.marginTop;
            else
                textboxMargin.Top = thisDateTime.marginTop;


            thisDateTime.Margin = textboxMargin;
            thisDateTime.SetDatetimeTextboxText();

            //om de skal være synlige
            if (thisDateTime.ShowAlwaysPlaceholderOnTop)
            {
                thisDateTime._hovertext.Visibility = Visibility.Visible;
            }
            else
            {
                thisDateTime._hovertext.Visibility = Visibility.Collapsed;
            }
        }
        public DateTime SelecteDate
        {
            get
            {
                return (DateTime)GetValue(SelecteDateProperty);
            }
            set
            {
                this.HasBeenSet = true;
                SetValue(SelecteDateProperty, value);
            }
        }
        public string Format
        {
            get { return (string)GetValue(FormatDateProperty); }
            set { SetValue(FormatDateProperty, value); }
        }
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
        public Visibility TimeVisibility
        {
            get { return (Visibility)GetValue(TimeVisibilityProperty); }
            set { SetValue(TimeVisibilityProperty, value); }
        }
        public bool IsUnLock
        {
            get { return (bool)GetValue(IsUnLockProperty); }
            set { SetValue(IsUnLockProperty, value); }
        }

        public bool HasBeenSet {
            get { return (bool)GetValue(HasBeenSetProperty); }
            set {
                SetValue(HasBeenSetProperty, value);
            }
        }
        public bool ShowAlwaysPlaceholderOnTop
        {
            get { return (bool)GetValue(ShowAlwaysPlaceholderOnTopProperty); }
            set { SetValue(ShowAlwaysPlaceholderOnTopProperty, value); }
        }
        #endregion

        private bool AllowTimeChange = false;
        private DateTime _selectedDate;
        //public bool hasBeenSet = false;
        private bool monthButtonClick = false;
        private bool yearButtonClick = false;
        private double marginTop = -1;

        public event EventHandler Changed;


        private delegate void NoArgDelegate();

        public DateTimePicker()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Lav ekstra indstillinger
        /// </summary>
        private void _DateTimePicker_Loaded(object sender, RoutedEventArgs e)
        {
            _slider_Hour.Value = this.SelecteDate.Hour;
            _slider_Minute.Value = this.SelecteDate.Minute;
            AllowTimeChange = true;
            

            SetDatetimeTextboxText();
            UpdateDayButtons();
        }
        private void UpdateHoverSize()
        {
            //sæt til at være over tekst
            _hovertext.Visibility = Visibility.Visible;
            string hoverText = _hovertext.Content.ToString();
            _hovertext.Content = "GetHeight";
            this._hovertext.Dispatcher.Invoke(DispatcherPriority.Render, (NoArgDelegate)delegate { });
            double hoverMarginTop = (_hovertext.ActualHeight * -1) + 1;
            this._hovertext.Margin = new Thickness(0, hoverMarginTop, 0, 0);
            _hovertext.Content = hoverText;
            this._hovertext.Dispatcher.Invoke(DispatcherPriority.Render, (NoArgDelegate)delegate { });
        }
        #region Functions

        /// <summary>
        /// Opret dag menu knapper
        /// </summary>
        private void UpdateDayButtons()
        {
            //vis tids menu hvis valgt
            _timePicker.Visibility = TimeVisibility;

            // nulstil knap grid
            Grid_DateSelectButtons.Children.Clear();
            Grid_DateSelectButtons.ColumnDefinitions.Clear();
            Grid_DateSelectButtons.RowDefinitions.Clear();

            string[] dayShorText = { "m", "t", "o", "t", "f", "l", "s" };//dag navne forkortelse

            DateTime SelectDay = this.SelecteDate;

            #region lav år og måned variabler

            int thisYear = SelectDay.Year;
            int thisMonth = SelectDay.Month;
            int lastYear = thisYear;
            int lastMonth = thisMonth - 1;
            int nextYear = thisYear;
            int nextMonth = thisMonth + 1;

            if (lastMonth == 0)
            {
                lastMonth = 12;
                lastYear--;
            }
            if (nextMonth == 13)
            {
                nextYear++;
                nextMonth = 1;
            }
            if (lastYear == 0)
            {
                lastYear = 1;
            }

            #endregion


            int daysInMonth_This = DateTime.DaysInMonth(thisYear, thisMonth); //dage i denne måned
            int daysInMonth_Last = DateTime.DaysInMonth(lastYear, lastMonth); //dage i sidste måned
            int daysInMonth_Last_Monday = 0; //sidste måned mandag dato

            //find sidste måned mandag dato
            for (int i = daysInMonth_Last; i > 0; i--)
            {
                DateTime dateVal = new DateTime(lastYear, lastMonth, i);
                if (dateVal.DayOfWeek == DayOfWeek.Sunday)
                {
                    daysInMonth_Last_Monday = i + 1;
                    break;
                }
            }

            //lav kolonne for hver dag
            for (int i = 0; i < 7; i++)
            {
                Grid_DateSelectButtons.ColumnDefinitions.Add(new ColumnDefinition());
            }
            //lav en række
            Grid_DateSelectButtons.RowDefinitions.Add(new RowDefinition());

            //lav header for dag navne forkortelse
            for (int i = 0; i < dayShorText.Count(); i++)
            {
                TextBlock dayTextBlock = new TextBlock();
                dayTextBlock.Text = dayShorText[i].ToUpper();
                dayTextBlock.FontWeight = FontWeights.Bold;
                dayTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
                
                dayTextBlock.Foreground = Brushes.White;
                dayTextBlock.Padding = new Thickness(5);

                Grid.SetColumn(dayTextBlock, i);
                Grid.SetRow(dayTextBlock, 0);
                Grid_DateSelectButtons.Children.Add(dayTextBlock);
            }


            int[] startInt = { daysInMonth_Last_Monday, 1, 1 }; //hvornår måned starter
            int[] stopInt = { daysInMonth_Last, daysInMonth_This, 20 }; //hvornår måned slutter
            int[] monthInt = { lastMonth, thisMonth, nextMonth }; //måneder
            int[] yearInt = { lastYear, thisYear, nextYear }; //år

            //lav dato knap menu
            for (int i = 0; i < 3; i++)
            {
                for (int a = startInt[i]; a <= stopInt[i]; a++)
                {
                    DateTime dateVal = new DateTime(yearInt[i], monthInt[i], a);

                    if (!AddGridDayCell(dateVal))
                    {
                        break;
                    }
                }
            }

            UpdateMonthYearValues();
        }

        /// <summary>
        /// Opret en dag menu knap
        /// </summary>
        private bool AddGridDayCell(DateTime Date, int maxWeek = 6)
        {
            DateTime SelectDay = this._selectedDate;

            //hvis det er en mandag lav en ny række
            if (Date.DayOfWeek == DayOfWeek.Monday)
            {
                Grid_DateSelectButtons.RowDefinitions.Add(new RowDefinition());
            }

            int startRow = 1;
            int rowId = Grid_DateSelectButtons.RowDefinitions.Count - startRow;
            int columnId = GetColumnIndex(Date); //hent columnid ud fra hvad dag det er

            //hvis man prøver at tilføj en dag der ligger 
            //uden for de antal uge der er valgt
            if (rowId == maxWeek + startRow)
            {
                return false;
            }

            //opret knap
            Button dayButton = new Button();
            dayButton.Style = (Style)this.FindResource(ToolBar.ButtonStyleKey);
            dayButton.Width = 30;
            dayButton.Content = Date.Day;
            dayButton.ToolTip = Date.ToShortDateString();
            dayButton.Click += Button_UpdateSelecteDay_Click;
            dayButton.BorderThickness = new Thickness(0);
            dayButton.Padding = new Thickness(5);
            dayButton.Margin = new Thickness(0);

            ChangeDayButtonColor(dayButton); //sæt knap farver

            Grid.SetRow(dayButton, rowId);//sæt række
            Grid.SetColumn(dayButton, columnId);//sæt kolonne

            //tilføj knap
            Grid_DateSelectButtons.Children.Add(dayButton);

            return true;
        }

        /// <summary>
        /// ændre på knap farve
        /// </summary>
        private void ChangeDayButtonColor(Button senderButton)
        {
            var funcClass = new Class.Functions.others();

            DateTime Date = DateTime.Parse(senderButton.ToolTip.ToString());

            //valgte knap
            if (this.SelecteDate.Year == Date.Year && this.SelecteDate.Month == Date.Month && this.SelecteDate.Day == Date.Day)
            {
                senderButton.Background = funcClass.ColorBrushHex("#FF3A8CDE");
                senderButton.Foreground = Brushes.White;
                senderButton.FontWeight = FontWeights.Bold;
            }

            //idag knap
            else if (DateTime.Now.Year == Date.Year && DateTime.Now.Month == Date.Month && DateTime.Now.Day == Date.Day)
                senderButton.Background = Brushes.LightBlue;

            //i denne måned
            else if (this.SelecteDate.Year == Date.Year && this.SelecteDate.Month == Date.Month)
                senderButton.Background = Brushes.White;

            //ikke i denne måned
            else
                senderButton.Background = Brushes.LightGray;
        }

        /// <summary>
        /// hent dag id
        /// </summary>
        private int GetColumnIndex(DateTime Date) 
        {
            switch (Date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
            }
            return -1;
        }

        /// <summary>
        /// opdatere knap tekst 
        /// for måned og år
        /// </summary>
        private void UpdateMonthYearValues()
        {
            string monthNameLower = this.SelecteDate.ToString("MMMM");
            string monthNameCapitalize = monthNameLower.Substring(0, 1).ToUpper();
            monthNameCapitalize += monthNameLower.Substring(1).ToLower();
            this._month.Content = monthNameCapitalize;
            this._year.Content = this.SelecteDate.Year.ToString();
        }
        
        /// <summary>
        /// opret en knap til måned og år
        /// </summary>
        private void AddGridCell(string text, int row, int column, int value, string selectedValue, bool isNowDate) 
        {
            var funcClass = new Class.Functions.others();

            //hvis rækken ikke findes opret den
            while (row >= Grid_DateSelectButtons.RowDefinitions.Count)
            {
                Grid_DateSelectButtons.RowDefinitions.Add(new RowDefinition());
            }

            //hvis kolonnen ikke findes opret den
            while (column >= Grid_DateSelectButtons.ColumnDefinitions.Count)
            {
                Grid_DateSelectButtons.ColumnDefinitions.Add(new ColumnDefinition());
            }

            //hvis det er måned skal value gøre en gang større
            if (this.monthButtonClick)
                value++;

            //opret knap
            Button cellButton = new Button();
            cellButton.Style = (Style)this.FindResource(ToolBar.ButtonStyleKey); //fjern default button layout
            cellButton.Content = text;
            cellButton.Click += Button_SelectSmall_YM_Click;
            cellButton.ToolTip = value;
            cellButton.BorderThickness = new Thickness(0);
            cellButton.Padding = new Thickness(5);

            //sæt bredde
            if (this.monthButtonClick)
                cellButton.Width = 100;
            else
                cellButton.Width = 100;


            if (text == selectedValue) //valgte måned
            {
                cellButton.Background = funcClass.ColorBrushHex("#FF3A8CDE");
                cellButton.Foreground = Brushes.White;
                cellButton.FontWeight = FontWeights.Bold;
            }
            else if (isNowDate) //denne måned
            {
                cellButton.Background = Brushes.LightBlue;
            }
            else //andet
            {
                cellButton.Background = Brushes.White;
            }


            Grid.SetColumn(cellButton, column);
            Grid.SetRow(cellButton, row);
            Grid_DateSelectButtons.Children.Add(cellButton);
        }

        /// <summary>
        /// lav år menu knapper
        /// </summary>
        private void UpdateYearButtons()
        {
            //skal være skjult
            _timePicker.Visibility = Visibility.Collapsed;

            //nulstil tidligere knapper
            Grid_DateSelectButtons.RowDefinitions.Clear();
            Grid_DateSelectButtons.ColumnDefinitions.Clear();
            Grid_DateSelectButtons.Children.Clear();

            int selectedYear = this.SelecteDate.Year; //valgte år
            int startYear = this._selectedDate.Year - 12; //start menu knap år

            //hvis startYear er under 0
            if (startYear <= 0)
                startYear = 1;

            for (int i = 0; i < 5; i++)
            {
                for (int a = 0; a < 5; a++)
                {
                    //om det er startYear er det samme som nu
                    bool isNowYear = (startYear == DateTime.Now.Year);

                    //opret knap
                    AddGridCell(startYear.ToString(), i, a, startYear, selectedYear.ToString(), isNowYear);

                    startYear++;//sæt til næste år
                }
            }
        }
        
        /// <summary>
        /// lav måned menu knapper
        /// </summary>
        private void UpdateMonthButtons()
        {
            //skal være skjult
            _timePicker.Visibility = Visibility.Collapsed;

            //nulstil tidligere knapper
            Grid_DateSelectButtons.RowDefinitions.Clear();
            Grid_DateSelectButtons.ColumnDefinitions.Clear();
            Grid_DateSelectButtons.Children.Clear();

            string[] monthNames = {
                    "Januar", "Februar", "Marts",
                    "April", "Maj", "Juni",
                    "Juli", "August", "September",
                    "Oktober", "November", "December",
                };

            int monthId = 0;
            string selectedMonthName = monthNames[this._selectedDate.Month - 1];
            selectedMonthName = selectedMonthName.Substring(0, 3).ToUpper();


            //tilføj knapper til grid/menu
            for (int i = 0; i < 4; i++)
            {
                for (int a = 0; a < 3; a++)
                {

                    string monthName = monthNames[monthId].Substring(0, 3).ToUpper(); //knap tekst
                    bool isNowMonth = (this._selectedDate.Year == DateTime.Now.Year && (monthId + 1) == DateTime.Now.Month);

                    AddGridCell(monthName, i, a, monthId, selectedMonthName, isNowMonth);
                    monthId++;
                }
            }
        }
        
        #endregion Functions

            #region Events

            /// <summary>
            /// åben popup menu med dage
            /// </summary>
        private void Button_ShowCalendar_Click(object sender, RoutedEventArgs e)
        {
            showCalendar.IsOpen = !showCalendar.IsOpen;
        }

        /// <summary>
        /// skift til at vælge måneder
        /// </summary>
        private void Button_SelectMonth_Click(object sender, RoutedEventArgs e)
        {
            this.monthButtonClick = !this.monthButtonClick;
            this.yearButtonClick = false;

            //pile skal ikke være synlige
            this._back.Visibility = Visibility.Hidden;
            this._next.Visibility = Visibility.Hidden;


            if (this.monthButtonClick)
            {
                (sender as Button).FontWeight = FontWeights.Bold;
                _year.FontWeight = FontWeights.Normal;
                UpdateMonthButtons();                
            }
            else
            {
                (sender as Button).FontWeight = FontWeights.Normal;
                UpdateDayButtons();
            }
        }

        /// <summary>
        /// skift til at vælge år
        /// </summary>
        private void Button_SelectYear_Click(object sender, RoutedEventArgs e)
        {
            //hvad type knap det er
            this.yearButtonClick = !this.yearButtonClick;
            this.monthButtonClick = false;

            //vis pile knapperne
            this._back.Visibility = Visibility.Visible;
            this._next.Visibility = Visibility.Visible;

            //om den skal visse år eller dage
            if (this.yearButtonClick)
            {
                (sender as Button).FontWeight = FontWeights.Bold;
                _month.FontWeight = FontWeights.Normal;
                UpdateYearButtons();
            }
            else
            {
                (sender as Button).FontWeight = FontWeights.Normal;
                UpdateDayButtons();
            }
        }

        /// <summary>
        /// vælg måned eller år
        /// </summary>
        private void Button_SelectSmall_YM_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (sender as Button);
            int value = int.Parse(senderButton.ToolTip.ToString());
           
            bool dateHasBeenSet = this.HasBeenSet; //hent status om der er sat dato

            //hvis pile knapperne
            this._back.Visibility = Visibility.Visible;
            this._next.Visibility = Visibility.Visible;

            if (this.monthButtonClick)
            {
                _month.FontWeight = FontWeights.Normal;
                _year.FontWeight = FontWeights.Bold;

                this.SelecteDate = new DateTime(this.SelecteDate.Year, value, this.SelecteDate.Day);
                this.monthButtonClick = false;
                this.yearButtonClick = true;
                UpdateYearButtons();
            }
            else
            {
                _year.FontWeight = FontWeights.Normal;
                this.SelecteDate = new DateTime(value, this.SelecteDate.Month, this.SelecteDate.Day);

                this.yearButtonClick = false;
                UpdateDayButtons();
            }

            //skal ikke ændre datetimePicker knap/placeholder hvis der ikke er valgt en dato
            this.HasBeenSet = dateHasBeenSet;

            SetDatetimeTextboxText();
            UpdateMonthYearValues();
        }
        
        /// <summary>
        /// vælg dag
        /// </summary>
        private void Button_UpdateSelecteDay_Click(object sender, RoutedEventArgs e)
        {
            this.SelecteDate = DateTime.Parse((sender as Button).ToolTip.ToString());
            SetDatetimeTextboxText();

            //skal kun automatisk lukke hvis der ikke kan blive sat tid på
            if (this.TimeVisibility != Visibility.Visible)
                showCalendar.IsOpen = false;

            UpdateDayButtons();
        }

        /// <summary>
        /// skift mellem måneder og år
        /// </summary>
        private void Button_ShiftMonthArrow_Click(object sender, RoutedEventArgs e)
        {
            const string c_ButtonBack = "3";
            //const string c_ButtonNext = "4";

            bool dataHasBeenSet = this.HasBeenSet;

            Button senderButton = (sender as Button);

            if (this.yearButtonClick)
            {
                if (senderButton.Content.ToString() == c_ButtonBack)
                {
                    //antal år man skal kunne gå tilbage
                    if (this.SelecteDate.Year >= 26)
                        this.SelecteDate = this.SelecteDate.AddYears(-25);

                    else //år må ikke blive mindre end 1
                        this.SelecteDate = new DateTime(1, this.SelecteDate.Month, this.SelecteDate.Day);
                }
                else
                {
                    this.SelecteDate = this.SelecteDate.AddYears(25);
                }
            }
            else
            {
                if (senderButton.Content.ToString() == c_ButtonBack)
                {
                    this.SelecteDate = this.SelecteDate.AddMonths(-1);
                }
                else
                {
                    this.SelecteDate = this.SelecteDate.AddMonths(1);
                }
            }

            if (this.yearButtonClick)
                UpdateYearButtons();
            else
                UpdateDayButtons();

            this.HasBeenSet = dataHasBeenSet;
            UpdateMonthYearValues();
        }





        #endregion

        /// <summary>
        /// når tid slider ændres
        /// skal den opdatere teksten i datetimepicker knappen
        /// </summary>
        private void _slider_ChangeTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //skal kun opdatere når den er klar
            if (!this.AllowTimeChange)
                return;

            this.HasBeenSet = true;
            SetDatetimeTextboxText();
        }

        /// <summary>
        /// sæt teksten på datetimepicker knappen
        /// </summary>
        public void SetDatetimeTextboxText()
        {
            if (!this.IsLoaded)
                return;
            var funcClass = new Class.Functions.others();

            if (this.HasBeenSet)
            {
                //sæt tid hvis den er fremme
                if (this.TimeVisibility == Visibility.Visible)
                {
                    int hour = 0;
                    int minute = 0;
                    int.TryParse(_slider_Hour.Value.ToString(), out hour);
                    int.TryParse(_slider_Minute.Value.ToString(), out minute);

                    DateTime newDate = this.SelecteDate.Date;
                    TimeSpan time = new TimeSpan(hour, minute, 0);
                    newDate = newDate.Date + time;

                    this.SelecteDate = newDate;
                }
                
                this._selectedDate = this.SelecteDate;
                this._selectedText.Text = this._selectedDate.ToString(this.Format);
                this._button.Background = Brushes.White;
                this._selectedText.Foreground = Brushes.Black;
            }
            else
            {
                string selectDefaultText = this.ShowAlwaysPlaceholderOnTop ? "Vælg Dato" : this.Placeholder;
                this._selectedDate = DateTime.Now;
                this._selectedText.Text = selectDefaultText;
                this._selectedText.Foreground = funcClass.ColorBrushHex("#FF5489BD");
            }


            if (this.Changed != null)
                this.Changed(this, null);
        }

        /// <summary>
        /// gør det muligt at annuller dato
        /// og lukker popup menu ved at af keyboard
        /// genveje
        /// </summary>
        private void _DateTimePicker_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                case Key.Back:
                    this.SelecteDate = DateTime.Now;
                    this.HasBeenSet = false;
                    SetDatetimeTextboxText();
                    break;
                case Key.Escape:
                    showCalendar.IsOpen = false;
                    SetDatetimeTextboxText();
                    break;
            }
        }

        /// <summary>
        /// hvis header over tekst
        /// så man kan se hvad man
        /// har skrevet høre til
        /// </summary>
        private void _DateTimePicker_MouseEnter(object sender, MouseEventArgs e)
        {
            //hover tekst skal kun visse når der er valgt en dag
            //hvis ander ikke er valgt
            if ((!_DateTimePicker.IsMouseOver && this.HasBeenSet && !_hovertext.IsMouseOver) || this.ShowAlwaysPlaceholderOnTop)
                _hovertext.Visibility = Visibility.Visible;
            else
                _hovertext.Visibility = Visibility.Collapsed;
        }
        
    }
}
