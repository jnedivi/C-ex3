﻿﻿﻿﻿﻿﻿﻿﻿﻿using GarageLogic;
using System;
using System.Text;
using System.Collections.Generic;
namespace ConsoleUI
{
    public class UserInterface
    {
        /*** Data Members ***/

        private Garage m_Garage;
        private const int k_NumberOfMainMenuItems = 8;
        private const byte k_MinPhoneNumLength = 7;
        private const byte k_MaxPhoneNumLength = 10;
        private const byte k_LegalLicenseNumberLength = 7;

        /*** Constructor ***/

        public UserInterface()
        {
            m_Garage = new Garage();
            mainMenu();
        }

        /*** Class Logic ***/

        private void mainMenu()
        {
            System.Console.Clear();
            System.Console.WriteLine(MainMenuMessage());
            int mainMenuInputNumber = this.promptUserForMenuSelection(k_NumberOfMainMenuItems);

            if (mainMenuInputNumber == k_NumberOfMainMenuItems)
            {
                System.Console.WriteLine("Exit program selected. Bye Bye ...");
                System.Environment.Exit(1);
            }

            switch (mainMenuInputNumber)
            {
                case 1:
                    /* 1) Insert new Vehicle into Garage. */
                    insertNewVehicleIntoGarage();
                    break;
                case 2:
                    /* 2) Display list of licence numbers. */
                    displayListOfLicenceNumbers();
                    break;
                default:
                    /* Options 3 - 7 */
                    menuOptions(mainMenuInputNumber);
                    break;
            }
        }

        /* 1) Insert new Vehicle into Garage */
        private void insertNewVehicleIntoGarage()
        {
            System.Console.WriteLine(createMenuStringFromEnum(typeof(Factory.eVehicleType), "Insert New Vehicle"));
            int vehicleTypeNumber = promptUserForMenuSelection(Enum.GetNames(typeof(Factory.eVehicleType)).Length);
            Factory.eVehicleType vehicleToAdd = (Factory.eVehicleType)vehicleTypeNumber - 1;
            string licenseNumber;
            this.promptUserForLicenseNumber(out licenseNumber);

            if (this.m_Garage.IsInGarage(licenseNumber))
            {
                System.Console.WriteLine("Veicle currently in garage. Status changed to: In Repair.");
                this.m_Garage.StatusInRepairedUpdate(licenseNumber);
                System.Console.WriteLine(Environment.NewLine);
            }
            else
            {
                System.Console.WriteLine("Please enter Owner name:");
                string ownerName = System.Console.ReadLine();
                string phoneNumMessage = string.Format("Please enter Owners phone number ({0}-{1} digits):", k_MinPhoneNumLength, k_MaxPhoneNumLength);
                string validPhoneNumMessage = string.Format("Please enter a valid phone number ({0} -{1} digits:", k_MinPhoneNumLength, k_MaxPhoneNumLength);
                System.Console.WriteLine(phoneNumMessage);
                string ownerPhoneNumber = System.Console.ReadLine();
                while (!Vehicle.isLegalPhoneNumber(ownerPhoneNumber))
                {
                    System.Console.WriteLine(validPhoneNumMessage);
                    ownerPhoneNumber = System.Console.ReadLine();
                }

                System.Console.WriteLine("Please enter Vehicles Model Name:");
                string vehicleModelName = System.Console.ReadLine();

                this.m_Garage.InsertNewVehicle(vehicleToAdd, licenseNumber, ownerName, ownerPhoneNumber, vehicleModelName);

                System.Console.WriteLine(createMenuStringFromEnum(typeof(eTireAirPressureStatus), "Do all tires have the same air pressure"));
                int tireStatusNumber = promptUserForMenuSelection(Enum.GetNames(typeof(Factory.eVehicleType)).Length);
                Vehicle createdVehicle;
                m_Garage.GetVehicle(licenseNumber, out createdVehicle);
                List<float> tirePressures = new List<float>();

                if (tireStatusNumber == 1)
                {
                    float singleTireAirPressure = this.getTirePressureFromUser(createdVehicle);
                    for (int i = 0; i < createdVehicle.NumberOfWheels; i++)
                    {
                        tirePressures.Add(singleTireAirPressure);
                    }
                }
                else
                {
                    for (int i = 0; i < createdVehicle.NumberOfWheels; i++)
                    {
                        tirePressures.Add(getTirePressureFromUser(createdVehicle));
                    }
                }

                System.Console.WriteLine("Please enter Tires Manufacturer Name:");
                string tiresManufacturerName = System.Console.ReadLine();

                Factory.CreateWheels(createdVehicle, tiresManufacturerName, tirePressures); //TODO 
                int userChoice;

                if (createdVehicle is Car)
                {
                    System.Console.WriteLine(createMenuStringFromEnum(typeof(Car.eColor), "Enter the car's color:"));
                    userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(Car.eColor)).Length);
                    ((Car)createdVehicle).Color = (Car.eColor)userChoice - 1;

                    System.Console.WriteLine(createMenuStringFromEnum(typeof(Car.eNumOfDoors), "Enter the car's number of doors:"));
                    userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(Car.eNumOfDoors)).Length);
                    ((Car)createdVehicle).NumberOfDoors = (Car.eNumOfDoors)userChoice - 1;


                    /*Factory.CreateCarFeatures((Car)createdVehicle, (Car.eNumOfDoors)userChoice, (Car.eColor)userChoice); //TODO

                    ((Car)createdVehicle).SetCarDoorsAndColor((Car.eColor)userChoice , (Car.eNumOfDoors)userChoice);*/

                }
                else if (createdVehicle is Motorcycle)
                {
                    System.Console.WriteLine(createMenuStringFromEnum(typeof(Motorcycle.eLicenseType), "Enter the motorcycles's license type:"));
                    userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(Motorcycle.eLicenseType)).Length);
                    ((Motorcycle)createdVehicle).LicenceType = (Motorcycle.eLicenseType)userChoice - 1;
                    System.Console.WriteLine("Please enter engine volume:");
                    userChoice = (int)getFloatFromUser(0, int.MaxValue);
                    ((Motorcycle)createdVehicle).EngineVolume = userChoice;

                }
                else if (createdVehicle is Truck)
                {
                    System.Console.WriteLine(createMenuStringFromEnum(typeof(eIsCarryingHazardousMaterials), "Is the truck carrying hazardous materials?:"));
                    userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(eIsCarryingHazardousMaterials)).Length);

                    if (userChoice == 1)
                    {
                        ((Truck)createdVehicle).HasHazardousMaterials = true;
                    }
                    else
                    {
                        ((Truck)createdVehicle).HasHazardousMaterials = false;
                    }

                    System.Console.WriteLine("Please enter maximum allowed weight:");
                    float maxWeightAllowed = getFloatFromUser(0, int.MaxValue);
                    ((Truck)createdVehicle).MaxWeightAllowed = maxWeightAllowed;
                }

                if (createdVehicle.Engine is FuelBasedEngine)
                {
					System.Console.WriteLine("Please enter current amount of fuel:");
                    float currentAmountOfFuel = getFloatFromUser( 0, (float)((FuelBasedEngine)createdVehicle.Engine).MaxAmountOfFuel);
                    ((FuelBasedEngine)createdVehicle.Engine).CurrentAmountOfFuel = currentAmountOfFuel;
                }
                else if (createdVehicle.Engine is ElectricBasedEngine)
                {
					System.Console.WriteLine("Please enter remaining time of engine operation in hours:");
                    float currentBatteryEnergy = getFloatFromUser(0, (float)((ElectricBasedEngine)createdVehicle.Engine).MaxBatteryLife);
                    ((ElectricBasedEngine)createdVehicle.Engine).RemainingTimeOnBattery = currentBatteryEnergy;
                }
            }

            returnToMenuOrQuit();
        }


        /* 2) Display list of licence numbers */
        private void displayListOfLicenceNumbers()
        {
            System.Console.Clear();
            System.Console.WriteLine(createMenuStringFromEnum(typeof(eFilteredOrUnfiltered), "Would you like to see a filtered or unfiltered list of license numbers?"));

            int userSelection = this.promptUserForMenuSelection(Enum.GetNames(typeof(eFilteredOrUnfiltered)).Length);
            Dictionary<string, Vehicle>.KeyCollection listOfLicenses;

            if (userSelection == 1)
            {
                System.Console.WriteLine(createMenuStringFromEnum(typeof(Vehicle.eVehicleStatus), "Please enter a vehicle status you would like to filter by:"));
                int userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(Vehicle.eVehicleStatus)).Length);
                listOfLicenses = m_Garage.GetFilteredListOfLicenseNumbers((Vehicle.eVehicleStatus)(userChoice - 1));  
            }
            else
            {
                listOfLicenses = m_Garage.GetListOfLicenceNumbers();
            }

            if (listOfLicenses.Count == 0)
            {
                System.Console.WriteLine("No vehicles in the garage");
                System.Console.WriteLine(Environment.NewLine);
            }
            else
            {
                int vehicleNumber = 1;

                foreach (string license in listOfLicenses)
                {
                    System.Console.WriteLine(string.Format("{0}. Licence number: {1}{2}", vehicleNumber, license, Environment.NewLine));
                    vehicleNumber++;
                }
            }
            //System.Console.Clear();
            returnToMenuOrQuit();
        }

        /*** Options 3 - 7 ***/
        private void menuOptions(int i_MainMenuSelection)
        {
            int userChoice;
            System.Console.Clear();
            string licenseNumber;
            Vehicle currentVehicle = this.promptUserForLicenseNumber(out licenseNumber);
            if (currentVehicle != null)
            {
                switch (i_MainMenuSelection)
                {
                    case 3:
                        /* 3) Change a vehicle status */
                        System.Console.WriteLine(createMenuStringFromEnum(typeof(Vehicle.eVehicleStatus), "Enter a Vehicle Status"));
                        userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(Vehicle.eVehicleStatus)).Length);
                        try
                        {
                            m_Garage.ChangeVehicleStatus(licenseNumber, (Vehicle.eVehicleStatus)(userChoice - 1));
                            string vehicleStatusMessage = string.Format(@"The status of this vehicle is now: {0}
", (Vehicle.eVehicleStatus)(userChoice - 1));
                        }
                        catch(ArgumentException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                    case 4:
                        /* 4) Inflate tires to max */
                        currentVehicle.InflateAllWheelsToMax();
                        System.Console.Clear();
                        System.Console.WriteLine("Inflation of tires succesful.");
                        //System.Console.WriteLine(Environment.NewLine);
                        break;
                    case 5:
                        /* 5) Refuel vehicle */
                        
                        FuelBasedEngine fuelEngine = currentVehicle.Engine as FuelBasedEngine;

                        if (fuelEngine == null)
                        {
                            try
                            {
                                throw new FormatException();
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("This vehicle does not have a fuel based engine. Refuel Failed");
                                System.Console.WriteLine(Environment.NewLine);
                            }
                        }
                        else
                        {
                            System.Console.WriteLine(createMenuStringFromEnum(typeof(FuelBasedEngine.eFuelType), "Enter a Fuel Type"));
                            userChoice = this.promptUserForMenuSelection(Enum.GetNames(typeof(FuelBasedEngine.eFuelType)).Length);
                            string refuelMessage = string.Format(@"Current Amount Of Fuel: {0}
Maximum Amount Of Fuel: {1}
Please enter amount to refuel:", fuelEngine.CurrentAmountOfFuel, fuelEngine.MaxAmountOfFuel);
                            System.Console.WriteLine(refuelMessage);
                            float amountToRefuel = this.getFloatFromUser(0, fuelEngine.MaxAmountOfFuel - fuelEngine.CurrentAmountOfFuel);
                            
                            try
                            {
                                this.m_Garage.RefuelVehicle(licenseNumber, (FuelBasedEngine.eFuelType)(userChoice - 1), amountToRefuel);
                                System.Console.WriteLine("Refuel Succesful.");
                            }
                            catch (ArgumentException)
                            {
                                string fuelTypeExceptionMessage = string.Format("Wrong fuel type for this vehicle, expected {0}. Refuel Failed.", fuelEngine.FuelType);
                                Console.WriteLine(fuelTypeExceptionMessage);
                            }
                            System.Console.WriteLine(Environment.NewLine);
                        }
                        break;
                    case 6:
                        /* 6) charge vehicle */
                        ElectricBasedEngine electricEngine = currentVehicle.Engine as ElectricBasedEngine;

                        if(electricEngine == null)
                        {
                            try
                            {
                                throw new FormatException();
                            }
                            catch(FormatException)
                            {
                                System.Console.WriteLine("This vehicle does not have an electric based engine. Recharge Failed.");
                            }
                        }
                        string rechargeMessage = string.Format(@"Remaining time of engine operation in hours: {0}
Max time of engine operations in hours: {1}
Please enter number of minutes to recharge:", electricEngine.RemainingTimeOnBattery, electricEngine.MaxBatteryLife);
                        System.Console.WriteLine(rechargeMessage);
                        float amountToRecharge = this.getFloatFromUser(0, (electricEngine.MaxBatteryLife - electricEngine.RemainingTimeOnBattery)* 60);
                        m_Garage.ChargeElectricVehice(licenseNumber, amountToRecharge);
                        System.Console.WriteLine("Recharge Succesful.");
                        System.Console.WriteLine(Environment.NewLine);
                        break;
                    case 7:
                        /* 7) Display vehicle information */
                        System.Console.Clear();
                        System.Console.WriteLine(currentVehicle.ToString());
                        System.Console.WriteLine(Environment.NewLine);
                        break;
                }
            }
            else
            {
                System.Console.WriteLine(string.Format("Vehicle with licence number {0} is not in the garage.{1}", licenseNumber, Environment.NewLine));
            }

            returnToMenuOrQuit();
        }

        private float getFloatFromUser(float i_MinNumber, float i_MaxNumber)
        {
            float userInputNumber;
            string input = System.Console.ReadLine();
            while (!(float.TryParse(input, out userInputNumber) && userInputNumber >= i_MinNumber && userInputNumber <= i_MaxNumber))
            {
                try
                {
                    throw new ValueOutOfRangeException("getFloatFromUser", i_MinNumber, i_MaxNumber);
                }
                catch(ValueOutOfRangeException ex)
                {
                    System.Console.WriteLine(ex.ToString());
                    input = System.Console.ReadLine();
                }  
            }

            return userInputNumber;
        }

        /* Get Menu Selection From User */
        private int promptUserForMenuSelection(int i_NumberOfItems)
        {
            string userInputString = System.Console.ReadLine();
            int userInputNumber;
            string messageToUser = string.Format("Please enter a number between 1 and {0}", i_NumberOfItems);

            while (!(int.TryParse(userInputString, out userInputNumber) && userInputNumber >= 1 && userInputNumber <= i_NumberOfItems))
            {
                try
                {
                    throw new ValueOutOfRangeException("promptUserForMenuSelection,", 0, i_NumberOfItems);
                }
                catch(ValueOutOfRangeException ex)
                {
                    System.Console.WriteLine(ex.ToString());
                    userInputString = System.Console.ReadLine();
                }
            }
            System.Console.Clear();
            return userInputNumber;
        }

        /* Get Licence number and Vehilce from user */
        private Vehicle promptUserForLicenseNumber(out string o_licenceNumber)
        {
            string licenseNumberMessage = string.Format("Please enter the licence number ({0} digits) of your vehicle or Q to cancel:", k_LegalLicenseNumberLength);
            string invalidInputMessage = string.Format("Invalid input. Please enter a legal licence plate number ({0} digits).", k_LegalLicenseNumberLength);
            System.Console.WriteLine(licenseNumberMessage);
            o_licenceNumber = System.Console.ReadLine();
            if (o_licenceNumber == "Q" || o_licenceNumber == "q")
            {
                mainMenu();
            }
            else
            {
                while (!Vehicle.isLegalLicenseNumber(o_licenceNumber))
                {
                    System.Console.WriteLine(invalidInputMessage);
                    o_licenceNumber = System.Console.ReadLine();
                }
            }

            Vehicle currentVehicle;

            return m_Garage.GetVehicle(o_licenceNumber, out currentVehicle) ? currentVehicle : null;
        }

        /* Get Tire pressures from user */
        private float getTirePressureFromUser(Vehicle i_Vehicle)
        {
            float tirePressure;
            string maxAirPressumeMessage = string.Format("Please enter a number below or equal to the max air pressure: {0}.", i_Vehicle.MaxAirPressure);

            System.Console.WriteLine(maxAirPressumeMessage);
            string userInputPressure = System.Console.ReadLine();

            while (!(float.TryParse(userInputPressure, out tirePressure) && tirePressure <= i_Vehicle.MaxAirPressure && tirePressure >= 0))
            {
                try
                {
                    throw new ValueOutOfRangeException("getTirePressureFromUser", 0, i_Vehicle.MaxAirPressure);
                }
                catch(ValueOutOfRangeException ex)
                {
                    System.Console.WriteLine(ex.ToString());
                    userInputPressure = System.Console.ReadLine();
                }
            }

            return tirePressure;
        }


        /* Creates Menu From any given Enum */
        private static string createMenuStringFromEnum(Type i_EnumType, string i_Title)
        {
            int menuNumber = 1;
            StringBuilder menuString = new StringBuilder();
            menuString.AppendLine(i_Title);

            foreach (string menuValue in Enum.GetNames(i_EnumType))
            {
                menuString.Append(menuNumber.ToString()).Append(". ");
                menuString.Append(menuValue).Append(Environment.NewLine);
                menuNumber++;
            }

            return menuString.ToString();
        }

		/* Get Menu Selection From User */
		/*private int promptUserForMenuSelection(int i_NumberOfItems)
		{
			string userInputString = System.Console.ReadLine();
			int userInputNumber;
			string messageToUser = string.Format("Please enter a number between 1 and {0}", i_NumberOfItems);

			while (!(int.TryParse(userInputString, out userInputNumber) && userInputNumber >= 1 && userInputNumber <= i_NumberOfItems))
			{
				System.Console.WriteLine("Invalid Input. " + messageToUser);
				userInputString = System.Console.ReadLine();
			}
			System.Console.Clear();
			return userInputNumber;
		}*/

        private void returnToMenuOrQuit()
        {
            System.Console.WriteLine(createMenuStringFromEnum(typeof(eUserOptions), "Please choose whether to return to the Main Menu or Quit:"));
            int userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(eUserOptions)).Length);
            System.Console.Clear();

            switch (userChoice)
            {
                case 1:
                    mainMenu();
                    break;
                case 2:
                    System.Console.WriteLine("Exit program selected. Bye Bye ...");
                    System.Environment.Exit(1);
                    break;
            }
        } 


        public static string MainMenuMessage()
        {

            string mainMenuMessage = string.Format(
                @"Main Menu
Please Select a task number you wish to complete:
1) Insert new Vehicle into Garage.
2) Display list of licence numbers.
3) Change a Vehicle's status
4) Inflate tires
5) Refuel a vehicle.
6) Charge an electric vehicle.
7) Display vehicle information.
8) Quit ");

            return mainMenuMessage;
        }

        public enum eTireAirPressureStatus
        {
            Yes,
            No
        }

        public enum eIsCarryingHazardousMaterials
        {
            Yes,
            No
        }
        public enum eFilteredOrUnfiltered
        {
            Filtered,
            Unfiltered
        }

        public enum eUserOptions
        {
            Menu,
            Quit
        }
    }
}


