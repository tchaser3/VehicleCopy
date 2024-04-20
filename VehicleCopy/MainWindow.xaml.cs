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
using NewEventLogDLL;
using NewVehicleDLL;
using VehicleMainDLL;

namespace VehicleCopy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //setting up the classes
        WPFMessagesClass TheMessagesClass = new WPFMessagesClass();
        EventLogClass TheEventLogClass = new EventLogClass();
        VehicleClass TheVehicleClass = new VehicleClass();
        VehicleMainClass TheVehicleMainClass = new VehicleMainClass();

        VehiclesDataSet TheVehiclesDataSet = new VehiclesDataSet();
        FindVehicleMainByVehicleIDDataSet TheFindVehicleMainByVehicleIDDataSet = new FindVehicleMainByVehicleIDDataSet();
        VehicleMainDataSet TheVehicleMainDataSet = new VehicleMainDataSet();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TheVehiclesDataSet = TheVehicleClass.GetVehiclesInfo();

                dgrResults.ItemsSource = TheVehiclesDataSet.vehicles;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Copy // Window Loaded " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            int intCounter;
            int intNumberOfRecords;
            int intRecordsReturned;
            int intVehicleID;

            try
            {
                intNumberOfRecords = TheVehiclesDataSet.vehicles.Rows.Count - 1;

                for(intCounter = 0; intCounter <= intNumberOfRecords; intCounter++)
                {
                    intVehicleID = TheVehiclesDataSet.vehicles[intCounter].VehicleID;

                    TheFindVehicleMainByVehicleIDDataSet = TheVehicleMainClass.FindVehicleMainByVehicleID(intVehicleID);

                    intRecordsReturned = TheFindVehicleMainByVehicleIDDataSet.FindVehicleMainByVehicleID.Rows.Count;

                    if(intRecordsReturned == 0)
                    {
                        VehicleMainDataSet.vehiclemainRow NewVehicleRow = TheVehicleMainDataSet.vehiclemain.NewvehiclemainRow();

                        NewVehicleRow.Active = TheVehiclesDataSet.vehicles[intCounter].Active;
                        NewVehicleRow.AssignedOffice = TheVehiclesDataSet.vehicles[intCounter].AssignedOffice;
                        NewVehicleRow.Available = TheVehiclesDataSet.vehicles[intCounter].Available;
                        NewVehicleRow.EmployeeID = TheVehiclesDataSet.vehicles[intCounter].EmployeeID;
                        NewVehicleRow.LicensePlate = TheVehiclesDataSet.vehicles[intCounter].LicensePlate;
                        NewVehicleRow.Notes = TheVehiclesDataSet.vehicles[intCounter].Notes;
                        NewVehicleRow.OilChangeDate = TheVehiclesDataSet.vehicles[intCounter].OilChangeDate;
                        NewVehicleRow.OilChangeOdometer = TheVehiclesDataSet.vehicles[intCounter].OilChangeOdometer;
                        NewVehicleRow.VehicleMake = TheVehiclesDataSet.vehicles[intCounter].VehicleMake;
                        NewVehicleRow.VehicleModel = TheVehiclesDataSet.vehicles[intCounter].VehicleModel;
                        NewVehicleRow.VehicleNumber = Convert.ToString(TheVehiclesDataSet.vehicles[intCounter].BJCNumber);
                        NewVehicleRow.VehicleID = intVehicleID;
                        NewVehicleRow.VehicleYear = TheVehiclesDataSet.vehicles[intCounter].VehicleYear;
                        NewVehicleRow.VINNumber = TheVehiclesDataSet.vehicles[intCounter].VINNumber;

                        TheVehicleMainDataSet.vehiclemain.Rows.Add(NewVehicleRow);
                        TheVehicleMainClass.UpdateVehicleMainDB(TheVehicleMainDataSet);
                    }
                }

                TheVehicleMainDataSet = TheVehicleMainClass.GetVehicleMainInfo();

                dgrResults.ItemsSource = TheVehicleMainDataSet.vehiclemain;
            }
            catch (Exception Ex)
            {
                TheEventLogClass.InsertEventLogEntry(DateTime.Now, "Vehicle Copy // Process Button " + Ex.Message);

                TheMessagesClass.ErrorMessage(Ex.ToString());
            }
        }
    }
}
